namespace Backend.Domain.Saas.Entities;

public class PlanSubModule
{
    public Guid PlanId { get; set; }
    public Guid SubModuleId { get; set; }
    public bool IsIncluded { get; set; } = true;

    public SaasPlan Plan { get; set; } = null!;
    public SaasSubModule SubModule { get; set; } = null!;
}
