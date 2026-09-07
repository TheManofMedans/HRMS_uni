using HRMS.Application.DTOs.Attendance;
using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int id);
        Task CreateAsync(Notification notification);
        void Update(Notification notification);
        void Delete(Notification notification);
        Task<bool> SaveChangesAsync();
    }
}
