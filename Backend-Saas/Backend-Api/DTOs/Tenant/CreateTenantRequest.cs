namespace Backend_Api.DTOs.Tenant
{
    public record CreateTenantApiRequest(
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
        string? SubscriptionPlan);
}