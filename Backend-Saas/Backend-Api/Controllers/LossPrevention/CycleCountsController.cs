using Backend.Application.LossPrevention.Models;
using Backend.Application.LossPrevention.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/prevencion-perdidas/cycle-counts")]
    [Authorize]
    [RequireSaasAccess("prevencion-perdidas", "inventarios-ciclicos", "auditoria-aleatoria")]
    public class CycleCountsController : ControllerBase
    {
        private readonly ILossPreventionService _lossPrevention;

        public CycleCountsController(ILossPreventionService lossPrevention)
        {
            _lossPrevention = lossPrevention;
        }

        [HttpGet]
        [RequirePermission("prevencion-perdidas.inventarios-ciclicos.auditoria-aleatoria.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lossPrevention.GetCycleCountsAsync());
        }

        [HttpPost]
        [RequirePermission("prevencion-perdidas.inventarios-ciclicos.auditoria-aleatoria.administrar")]
        public async Task<IActionResult> Create([FromBody] CycleCountRequest request)
        {
            return Ok(await _lossPrevention.CreateCycleCountAsync(request));
        }

        [HttpPost("{id:guid}/close")]
        [RequirePermission("prevencion-perdidas.inventarios-ciclicos.auditoria-aleatoria.administrar")]
        public async Task<IActionResult> Close(Guid id, [FromBody] CycleCountCloseRequest request)
        {
            var count = await _lossPrevention.CloseCycleCountAsync(id, request);
            return count is null ? NotFound() : Ok(count);
        }
    }
}
