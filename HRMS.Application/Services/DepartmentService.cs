using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Application.DTOs.Department;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;


namespace HRMS.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departdmentRepository;
        public DepartmentService (IMapper mapper, IDepartmentRepository departdmentRepository,ICurrentUserService currentUser)
        {
            _mapper = mapper;
            _departdmentRepository = departdmentRepository;
            _currentUser = currentUser;
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
            var departments = await _departdmentRepository.GetByCompanyIdAsync(CompanyId);
            if (departments == null)
            {
                throw new NotFoundException("No Department is found!");
            }
            var visible = FilterVisible(departments, CompanyRole.HREmployee);
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(visible);
        }
        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
        {
            var department = await _departdmentRepository.GetAllAsync();
            var visible = FilterVisible(department,CompanyRole.HREmployee);
            return _mapper.Map<IEnumerable<DepartmentResponseDto>>(visible);
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
            if (dto.Name is not null)
            {
                department.Name = dto.Name;
            }
            if (dto.Description is not null)
            {
                department.Description = dto.Description;
            }
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
        private IEnumerable<Department> FilterVisible(IEnumerable<Department> departments, CompanyRole requiredRole)
        {
            if (_currentUser.IsSuperAdmin)
            {
                return departments;
            }
            return departments.Where(d =>d is not null && (_currentUser.CompanyRoles.TryGetValue(d.CompanyId,out var rolevalues) &&
            Enum.TryParse<CompanyRole>(rolevalues, out var role) && role <= requiredRole)).ToList();
        }
    }
}
