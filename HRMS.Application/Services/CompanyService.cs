using HRMS.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.domain.Entities;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using AutoMapper;
using HRMS.Application.DTOs.Company;
using HRMS.domain.Enums;
using HRMS.Application.Exceptions;

namespace HRMS.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        public CompanyService (IMapper mapper,ICompanyRepository companyRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }
        public async Task<CompanyResponseDto?> GetByIdAsync(int id)
        {
            var Company = await _companyRepository.GetByIdAsync(id);
            return Company is null ? null : _mapper.Map<CompanyResponseDto?>(Company);
        }
        public async Task<CompanyResponseDto?> GetByRegNumAsync(string RegNum)
        {
            var company = await _companyRepository.GetByRegNumAsync(RegNum);
            return company is null ? null : _mapper.Map<CompanyResponseDto>(company);
        }
        public async Task<CompanyResponseDto?> GetWithUserAsync(int UserId)
        {
            var user = await _userRepository.GetByIdAsync(UserId);
            if (user is null)
            {
                throw new NotFoundException("User is not found!");
            }
            var Company = await _companyRepository.GetWithUserAsync(UserId);
            return _mapper.Map<CompanyResponseDto>(Company);
        }
        public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyResponseDto>>(companies);
        }
        public async Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto)
        {
            var Company = _mapper.Map<Company>(dto);
            var User = await _userRepository.GetByIdAsync(dto.UserId);
            if (User is null)
            {
                throw new NotFoundException("User is not found!");
            }
            Company.UserCompanies.Add(new UserCompany
            {
                user = User,
                company = Company,
                Role = dto.Role,
            });
           await _companyRepository.AddAsync(Company);
            bool isAdded = await _companyRepository.SaveChangesAsync();
            if (!isAdded)
            {

            }
            return _mapper.Map<CompanyResponseDto>(Company);
        }
        public async Task<bool> UpdateAsync(int id,UpdateCompanyDto dto)
        {
            var Company = await _companyRepository.GetByIdAsync(id);
            if (Company is null)
            {
                throw new NotFoundException("Company is not found!");
            }
            Company.Address = dto.Address;
            _companyRepository.Update(Company);
            return await _companyRepository.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var Company = await _companyRepository.GetByIdAsync(id);
            if (Company is null) 
            {
                throw new NotFoundException("Company is not found!");
            }
            _companyRepository.Delete(id);
            return await _companyRepository.SaveChangesAsync();
        }
        public async Task<CompanyResponseDto> AddUsertoCompanyAsync(int CompanyId, int UserId,CompanyRole Role)
        {
            var _Company = await _companyRepository.GetByIdAsync(CompanyId);
            if (_Company is null)
            {
                throw new NotFoundException("Company is not found!");
            }
            var User = await  _userRepository.GetByIdAsync(UserId);
            if (User is null)
            {
                throw new NotFoundException("User is not found!");
            }
            if (_Company.UserCompanies.Any(uc => uc.UserId == User.Id))
            {
                throw new ConflictException("The User is already a member of the company");
            }
            _Company.UserCompanies.Add(new UserCompany
            {
                company = _Company,
                user = User,
                Role = Role,

            });
            _companyRepository.Update(_Company);
            bool isAdded = await _companyRepository.SaveChangesAsync();
            if (!isAdded)
            {
                throw new Exception("Failed to add user to the company");
            }
            return _mapper.Map<CompanyResponseDto>(_Company);
        }
    }
}
