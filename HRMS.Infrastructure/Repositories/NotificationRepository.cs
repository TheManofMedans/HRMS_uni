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
    public class NotificationRepository : INotificationRepository
    {
        private readonly HRMSDbContext _context;
        public NotificationRepository(HRMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.Include(n => n.User)
                .Include(n => n.Attendance).ThenInclude(n => n.Department)
                .ThenInclude(d => d.Company)
                .Include(n => n.Request).ThenInclude(n => n.Department)
                .ThenInclude(d => d.Company)
                .ToListAsync();
        }
        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.Include(n => n.User)
                .Include(n => n.Attendance).ThenInclude(n => n.Department)
                .ThenInclude(d => d.Company)
                .Include(n => n.Request).ThenInclude(n => n.Department)
                .ThenInclude(d => d.Company)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task CreateAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }
        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
        }
        public void Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
