using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Application.DTOs.Department;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;


namespace HRMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IDepartdmentRepository _departdmentRepository;
        public DepartmentService (IMapper mapper, IDepartdmentRepository departdmentRepository)
        {
            _mapper = mapper;
            _departdmentRepository = departdmentRepository;
        }
        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            var department = await _departdmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                throw new NotFoundException("Department is not found!");
            }
            return department is null ? null : _mapper.Map<DepartmentResponseDto?>(department);
        }
        public async Task<IEnumerable<DepartmentResponseDto>> GetByCompanyIdAsync (int CompanyId)
        { 
            var departments = await _departdmentRepository.FindByCompanyIdAsync(CompanyId);
            if (departments == null)
            {
                throw new NotFoundException("No Department is found!");
            }
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
        }
        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
        {
            var department = await _departdmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(department);
        }
        public async Task<DepartmentResponseDto> CreateAsync (CreateDepartmentDto dto)
        {
            var department = _mapper.Map<Department>(dto);
            await _departdmentRepository.AddAsync(department);
            await _departdmentRepository.SaveChangesAsync();
            return _mapper.Map<DepartmentResponseDto>(department);
        }
        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _departdmentRepository.GetByIdAsync (id);
            if (department is null)
            {
                return false;
            }
            department.Name = dto.Name;
            department.Description = dto.Description;
            _departdmentRepository.Update(department);
            return await _departdmentRepository.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _departdmentRepository.GetByIdAsync(id);
            if (department is null)
            {
                return false;
            }
            _departdmentRepository.DeleteAsync(department);
            return await _departdmentRepository.SaveChangesAsync();
        }
    }
}
