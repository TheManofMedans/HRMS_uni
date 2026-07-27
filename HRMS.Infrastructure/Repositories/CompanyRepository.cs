using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace HRMS.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly HRMSDbContext _context;
        public CompanyRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }
        public async Task<Company?> GetByRegNumAsync(string RegNum)
        {
            return await _context.Companies
                .Include (c => c.UserCompanies)
                .ThenInclude (uc => uc.user).FirstOrDefaultAsync(c => c.RegNum == RegNum);
        }
        public async Task<Company?> GetWithUserAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.UserCompanies)
                .ThenInclude(uc => uc.user)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Company>> GetByUserIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.UserCompanies)
                .ThenInclude(uc => uc.user)
                .Where(c => c.UserCompanies.Where(uc => uc.UserId == id).Any()).
                ToListAsync();
        }
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies
                .Include(c => c.UserCompanies)
                .ThenInclude(uc => uc.user)
                .ToListAsync();
        }
        public async Task AddAsync (Company company)
        {
            await _context.Companies.AddAsync(company);
        }
        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }
        public void Delete(Company company)
        {
            _context.Companies.Remove(company);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
