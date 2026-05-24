namespace Backend.Domain.Common;

using Backend.Domain.Common.Interfaces;

public class RolePermission : ISoftDelete
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public ApplicationRole Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
