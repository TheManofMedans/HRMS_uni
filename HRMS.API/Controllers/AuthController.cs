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

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config,IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _userRepository = userRepository;
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
        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userCompanies = user.UserCompanies; // requires User to be loaded with .Include(u => u.UserCompanies)
            foreach (var uc in userCompanies)
            {
                claims.Add(new Claim("company_role", $"{uc.CompanyId}:{uc.Role}"));
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
