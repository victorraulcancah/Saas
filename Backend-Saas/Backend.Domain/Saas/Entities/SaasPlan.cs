namespace Backend.Domain.Saas.Entities;

public class SaasPlan
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "PEN";
    public string BillingCycle { get; set; } = "Monthly";
    public int MaxUsers { get; set; }
    public int MaxBranches { get; set; }
    public int MaxDocumentsPerMonth { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<PlanSystem> Systems { get; set; } = new List<PlanSystem>();
    public ICollection<PlanModule> Modules { get; set; } = new List<PlanModule>();
    public ICollection<PlanSubModule> SubModules { get; set; } = new List<PlanSubModule>();
    public ICollection<TenantSubscription> Subscriptions { get; set; } = new List<TenantSubscription>();
}
