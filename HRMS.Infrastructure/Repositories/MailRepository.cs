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
    public class MailRepository : IMailRepository
    {
        private readonly HRMSDbContext _context;
        public MailRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<MailThread?> GetByIdAsync(int Id)
        {
            return await _context.MailThreads.Include(mt => mt.CreatedByUser)
                .Include(mt => mt.Company).Include(mt => mt.Participants)
                .Include(mt => mt.Messages).Include(mt => mt.RelatedRequest)
                .FirstOrDefaultAsync(mt => mt.Id == Id);
        }
        public async Task<IEnumerable<MailThread>> GetForUserAsync(int userId)
        {
            return await _context.MailThreads.Include(mt => mt.CreatedByUser)
                .Include(mt => mt.Company).Include(mt => mt.Participants)
                .Include(mt => mt.Messages).Include(mt => mt.RelatedRequest)
                .Where(mt => mt.Participants.Any(p => p.UserId == userId)).ToListAsync();
        }
        public async Task AddThreadAsync(MailThread thread)
        {
            await _context.MailThreads.AddAsync(thread);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
