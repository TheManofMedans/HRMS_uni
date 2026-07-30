using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.DTOs.Shift;

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
        public async Task<IActionResult> GetAll()
        {
            var shifts = await _shiftService.GetAllAsync();
            return Ok(shifts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id);
            return shift is null ? NotFound() : Ok(shift);
        }
        [HttpGet("Company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var shifts = await _shiftService.GetByCompanyIdAsync(companyId);
            return Ok(shifts);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShiftDto dto)
        {
            var created = await _shiftService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync),new {id =  created.Id},created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShiftDto dto)
        {
            await _shiftService.UpdateAsync(id,dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _shiftService.DeleteAsync(id);
            return NoContent();
        }
    }
}
