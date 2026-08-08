using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _context;
        public EmployeeRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<Employee?> GetbyIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.EmployeeDepartments)
                .ThenInclude(ed => ed.Department).ToListAsync();
        }
        public async Task<Employee?> GetByIdWithDepartmentsAsync(int id)
        {
            return await _context.Employees.
                Include(e => e.EmployeeDepartments).
                ThenInclude(ed => ed.Department).
                FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Employee?> GetByUserIdAsync(int userId)
        {
            return await _context.Employees.Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }
        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }
        public void Update(Employee employee) 
        {
            _context.Employees.Update(employee);
        }
        public void Delete(Employee employee)
        {
            _context.Remove(employee);
        }
        public async Task<bool> EmailExistsAsync(string email) 
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }
        public async Task<bool> SSNExistsAsync(string SSN)
        {
            return await _context.Employees.AnyAsync(e => e.SSN == SSN);
        }
        public async Task<bool> UserIdExistsAsync(int userId)
        {
            return await _context.Employees.AnyAsync(e => e.UserId == userId);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
