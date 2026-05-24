namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public record CreateTenantRequest(
    string Name,
    string Slug,
    string Ruc,
    string RazonSocial,
    string? NombreComercial,
    string? Email,
    string? EmailFacturacion,
    string? Phone,
    string? TelefonoSecundario,
    string? Address,
    string? DireccionFiscal,
    string? Ubigeo,
    string? Departamento,
    string? Provincia,
    string? Distrito,
    string? Website,
    string? LogoBase64,
    string? ClaveSol,
    byte[]? CertificadoPem,
    string? CertificadoPassword,
    string? SubscriptionPlan,
    bool IsActive = true
);

public record UpdateTenantRequest(
    string Name,
    string Slug,
    string Ruc,
    string RazonSocial,
    string? NombreComercial,
    string? Email,
    string? EmailFacturacion,
    string? Phone,
    string? TelefonoSecundario,
    string? Address,
    string? DireccionFiscal,
    string? Ubigeo,
    string? Departamento,
    string? Provincia,
    string? Distrito,
    string? Website,
    string? LogoBase64,
    string? ClaveSol,
    byte[]? CertificadoPem,
    string? CertificadoPassword,
    string? SubscriptionPlan,
    bool IsActive
);

public interface ITenantService
{
    Task<Tenant> CreateTenantAsync(CreateTenantRequest request);
    Task<Tenant?> GetTenantByIdAsync(Guid id);
    Task<Tenant> UpdateTenantAsync(Guid id, UpdateTenantRequest request);
    Task DeleteTenantAsync(Guid id);
    Task<IEnumerable<Tenant>> GetAllTenantsAsync();
    Task<Tenant?> GetTenantBySlugAsync(string slug);
}
