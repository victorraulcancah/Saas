using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Domain.Saas.Entities;
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
        _context.TenantCompanyProfiles.Add(new TenantCompanyProfile
        {
            TenantId = tenant.Id,
            Ruc = request.Ruc,
            RazonSocial = request.RazonSocial,
            NombreComercial = request.NombreComercial,
            EmailFacturacion = request.EmailFacturacion,
            Phone = request.Phone,
            DireccionFiscal = request.DireccionFiscal ?? request.Address ?? string.Empty,
            Ubigeo = request.Ubigeo ?? string.Empty,
            Departamento = request.Departamento ?? string.Empty,
            Provincia = request.Provincia ?? string.Empty,
            Distrito = request.Distrito ?? string.Empty
        });

        _context.TenantBranches.Add(new TenantBranch
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Name = "Principal",
            Code = "MAIN",
            SunatEstablishmentCode = "0000",
            Address = request.DireccionFiscal ?? request.Address ?? string.Empty,
            Ubigeo = request.Ubigeo ?? string.Empty,
            Departamento = request.Departamento ?? string.Empty,
            Provincia = request.Provincia ?? string.Empty,
            Distrito = request.Distrito ?? string.Empty,
            IsMain = true
        });

        if (!string.IsNullOrWhiteSpace(request.ClaveSol))
        {
            _context.TenantSunatCredentials.Add(new TenantSunatCredential
            {
                TenantId = tenant.Id,
                Environment = "Beta",
                SendMode = "DirectSunat",
                EncryptedSolPassword = request.ClaveSol
            });
        }

        if (request.CertificadoPem is { Length: > 0 })
        {
            _context.TenantCertificates.Add(new TenantCertificate
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Name = "Certificado principal",
                Format = "PEM",
                CertificateContent = request.CertificadoPem,
                EncryptedPassword = request.CertificadoPassword
            });
        }

        await _context.SaveChangesAsync();
        await _audit.LogAsync("CREATE", "Tenant", tenant.Id.ToString(), $"Tenant {request.Name} creado");
        return tenant;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid id)
    {
        return await _context.Tenants
            .Include(t => t.CompanyProfile)
            .Include(t => t.SunatCredential)
            .Include(t => t.Branches)
            .Include(t => t.Certificates)
            .Include(t => t.Subscriptions)
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

        var profile = await _context.TenantCompanyProfiles.FindAsync(id);
        if (profile is null)
        {
            profile = new TenantCompanyProfile { TenantId = id };
            _context.TenantCompanyProfiles.Add(profile);
        }

        profile.Ruc = request.Ruc;
        profile.RazonSocial = request.RazonSocial;
        profile.NombreComercial = request.NombreComercial;
        profile.EmailFacturacion = request.EmailFacturacion;
        profile.Phone = request.Phone;
        profile.DireccionFiscal = request.DireccionFiscal ?? request.Address ?? string.Empty;
        profile.Ubigeo = request.Ubigeo ?? string.Empty;
        profile.Departamento = request.Departamento ?? string.Empty;
        profile.Provincia = request.Provincia ?? string.Empty;
        profile.Distrito = request.Distrito ?? string.Empty;
        profile.UpdatedAt = DateTime.UtcNow;

        var sunat = await _context.TenantSunatCredentials.FindAsync(id);
        if (!string.IsNullOrWhiteSpace(request.ClaveSol))
        {
            if (sunat is null)
            {
                sunat = new TenantSunatCredential { TenantId = id };
                _context.TenantSunatCredentials.Add(sunat);
            }

            sunat.EncryptedSolPassword = request.ClaveSol;
            sunat.UpdatedAt = DateTime.UtcNow;
        }

        if (request.CertificadoPem is { Length: > 0 })
        {
            var certificate = await _context.TenantCertificates
                .FirstOrDefaultAsync(c => c.TenantId == id && c.IsActive);

            if (certificate is null)
            {
                certificate = new TenantCertificate
                {
                    Id = Guid.NewGuid(),
                    TenantId = id,
                    Name = "Certificado principal",
                    Format = "PEM"
                };
                _context.TenantCertificates.Add(certificate);
            }

            certificate.CertificateContent = request.CertificadoPem;
            certificate.EncryptedPassword = request.CertificadoPassword;
            certificate.UpdatedAt = DateTime.UtcNow;
        }

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
            .Include(t => t.CompanyProfile)
            .Include(t => t.Branches)
            .ToListAsync();
    }

    public async Task<Tenant?> GetTenantBySlugAsync(string slug)
    {
        return await _context.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.Slug == slug);
    }
}
