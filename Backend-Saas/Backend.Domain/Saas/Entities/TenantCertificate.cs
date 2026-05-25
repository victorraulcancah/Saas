using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantCertificate
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Format { get; set; } = "PEM";
    public byte[] CertificateContent { get; set; } = [];
    public string? EncryptedPassword { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
}
