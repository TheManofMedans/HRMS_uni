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
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        public CompanyService (IMapper mapper,ICompanyRepository companyRepository, IUserRepository userRepository,ICurrentUserService currentUser,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _userManager = userManager;
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
        public async Task<CompanyResponseDto?> GetWithUserAsync(int Id)
        {
            var Company = await _companyRepository.GetWithUserAsync(Id);
            return _mapper.Map<CompanyResponseDto>(Company);
        }
        public async Task<IEnumerable<CompanyResponseDto>> GetByUserIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }
            var Companies = await _companyRepository.GetByUserIdAsync(userId);
            var visible = FilterVisible(Companies, CompanyRole.HREmployee);
            return _mapper.Map<IEnumerable<CompanyResponseDto>>(visible);
        }
        public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            var visible = FilterVisible(companies,CompanyRole.HREmployee);
            return _mapper.Map<IEnumerable<CompanyResponseDto>>(visible);
        }
        public async Task<CompanyResponseDto> CreateAsync(CreateCompanyDto dto)
        {
            var Company = _mapper.Map<Company>(dto);
            var User = await _userRepository.GetByIdAsync(dto.UserId);
            if (User is null)
            {
                throw new NotFoundException("User is not found!");
            }
            if (await _companyRepository.RegNumExistsAsync(dto.RegNum))
            {
                throw new ConflictException("This registration number already exists!");
            }
            Company.UserCompanies.Add(new UserCompany
            {
                User = User,
                Company = Company,
                Role = dto.Role,
            });
           await _companyRepository.AddAsync(Company);
            bool isAdded = await _companyRepository.SaveChangesAsync();
            if (!isAdded)
            {
                throw new Exception("Failed to add the data to the database");
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
            if (dto.Address != null)
            {
                Company.Address = dto.Address;
            }
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
            _companyRepository.Delete(Company);
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
                Company = _Company,
                User = User,
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
        public async Task<CompanyResponseDto> UpdateUserCompanyAsync(int userId,int companyId, CompanyRole Role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException(nameof(User),userId);
            }
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
            {
                throw new NotFoundException(nameof(Company),companyId);
            }
            if (!user.UserCompanies.Any(uc => uc.CompanyId == companyId))
            {
                throw new ConflictException("This user is not related to this company!");
            }
            var oldusercompany = user.UserCompanies.FirstOrDefault(uc => uc.CompanyId == companyId);
            oldusercompany.Role = Role;
            var isadded =  await _userManager.UpdateAsync(user);
            if (!isadded.Succeeded)
            {
                throw new Exception("Error while editing usercompany!");
            }
            return _mapper.Map<CompanyResponseDto>(company);
        }
        private IEnumerable<Company> FilterVisible (IEnumerable<Company> companies, CompanyRole minimumRole)
        {
            if (_currentUser.IsSuperAdmin)
            {
                return companies;
            }
            return companies.Where(c => c is not null && (_currentUser.CompanyRoles.TryGetValue(c.Id,out var rolevalues)
            && Enum.TryParse<CompanyRole>(rolevalues,out var role) && role <= minimumRole)).ToList();
        }
    }
}
