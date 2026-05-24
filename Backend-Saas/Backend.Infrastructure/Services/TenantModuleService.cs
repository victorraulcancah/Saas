using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class TenantModuleService : ITenantModuleService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _audit;

    public TenantModuleService(AppDbContext context, IAuditService audit)
    {
        _context = context;
        _audit = audit;
    }

    public async Task AssignModuleAsync(Guid tenantId, Guid moduleId, string? config)
    {
        var exists = await _context.TenantModules.AnyAsync(tm => tm.TenantId == tenantId && tm.ModuleId == moduleId);
        if (exists) return;

        _context.TenantModules.Add(new TenantModule
        {
            TenantId = tenantId,
            ModuleId = moduleId,
            IsEnabled = true,
            Config = config,
            EnabledAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        await _audit.LogAsync("ASSIGN_MODULE", "TenantModule", $"{tenantId}:{moduleId}", "Módulo asignado a tenant");
    }

    public async Task RemoveModuleFromTenantAsync(Guid tenantId, Guid moduleId)
    {
        var tm = await _context.TenantModules.FindAsync(tenantId, moduleId);
        if (tm is null) return;

        _context.TenantModules.Remove(tm);
        await _context.SaveChangesAsync();
        await _audit.LogAsync("REMOVE_MODULE", "TenantModule", $"{tenantId}:{moduleId}", "Módulo removido de tenant");
    }

    public async Task<IEnumerable<TenantModule>> GetTenantModulesAsync(Guid tenantId)
    {
        return await _context.TenantModules
            .Where(tm => tm.TenantId == tenantId)
            .Include(tm => tm.Module)
            .ToListAsync();
    }
}
