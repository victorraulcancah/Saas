namespace Backend_Saas.DTOs.Tenant;

public record TenantResponse(
    Guid Id,
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
    bool IsActive,
    string? SubscriptionPlan,
    DateTime CreatedAt
);
