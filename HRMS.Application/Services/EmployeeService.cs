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
using HRMS.Application.Exceptions;

namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartdmentRepository _departdmentRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository,IDepartdmentRepository departdmentRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departdmentRepository = departdmentRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto)
        {
            if (await _employeeRepository.EmailExistsAsync(dto.Email))
            {
                throw new RepeatDataException("Email",dto.Email);
            }
            if (await _employeeRepository.SSNExistsAsync(dto.SSN))
            {
                throw new RepeatDataException("SSN", dto.SSN);
            }
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);
           var saved = await _employeeRepository.SaveChangesAsync();
            if (!saved)
            {
                throw new Exception("Failed to save new employee!");
            }
            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<bool> AddToDepartmentAsync(int EmployeeId,int DepartmentId)
        {
            var Employee = await _employeeRepository.GetByIdWithDepartmentsAsync(EmployeeId);
            if (Employee == null)
            {
                throw new NotFoundException(nameof(Employee),EmployeeId);
            }
            var department = await _departdmentRepository.GetByIdAsync(DepartmentId);
            if (department == null)
            {
                throw new NotFoundException(nameof (Department),DepartmentId);
            }
            if (Employee.EmployeeDepartments.Any(uc => uc.DepartmentID == DepartmentId))
            {
                throw new ConflictException($"The employee {Employee.Id} already is in this department!");
            }
            Employee.EmployeeDepartments.Add(new EmployeeDepartment
            {
                EmployeeID = Employee.Id,
                DepartmentID = DepartmentId,
                AssignedAt = DateTime.UtcNow,
                IsPrimary = false
            }); 
            _employeeRepository.Update(Employee);
            return await _employeeRepository.SaveChangesAsync();
        }
        public async Task<bool> SetPrimary(int EmployeeId,int DepartmentId)
        {
            var employee = await _employeeRepository.GetByIdWithDepartmentsAsync(EmployeeId);
            if (employee is null)
            {
                throw new NotFoundException(nameof(employee), EmployeeId);
            }
            var department = await _departdmentRepository.GetByIdAsync (DepartmentId);
            if (department is null)
            {
                throw new NotFoundException(nameof(Department), DepartmentId);
            }
            if (!employee.EmployeeDepartments.Any(ed => ed.DepartmentID == DepartmentId))
            {
                throw new ConflictException($"The employee {EmployeeId} already prime is in this department!");
            }
            var ED = employee.EmployeeDepartments.FirstOrDefault(ed => ed.DepartmentID == DepartmentId);
            if (ED is null)
            {
                throw new NotFoundException("This user is not in the selected department!");
            }
            ED.IsPrimary = true;
            _employeeRepository.Update(employee);
            return await _employeeRepository.SaveChangesAsync();
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
                throw new NotFoundException(nameof(employee),id);
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
            {
                throw new NotFoundException(nameof(employee),id);
            }
            _employeeRepository.Delete(employee);
            return await _employeeRepository.SaveChangesAsync();
        }
    }
}
