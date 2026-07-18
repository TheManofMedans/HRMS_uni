using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IShiftRepository
    {
        Task<Shift?> GetByIdAsync(int id);
        Task<IEnumerable<Shift>> GetAllAsync();
        Task<IEnumerable<Shift>?> GetByUserIdAsync(int UserId);
        Task AddAsync(Shift shift);
        void  Update(Shift shift);
        void Delete(int id);
        Task<bool> SaveChangesAsync();
    }
}
