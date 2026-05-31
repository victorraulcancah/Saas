using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/shifts")]
    [Authorize]
    [RequireSaasAccess("rh", "turnos")]
    public class ShiftsController : ControllerBase
    {
        private readonly IHrShiftService _shifts;

        public ShiftsController(IHrShiftService shifts)
        {
            _shifts = shifts;
        }

        [HttpGet]
        [RequirePermission("rh.turnos.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _shifts.GetShiftsAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("rh.turnos.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var shift = await _shifts.GetShiftByIdAsync(id);
            return shift is null ? NotFound() : Ok(shift);
        }

        [HttpPost]
        [RequirePermission("rh.turnos.administrar")]
        public async Task<IActionResult> Create([FromBody] ShiftRequest request)
        {
            return Ok(await _shifts.CreateShiftAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("rh.turnos.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShiftRequest request)
        {
            var shift = await _shifts.UpdateShiftAsync(id, request);
            return shift is null ? NotFound() : Ok(shift);
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("rh.turnos.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _shifts.DeleteShiftAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
