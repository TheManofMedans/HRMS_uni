using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.domain.Enums;
using System.Reflection.Metadata.Ecma335;
using HRMS.Application.DTOs.Attendance;
using Microsoft.AspNetCore.Authorization;
using HRMS.API.Extensions;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "Attendance_OwnOrHRManager")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            return attendance is null ? NotFound() : Ok(attendance); 
        }
        [HttpGet("Employee/{employeeId}")]
        [Authorize]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var attendances = await _attendanceService.GetByEmployeeIdAsync(employeeId);
            return Ok(attendances);
        }
        [HttpGet("Status/{status}")]
        [Authorize]
        public async Task<IActionResult> GetByStatus(AttendanceStatus status)
        {
            var attendances = await _attendanceService.GetByStatusAsync(status);
            return Ok(attendances);
        }
        [HttpGet("Employee/{employeeId}/Status/{status}")]
        [Authorize]
        public async Task<IActionResult> GetByEmployeeAndStatus(int employeeId, AttendanceStatus status)
        {
            var attendances = await _attendanceService.GetByEmployeeAndStatusAsync(employeeId, status);
            return Ok(attendances);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
        {
            var created = await _attendanceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById),new {id =  created.Id},created);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "Attendance_OwnOrHRManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
        {
            await _attendanceService.UpdateAsync(id,dto);
            return NoContent();
        }
        [HttpPut("Clearance/{id}")]
        [Authorize(Policy = "Attendance_RequireHRManager")]
        public async Task<IActionResult> HighClearanceUpdate(int id, [FromBody] UpdateAttendanceDto dto)
        {
            await _attendanceService.HighClearanceUpdateAsync(id,dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "Attendance_RequireHRManager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceService.DeleteAsync(id);
            return NoContent();
        }
    } 
}
