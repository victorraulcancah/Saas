using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class SaasLicenseService : ISaasLicenseService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _audit;
    private readonly ICacheService _cache;

    public SaasLicenseService(AppDbContext context, IAuditService audit, ICacheService cache)
    {
        _context = context;
        _audit = audit;
        _cache = cache;
    }

    public async Task EnableSystemAsync(Guid tenantId, Guid systemId, string source = "Manual", DateTime? expiresAt = null)
    {
        var license = await _context.TenantSystemLicenses.FindAsync(tenantId, systemId);
        if (license is null)
        {
            license = new TenantSystemLicense { TenantId = tenantId, SystemId = systemId };
            _context.TenantSystemLicenses.Add(license);
        }

        license.IsEnabled = true;
        license.Source = source;
        license.ExpiresAt = expiresAt;
        license.EnabledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await InvalidateSnapshotAsync(tenantId);
        await _audit.LogAsync("ENABLE_SYSTEM_LICENSE", "TenantSystemLicense", $"{tenantId}:{systemId}", "Sistema habilitado para tenant");
    }

    public async Task EnableModuleAsync(Guid tenantId, Guid moduleId, string source = "Manual", DateTime? expiresAt = null)
    {
        var module = await _context.SaasModules.AsNoTracking().FirstOrDefaultAsync(m => m.Id == moduleId)
            ?? throw new KeyNotFoundException($"Módulo {moduleId} no encontrado");

        await EnableSystemAsync(tenantId, module.SystemId, source, expiresAt);

        var license = await _context.TenantModuleLicenses.FindAsync(tenantId, moduleId);
        if (license is null)
        {
            license = new TenantModuleLicense { TenantId = tenantId, ModuleId = moduleId };
            _context.TenantModuleLicenses.Add(license);
        }

        license.IsEnabled = true;
        license.Source = source;
        license.ExpiresAt = expiresAt;
        license.EnabledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await InvalidateSnapshotAsync(tenantId);
        await _audit.LogAsync("ENABLE_MODULE_LICENSE", "TenantModuleLicense", $"{tenantId}:{moduleId}", "Módulo habilitado para tenant");
    }

    public async Task EnableSubModuleAsync(Guid tenantId, Guid subModuleId, string source = "Manual", DateTime? expiresAt = null)
    {
        var subModule = await _context.SaasSubModules
            .AsNoTracking()
            .Include(sm => sm.Module)
            .FirstOrDefaultAsync(sm => sm.Id == subModuleId)
            ?? throw new KeyNotFoundException($"Submódulo {subModuleId} no encontrado");

        await EnableModuleAsync(tenantId, subModule.ModuleId, source, expiresAt);

        var license = await _context.TenantSubModuleLicenses.FindAsync(tenantId, subModuleId);
        if (license is null)
        {
            license = new TenantSubModuleLicense { TenantId = tenantId, SubModuleId = subModuleId };
            _context.TenantSubModuleLicenses.Add(license);
        }

        license.IsEnabled = true;
        license.Source = source;
        license.ExpiresAt = expiresAt;
        license.EnabledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await InvalidateSnapshotAsync(tenantId);
        await _audit.LogAsync("ENABLE_SUBMODULE_LICENSE", "TenantSubModuleLicense", $"{tenantId}:{subModuleId}", "Submódulo habilitado para tenant");
    }

    public async Task DisableSystemAsync(Guid tenantId, Guid systemId)
    {
        var license = await _context.TenantSystemLicenses.FindAsync(tenantId, systemId);
        if (license is null) return;
        license.IsEnabled = false;
        await _context.SaveChangesAsync();
        await InvalidateSnapshotAsync(tenantId);
        await _audit.LogAsync("DISABLE_SYSTEM_LICENSE", "TenantSystemLicense", $"{tenantId}:{systemId}", "Sistema deshabilitado para tenant");
    }

    public async Task DisableModuleAsync(Guid tenantId, Guid moduleId)
    {
        var license = await _context.TenantModuleLicenses.FindAsync(tenantId, moduleId);
        if (license is null) return;
        license.IsEnabled = false;
        await _context.SaveChangesAsync();
        await InvalidateSnapshotAsync(tenantId);
        await _audit.LogAsync("DISABLE_MODULE_LICENSE", "TenantModuleLicense", $"{tenantId}:{moduleId}", "Módulo deshabilitado para tenant");
    }

    public async Task DisableSubModuleAsync(Guid tenantId, Guid subModuleId)
    {
        var license = await _context.TenantSubModuleLicenses.FindAsync(tenantId, subModuleId);
        if (license is null) return;
        license.IsEnabled = false;
        await _context.SaveChangesAsync();
        await InvalidateSnapshotAsync(tenantId);
        await _audit.LogAsync("DISABLE_SUBMODULE_LICENSE", "TenantSubModuleLicense", $"{tenantId}:{subModuleId}", "Submódulo deshabilitado para tenant");
    }

    private Task InvalidateSnapshotAsync(Guid tenantId)
    {
        return _cache.RemoveAsync(SaasAccessService.CacheKey(tenantId));
    }
}
