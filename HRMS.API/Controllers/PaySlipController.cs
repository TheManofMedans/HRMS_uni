using HRMS.API.Extensions;
using HRMS.Application.Exceptions;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Entities;
using HRMS.domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaySlipController : ControllerBase
    {
        private readonly IPaySlipService _paySlipService;
        private readonly IDepartmentRepository _departmentRepository;
        public PaySlipController(IPaySlipService paySlipService, IDepartmentRepository departmentRepository)
        {
            _paySlipService = paySlipService;
            _departmentRepository = departmentRepository;
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var payslips = await _paySlipService.GetAllAsync();
            return Ok(payslips);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "PaySlip_OwnOrHRManager")]
        public async Task<IActionResult> GetById(int id)
        {
            var paySlip = await _paySlipService.GetByIdAsync(id);
            return paySlip is null ? NotFound() : Ok(paySlip);
        }
        [HttpGet("Company/{companyId}")]
        [Authorize(Policy = "Company_RequireHRManager")]
        public async Task<IActionResult> GetCompanyPayRoll(int companyId, DateTime weekStart)
        {
            var paySlip = await _paySlipService.GetCompanyPayrollSummaryAsync(companyId, weekStart);
            return Ok(paySlip);
        }
        [HttpGet("Employee/{id}")]
        [Authorize(Policy = "PaySlip_OwnOrHRManager")]
        public async Task<IActionResult> GetByEmployeeId(int id)
        {
            var paySlips = await _paySlipService.GetByEmployeeIdAsync(id);
            return Ok(paySlips);
        }
        [HttpGet("Department/{departmentId}")]
        [Authorize(Policy = "Department_RequireHRManager")]
        public async Task<IActionResult> GetByDepartmentId(int departmentId)
        {
            var paySlips = await _paySlipService.GetByDepartmentIdAsync(departmentId);
            return Ok(paySlips);
        }
        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery]DateTime? weekStart,[FromQuery]DateTime? endWeek,[FromQuery]int? companyId,[FromQuery]int? departmentId)
        {
            var paySlips = await _paySlipService.SearchAsync(weekStart, endWeek, companyId,departmentId);
            return Ok(paySlips);
        }
        [HttpPost("Employee/{employeeId}/Department/{departmentId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaySlip(int employeeId,int departmentId,[FromQuery]DateTime startWeek)
        {
            var department = await _departmentRepository.GetByIdAsync(employeeId);
            if (department == null)
            {
                throw new NotFoundException(nameof(Department),departmentId);
            }
            if(!User.IsInRole("SuperAdmin") && !User.hasSufficientCompanyRole(department.CompanyId,CompanyRole.HRManager))
            {
                return Forbid();
            }
            var paySlip = await _paySlipService.GenerateForWeekAsync(employeeId, departmentId, startWeek);
            return CreatedAtAction(nameof(GetById), new { id = paySlip.Id }, paySlip);
        }
    }
}
