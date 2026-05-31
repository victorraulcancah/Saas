using Backend.Application.SFA.Models;
using Backend.Application.SFA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.SFA
{
    [ApiController]
    [Route("api/sfa/field-visits")]
    [Authorize]
    [RequireSaasAccess("sfa", "visitas")]
    public class FieldVisitsController : ControllerBase
    {
        private readonly ISfaFieldVisitService _visits;

        public FieldVisitsController(ISfaFieldVisitService visits)
        {
            _visits = visits;
        }

        [HttpGet]
        [RequirePermission("sfa.visitas.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _visits.GetFieldVisitsAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("sfa.visitas.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var visit = await _visits.GetFieldVisitByIdAsync(id);
            return visit is null ? NotFound() : Ok(visit);
        }

        [HttpPost]
        [RequirePermission("sfa.visitas.administrar")]
        public async Task<IActionResult> Create([FromBody] FieldVisitRequest request)
        {
            return Ok(await _visits.CreateFieldVisitAsync(request));
        }

        [HttpPost("{id:guid}/check-in")]
        [RequirePermission("sfa.visitas.ejecutar")]
        public async Task<IActionResult> CheckIn(Guid id, [FromQuery] string latitude, [FromQuery] string longitude)
        {
            var visit = await _visits.CheckInAsync(id, latitude, longitude);
            return visit is null ? NotFound() : Ok(visit);
        }

        [HttpPost("{id:guid}/check-out")]
        [RequirePermission("sfa.visitas.ejecutar")]
        public async Task<IActionResult> CheckOut(Guid id, [FromQuery] string latitude, [FromQuery] string longitude)
        {
            var visit = await _visits.CheckOutAsync(id, latitude, longitude);
            return visit is null ? NotFound() : Ok(visit);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("sfa.visitas.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _visits.CancelFieldVisitAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
