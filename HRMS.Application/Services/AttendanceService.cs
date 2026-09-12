using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using AutoMapper;
using HRMS.Application.DTOs.Attendance;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces;

namespace HRMS.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly INotificationService _notificationService;
        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository
            , IDepartmentRepository departdmentRepository, ICurrentUserService currentUser, INotificationService notificationService)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _departmentRepository = departdmentRepository;
            _currentUser = currentUser;
            _notificationService = notificationService;
        }
        public async Task<AttendanceResponseDto?> GetByIdAsync(int id)
        {
            var Attendance = await _attendanceRepository.GetByIdAsync(id);
            return Attendance is null ? null : _mapper.Map<AttendanceResponseDto>(Attendance);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetAllAsync()
        {
            var Attendances = await _attendanceRepository.GetAllAsync();
            var visible = FilterVisible(Attendances);
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(visible);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeIdAsync(int EmployeeId)
        {
            var Employee = await _employeeRepository.GetbyIdAsync(EmployeeId);
            if (Employee is null)
            {
                throw new NotFoundException("Employee is not found!");
            }
            var Attendances = await _attendanceRepository.GetByEmployeeIdAsync(EmployeeId);
            if (Attendances == null)
            {
                throw new NotFoundException("Attendance Record is not found!");
            }
            var visible = FilterVisible(Attendances);
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(visible);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetByStatusAsync(AttendanceStatus status)
        {
            var Attendances = await _attendanceRepository.GetByStatusAsync(status);
            if (Attendances == null)
            {
                throw new NotFoundException("Attendance Record is not found!");
            }
            var visible = FilterVisible(Attendances);
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(visible);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeAndStatusAsync(int id, AttendanceStatus status)
        {
            var Employee = await _employeeRepository.GetbyIdAsync(id);
            if (Employee is null)
            {
                throw new NotFoundException("Employee is not found!");
            }
            var Attendances = await _attendanceRepository.GetByEmployeeAndStatusAsync(Employee.Id, status);
            var visible = FilterVisible(Attendances);
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(visible); 
        }
        public async Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto)
        {
            var Attendance = _mapper.Map<Attendance>(dto);
            var Employee = await _employeeRepository.GetbyIdAsync(dto.EmployeeId);
            var Department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            var Shift = await _shiftRepository.GetByIdAsync(dto.ShiftId);
            if (Employee is null)
            {
                throw new NotFoundException("Employee is not found!");
            }
            if (Department is null)
            {
                throw new NotFoundException("Department is not found!");
            }
            if (Shift is null)
            {
                throw new NotFoundException("Shift is not found!");
            }
            if (!Employee.EmployeeDepartments.Any(ed => ed.DepartmentID == dto.DepartmentId))
            {
                throw new ConflictException("The Employee doesnt work in this department!");
            }
            await _attendanceRepository.AddAsync(Attendance);
            var isadded = await _attendanceRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("The Create action hasnt been successful!");
            }
            await _notificationService.NotifyAsync(Employee.UserId,
                $"A new attendance record has been scheduled for {Attendance.Date:yyyy-MM-dd}.",
                NotificationType.AttendanceCreated,
                attendanceId: Attendance.Id);
            return _mapper.Map<AttendanceResponseDto>(Attendance);
        }
        public async Task<bool> UpdateAsync(int id, UpdateAttendanceDto dto)
        {
            NotificationType notifType = new();
            var Attendance = await _attendanceRepository.GetByIdAsync(id);
            if (Attendance is null)
            {
                throw new NotFoundException(nameof(Attendance),id);
            }
            if (Attendance.AttendanceStatus == AttendanceStatus.OnUnpaidLeave || Attendance.AttendanceStatus == AttendanceStatus.OnPaidLeave ||
                Attendance.ShiftId == null)
            {
                throw new ConflictException("You cannot clock in or out for a \"On Leave\" attendance record!");
            }
            if (DateTime.UtcNow < CalculateShiftStart(Attendance))
            {
                throw new ConflictException("You cannot Clock in before due time!");
            }
            if (Attendance.Clockedin is null)
            {
                if (dto.ClockedIn != null)
                {
                    Attendance.Clockedin = dto.ClockedIn;
                    notifType = NotificationType.AttendanceClockIn;
                }
                else
                {
                    throw new ConflictException("Cannot clock out when you havent clocked in!");
                }
            }
            else if (Attendance.Clockedout is null)
            {
                if (dto.ClockedOut != null)
                {
                    if (dto.ClockedIn <= CalculateShiftStart(Attendance))
                    {
                        throw new ConflictException("You cannot clockin before the shift start!");
                    }
                    Attendance.Clockedout = dto.ClockedOut;
                    Attendance.AttendanceStatus = AttendanceStatus.Present;
                    notifType = NotificationType.AttendanceClockOut;
                }
                else
                {
                    throw new ConflictException("You have already clocked in!");
                }
            }
            if (dto.ClockedOut != null && dto.ClockedIn != null)
            {
                throw new ConflictException("You cannot clock in and clock out at the same time!");
            }
            if (DateTime.UtcNow - dto.ClockedIn < TimeSpan.FromMinutes(5))
            {
                Attendance.AttendanceStatus = AttendanceStatus.Pending;
            }
            if (DateTime.UtcNow - dto.ClockedIn > TimeSpan.FromMinutes(5))
            {
                Attendance.AttendanceStatus = AttendanceStatus.Late;
            }
            if (DateTime.UtcNow - dto.ClockedIn > TimeSpan.FromMinutes(30))
            {
                Attendance.AttendanceStatus = AttendanceStatus.Absent;
            }
            _attendanceRepository.Update(Attendance);
            var isadded = await _attendanceRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("Could not Update attendance!");
            }
            await _notificationService.NotifyAsync(Attendance.Employee.UserId,
                $"{Attendance.Employee.FirstName} {Attendance.Employee.LastName} clocked in/out for {Attendance.Date:yyyy-MM-dd}.",
                notifType,
                attendanceId : Attendance.Id);
            return isadded;
        }
        public async Task<bool> HighClearanceUpdateAsync(int id, UpdateAttendanceDto dto)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance == null)
            {
                throw new NotFoundException(nameof(attendance),id);
            }
            if (dto.ShiftId != null)
            {
                var shift = await _shiftRepository.GetByIdAsync(dto.ShiftId.Value);
                if (shift == null)
                {
                    throw new NotFoundException(nameof(shift),dto.ShiftId);
                }
                attendance.Shift = shift;
            }
            if (dto.ClockedIn != null)
            {
                attendance.Clockedin = dto.ClockedIn.Value;
            }
            if (dto.ClockedOut != null)
            {
                attendance.Clockedout = dto.ClockedOut.Value;
            }
            if (dto.attendanceStatus != null)
            {
                attendance.AttendanceStatus = dto.attendanceStatus.Value;
            }
            _attendanceRepository.Update(attendance);
            var isadded = await _attendanceRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("Could not update attendance!");
            }
            await _notificationService.NotifyAsync(attendance.Employee.UserId,
                $"The attendance record of employee with {attendance.Employee.Id} on {attendance.Date:yyyy-MM-dd} Has been changed by {_currentUser.UserId}"
                , NotificationType.AttendanceInfoUpdate,
                attendanceId: attendance.Id);
            return isadded;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var Attendance = await _attendanceRepository.GetByIdAsync(id);
            if (Attendance is null)
            {
                return false;
            }
            _attendanceRepository.Delete(Attendance);
            return await _attendanceRepository.SaveChangesAsync();
        }
        private IEnumerable<Attendance> FilterVisible(IEnumerable<Attendance> attendances)
        {
            if (_currentUser.IsSuperAdmin)
            {
                return attendances;
            }
            return attendances.Where(a => a.EmployeeId == _currentUser.EmployeeId || 
            (_currentUser.CompanyRoles.TryGetValue(a.Department.CompanyId,out var rolevalues) && 
            Enum.TryParse<CompanyRole>(rolevalues,out var role) && role <= CompanyRole.HREmployee)).ToList();
        }
        private DateTime CalculateShiftEnd(Attendance attendance)
        {
            var shiftend = attendance.Date;
            if (attendance.Shift.StartTime > attendance.Shift.EndTime)
            {
                shiftend.Add(attendance.Shift.EndTime);
                shiftend.AddDays(1);
            }
            else
            {
                shiftend.Add(attendance.Shift.EndTime);
            }
            return shiftend;
        }
        private DateTime CalculateShiftStart(Attendance attendance)
        {
            var shiftstart = attendance.Date;
            shiftstart.Add(attendance.Shift.StartTime);
            return shiftstart;
        }
    }
}
