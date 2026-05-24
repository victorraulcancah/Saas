using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _audit;

    public TenantService(AppDbContext context, IAuditService audit)
    {
        _context = context;
        _audit = audit;
    }

    public async Task<Tenant> CreateTenantAsync(string name, string slug, string? email, string? subscriptionPlan)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            Email = email,
            SubscriptionPlan = subscriptionPlan,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        await _audit.LogAsync("CREATE", "Tenant", tenant.Id.ToString(), $"Tenant {name} creado");
        return tenant;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid id)
    {
        return await _context.Tenants
            .Include(t => t.TenantModules)
            .ThenInclude(tm => tm.Module)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tenant> UpdateTenantAsync(Guid id, string name, string slug, string? email, string? subscriptionPlan, bool isActive)
    {
        var tenant = await _context.Tenants.FindAsync(id)
            ?? throw new KeyNotFoundException($"Tenant {id} no encontrado");

        tenant.Name = name;
        tenant.Slug = slug;
        tenant.Email = email;
        tenant.SubscriptionPlan = subscriptionPlan;
        tenant.IsActive = isActive;
        tenant.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _audit.LogAsync("UPDATE", "Tenant", id.ToString(), $"Tenant {name} actualizado");
        return tenant;
    }

    public async Task DeleteTenantAsync(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id)
            ?? throw new KeyNotFoundException($"Tenant {id} no encontrado");

        tenant.IsActive = false;
        tenant.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await _audit.LogAsync("DEACTIVATE", "Tenant", id.ToString(), $"Tenant {tenant.Name} desactivado");
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
