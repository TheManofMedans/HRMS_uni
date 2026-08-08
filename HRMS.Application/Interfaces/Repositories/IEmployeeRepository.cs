using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetbyIdAsync(int id);
        Task<Employee?> GetByIdWithDepartmentsAsync(int Id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<bool> UserIdExistsAsync(int userId);
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        Task<bool> SSNExistsAsync(string SSN);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> SaveChangesAsync();
    }
}
