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

    public async Task<Tenant> CreateTenantAsync(CreateTenantRequest request)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            Ruc = request.Ruc,
            RazonSocial = request.RazonSocial,
            NombreComercial = request.NombreComercial,
            Email = request.Email,
            EmailFacturacion = request.EmailFacturacion,
            Phone = request.Phone,
            TelefonoSecundario = request.TelefonoSecundario,
            Address = request.Address,
            DireccionFiscal = request.DireccionFiscal,
            Ubigeo = request.Ubigeo,
            Departamento = request.Departamento,
            Provincia = request.Provincia,
            Distrito = request.Distrito,
            Website = request.Website,
            LogoBase64 = request.LogoBase64,
            ClaveSol = request.ClaveSol,
            CertificadoPem = request.CertificadoPem,
            CertificadoPassword = request.CertificadoPassword,
            SubscriptionPlan = request.SubscriptionPlan,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        await _audit.LogAsync("CREATE", "Tenant", tenant.Id.ToString(), $"Tenant {request.Name} creado");
        return tenant;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid id)
    {
        return await _context.Tenants
            .Include(t => t.TenantModules)
            .ThenInclude(tm => tm.Module)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tenant> UpdateTenantAsync(Guid id, UpdateTenantRequest request)
    {
        var tenant = await _context.Tenants.FindAsync(id)
            ?? throw new KeyNotFoundException($"Tenant {id} no encontrado");

        tenant.Name = request.Name;
        tenant.Slug = request.Slug;
        tenant.Ruc = request.Ruc;
        tenant.RazonSocial = request.RazonSocial;
        tenant.NombreComercial = request.NombreComercial;
        tenant.Email = request.Email;
        tenant.EmailFacturacion = request.EmailFacturacion;
        tenant.Phone = request.Phone;
        tenant.TelefonoSecundario = request.TelefonoSecundario;
        tenant.Address = request.Address;
        tenant.DireccionFiscal = request.DireccionFiscal;
        tenant.Ubigeo = request.Ubigeo;
        tenant.Departamento = request.Departamento;
        tenant.Provincia = request.Provincia;
        tenant.Distrito = request.Distrito;
        tenant.Website = request.Website;
        tenant.LogoBase64 = request.LogoBase64;
        tenant.ClaveSol = request.ClaveSol;
        tenant.CertificadoPem = request.CertificadoPem;
        tenant.CertificadoPassword = request.CertificadoPassword;
        tenant.SubscriptionPlan = request.SubscriptionPlan;
        tenant.IsActive = request.IsActive;
        tenant.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _audit.LogAsync("UPDATE", "Tenant", id.ToString(), $"Tenant {request.Name} actualizado");
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
