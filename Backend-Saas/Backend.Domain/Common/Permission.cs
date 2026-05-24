namespace Backend.Domain.Common;

public class Permission
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public Module Module { get; set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
