using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.domain.Enums;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<IEnumerable<Attendance>?> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Attendance>?> GetByStatusAsync(AttendanceStatus status);
        Task<IEnumerable<Attendance>?> GetByEmployeeAndStatusAsync(int employeeId, AttendanceStatus status);
        Task AddAsync(Attendance attendance);
        void Update(Attendance attendance);
        void Delete(int id);
        Task<bool> SaveChangesAsync();
    }
}
