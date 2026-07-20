using HRMS.Application.DTOs.Attendance;
using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceResponseDto>> GetAllAsync();
        Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeIdAsync(int EmployeeId);
        Task<IEnumerable<AttendanceResponseDto>> GetByStatusAsync(AttendanceStatus status);
        Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeAndStatusAsync(int EmployeeId, AttendanceStatus status);
        Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto);
        Task<bool> UpdateAsync(int id,UpdateAttendanceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
