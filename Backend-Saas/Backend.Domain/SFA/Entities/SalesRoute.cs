namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class SalesRoute : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid SalesRepId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public virtual ICollection<RouteCustomer> Customers { get; set; } = new List<RouteCustomer>();
}
