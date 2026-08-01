using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Enums;
using HRMS.Application.DTOs.Request;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _requestService.GetAllAsync();
            return Ok(requests);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            return request is null ? NotFound() : Ok(request);
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var requests = await _requestService.GetByEmployeeIdAsync(employeeId);
            return Ok(requests);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(RequestStatus status)
        {
            var requests = await _requestService.GetWithStatusAsync(status);
            return Ok(requests);
        }
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(RequestType type)
        {
            var requests = await _requestService.GetWithTypeAsync(type);
            return Ok(requests);
        }
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var requests = await _requestService.GetWithCompanyIdAsync(companyId);
            return Ok(requests);
        }
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(int  departmentId)
        {
            var requests = await _requestService.GetWithDepartmentIdAsync(departmentId);
            return Ok(requests);
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetWithCustomDataAsync([FromQuery]int? employeeId,
            [FromQuery]RequestStatus? status, 
            [FromQuery] RequestType? type)
        {
            var requests = await _requestService.GetWithCustomDataAsync(employeeId, status, type);
            return Ok(requests);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
           var created = await _requestService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {id = created.Id},created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequestDto dto)
        {
            await _requestService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _requestService.DeleteAsync(id);
            return NoContent();
        }
    }
}
