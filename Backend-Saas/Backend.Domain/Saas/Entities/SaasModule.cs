namespace Backend.Domain.Saas.Entities;

public class SaasModule
{
    public Guid Id { get; set; }
    public Guid SystemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? BasePath { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public SaasSystem System { get; set; } = null!;
    public ICollection<SaasSubModule> SubModules { get; set; } = new List<SaasSubModule>();
    public ICollection<PlanModule> PlanModules { get; set; } = new List<PlanModule>();
    public ICollection<TenantModuleLicense> TenantLicenses { get; set; } = new List<TenantModuleLicense>();
}
