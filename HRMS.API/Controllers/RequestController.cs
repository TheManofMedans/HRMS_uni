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
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
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
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            return request is null ? NotFound() : Ok(request);
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var requests = await _requestService.GetByEmployeeIdAsync(employeeId);
            return requests is null ? NotFound() : Ok(requests);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(RequestStatus status)
        {
            var requests = await _requestService.GetWithStatusAsync(status);
            return requests is null ? NotFound() : Ok(requests);
        }
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(RequestType type)
        {
            var requests = await _requestService.GetWithTypeAsync(type);
            return requests is null ? NotFound() : Ok(requests);
        }
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var requests = await _requestService.GetWithCompanyIdAsync(companyId);
            return requests is null ? NotFound() : Ok(requests);
        }
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(int  departmentId)
        {
            var requests = await _requestService.GetWithDepartmentIdAsync(departmentId);
            return requests is null ? NotFound() : Ok(requests);
        }
        [HttpGet("employee/{employeeId}/Status/{status}/Type/{type}")]
        public async Task<IActionResult> GetWithCustomDataAsync(int? employeeId, RequestStatus? status, RequestType? type)
        {
            var requests = await _requestService.GetWithCustomDataAsync(employeeId, status, type);
            return requests is null ? NotFound() : Ok(requests);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
           var created = await _requestService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync),new {id = created.Id},created);
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
