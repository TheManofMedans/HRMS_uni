using HRMS.Application.DTOs.Company;
using HRMS.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto?> GetByIdAsync(int id);
        Task<CompanyResponseDto?> GetByRegNumAsync(string RegNum);
        Task<CompanyResponseDto?> GetWithUserAsync(int id);
        Task<IEnumerable<CompanyResponseDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<CompanyResponseDto>> GetAllAsync();
        Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto);
        Task<CompanyResponseDto> AddUsertoCompanyAsync(int CompanyId,int UserId,CompanyRole Role);
        Task<CompanyResponseDto> UpdateUserCompanyAsync(int userId,int companyId,CompanyRole Role);
        Task<CompanyResponseDto> DeleteUserCompanyAsync(int userId, int companyId);
        Task<bool> UpdateAsync(int id,UpdateCompanyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
