using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using HRMS.Infrastructure.Persistence;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HRMS.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly HRMSDbContext _context;
        public ShiftRepository (HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<Shift?> GetByIdAsync(int id)
        {
            return await _context.Shifts
                .Include(s => s.Company)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IEnumerable<Shift>> GetAllAsync()
        {
            return await _context.Shifts.Include(s => s.Company)
                .ToListAsync();
        }
        public async Task<IEnumerable<Shift>> GetByCompanyIdAsync(int id)
        {
            return await _context.Shifts
                .Include(s => s.Company)
                .Where(s => s.CompanyId == id)
                .ToListAsync();
        }
        public async Task AddAsync(Shift shift)
        {
            await _context.Shifts.AddAsync(shift);
        }
        public void Update(Shift shift) 
        {
            _context.Shifts.Update(shift);
        }
        public void Delete(Shift shift) 
        {
            _context?.Shifts.Remove(shift);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
