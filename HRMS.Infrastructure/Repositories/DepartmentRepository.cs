using HRMS.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace HRMS.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRMSDbContext _context;
        public DepartmentRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.Include(d => d.Company)
                .Include(d => d.DepartmentEmployees)
                .ThenInclude(de => de.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task<IEnumerable<Department>> GetByCompanyIdAsync(int id)
        {
            return await _context.Departments
                .Where(d => d.CompanyId == id)
                .Include(d => d.Company)
                .Include(d => d.DepartmentEmployees)
                .ThenInclude(de => de.Employee)
                .ToListAsync();
        }
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.DepartmentEmployees)
                .ThenInclude(de => de.Employee)
                .Include(d => d.Company)
                .ToListAsync();
        }
        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }
        public void Update(Department department)
        {
            _context.Departments.Update(department);
        }
        public void DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
