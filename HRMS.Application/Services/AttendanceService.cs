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

namespace HRMS.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IDepartdmentRepository _departmentRepository;
        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository, IDepartdmentRepository departdmentRepository)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _departmentRepository = departdmentRepository;
        }
        public async Task<AttendanceResponseDto?> GetByIdAsync(int id)
        {
            var Attendance = await _attendanceRepository.GetByIdAsync(id);
            return Attendance is null ? null : _mapper.Map<AttendanceResponseDto>(Attendance);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetAllAsync()
        {
            var Attendances = await _attendanceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(Attendances);
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
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(Attendances);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetByStatusAsync(AttendanceStatus status)
        {
            var Attendances = await _attendanceRepository.GetByStatusAsync(status);
            if (Attendances == null)
            {
                throw new NotFoundException("Attendance Record is not found!");
            }
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(Attendances);
        }
        public async Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeAndStatusAsync(int id, AttendanceStatus status)
        {
            var Employee = await _employeeRepository.GetbyIdAsync(id);
            if (Employee is null)
            {
                throw new NotFoundException("Employee is not found!");
            }
            var Attendances = await _attendanceRepository.GetByEmployeeAndStatusAsync(Employee.Id, status);
            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(Attendances); 
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
            await _attendanceRepository.AddAsync(Attendance);
            var isadded = await _attendanceRepository.SaveChangesAsync();
            if (!isadded)
            {
                throw new Exception("The Create action hasnt been successful!");
            }
            return _mapper.Map<AttendanceResponseDto>(Attendance);
        }
        public async Task<bool> UpdateAsync(int id, UpdateAttendanceDto dto)
        {
            var Attendance = await _attendanceRepository.GetByIdAsync(id);
            if (Attendance is null)
            {
                return false;
            }
            if (dto.ClockedIn != null)
            {
                Attendance.Clockedin = dto.ClockedIn;
            }
            if (dto.ClockedOut != null)
            {
                Attendance.Clockedout = dto.ClockedOut;
            }
            Attendance.AttendanceStatus = dto.AttendanceStatus;
            _attendanceRepository.Update(Attendance);
            return await _attendanceRepository.SaveChangesAsync();
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
    }
}
