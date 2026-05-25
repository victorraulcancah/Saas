namespace Backend.Domain.Common;

using Backend.Domain.Saas.Entities;

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
    public TenantCompanyProfile? CompanyProfile { get; set; }
    public TenantSunatCredential? SunatCredential { get; set; }
    public ICollection<TenantBranch> Branches { get; set; } = new List<TenantBranch>();
    public ICollection<TenantCertificate> Certificates { get; set; } = new List<TenantCertificate>();
    public ICollection<InvoiceSeries> InvoiceSeries { get; set; } = new List<InvoiceSeries>();
    public ICollection<TenantSubscription> Subscriptions { get; set; } = new List<TenantSubscription>();
    public ICollection<TenantSystemLicense> SystemLicenses { get; set; } = new List<TenantSystemLicense>();
    public ICollection<TenantModuleLicense> ModuleLicenses { get; set; } = new List<TenantModuleLicense>();
    public ICollection<TenantSubModuleLicense> SubModuleLicenses { get; set; } = new List<TenantSubModuleLicense>();
}
