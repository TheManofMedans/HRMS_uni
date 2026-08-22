using AutoMapper;
using HRMS.Application.DTOs.User;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper,ICompanyRepository companyRepository
            , UserManager<User> userManager, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _userManager = userManager;
            _employeeRepository = employeeRepository;
        }
        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdWithEverythingAsync(id);
            return user is null ? null : _mapper.Map<UserResponseDto>(user);
        }
        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var Users = await _userRepository.GetAllAsync();
            return _mapper.Map <IEnumerable< UserResponseDto >> (Users);
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new NotFoundException(nameof(user),id);
            }
            var employee = await _employeeRepository.GetByUserIdAsync(user.Id);
            if (dto.NewPassword != null && dto.CurrentPass != null)
            {
               await _userManager.ChangePasswordAsync(user, dto.CurrentPass, dto.NewPassword);
            }
            if (dto.FirstName != null)
            {
                user.FirstName = dto.FirstName;
                if (employee != null)
                {
                    employee.FirstName = dto.FirstName;
                }
            }
            if (dto.LastName != null)
            {
                user.LastName = dto.LastName;
                if (employee != null)
                {
                    employee.LastName = dto.LastName;
                }
            }
            if (dto.Phone != null)
            {
                user.PhoneNumber = dto.Phone;
                if (employee != null)
                {
                    employee.Phone = dto.Phone;
                }
            }
            if (dto.Gender != null)
            {
                user.Gender = dto.Gender.Value;
                if (employee != null)
                {
                    employee.Gender = dto.Gender.Value;
                }
            }
            if (dto.SSN != null)
            {
                user.SSN = dto.SSN;
                if (employee != null)
                {
                    employee.SSN = dto.SSN;
                }
            }
            if (employee == null)
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("Could not update user info!");
                }
            }
            else
            {
                var result = await _userManager.UpdateAsync(user);
                _employeeRepository.Update(employee);
                var empresult = await _employeeRepository.SaveChangesAsync();
                if (!result.Succeeded || !empresult)
                {
                    throw new Exception("Error while changing the employee and user!");
                }
            }
          
            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new NotFoundException(nameof(user), id);
            }
            var employee = await _employeeRepository.GetByUserIdAsync(user.Id);
            if (employee is not null )
            {
                _employeeRepository.Delete(employee);
                var isdeleted = await _employeeRepository.SaveChangesAsync();
                if (!isdeleted)
                {
                    throw new Exception("Error while deleting the employee involved with the user!");
                }
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Couldnt delete the user!");
            }
            return true;
        }
    }
}
