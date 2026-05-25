using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantSubModuleLicense
{
    public Guid TenantId { get; set; }
    public Guid SubModuleId { get; set; }
    public bool IsEnabled { get; set; } = true;
    public string Source { get; set; } = "Subscription";
    public string? Config { get; set; }
    public DateTime EnabledAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public SaasSubModule SubModule { get; set; } = null!;
}
