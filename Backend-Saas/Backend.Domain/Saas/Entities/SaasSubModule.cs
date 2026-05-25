namespace Backend.Domain.Saas.Entities;

public class SaasSubModule
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RoutePath { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public SaasModule Module { get; set; } = null!;
    public ICollection<PlanSubModule> PlanSubModules { get; set; } = new List<PlanSubModule>();
    public ICollection<TenantSubModuleLicense> TenantLicenses { get; set; } = new List<TenantSubModuleLicense>();
}
