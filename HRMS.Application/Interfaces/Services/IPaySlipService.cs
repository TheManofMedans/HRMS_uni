using HRMS.Application.DTOs.PaySlip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IPaySlipService
    {
        Task<PaySlipResponseDto> GenerateForWeekAsync(int employeeId,int departmentId,DateTime weekStart);
        Task<IEnumerable<CompanyPayrollSummaryDto>> GetCompanyPayrollSummaryAsync(int employeeId,DateTime weekStart);
        Task<PaySlipResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<PaySlipResponseDto>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<PaySlipResponseDto>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<PaySlipResponseDto>> SearchAsync(DateTime? weekStart,DateTime? weekEnd,int? companyId,int? departmentId);
        Task<IEnumerable<PaySlipResponseDto>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
