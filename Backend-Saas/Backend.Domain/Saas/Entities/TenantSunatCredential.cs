using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantSunatCredential
{
    public Guid TenantId { get; set; }
    public string Environment { get; set; } = "Beta";
    public string SendMode { get; set; } = "DirectSunat";
    public string? SolUser { get; set; }
    public string? EncryptedSolPassword { get; set; }
    public string? OseProvider { get; set; }
    public string? OseApiUrl { get; set; }
    public string? EncryptedOseToken { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
}
