using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class ModuleService : IModuleService
{
    private readonly AppDbContext _context;

    public ModuleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Module>> GetAllModulesAsync()
    {
        return await _context.Modules.AsNoTracking().ToListAsync();
    }

    public async Task<Module> CreateModuleAsync(string name, string key, string? description, string? icon)
    {
        var module = new Module
        {
            Id = Guid.NewGuid(),
            Name = name,
            Key = key,
            Description = description,
            Icon = icon,
            IsActive = true
        };

        _context.Modules.Add(module);
        await _context.SaveChangesAsync();
        return module;
    }
}
