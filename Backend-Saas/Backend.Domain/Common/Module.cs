namespace Backend.Domain.Common;

public class Module
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public string? Icon { get; set; }
    public string? BasePath { get; set; }
    public ICollection<TenantModule> TenantModules { get; set; } = new List<TenantModule>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
