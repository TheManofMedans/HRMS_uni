using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.DTOs;
using HRMS.domain.Enums;
using HRMS.Application.DTOs.Company;
using Microsoft.AspNetCore.Authorization;
using HRMS.API.Extensions;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }
        [HttpGet("{companyId}")]
        [Authorize(Policy = "Company_RequireHREmployee")]
        public async  Task<IActionResult> GetById(int companyId)
        {
            var company = await _companyService.GetByIdAsync(companyId);
            return company is null ? NotFound() : Ok(company);
        }
        [HttpGet("User/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var companies = await _companyService.GetByUserIdAsync(userId);
            return Ok(companies);
        }
        [HttpGet("RegNum/{RegNum}")]
        [Authorize]
        public async Task<IActionResult> GetByRegNum(string RegNum)
        {
            var company = await _companyService.GetByRegNumAsync(RegNum);
            return company is null ? NotFound() : Ok(company);
        }
        [HttpPost("{companyId}/User/{userId}")]
        [Authorize(Policy = "Company_RequireCEO")]
        public async Task<IActionResult> AddUserToCompany(int companyId, int userId,[FromQuery]CompanyRole role)
        {
            var created = await _companyService.AddUsertoCompanyAsync(companyId, userId, role);
            return Ok(created);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            var created = await _companyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {companyId = created.Id},created);
        }
        [HttpPut("{companyId}")]
        [Authorize(Policy = "Company_RequireCEO")]
        public async Task<IActionResult> Update(int companyId, [FromBody] UpdateCompanyDto dto)
        {
            await _companyService.UpdateAsync(companyId,dto);
            return NoContent();
        }
        [HttpDelete("{companyId}")]
        [Authorize(Policy = "Company_RequireCEO")]
        public async Task<IActionResult> Delete(int companyId)
        {
            await _companyService.DeleteAsync(companyId);
            return NoContent();
        }
    }
}
