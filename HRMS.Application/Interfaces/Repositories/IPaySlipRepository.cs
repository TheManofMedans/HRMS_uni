using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IPaySlipRepository
    {
        Task<PaySlip?> GetByIdAsync(int id);
        Task<IEnumerable<PaySlip>> GetAllAsync();
        Task<IEnumerable<PaySlip>> GetByDateAsync(DateTime? startDate = null, DateTime? endDate = null, int? companyId = null,int? departmentId = null);
        Task<IEnumerable<PaySlip>> GetByEmployeeIdAsync(int id,DateTime? startDate= null,DateTime? endDate = null);
        Task<IEnumerable<PaySlip>> GetByEmployeeIdAndWeekAsync(int id, DateTime startDate);
        Task<IEnumerable<PaySlip>> GetByEmployeeAndDepartmentAsync(int employeeId,int departmentId);
        Task<IEnumerable<PaySlip>> GetByDepartmentIdAsync(int departmentId);
        Task<bool> PaySlipExistsForWeekAsync(int employeeId, int departmentId, DateTime startWeek);
        Task AddAsync(PaySlip paySlip);
        void Update(PaySlip paySlip);
        void Delete(PaySlip paySlip);
        Task<bool> SaveChangesAsync();
    }
}
