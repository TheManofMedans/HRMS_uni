using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRMS.domain.Entities;
using HRMS.Application.DTOs.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeerepository;
        private readonly ICompanyRepository _companyRepository;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config
            ,IUserRepository userRepository, IEmployeeRepository employeerepository, ICompanyRepository companyRepository
            , RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _userRepository = userRepository;
            _employeerepository = employeerepository;
            _companyRepository = companyRepository;
            _roleManager = roleManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid Email or Password!");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password,lockoutOnFailure : true);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid Email or Password");
            }
            var uc = await _userRepository.GetByIdWithCompanyAsync(user.Id);
            if (uc.UserCompanies is not null)
            {
                user.UserCompanies = uc.UserCompanies;
            }
            var token = await GenerateJwtToken(user);
            return Ok(new { token });
        }
        [HttpPost("Register")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                throw new RepeatDataException("This Email already exists!");
            }
            if (await _userRepository.SSNExistsAsync(dto.SSN))
            {
                throw new RepeatDataException("This SSN already exists!");
            }
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                SSN = dto.SSN,
                UserName = dto.Email,
                Gender = dto.Gender,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(";",result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }
            if (dto.CompanyId != null)
            {
                var company = await _companyRepository.GetByIdAsync(dto.CompanyId.Value);
                if (company == null)
                {
                    throw new NotFoundException(nameof(company),dto.CompanyId.Value);
                }
                if (dto.Role != null)
                {
                    user.UserCompanies.Add(new UserCompany
                    {
                        User = user,
                        Company = company,
                        Role = dto.Role.Value
                    });
                    _userRepository.Update(user);
                    var isadded = await _userRepository.SaveChangesAsync();
                    if (!isadded)
                    {
                        throw new Exception("Error While adding the user to a company!");
                    }
                }
                else
                {
                    throw new Exception("Cant add a company without Role!");
                }
            }
            return Ok(new { user.Id, user.Email });
        }
        [HttpPost("Add-to-admin/{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddtoAdminRole(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException(nameof(user),userId);
            }
            var isadded = await _userManager.AddToRoleAsync(user, "SuperAdmin");
            if (!isadded.Succeeded)
            {
                throw new Exception("Error in adding to role in database!");
            }
            return Ok();
        }
        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userCompanies = user.UserCompanies; 
            foreach (var uc in userCompanies)
            {
                claims.Add(new Claim("company_role", $"{uc.CompanyId}:{uc.Role}"));
            }
            var employee = await _employeerepository.GetByUserIdAsync(user.Id);
            if (employee is not null)
            {
                claims.Add(new Claim("employee_id", $"{employee.Id}"));
            }
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
