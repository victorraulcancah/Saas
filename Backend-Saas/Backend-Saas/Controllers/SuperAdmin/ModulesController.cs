using Backend.Application.Common.Interfaces;
using Backend_Saas.DTOs.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.SuperAdmin;

[ApiController]
[Route("api/superadmin/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class ModulesController : ControllerBase
{
    private readonly IModuleService _moduleService;

    public ModulesController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var modules = await _moduleService.GetAllModulesAsync();
        var response = modules.Select(m => new ModuleResponse(m.Id, m.Name, m.Key, m.Description, m.Icon));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequest request)
    {
        var module = await _moduleService.CreateModuleAsync(request.Name, request.Key, request.Description, request.Icon);
        return Ok(new ModuleResponse(module.Id, module.Name, module.Key, module.Description, module.Icon));
    }
}
