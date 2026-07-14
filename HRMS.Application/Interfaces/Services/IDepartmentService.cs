using HRMS.Application.DTOs.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<DepartmentResponseDto>> GetByCompanyIdAsync(int CompanyId);
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
        Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto);
        Task<bool> UpdateAsync(int id,UpdateDepartmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
