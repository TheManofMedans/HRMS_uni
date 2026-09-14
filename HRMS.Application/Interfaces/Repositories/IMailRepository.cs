using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IMailRepository
    {
        Task<MailThread?> GetByIdAsync(int Id);
        Task<IEnumerable<MailThread>> GetForUserAsync(int UserId);
        Task AddThreadAsync(MailThread thread);
        Task<bool> SaveChangesAsync();
    }
}
