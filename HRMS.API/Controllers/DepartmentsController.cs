using HRMS.Application.Interfaces.Services;
using HRMS.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs.Department;
using Microsoft.AspNetCore.Authorization;
using HRMS.API.Extensions;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var Departments = await _departmentService.GetAllAsync();
            return Ok(Departments);
        }
        [HttpGet("{departmentId}")]
        [Authorize(Policy = "Department_RequireHREmployee")]
        public async Task<IActionResult> GetById(int departmentId)
        {
            var department = await _departmentService.GetByIdAsync(departmentId);
            return department is null ? NotFound() : Ok(department);
        }
        [HttpGet("company/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var departments = await _departmentService.GetByCompanyIdAsync(companyId);
            return Ok(departments);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            if (!User.hasSufficientCompanyRole(dto.CompanyId,domain.Enums.CompanyRole.CEO))
            {
                return Forbid();
            }
            var created = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {departmentId = created.Id},created);
        }
        [HttpPut("{departmentId}")]
        [Authorize(Policy = "Department_RequireCEO")]
        public async Task<IActionResult> Update(int departmentId, [FromBody] UpdateDepartmentDto dto)
        {
            await _departmentService.UpdateAsync(departmentId,dto);
            return NoContent();
        }
        [HttpDelete("{departmentId}")]
        [Authorize(Policy = "Department_RequireCEO")]
        public async Task<IActionResult> Delete(int departmentId)
        {
            await _departmentService.DeleteAsync(departmentId);
            return NoContent();
        }
    }
}
