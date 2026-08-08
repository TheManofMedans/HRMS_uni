using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.DTOs.Employee;

namespace HRMS.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeResponseDto>> GetAllAsync();
        Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto);
        Task<EmployeeResponseDto> RegisterEmployeeAsync(RegisterEmployeeDto dto);
        Task<bool> AddToDepartmentAsync(int EmployeeId,int DepartmentId);
        Task<bool> SetPrimary(int EmployeeId, int DepartmentId);
        Task<bool> UpdateAsync(int id,UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
