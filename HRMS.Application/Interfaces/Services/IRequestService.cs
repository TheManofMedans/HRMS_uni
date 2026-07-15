using HRMS.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<RequestResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<RequestResponseDto>> GetAllAsync();
        Task<IEnumerable<RequestResponseDto>?> GetByEmployeeIdAsync(int Employeeid);
        Task<IEnumerable<RequestResponseDto>?> GetWithStatusAsync(int status);
        Task<IEnumerable<RequestResponseDto>?> GetWithTypeAsync(int type);
        Task<IEnumerable<RequestResponseDto>?> GetWithCustomDataAsync(int? EmployeeId, int? Status, int? Type);
        Task<RequestResponseDto> CreateAsync(CreateRequestDto dto);
        Task<bool> UpdateAsync(int id,UpdateRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
