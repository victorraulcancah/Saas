using Backend.Application.Common.Interfaces;
using Backend_Saas.DTOs.SaasCatalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.SuperAdmin;

[ApiController]
[Route("api/superadmin/tenants/{tenantId:guid}/licenses")]
[Authorize(Roles = "SuperAdmin")]
public class TenantLicensesController : ControllerBase
{
    private readonly ISaasLicenseService _licenseService;

    public TenantLicensesController(ISaasLicenseService licenseService)
    {
        _licenseService = licenseService;
    }

    [HttpPost("systems/{systemId:guid}")]
    public async Task<IActionResult> EnableSystem(Guid tenantId, Guid systemId, [FromBody] EnableTenantLicenseRequest request)
    {
        await _licenseService.EnableSystemAsync(tenantId, systemId, request.Source ?? "Manual", request.ExpiresAt);
        return Ok(new { message = "Sistema habilitado" });
    }

    [HttpDelete("systems/{systemId:guid}")]
    public async Task<IActionResult> DisableSystem(Guid tenantId, Guid systemId)
    {
        await _licenseService.DisableSystemAsync(tenantId, systemId);
        return Ok(new { message = "Sistema deshabilitado" });
    }

    [HttpPost("modules/{moduleId:guid}")]
    public async Task<IActionResult> EnableModule(Guid tenantId, Guid moduleId, [FromBody] EnableTenantLicenseRequest request)
    {
        await _licenseService.EnableModuleAsync(tenantId, moduleId, request.Source ?? "Manual", request.ExpiresAt);
        return Ok(new { message = "Módulo habilitado" });
    }

    [HttpDelete("modules/{moduleId:guid}")]
    public async Task<IActionResult> DisableModule(Guid tenantId, Guid moduleId)
    {
        await _licenseService.DisableModuleAsync(tenantId, moduleId);
        return Ok(new { message = "Módulo deshabilitado" });
    }

    [HttpPost("submodules/{subModuleId:guid}")]
    public async Task<IActionResult> EnableSubModule(Guid tenantId, Guid subModuleId, [FromBody] EnableTenantLicenseRequest request)
    {
        await _licenseService.EnableSubModuleAsync(tenantId, subModuleId, request.Source ?? "Manual", request.ExpiresAt);
        return Ok(new { message = "Submódulo habilitado" });
    }

    [HttpDelete("submodules/{subModuleId:guid}")]
    public async Task<IActionResult> DisableSubModule(Guid tenantId, Guid subModuleId)
    {
        await _licenseService.DisableSubModuleAsync(tenantId, subModuleId);
        return Ok(new { message = "Submódulo deshabilitado" });
    }
}
