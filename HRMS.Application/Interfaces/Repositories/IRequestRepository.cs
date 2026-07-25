using HRMS.domain.Entities;
using HRMS.domain.Enums;
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
        Task<IEnumerable<Request>?> GetWithStatusAsync(RequestStatus status);
        Task<IEnumerable<Request>?> GetWithTypeAsync(RequestType type);
        Task<IEnumerable<Request>?> GetWithCustomDataAsync(int? EmployeeId,RequestStatus? status,RequestType? type);
        Task AddAsync(Request request);
        void Update(Request request);
        void Delete(int id);
        Task<bool> SaveChangesAsync();
    }
}
