using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using AutoMapper;
using HRMS.domain.Entities;

namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto)
        {
            if (await _employeeRepository.EmailExistsAsync(dto.Email))
            {

            }
            if (await _employeeRepository.SSNExistsAsync(dto.SSN))
            {

            }
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();
            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetbyIdAsync(id);
           return employee is null ? null : _mapper.Map<EmployeeResponseDto?>(employee);
        }
        public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
        {
            var employeelist = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employeelist);
        }
        public async Task<bool> UpdateAsync (int id,UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetbyIdAsync (id);
            if (employee is null)
            {
                return false;
            }
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Phone = dto.Phone;
            employee.Address = dto.Address;
            _employeeRepository.Update(employee);
            return await _employeeRepository.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync (int id)
        {
            var employee = await _employeeRepository.GetbyIdAsync (id);
            if (employee is null)
            {  return false; }
            _employeeRepository.Delete(employee);
            return await _employeeRepository.SaveChangesAsync();
        }
    }
}
