using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class PaySlipRepository : IPaySlipRepository
    {
        private readonly HRMSDbContext _context;
        public PaySlipRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<PaySlip?> GetByIdAsync(int id)
        {
            return await _context.PaySlips.Include(p => p.Employee).ThenInclude(e => e.EmployeeDepartments)
                .Include(p => p.Department).ThenInclude(d => d.Company)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<PaySlip>> GetAllAsync()
        {
            return await _context.PaySlips.Include(p => p.Employee).ThenInclude(e => e.EmployeeDepartments)
                .Include(p => p.Department).ThenInclude(d => d.Company).ToListAsync();
        }
        public async Task<IEnumerable<PaySlip>> GetByDateAsync(DateTime? startWeek = null, DateTime? endWeek = null, int? companyId = null, int? departmentId = null)
        {
            var query = _context.PaySlips.AsQueryable();
            if (startWeek != null)
            {
                query = query.Where(p => p.WeekStart >= startWeek);
            }
            if (endWeek != null)
            {
                query = query.Where(p => p.WeekEnd <= endWeek);
            }
            if (companyId.HasValue)
            {
                query = query.Where(p => p.Department.CompanyId == companyId);
            }
            if (departmentId.HasValue)
            {
                query = query.Where(p => p.DepartmentId == departmentId);
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<PaySlip>> GetByEmployeeIdAsync(int id, DateTime? startWeek= null, DateTime? endWeek = null)
        {
            var query = _context.PaySlips.AsQueryable<PaySlip>();
            query = query.Where(p => p.EmployeeId == id);
            if (startWeek != null)
            {
                query = query.Where(p => p.WeekStart >= startWeek);
            }
            if (endWeek != null)
            {
                query = query.Where(p => p.WeekEnd <= endWeek);
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<PaySlip>> GetByEmployeeIdAndWeekAsync(int employeeId,DateTime startWeek)
        {
            return await _context.PaySlips.Include(p => p.Employee).ThenInclude(e => e.EmployeeDepartments)
                .Include(p => p.Department).ThenInclude(d => d.Company).Where(p => p.EmployeeId ==  employeeId && p.WeekStart == startWeek)
                .ToListAsync();
        }
        public async Task<IEnumerable<PaySlip>> GetByEmployeeAndDepartmentAsync(int employeeId,int departmentId)
        {
            return await _context.PaySlips.Include(p => p.Employee).ThenInclude(e => e.EmployeeDepartments)
                .Include(p => p.Department).ThenInclude(d => d.Company).Where(p => p.EmployeeId == employeeId && p.DepartmentId == departmentId)
                .ToListAsync();
        }
        public async Task<IEnumerable<PaySlip>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.PaySlips.Include(p => p.Employee).ThenInclude(e => e.EmployeeDepartments)
                .Include(p => p.Department).ThenInclude(d => d.Company).Where(p => p.DepartmentId == departmentId)
                .ToListAsync();
        }
        public async Task<bool> PaySlipExistsForWeekAsync(int employeeId,int departmentId,DateTime weekStart)
        {
            return await _context.PaySlips.AnyAsync(p => p.EmployeeId == employeeId && p.DepartmentId == departmentId && p.WeekStart == weekStart);
        }
        public async Task AddAsync(PaySlip paySlip)
        {
            await _context.PaySlips.AddAsync(paySlip);
        }
        public void Update(PaySlip paySlip)
        {
            _context.PaySlips.Update(paySlip);
        }
        public void Delete(PaySlip paySlip)
        {
            _context.PaySlips.Remove(paySlip);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
