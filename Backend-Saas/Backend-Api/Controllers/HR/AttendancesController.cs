using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/hr/attendances")]
    [Authorize]
    [RequireSaasAccess("rh", "asistencia-campo", "faltas-tardanzas")]
    public class AttendancesController : ControllerBase
    {
        private readonly IHrAttendanceService _attendances;

        public AttendancesController(IHrAttendanceService attendances)
        {
            _attendances = attendances;
        }

        [HttpGet]
        [RequirePermission("rh.asistencia-campo.faltas-tardanzas.ver")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return Ok(await _attendances.GetAttendancesAsync(from, to));
        }

        [HttpPost]
        [RequirePermission("rh.asistencia-campo.faltas-tardanzas.administrar")]
        public async Task<IActionResult> Register([FromBody] AttendanceRequest request)
        {
            return Ok(await _attendances.RegisterAttendanceAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("rh.asistencia-campo.faltas-tardanzas.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AttendanceRequest request)
        {
            var attendance = await _attendances.UpdateAttendanceAsync(id, request);
            return attendance is null ? NotFound() : Ok(attendance);
        }
    }
}
