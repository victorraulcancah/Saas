using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class SaasCatalogService : ISaasCatalogService
{
    private readonly AppDbContext _context;

    public SaasCatalogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SaasSystem>> GetCatalogAsync()
    {
        return await _context.SaasSystems
            .AsNoTracking()
            .Include(s => s.Modules.OrderBy(m => m.DisplayOrder))
            .ThenInclude(m => m.SubModules.OrderBy(sm => sm.DisplayOrder))
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
    }

    public async Task<SaasSystem> CreateSystemAsync(string name, string key, string? description, string? icon, string? basePath)
    {
        var system = new SaasSystem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Key = key,
            Description = description,
            Icon = icon,
            BasePath = basePath,
            DisplayOrder = await _context.SaasSystems.CountAsync() + 1
        };

        _context.SaasSystems.Add(system);
        await _context.SaveChangesAsync();
        return system;
    }

    public async Task<SaasModule> CreateModuleAsync(Guid systemId, string name, string key, string? description, string? icon, string? basePath)
    {
        var exists = await _context.SaasSystems.AnyAsync(s => s.Id == systemId);
        if (!exists) throw new KeyNotFoundException($"Sistema {systemId} no encontrado");

        var module = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = systemId,
            Name = name,
            Key = key,
            Description = description,
            Icon = icon,
            BasePath = basePath,
            DisplayOrder = await _context.SaasModules.CountAsync(m => m.SystemId == systemId) + 1
        };

        _context.SaasModules.Add(module);
        await _context.SaveChangesAsync();
        return module;
    }

    public async Task<SaasSubModule> CreateSubModuleAsync(Guid moduleId, string name, string key, string? description, string? routePath)
    {
        var exists = await _context.SaasModules.AnyAsync(m => m.Id == moduleId);
        if (!exists) throw new KeyNotFoundException($"Módulo {moduleId} no encontrado");

        var subModule = new SaasSubModule
        {
            Id = Guid.NewGuid(),
            ModuleId = moduleId,
            Name = name,
            Key = key,
            Description = description,
            RoutePath = routePath,
            DisplayOrder = await _context.SaasSubModules.CountAsync(sm => sm.ModuleId == moduleId) + 1
        };

        _context.SaasSubModules.Add(subModule);
        await _context.SaveChangesAsync();
        return subModule;
    }
}
