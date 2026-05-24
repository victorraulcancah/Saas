namespace Backend.Domain.Common;

public class TenantModule
{
    public Guid TenantId { get; set; }
    public Guid ModuleId { get; set; }
    public bool IsEnabled { get; set; } = true;
    public string? Config { get; set; }
    public DateTime EnabledAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Module Module { get; set; } = null!;
}
