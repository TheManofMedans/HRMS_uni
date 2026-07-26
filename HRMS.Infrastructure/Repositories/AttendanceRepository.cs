using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly HRMSDbContext _context;
        public AttendanceRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances.Include(a => a.Employee)
                .Include(a => a.shift).Include(a => a.department).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances.Include(a => a.shift)
                .Include(a => a.department)
                .Include(a => a.Employee).ToListAsync();
        }
        public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int id)
        {
            return await _context.Attendances.Include(a => a.shift)
                .Include(a => a.department)
                .Include(a => a.Employee).Where(a => a.Employee.Id == id)
                .ToListAsync();
        }
        public async Task<IEnumerable<Attendance>> GetByStatusAsync(AttendanceStatus status)
        {
            return await _context.Attendances.Include(a => a.shift)
                .Include(a => a.department)
                .Include(a => a.Employee)
                .Where(a => a.AttendanceStatus == status)
                .ToListAsync();
        }
        public async Task<IEnumerable<Attendance>> GetByEmployeeAndStatusAsync(int id, AttendanceStatus status)
        {
            return await _context.Attendances.Include(a => a.shift)
                .Include(a => a.department)
                .Include(a => a.Employee)
                .Where(a => a.Employee.Id == id && a.AttendanceStatus == status)
                .ToListAsync();
        }
        public async Task AddAsync(Attendance attendance)
        {
            await _context.AddAsync(attendance);
        }
        public void Update(Attendance attendance)
        {
            _context.Update(attendance);
        }
        public void Delete(Attendance attendance)
        {
            _context.Remove(attendance);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
