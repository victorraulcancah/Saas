using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend_Saas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.ERP;

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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _billing.GetDispatchGuidesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DispatchGuideRequest request)
    {
        return Ok(await _billing.CreateDispatchGuideAsync(request));
    }

    [HttpPost("{id:guid}/issue")]
    public async Task<IActionResult> Issue(Guid id)
    {
        var guide = await _billing.IssueDispatchGuideAsync(id);
        return guide is null ? NotFound() : Ok(guide);
    }
}
