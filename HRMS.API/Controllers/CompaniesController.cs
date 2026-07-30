using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.DTOs;
using HRMS.domain.Enums;
using HRMS.Application.DTOs.Company;

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
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }
        [HttpGet("{id}")]
        public async  Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return company is null ? NotFound() : Ok(company);
        }
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var companies = await _companyService.GetWithUserAsync(userId);
            return Ok(companies);
        }
        [HttpGet("RegNum/{RegNum}")]
        public async Task<IActionResult> GetByRegNum(string RegNum)
        {
            var company = await _companyService.GetByRegNumAsync(RegNum);
            return company is null ? NotFound() : Ok(company);
        }
        [HttpPost("{companyId}/User/{userId}")]
        public async Task<IActionResult> AddUserToCompany(int companyId, int userId,[FromQuery]CompanyRole role)
        {
            var created = await _companyService.AddUsertoCompanyAsync(companyId, userId, role);
            return Ok(created);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            var created = await _companyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {id = created.Id},created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto dto)
        {
            await _companyService.UpdateAsync(id,dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
