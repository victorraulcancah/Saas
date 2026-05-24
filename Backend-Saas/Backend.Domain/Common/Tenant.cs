namespace Backend.Domain.Common;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }
    public string? Email { get; set; }
    public string? EmailFacturacion { get; set; }
    public string? Phone { get; set; }
    public string? TelefonoSecundario { get; set; }
    public string? Address { get; set; }
    public string? DireccionFiscal { get; set; }
    public string? Ubigeo { get; set; }
    public string? Departamento { get; set; }
    public string? Provincia { get; set; }
    public string? Distrito { get; set; }
    public string? Website { get; set; }
    public string? LogoBase64 { get; set; }
    public string? ClaveSol { get; set; }
    public byte[]? CertificadoPem { get; set; }
    public string? CertificadoPassword { get; set; }
    public bool IsActive { get; set; } = true;
    public string? SubscriptionPlan { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<TenantModule> TenantModules { get; set; } = new List<TenantModule>();
}
