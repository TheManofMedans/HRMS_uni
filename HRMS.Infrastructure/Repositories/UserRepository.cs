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
    public class UserRepository : IUserRepository
    {
        private readonly HRMSDbContext _context;
        public UserRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include (u => u.UserCompanies)
                .ToListAsync();
        }
        public async Task<User?> GetByIdWithCompanyAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserCompanies)
                .ThenInclude(uc => uc.Company)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User?> GetByIdWithEverythingAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserCompanies)
                .FirstOrDefaultAsync (u => u.Id == id);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> SSNExistsAsync(string SSN)
        {
            return await _context.Users.AnyAsync(u => u.SSN == SSN);
        }
        public async Task AddAsync (User user)
        {
            await _context.Users.AddAsync(user);
        }
        public void Update (User user)
        {
            _context.Users.Update(user);
        }
        public void Delete (User user) 
        {
            _context.Users.Remove(user);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
