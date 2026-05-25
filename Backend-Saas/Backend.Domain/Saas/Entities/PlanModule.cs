namespace Backend.Domain.Saas.Entities;

public class PlanModule
{
    public Guid PlanId { get; set; }
    public Guid ModuleId { get; set; }
    public bool IsIncluded { get; set; } = true;

    public SaasPlan Plan { get; set; } = null!;
    public SaasModule Module { get; set; } = null!;
}
