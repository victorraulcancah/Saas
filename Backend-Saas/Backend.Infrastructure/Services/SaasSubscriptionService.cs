using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class SaasSubscriptionService : ISaasSubscriptionService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;
    private readonly IAuditService _audit;

    public SaasSubscriptionService(AppDbContext context, ICacheService cache, IAuditService audit)
    {
        _context = context;
        _cache = cache;
        _audit = audit;
    }

    public async Task<IEnumerable<SaasPlan>> GetPlansAsync()
    {
        return await _context.SaasPlans
            .AsNoTracking()
            .Include(p => p.Systems).ThenInclude(ps => ps.System)
            .Include(p => p.Modules).ThenInclude(pm => pm.Module)
            .Include(p => p.SubModules).ThenInclude(psm => psm.SubModule)
            .OrderBy(p => p.Price)
            .ToListAsync();
    }

    public async Task<TenantSubscription> AssignPlanAsync(Guid tenantId, Guid planId, string status = "Active", DateTime? trialEndsAt = null, DateTime? currentPeriodEndsAt = null)
    {
        var tenantExists = await _context.Tenants.AnyAsync(t => t.Id == tenantId && t.IsActive);
        if (!tenantExists) throw new KeyNotFoundException($"Tenant {tenantId} no encontrado o inactivo");

        var plan = await _context.SaasPlans
            .Include(p => p.Systems)
            .Include(p => p.Modules)
            .Include(p => p.SubModules)
            .FirstOrDefaultAsync(p => p.Id == planId && p.IsActive)
            ?? throw new KeyNotFoundException($"Plan {planId} no encontrado o inactivo");

        var activeSubscriptions = await _context.TenantSubscriptions
            .Where(s => s.TenantId == tenantId && s.CancelledAt == null && (s.Status == "Active" || s.Status == "Trial"))
            .ToListAsync();

        foreach (var active in activeSubscriptions)
        {
            active.Status = "Cancelled";
            active.CancelledAt = DateTime.UtcNow;
            active.UpdatedAt = DateTime.UtcNow;
            active.AutoRenew = false;
        }

        var subscription = new TenantSubscription
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            PlanId = planId,
            Status = status,
            StartsAt = DateTime.UtcNow,
            TrialEndsAt = trialEndsAt,
            CurrentPeriodEndsAt = currentPeriodEndsAt ?? DateTime.UtcNow.AddMonths(1),
            AutoRenew = true
        };

        _context.TenantSubscriptions.Add(subscription);

        foreach (var item in plan.Systems.Where(ps => ps.IsIncluded))
        {
            await UpsertSystemLicenseAsync(tenantId, item.SystemId, "Subscription", subscription.CurrentPeriodEndsAt);
        }

        foreach (var item in plan.Modules.Where(pm => pm.IsIncluded))
        {
            await UpsertModuleLicenseAsync(tenantId, item.ModuleId, "Subscription", subscription.CurrentPeriodEndsAt);
        }

        foreach (var item in plan.SubModules.Where(psm => psm.IsIncluded))
        {
            await UpsertSubModuleLicenseAsync(tenantId, item.SubModuleId, "Subscription", subscription.CurrentPeriodEndsAt);
        }

        await _context.SaveChangesAsync();
        await _cache.RemoveAsync(SaasAccessService.CacheKey(tenantId));
        await _audit.LogAsync("ASSIGN_PLAN", "TenantSubscription", subscription.Id.ToString(), $"Plan {plan.Key} asignado a tenant {tenantId}");
        return subscription;
    }

    private async Task UpsertSystemLicenseAsync(Guid tenantId, Guid systemId, string source, DateTime? expiresAt)
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
    }

    private async Task UpsertModuleLicenseAsync(Guid tenantId, Guid moduleId, string source, DateTime? expiresAt)
    {
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
    }

    private async Task UpsertSubModuleLicenseAsync(Guid tenantId, Guid subModuleId, string source, DateTime? expiresAt)
    {
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
    }
}
