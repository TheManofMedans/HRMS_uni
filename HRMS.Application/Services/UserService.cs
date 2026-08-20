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
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper,ICompanyRepository companyRepository,UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _userManager = userManager;
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
            if (dto.NewPassword != null && dto.CurrentPass != null)
            {
               await _userManager.ChangePasswordAsync(user, dto.CurrentPass, dto.NewPassword);
            }
            if (dto.FirstName != null)
            {
                user.FirstName = dto.FirstName;
            }
            if (dto.LastName != null)
            {
                user.LastName = dto.LastName;
            }
            if (dto.Phone != null)
            {
                user.PhoneNumber = dto.Phone;
            }
            if (dto.Gender != null)
            {
                user.Gender = dto.Gender.Value;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Could not update user info!");
            }
            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException(nameof(user), id);
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
