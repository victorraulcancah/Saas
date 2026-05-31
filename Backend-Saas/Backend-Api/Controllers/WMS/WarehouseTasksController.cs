using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/wms/tasks")]
    [Authorize]
    [RequireSaasAccess("wms", "operaciones-almacen", "picking-packing")]
    public class WarehouseTasksController : ControllerBase
    {
        private readonly IWmsTaskService _tasks;

        public WarehouseTasksController(IWmsTaskService tasks)
        {
            _tasks = tasks;
        }

        [HttpGet]
        [RequirePermission("wms.operaciones-almacen.picking-packing.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tasks.GetTasksAsync());
        }

        [HttpPost]
        [RequirePermission("wms.operaciones-almacen.picking-packing.administrar")]
        public async Task<IActionResult> Create([FromBody] WarehouseTaskRequest request)
        {
            return Ok(await _tasks.CreateTaskAsync(request));
        }

        [HttpPatch("{id:guid}/status")]
        [RequirePermission("wms.operaciones-almacen.picking-packing.administrar")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] WarehouseTaskStatusRequest request)
        {
            var task = await _tasks.UpdateStatusAsync(id, request);
            return task is null ? NotFound() : Ok(task);
        }
    }
}
