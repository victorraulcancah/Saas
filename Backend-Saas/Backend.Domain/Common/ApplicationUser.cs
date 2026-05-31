namespace Backend.Domain.Common;

using Backend.SharedKernel.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<Guid>, ITenantEntity, ISoftDelete
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Tenant? Tenant { get; set; }
}
