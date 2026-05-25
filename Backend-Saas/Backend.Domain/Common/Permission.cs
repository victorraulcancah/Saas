namespace Backend.Domain.Common;

using Backend.Domain.Saas.Entities;

public class Permission
{
    public Guid Id { get; set; }
    public Guid? SaasSystemId { get; set; }
    public Guid? SaasModuleId { get; set; }
    public Guid? SaasSubModuleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Action { get; set; } = "view";
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public SaasSystem? SaasSystem { get; set; }
    public SaasModule? SaasModule { get; set; }
    public SaasSubModule? SaasSubModule { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
