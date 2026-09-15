using AutoMapper;
using HRMS.Application.DTOs.PaySlip;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using HRMS.domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services
{
    public class PaySlipService : IPaySlipService
    {
        private readonly IPaySlipRepository _paySlipRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public PaySlipService(IPaySlipRepository paySlipRepository, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository
            ,IAttendanceRepository attendanceRepository, ICompanyRepository companyRepository
            ,IMapper mapper,ICurrentUserService currentUser)
        {
            _paySlipRepository = paySlipRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _attendanceRepository = attendanceRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<PaySlipResponseDto> GenerateForWeekAsync(int employeeId,int departmentId,DateTime weekStart)
        {
            int daysSinceSaturday = ((int)weekStart.DayOfWeek + 1) % 7;
            var saturdayWeekStart = weekStart.AddDays(-daysSinceSaturday);
            if (await _paySlipRepository.PaySlipExistsForWeekAsync(employeeId, departmentId, saturdayWeekStart))
            {
                throw new ConflictException("Error!");
            }
            var employee = await _employeeRepository.GetbyIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException(nameof(Employee),employeeId);
            }
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
            {
                throw new NotFoundException(nameof(Department),departmentId);
            }
            var membership = employee.EmployeeDepartments.FirstOrDefault(ed => ed.DepartmentID == departmentId);
            if (membership == null)
            {
                throw new NotFoundException("This employee does not work in said department!");
            }
            var company = await _companyRepository.GetByIdAsync(department.CompanyId);
            if (company == null)
            {
                throw new NotFoundException("this company does not Exist!");
            }
            decimal monthlySalary = membership.Salary;
            decimal BaseWeeklyPay = monthlySalary * 12 / 52;
            //weekStart = weekStart.Date;
            var weekEnd = saturdayWeekStart.AddDays(6);
            var attendances = await _attendanceRepository.GetByEmployeeAndDepartmentAndWeekAsync(employeeId,departmentId,saturdayWeekStart,weekEnd);
            if (attendances.Count() == 0)
            {
                throw new NotFoundException("There are no attendances for this user amd department! cannot calculate pay!");
            }
            int scheduledDays = attendances.Count();
            decimal dailyRate = BaseWeeklyPay / scheduledDays;
            decimal totalScheduledHours = 0;
            decimal totalOverTimeHours = 0;
            int absentDays = 0;
            int unpaidLeaveDays = 0;
            int lateDays = 0;
            foreach (var attendance in attendances)
            {
                switch (attendance.AttendanceStatus)
                {
                    case AttendanceStatus.Absent:
                        absentDays++;
                        break;
                    case AttendanceStatus.OnUnpaidLeave:
                        unpaidLeaveDays++;
                        break;
                    case AttendanceStatus.Late:
                        lateDays++;
                        break;
                }
                if (attendance.Shift is not null)
                {
                    var scheduledDuration = CalculateShiftDuration(attendance.Shift.StartTime, attendance.Shift.EndTime);
                    totalScheduledHours +=  (decimal)scheduledDuration.TotalHours;
                    if (attendance.Clockedin != null && attendance.Clockedout != null)
                    {
                        var actualHours = (decimal)(attendance.Clockedout.Value - attendance.Clockedin.Value).TotalHours;
                        var excess = actualHours - (decimal)scheduledDuration.TotalHours;
                        if (excess > 0)
                        {
                            totalOverTimeHours += excess;
                        }
                    }
                }
            }
            decimal absenceDeduction = dailyRate * absentDays;
            decimal unpaidLeaveDeduction = dailyRate * unpaidLeaveDays;
            decimal lateDeduction = company.LateDeductionType == LateDeductionType.Percentage
                ? dailyRate * (company.LateDeductionValue / 100m) * lateDays
                : company.LateDeductionValue * lateDays;
            decimal hourlyRate = totalScheduledHours > 0 ? BaseWeeklyPay / totalScheduledHours : 0;
            decimal overtimePay = totalOverTimeHours * hourlyRate * company.OvertimeRate;
            decimal netPay = BaseWeeklyPay - absenceDeduction - lateDeduction - unpaidLeaveDeduction + overtimePay;
            var payslip = new PaySlip
            {
                EmployeeId = employeeId,
                DepartmentId = departmentId,
                WeekStart = saturdayWeekStart,
                WeekEnd = weekEnd,
                BasePay = BaseWeeklyPay,
                AbsenceDeduction = absenceDeduction,
                LateDeduction = lateDeduction,
                UnpaidLeaveDeduction = unpaidLeaveDeduction,
                OvertimePay = overtimePay,
                NetPay = netPay,
            };
            //var payslip = await BuildPayslipAsync(employee, membership, company, weekStart, weekStart.AddDays(6));
            await _paySlipRepository.AddAsync(payslip);
            var saved = await _paySlipRepository.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to save the calculated payslip.");

            return _mapper.Map<PaySlipResponseDto>(payslip);
        }
        public async Task RecalculateIfRecentAsync(int employeeId, int departmentId, DateTime attendanceDate)
        {
            var weekStart = PayrollWeekCalculator.GetWeekStart(attendanceDate);

            var existing = (await _paySlipRepository.GetByEmployeeIdAndWeekAsync(employeeId, weekStart))
                .FirstOrDefault(p => p.DepartmentId == departmentId);

            if (existing is null)
                return; 

            if (DateTime.UtcNow - existing.WeekEnd > PayrollWeekCalculator.RecalculationWindow)
                return; 

            var employee = await _employeeRepository.GetByIdWithDepartmentsAsync(employeeId);
            if (employee is null) return;

            var membership = employee.EmployeeDepartments.FirstOrDefault(ed => ed.DepartmentID == departmentId);
            if (membership is null) return;

            var company = await _companyRepository.GetByIdAsync(membership.Department.CompanyId);
            if (company is null) return;

            var recalculated = await BuildPayslipAsync(employee, membership, company, existing.WeekStart, existing.WeekEnd);

            existing.BasePay = recalculated.BasePay;
            existing.AbsenceDeduction = recalculated.AbsenceDeduction;
            existing.LateDeduction = recalculated.LateDeduction;
            existing.UnpaidLeaveDeduction = recalculated.UnpaidLeaveDeduction;
            existing.OvertimePay = recalculated.OvertimePay;
            existing.NetPay = recalculated.NetPay;
            existing.GeneratedAt = DateTime.UtcNow;

            _paySlipRepository.Update(existing);
            var isadded = await _paySlipRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("Could not change payslip!");
            }
        }
        public async Task<IEnumerable<CompanyPayrollSummaryDto>> GetCompanyPayrollSummaryAsync(int employeeId,DateTime weekStart)
        {
            var payslips = await _paySlipRepository.GetByEmployeeIdAndWeekAsync(employeeId, weekStart.Date);
            return payslips
                .GroupBy(p => new { p.Department.CompanyId, p.Department.Company.Name })
                .Select(g => new CompanyPayrollSummaryDto
                {
                    CompanyId = g.Key.CompanyId,
                    CompanyName = g.Key.Name,
                    TotalNetPay = g.Sum(p => p.NetPay),
                    Payslips = _mapper.Map<List<PaySlipResponseDto>>(g.ToList())
                });
        }
        public async Task<PaySlipResponseDto?> GetByIdAsync(int id)
        {
            var PaySlip = await _paySlipRepository.GetByIdAsync(id);
            return PaySlip is null ? null : _mapper.Map<PaySlipResponseDto>(PaySlip);
        }
        public async Task<IEnumerable<PaySlipResponseDto>> GetAllAsync()
        {
            var paySlips = await _paySlipRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaySlipResponseDto>>(paySlips);
        }
        public async Task<IEnumerable<PaySlipResponseDto>> GetByEmployeeIdAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetbyIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException(nameof(Employee), employeeId);
            }
            var paySlips = await _paySlipRepository.GetByEmployeeIdAsync(employeeId);
            var result = FilterVisible(paySlips);
            return _mapper.Map<IEnumerable<PaySlipResponseDto>>(result);
        }
        public async Task<IEnumerable<PaySlipResponseDto>> GetByDepartmentIdAsync(int departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
            {
                throw new NotFoundException(nameof(Department), departmentId);
            }
            var paySlips = await _paySlipRepository.GetByDepartmentIdAsync(departmentId);
            var filtered = FilterVisible(paySlips);
            return _mapper.Map<IEnumerable<PaySlipResponseDto>>(filtered);
        }
        public async Task<IEnumerable<PaySlipResponseDto>> SearchAsync(DateTime? startWeek = null,DateTime? endWeek= null,int? companyId= null,int? departmentId = null)
        {
            if (companyId != null)
            {
                var company = await _companyRepository.GetByIdAsync(companyId.Value);
                if (company == null)
                {
                    throw new NotFoundException(nameof(Company),companyId.Value);
                }
            }
            if (departmentId != null)
            {
                var department = await _departmentRepository.GetByIdAsync(departmentId.Value);
                if (department == null)
                {
                    throw new NotFoundException(nameof(Department),departmentId.Value);
                }
            }
            var payslips = await _paySlipRepository.GetByDateAsync(startWeek,endWeek,companyId,departmentId);
            var result = FilterVisible(payslips);
            return _mapper.Map<IEnumerable<PaySlipResponseDto>>(result);
        }
        public async Task DeleteAsync(int id)
        {
            var paySlip = await _paySlipRepository.GetByIdAsync(id);
            if (paySlip == null)
            {
                throw new NotFoundException(nameof(PaySlip),id);
            }
            _paySlipRepository.Delete(paySlip);
            var isdeleted = await _paySlipRepository.SaveChangesAsync();
            if (!isdeleted)
            {
                throw new Exception("could not delete the pay slip!");
            }
        }
        private static TimeSpan CalculateShiftDuration(TimeSpan start, TimeSpan end)
        {
            var duration = end - start;
            if (duration <= TimeSpan.Zero)
                duration = duration.Add(TimeSpan.FromHours(24));
            return duration;
        }
        private IEnumerable<PaySlip> FilterVisible(IEnumerable<PaySlip> slips)
        {
            return slips.Where(s => s.EmployeeId == _currentUser.EmployeeId || 
            (_currentUser.CompanyRoles.TryGetValue(s.Department.CompanyId,out var role) && Enum.TryParse<CompanyRole>(role,out var companyRole)
            && companyRole <= CompanyRole.HRManager)).ToList();
        }
        private async Task<PaySlip> BuildPayslipAsync(Employee employee, EmployeeDepartment membership, Company company, DateTime weekStart, DateTime weekEnd)
        {
            var attendances = (await _attendanceRepository.GetByEmployeeAndDepartmentAndWeekAsync(employee.Id, membership.DepartmentID, weekStart, weekEnd)).ToList();
            if (attendances.Count == 0)
                throw new ConflictException("No attendance records found for this employee, department, and week — cannot calculate payroll.");

            decimal monthlySalary = membership.Salary;
            decimal weeklyBasePay = monthlySalary * 12 / 52;
            int scheduledDays = attendances.Count;
            decimal dailyRate = weeklyBasePay / scheduledDays;

            decimal totalScheduledHours = 0, totalOvertimeHours = 0;
            int absentDays = 0, unpaidLeaveDays = 0, lateDays = 0;

            foreach (var a in attendances)
            {
                switch (a.AttendanceStatus)
                {
                    case AttendanceStatus.Absent: absentDays++; break;
                    case AttendanceStatus.OnUnpaidLeave: unpaidLeaveDays++; break;
                    case AttendanceStatus.Late: lateDays++; break;
                }

                if (a.Shift is not null)
                {
                    var scheduledDuration = CalculateShiftDuration(a.Shift.StartTime, a.Shift.EndTime);
                    totalScheduledHours += (decimal)scheduledDuration.TotalHours;

                    if (a.Clockedin is not null && a.Clockedout is not null)
                    {
                        var actualHours = (decimal)(a.Clockedout.Value - a.Clockedin.Value).TotalHours;
                        var excess = actualHours - (decimal)scheduledDuration.TotalHours;
                        if (excess > 0) totalOvertimeHours += excess;
                    }
                }
            }

            decimal absenceDeduction = dailyRate * absentDays;
            decimal unpaidLeaveDeduction = dailyRate * unpaidLeaveDays;
            decimal lateDeduction = company.LateDeductionType == LateDeductionType.Percentage
                ? dailyRate * (company.LateDeductionValue / 100m) * lateDays
                : company.LateDeductionValue * lateDays;
            decimal hourlyRate = totalScheduledHours > 0 ? weeklyBasePay / totalScheduledHours : 0;
            decimal overtimePay = totalOvertimeHours * hourlyRate * company.OvertimeRate;
            decimal netPay = weeklyBasePay - absenceDeduction - lateDeduction - unpaidLeaveDeduction + overtimePay;

            return new PaySlip
            {
                EmployeeId = employee.Id,
                DepartmentId = membership.DepartmentID,
                WeekStart = weekStart,
                WeekEnd = weekEnd,
                BasePay = weeklyBasePay,
                AbsenceDeduction = absenceDeduction,
                LateDeduction = lateDeduction,
                UnpaidLeaveDeduction = unpaidLeaveDeduction,
                OvertimePay = overtimePay,
                NetPay = netPay,
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}
