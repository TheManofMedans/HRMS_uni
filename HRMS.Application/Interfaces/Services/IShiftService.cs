using HRMS.Application.DTOs.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IShiftService
    {
        Task<ShiftResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<ShiftResponseDto>> GetAllAsync();
        Task<IEnumerable<ShiftResponseDto>?> GetByCompanyIdAsync(int CompanyId);
        Task<ShiftResponseDto> CreateAsync(CreateShiftDto dto);
        Task<bool> UpdateAsync(int id,UpdateShiftDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
