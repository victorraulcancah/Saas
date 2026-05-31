using Backend.Application.OMS.Models;
using Backend.Application.OMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.OMS
{
    [ApiController]
    [Route("api/oms/sales-channels")]
    [Authorize]
    [RequireSaasAccess("oms", "canales")]
    public class SalesChannelsController : ControllerBase
    {
        private readonly IOmsSalesChannelService _channels;

        public SalesChannelsController(IOmsSalesChannelService channels)
        {
            _channels = channels;
        }

        [HttpGet]
        [RequirePermission("oms.canales.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _channels.GetSalesChannelsAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("oms.canales.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var channel = await _channels.GetSalesChannelByIdAsync(id);
            return channel is null ? NotFound() : Ok(channel);
        }

        [HttpPost]
        [RequirePermission("oms.canales.administrar")]
        public async Task<IActionResult> Create([FromBody] SalesChannelRequest request)
        {
            return Ok(await _channels.CreateSalesChannelAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("oms.canales.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SalesChannelRequest request)
        {
            var channel = await _channels.UpdateSalesChannelAsync(id, request);
            return channel is null ? NotFound() : Ok(channel);
        }

        [HttpPost("{id:guid}/toggle-status")]
        [RequirePermission("oms.canales.administrar")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var toggled = await _channels.ToggleChannelStatusAsync(id);
            return toggled ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("oms.canales.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _channels.DeleteSalesChannelAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
