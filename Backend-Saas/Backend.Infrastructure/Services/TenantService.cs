namespace Backend.Infrastructure.Services;

using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

public class TenantService : ITenantService
{
    private readonly AppDbContext _context;

    public TenantService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant> CreateTenantAsync(string name, string slug, string? email, string? subscriptionPlan)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            Email = email ?? string.Empty,
            SubscriptionPlan = subscriptionPlan ?? string.Empty,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task AssignModuleAsync(Guid tenantId, Guid moduleId, string? config)
    {
        var tenantModule = new TenantModule
        {
            TenantId = tenantId,
            ModuleId = moduleId,
            IsEnabled = true,
            Config = config,
            EnabledAt = DateTime.UtcNow
        };

        _context.TenantModules.Add(tenantModule);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
    {
        return await _context.Tenants
            .Include(t => t.TenantModules)
            .ThenInclude(tm => tm.Module)
            .ToListAsync();
    }

    public async Task<Tenant?> GetTenantBySlugAsync(string slug)
    {
        return await _context.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.Slug == slug);
    }
}
