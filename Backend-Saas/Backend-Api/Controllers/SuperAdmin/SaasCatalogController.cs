using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/superadmin/saas-catalog")]
    [Authorize(Roles = "SuperAdmin")]
    public class SaasCatalogController : ControllerBase
    {
        private readonly ISaasCatalogService _catalogService;

        public SaasCatalogController(ISaasCatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalog()
        {
            var systems = await _catalogService.GetCatalogAsync();
            return Ok(systems.Select(MapSystem));
        }

        [HttpPost("systems")]
        public async Task<IActionResult> CreateSystem([FromBody] DTOs.SaasCatalog.CreateSaasSystemRequest request)
        {
            var system = await _catalogService.CreateSystemAsync(request.Name, request.Key, request.Description, request.Icon, request.BasePath);
            return Ok(MapSystem(system));
        }

        [HttpPost("modules")]
        public async Task<IActionResult> CreateModule([FromBody] DTOs.SaasCatalog.CreateSaasModuleRequest request)
        {
            var module = await _catalogService.CreateModuleAsync(request.SystemId, request.Name, request.Key, request.Description, request.Icon, request.BasePath);
            return Ok(MapModule(module));
        }

        [HttpPost("submodules")]
        public async Task<IActionResult> CreateSubModule([FromBody] DTOs.SaasCatalog.CreateSaasSubModuleRequest request)
        {
            var subModule = await _catalogService.CreateSubModuleAsync(request.ModuleId, request.Name, request.Key, request.Description, request.RoutePath);
            return Ok(MapSubModule(subModule));
        }

        private static DTOs.SaasCatalog.SaasSystemResponse MapSystem(SaasSystem system) => new(
            system.Id,
            system.Name,
            system.Key,
            system.Description,
            system.Icon,
            system.BasePath,
            system.IsActive,
            system.Modules.OrderBy(m => m.DisplayOrder).Select(MapModule)
        );

        private static DTOs.SaasCatalog.SaasModuleResponse MapModule(SaasModule module) => new(
            module.Id,
            module.Name,
            module.Key,
            module.Description,
            module.Icon,
            module.BasePath,
            module.IsActive,
            module.SubModules.OrderBy(s => s.DisplayOrder).Select(MapSubModule)
        );

        private static DTOs.SaasCatalog.SaasSubModuleResponse MapSubModule(SaasSubModule subModule) => new(
            subModule.Id,
            subModule.Name,
            subModule.Key,
            subModule.Description,
            subModule.RoutePath,
            subModule.IsActive
        );
    }
}