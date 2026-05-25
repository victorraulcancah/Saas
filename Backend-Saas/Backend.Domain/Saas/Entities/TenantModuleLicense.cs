using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantModuleLicense
{
    public Guid TenantId { get; set; }
    public Guid ModuleId { get; set; }
    public bool IsEnabled { get; set; } = true;
    public string Source { get; set; } = "Subscription";
    public string? Config { get; set; }
    public DateTime EnabledAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public SaasModule Module { get; set; } = null!;
}
