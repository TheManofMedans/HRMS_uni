using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.Application.Interfaces.Repositories;
using HRMS.domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly HRMSDbContext _context;
        public RequestRepository(HRMSDbContext context)
        {
             _context = context;
        }
        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _context.Requests
                .Include(c => c.Employee)
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Request>> GetAllAsync()
        {
            return await _context.Requests
                .Include(c => c.Employee)
                .Include (c => c.Department)
                .ToListAsync();
        }
        public async Task<IEnumerable<Request>> GetByEmployeeIdAsync(int id)
        {
            return await _context.Requests
                .Include(c => c.Employee)
                .Include(c => c.Department)
                .Where(r => r.EmployeeId == id)
                .ToListAsync();
        }
        public async Task<IEnumerable<Request>> GetWithStatusAsync(RequestStatus status)
        {
            return await _context.Requests
                .Include(r => r.Employee)
                .Include(r => r.Department)
                .Where(r => r.Status == status)
                .ToListAsync();
        }
        public async Task<IEnumerable<Request>> GetWithTypeAsync(RequestType type)
        {
            return await _context.Requests
                .Include(r => r.Employee)
                .Include(r => r.Department)
                .Where(r => r.Type == type)
                .ToListAsync();
        }
        public async Task<IEnumerable<Request>> GetWithCustomDataAsync(int? id, RequestStatus? status, RequestType? type)
        {
            var query = _context.Requests
                .Include(r => r.Employee)
                .Include(r => r.Department)
                .AsQueryable();
            if (id.HasValue)
            {
                query.Where(r => r.EmployeeId == id);
            }
            if (status.HasValue)
            {
                query.Where (r => r.Status == status);
            }
            if (type.HasValue)
            {
                query.Where(r => r.Type == type);
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Request>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Requests.Include(r => r.Employee)
                .ThenInclude(e => e.EmployeeDepartments)
                .ThenInclude(ed => ed.Department)
                .ThenInclude(d => d.Company)
                .Where(r => r.Employee.EmployeeDepartments.Any(ed => ed.Department.CompanyId == companyId))
                .ToListAsync();
        }
        public async Task<IEnumerable<Request>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Requests.Include(r => r.Department)
                .Include(r => r.Employee)
                .ThenInclude (e => e.EmployeeDepartments)
                .Where(r => r.Employee.EmployeeDepartments.Any(ed => ed.DepartmentID == departmentId))
                .ToListAsync();
        }
        public async Task AddAsync(Request request)
        {
            await _context.Requests.AddAsync(request);
        }
        public void Update(Request request) 
        {
            _context.Requests.Update(request);
        }
        public void Delete(Request request)
        {
            _context.Requests.Remove(request);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
