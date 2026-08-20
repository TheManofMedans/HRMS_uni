using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.DTOs.Shift;
using Microsoft.AspNetCore.Authorization;
using HRMS.API.Extensions;
using HRMS.Application.Exceptions;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var shifts = await _shiftService.GetAllAsync();
            return Ok(shifts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id);
            return shift is null ? NotFound() : Ok(shift);
        }
        [HttpGet("Company/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var shifts = await _shiftService.GetByCompanyIdAsync(companyId);
            return Ok(shifts);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShiftDto dto)
        {
            if (!User.HasAnySufficientCompanyRole(domain.Enums.CompanyRole.HRManager))
            {
                throw new ForbiddenException("You cannot create a new Shift!");
            }
            var created = await _shiftService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {id =  created.Id},created);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "Shift_RequireHRManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShiftDto dto)
        {
            await _shiftService.UpdateAsync(id,dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "Shift_RequireHRManager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _shiftService.DeleteAsync(id);
            return NoContent();
        }
    }
}
