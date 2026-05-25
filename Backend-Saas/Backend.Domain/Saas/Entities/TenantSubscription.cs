using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantSubscription
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid PlanId { get; set; }
    public string Status { get; set; } = "Trial";
    public DateTime StartsAt { get; set; } = DateTime.UtcNow;
    public DateTime? TrialEndsAt { get; set; }
    public DateTime? CurrentPeriodEndsAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public bool AutoRenew { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public SaasPlan Plan { get; set; } = null!;
}
