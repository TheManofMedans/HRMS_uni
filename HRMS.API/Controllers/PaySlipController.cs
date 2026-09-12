using HRMS.Application.Interfaces.Services;
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
        public PaySlipController(IPaySlipService paySlipService)
        {
            _paySlipService = paySlipService;
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var payslips = await _paySlipService.GetAllAsync();
            return Ok(payslips);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paySlip = await _paySlipService.GetByIdAsync(id);
            return paySlip is null ? NotFound() : Ok(paySlip);
        }
    }
}
