using HRMS.Application.Interfaces.Services;
using HRMS.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs.Department;

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
        public async Task<IActionResult> GetAll()
        {
            var Departments = await _departmentService.GetAllAsync();
            return Ok(Departments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            return department is null ? NotFound() : Ok(department);
        }
        [HttpGet("company/{Companyid}")]
        public async Task<IActionResult> GetByCompanyId(int Companyid)
        {
            var departments = await _departmentService.GetByCompanyIdAsync(Companyid);
            return Ok(departments);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            var created = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {id = created.Id},created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            await _departmentService.UpdateAsync(id,dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
