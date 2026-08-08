using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.DTOs.Employee;

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
        public async Task<IActionResult> GetAll()
        {
            var Employees = await _employeeService.GetAllAsync();
            return Ok(Employees);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Employee = await _employeeService.GetByIdAsync(id);
            return Employee is null ? NotFound() : Ok(Employee);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            var Created = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = Created.Id }, Created);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto dto)
        {
            var created = await _employeeService.RegisterEmployeeAsync(dto);
            return CreatedAtAction(nameof(Register),new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            await _employeeService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPost("{Employeeid}/departments/{DepartmentId}/primary")]
        public async Task<IActionResult> SetPrimary(int Employeeid,int DepartmentId)
        {
            await _employeeService.SetPrimary(Employeeid, DepartmentId);
            return NoContent();
        }
        [HttpPost("{Employeeid}/department/{DepartmentId}")]
        public async Task<IActionResult> AddToDepartmentAsync(int Employeeid,int DepartmentId, [FromBody] UpdateEmployeeDto dto)
        {
            await _employeeService.AddToDepartmentAsync(Employeeid, DepartmentId,dto);
            return NoContent();
        }
    }
}
