namespace Backend.Domain.Saas.Entities;

public class SaasSystem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? BasePath { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<SaasModule> Modules { get; set; } = new List<SaasModule>();
    public ICollection<PlanSystem> PlanSystems { get; set; } = new List<PlanSystem>();
    public ICollection<TenantSystemLicense> TenantLicenses { get; set; } = new List<TenantSystemLicense>();
}
