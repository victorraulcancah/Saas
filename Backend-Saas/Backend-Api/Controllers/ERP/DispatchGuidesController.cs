using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/dispatch-guides")]
    [Authorize]
    [RequireSaasAccess("erp", "facturacion", "guias-remision")]
    public class DispatchGuidesController : ControllerBase
    {
        private readonly IErpBillingService _billing;

        public DispatchGuidesController(IErpBillingService billing)
        {
            _billing = billing;
        }

        [HttpGet]
        [RequirePermission("erp.facturacion.guias-remision.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _billing.GetDispatchGuidesAsync());
        }

        [HttpPost]
        [RequirePermission("erp.facturacion.guias-remision.administrar")]
        public async Task<IActionResult> Create([FromBody] DispatchGuideRequest request)
        {
            return Ok(await _billing.CreateDispatchGuideAsync(request));
        }

        [HttpPost("{id:guid}/issue")]
        [RequirePermission("erp.facturacion.guias-remision.administrar")]
        public async Task<IActionResult> Issue(Guid id)
        {
            var guide = await _billing.IssueDispatchGuideAsync(id);
            return guide is null ? NotFound() : Ok(guide);
        }
    }
}
