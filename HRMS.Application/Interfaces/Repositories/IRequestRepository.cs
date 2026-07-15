using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IRequestRepository
    {
        Task<Request> GetByIdAsync(int id);
        Task<IEnumerable<Request>> GetAllAsync();
        Task<IEnumerable<Request>?> GetByEmployeeIdAsync(int id);
        Task<IEnumerable<Request>?> GetWithStatusAsync(int status);
        Task<IEnumerable<Request>?> GetWithTypeAsync(int type);
        Task<IEnumerable<Request>?> GetWithCustomDataAsync(int? EmployeeId,int? status,int? type);
        Task AddAsync(Request request);
        void Update(Request request);
        void Delete(int id);
        Task<bool> SaveChangesAsync();
    }
}
