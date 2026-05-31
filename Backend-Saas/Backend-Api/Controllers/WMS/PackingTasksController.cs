using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.WMS
{
    [ApiController]
    [Route("api/wms/packing-tasks")]
    [Authorize]
    [RequireSaasAccess("wms", "packing")]
    public class PackingTasksController : ControllerBase
    {
        private readonly IWmsPackingService _packing;

        public PackingTasksController(IWmsPackingService packing)
        {
            _packing = packing;
        }

        [HttpGet]
        [RequirePermission("wms.packing.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _packing.GetPackingTasksAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("wms.packing.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _packing.GetPackingTaskByIdAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost]
        [RequirePermission("wms.packing.administrar")]
        public async Task<IActionResult> Create([FromBody] PackingTaskRequest request)
        {
            return Ok(await _packing.CreatePackingTaskAsync(request));
        }

        [HttpPost("{id:guid}/start")]
        [RequirePermission("wms.packing.ejecutar")]
        public async Task<IActionResult> Start(Guid id)
        {
            var task = await _packing.StartPackingTaskAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("{id:guid}/complete")]
        [RequirePermission("wms.packing.ejecutar")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var task = await _packing.CompletePackingTaskAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("wms.packing.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _packing.CancelPackingTaskAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
