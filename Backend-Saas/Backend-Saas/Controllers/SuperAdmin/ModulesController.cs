using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Backend_Saas.DTOs.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Saas.Controllers.SuperAdmin;

[ApiController]
[Route("api/superadmin/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class ModulesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ModulesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var modules = await _db.Modules.AsNoTracking().ToListAsync();
        var response = modules.Select(m => new ModuleResponse(m.Id, m.Name, m.Key, m.Description, m.Icon));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequest request)
    {
        var module = new Module
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Key = request.Key,
            Description = request.Description,
            Icon = request.Icon,
            IsActive = true
        };

        _db.Modules.Add(module);
        await _db.SaveChangesAsync();
        return Ok(new ModuleResponse(module.Id, module.Name, module.Key, module.Description, module.Icon));
    }
}

public record CreateModuleRequest(string Name, string Key, string? Description, string? Icon);
