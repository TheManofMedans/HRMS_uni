using AutoMapper;
using HRMS.Application.DTOs.Shift;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IMapper _mapper;
        private readonly IShiftRepository _shiftRepository;
        private readonly ICompanyRepository _companyRepository;
        public ShiftService(Mapper mapper, IShiftRepository shiftRepository, ICompanyRepository companyRepository)
        {
            _mapper = mapper;
           _shiftRepository = shiftRepository;
            _companyRepository = companyRepository;
        }

        public async Task<ShiftResponseDto?> GetByIdAsync(int id)
        {
            var Shift = await _shiftRepository.GetByIdAsync(id);
            return Shift is null ? null : _mapper.Map<ShiftResponseDto>(Shift);
        }
        public async Task <IEnumerable<ShiftResponseDto>> GetAllAsync()
        {
            var Shifts = await  _shiftRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShiftResponseDto>>(Shifts);
        }
        public async Task<IEnumerable<ShiftResponseDto>?> GetByCompanyIdAsync(int CompanyId)
        {
            var Company = await _companyRepository.GetByIdAsync(CompanyId);
            if (Company == null)
            {

            }
            var Shifts = await _shiftRepository.GetByCompanyIdAsync(CompanyId);
            if (Shifts == null)
            {

            }
            return _mapper.Map<IEnumerable<ShiftResponseDto>>(Shifts);
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

            }
            return _mapper.Map<ShiftResponseDto>(Shift);
        }
        public async Task<bool> UpdateAsync(int id, UpdateShiftDto dto)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null)
            {

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

            }
            _shiftRepository.Delete(shift);
            return await _shiftRepository.SaveChangesAsync();
        }
    }
}
