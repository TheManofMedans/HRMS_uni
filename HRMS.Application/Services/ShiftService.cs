using AutoMapper;
using HRMS.Application.DTOs.Shift;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly IShiftRepository _shiftRepository;
        private readonly ICompanyRepository _companyRepository;
        public ShiftService(IMapper mapper, IShiftRepository shiftRepository, ICompanyRepository companyRepository,ICurrentUserService currentUser)
        {
            _mapper = mapper;
           _shiftRepository = shiftRepository;
            _companyRepository = companyRepository;
            _currentUser = currentUser;
        }

        public async Task<ShiftResponseDto?> GetByIdAsync(int id)
        {
            var Shift = await _shiftRepository.GetByIdAsync(id);
            return Shift is null ? null : _mapper.Map<ShiftResponseDto>(Shift);
        }
        public async Task <IEnumerable<ShiftResponseDto>> GetAllAsync()
        {
            var Shifts = await  _shiftRepository.GetAllAsync();
            var visible = FilterVisible(Shifts);
            return _mapper.Map<IEnumerable<ShiftResponseDto>>(visible);
        }
        public async Task<IEnumerable<ShiftResponseDto>> GetByCompanyIdAsync(int CompanyId)
        {
            var Company = await _companyRepository.GetByIdAsync(CompanyId);
            if (Company == null)
            {
                throw new NotFoundException(nameof(Company),CompanyId);
            }
            var Shifts = await _shiftRepository.GetByCompanyIdAsync(CompanyId);
            var visible = FilterVisible(Shifts);
            return _mapper.Map<IEnumerable<ShiftResponseDto>>(visible);
        }
        public async Task<ShiftResponseDto> CreateAsync(CreateShiftDto dto)
        {
            var Shifts = await _shiftRepository.GetByCompanyIdAsync(dto.CompanyId);
            bool AlreadyExists = Shifts is not null &&
                Shifts.Any(s => s.StartTime ==  dto.StartTime && s.EndTime == dto.EndTime);
            if (AlreadyExists)
            {
                throw new ConflictException("A Shift with the same start and end times already exists for this company!");
            }
            var Shift = _mapper.Map<Shift>(dto);
            Shift.CompanyId = dto.CompanyId;
            await _shiftRepository.AddAsync(Shift);
            var isdone = await _shiftRepository.SaveChangesAsync();
            if (!isdone)
            {
                throw new Exception("Could not Save the shift!");
            }
            return _mapper.Map<ShiftResponseDto>(Shift);
        }
        public async Task<bool> UpdateAsync(int id, UpdateShiftDto dto)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null)
            {
                throw new NotFoundException(nameof(Shift),id);
            }
            shift.ShiftName = dto.ShiftName;
            shift.StartTime = dto.StartTime;
            shift.EndTime = dto.EndTime;
            _shiftRepository.Update(shift);
            return await _shiftRepository.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null)
            {
                throw new NotFoundException(nameof(Shift),id);
            }
            _shiftRepository.Delete(shift);
            return await _shiftRepository.SaveChangesAsync();
        }
        private IEnumerable<Shift> FilterVisible(IEnumerable<Shift> shifts)
        {
            if (_currentUser.IsSuperAdmin)
            {
                return shifts;
            }
            return shifts.Where(s => s is not null && (_currentUser.CompanyRoles.TryGetValue(s.CompanyId, out var rolevalue)
            && Enum.TryParse<CompanyRole>(rolevalue,out var role) && role <= CompanyRole.HREmployee)).ToList();
        }
    }
}
