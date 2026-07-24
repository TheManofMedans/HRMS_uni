using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;


namespace HRMS.Application.Interfaces.Repositories
{
    public interface IDepartdmentRepository
    {
        Task<Department?> GetByIdAsync(int id);
        Task<IEnumerable<Department>> FindByCompanyIdAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task AddAsync(Department department);
        void Update(Department department);
        void DeleteAsync(Department department);
        Task<bool> SaveChangesAsync();
    }
}
