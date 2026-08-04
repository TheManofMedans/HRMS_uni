using AutoMapper;
using HRMS.Application.DTOs.User;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
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
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper,ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
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
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException(nameof(user),id);
            }
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.Phone;
            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync();
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException(nameof(user), id);
            }
            _userRepository.Delete(user);
            return await _userRepository.SaveChangesAsync();
        }
    }
}
