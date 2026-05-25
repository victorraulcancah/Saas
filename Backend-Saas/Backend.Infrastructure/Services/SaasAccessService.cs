using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class SaasAccessService : ISaasAccessService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public SaasAccessService(AppDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<SaasAccessResult> CanAccessAsync(Guid tenantId, string? systemKey, string? moduleKey = null, string? subModuleKey = null)
    {
        var snapshot = await GetSnapshotAsync(tenantId);

        if (!snapshot.IsTenantActive)
            return new SaasAccessResult(false, "Tenant inactivo o suspendido.");

        if (!snapshot.HasActiveSubscription)
            return new SaasAccessResult(false, "Suscripción vencida, cancelada o inexistente.");

        if (!string.IsNullOrWhiteSpace(systemKey) && !snapshot.Systems.Contains(systemKey))
            return new SaasAccessResult(false, $"Sistema no habilitado: {systemKey}.");

        if (!string.IsNullOrWhiteSpace(moduleKey))
        {
            var key = Compose(systemKey, moduleKey);
            if (!snapshot.Modules.Contains(key))
                return new SaasAccessResult(false, $"Módulo no habilitado: {key}.");
        }

        if (!string.IsNullOrWhiteSpace(subModuleKey))
        {
            var key = Compose(systemKey, moduleKey, subModuleKey);
            if (!snapshot.SubModules.Contains(key))
                return new SaasAccessResult(false, $"Submódulo no habilitado: {key}.");
        }

        return new SaasAccessResult(true);
    }

    private async Task<TenantLicenseSnapshot> GetSnapshotAsync(Guid tenantId)
    {
        var cacheKey = CacheKey(tenantId);
        var cached = await _cache.GetAsync<TenantLicenseSnapshot>(cacheKey);
        if (cached is not null) return cached;

        var now = DateTime.UtcNow;
        var tenant = await _context.Tenants
            .AsNoTracking()
            .Include(t => t.Subscriptions)
            .Include(t => t.SystemLicenses).ThenInclude(l => l.System)
            .Include(t => t.ModuleLicenses).ThenInclude(l => l.Module).ThenInclude(m => m.System)
            .Include(t => t.SubModuleLicenses).ThenInclude(l => l.SubModule).ThenInclude(sm => sm.Module).ThenInclude(m => m.System)
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        if (tenant is null)
        {
            return new TenantLicenseSnapshot(false, false, [], [], []);
        }

        var hasActiveSubscription = tenant.Subscriptions.Any(s =>
            s.Status is "Trial" or "Active" &&
            s.CancelledAt is null &&
            (s.TrialEndsAt is null || s.TrialEndsAt > now) &&
            (s.CurrentPeriodEndsAt is null || s.CurrentPeriodEndsAt > now));

        var systems = tenant.SystemLicenses
            .Where(l => IsLicenseActive(l.IsEnabled, l.ExpiresAt, now))
            .Select(l => l.System.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var modules = tenant.ModuleLicenses
            .Where(l => IsLicenseActive(l.IsEnabled, l.ExpiresAt, now))
            .Select(l => Compose(l.Module.System.Key, l.Module.Key))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var subModules = tenant.SubModuleLicenses
            .Where(l => IsLicenseActive(l.IsEnabled, l.ExpiresAt, now))
            .Select(l => Compose(l.SubModule.Module.System.Key, l.SubModule.Module.Key, l.SubModule.Key))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var snapshot = new TenantLicenseSnapshot(tenant.IsActive, hasActiveSubscription, systems, modules, subModules);
        await _cache.SetAsync(cacheKey, snapshot, CacheTtl);
        return snapshot;
    }

    internal static string CacheKey(Guid tenantId) => $"tenant:{tenantId}:saas-license-snapshot";

    private static bool IsLicenseActive(bool isEnabled, DateTime? expiresAt, DateTime now)
    {
        return isEnabled && (expiresAt is null || expiresAt > now);
    }

    private static string Compose(params string?[] parts)
    {
        return string.Join(".", parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p!.Trim().ToLowerInvariant()));
    }

    private sealed record TenantLicenseSnapshot(
        bool IsTenantActive,
        bool HasActiveSubscription,
        HashSet<string> Systems,
        HashSet<string> Modules,
        HashSet<string> SubModules);
}
