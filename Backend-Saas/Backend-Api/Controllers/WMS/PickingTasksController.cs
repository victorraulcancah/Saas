using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.WMS
{
    [ApiController]
    [Route("api/wms/picking-tasks")]
    [Authorize]
    [RequireSaasAccess("wms", "picking")]
    public class PickingTasksController : ControllerBase
    {
        private readonly IWmsPickingService _picking;

        public PickingTasksController(IWmsPickingService picking)
        {
            _picking = picking;
        }

        [HttpGet]
        [RequirePermission("wms.picking.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _picking.GetPickingTasksAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("wms.picking.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _picking.GetPickingTaskByIdAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost]
        [RequirePermission("wms.picking.administrar")]
        public async Task<IActionResult> Create([FromBody] PickingTaskRequest request)
        {
            return Ok(await _picking.CreatePickingTaskAsync(request));
        }

        [HttpPost("{id:guid}/assign")]
        [RequirePermission("wms.picking.administrar")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] Guid userId)
        {
            var task = await _picking.AssignPickingTaskAsync(id, userId);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("{id:guid}/start")]
        [RequirePermission("wms.picking.ejecutar")]
        public async Task<IActionResult> Start(Guid id)
        {
            var task = await _picking.StartPickingTaskAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("{id:guid}/complete")]
        [RequirePermission("wms.picking.ejecutar")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var task = await _picking.CompletePickingTaskAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("wms.picking.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _picking.CancelPickingTaskAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
