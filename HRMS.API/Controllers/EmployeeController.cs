using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;
using HRMS.API.Extensions;
using HRMS.domain.Enums;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var Employees = await _employeeService.GetAllAsync();
            return Ok(Employees);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "Employee_ViewOrEditScoped")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int? companyId)
        {
            var Employee = await _employeeService.GetByIdAsync(id);
            return Employee is null ? NotFound() : Ok(Employee);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            if (!User.IsInRole("SuperAdmin") && !User.HasAnySufficientCompanyRole(CompanyRole.HRManager))
            {
                return Forbid();
            }
            var Created = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = Created.Id }, Created);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto dto)
        {
            if (!User.IsInRole("SuperAdmin") && !User.HasAnySufficientCompanyRole(CompanyRole.HRManager))
            {
                return Forbid();
            }
            var created = await _employeeService.RegisterEmployeeAsync(dto);
            return CreatedAtAction(nameof(Register),new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "Employee_ViewOrEditScoped")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto, [FromQuery] int? companyId)
        {
            await _employeeService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "Employee_ViewOrEditScoped")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int? companyId)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPost("{employeeId}/departments/{departmentId}/primary")]
        [Authorize(Policy = "Department_RequireHRManager")]
        public async Task<IActionResult> SetPrimary(int employeeId,int departmentId)
        {
            await _employeeService.SetPrimary(employeeId, departmentId);
            return NoContent();
        }
        [HttpPost("{employeeId}/department/{departmentId}")]
        [Authorize(Policy = "Department_RequireCEO")]
        public async Task<IActionResult> AddToDepartmentAsync(int employeeId,int departmentId, [FromBody] UpdateEmployeeDto dto)
        {
            await _employeeService.AddToDepartmentAsync(employeeId, departmentId,dto);
            return NoContent();
        }
        [HttpPut("{employeeId}/department/{departmentId}")]
        [Authorize(Policy = "Department_RequireCEO")]
        public async Task<IActionResult> ChangeEmployeeRelation (int employeeId, int departmentId, [FromBody] UpdateEmployeeDto dto)
        {
            await _employeeService.ChangeDepartmentRelationAsync(employeeId, departmentId,dto);
            return NoContent();
        }
        [HttpDelete("{employeeId}/department/{departmentId}")]
        [Authorize(Policy = "Department_RequireCEO")]
        public async Task<IActionResult> RemoveFromDepartment(int employeeId,int departmentId)
        {
            await _employeeService.RemoveFromDepartmentAsync(employeeId, departmentId);
            return NoContent();
        }
    }
}
