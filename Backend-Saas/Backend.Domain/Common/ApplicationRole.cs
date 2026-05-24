namespace Backend.Domain.Common;

using Backend.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole<Guid>, ITenantEntity, ISoftDelete
{
    public string Description { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Tenant? Tenant { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
