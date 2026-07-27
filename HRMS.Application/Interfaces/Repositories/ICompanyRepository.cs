using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(int id);
        Task<Company?> GetByRegNumAsync(string RegNum);
        Task<Company?> GetWithUserAsync(int id);
        Task<IEnumerable<Company>> GetAllAsync();
        Task<IEnumerable<Company>> GetByUserIdAsync(int userId);
        Task AddAsync(Company company);
        void Update(Company company);
        void Delete(Company company);
        Task <bool> SaveChangesAsync();
    }
}
