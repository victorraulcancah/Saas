using Backend.Application.TMS.Models;
using Backend.Application.TMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.TMS
{
    [ApiController]
    [Route("api/tms/vehicles")]
    [Authorize]
    [RequireSaasAccess("tms", "vehiculos")]
    public class VehiclesController : ControllerBase
    {
        private readonly ITmsVehicleService _vehicles;

        public VehiclesController(ITmsVehicleService vehicles)
        {
            _vehicles = vehicles;
        }

        [HttpGet]
        [RequirePermission("tms.vehiculos.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _vehicles.GetVehiclesAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("tms.vehiculos.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicles.GetVehicleByIdAsync(id);
            return vehicle is null ? NotFound() : Ok(vehicle);
        }

        [HttpPost]
        [RequirePermission("tms.vehiculos.administrar")]
        public async Task<IActionResult> Create([FromBody] VehicleRequest request)
        {
            return Ok(await _vehicles.CreateVehicleAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("tms.vehiculos.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleRequest request)
        {
            var vehicle = await _vehicles.UpdateVehicleAsync(id, request);
            return vehicle is null ? NotFound() : Ok(vehicle);
        }

        [HttpPost("{id:guid}/toggle-status")]
        [RequirePermission("tms.vehiculos.administrar")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var toggled = await _vehicles.ToggleVehicleStatusAsync(id);
            return toggled ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("tms.vehiculos.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _vehicles.DeleteVehicleAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
