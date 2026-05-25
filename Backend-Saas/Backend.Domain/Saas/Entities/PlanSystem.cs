namespace Backend.Domain.Saas.Entities;

public class PlanSystem
{
    public Guid PlanId { get; set; }
    public Guid SystemId { get; set; }
    public bool IsIncluded { get; set; } = true;

    public SaasPlan Plan { get; set; } = null!;
    public SaasSystem System { get; set; } = null!;
}
