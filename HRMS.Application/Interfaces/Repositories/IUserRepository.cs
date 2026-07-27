using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByIdWithCompanyAsync(int id);
        Task<User?> GetByIdWithEverythingAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> SSNExistsAsync(string SSN);
        Task<bool> SaveChangesAsync();

    }
}
