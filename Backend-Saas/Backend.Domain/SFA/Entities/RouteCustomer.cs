namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class RouteCustomer : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid SalesRouteId { get; set; }
    public virtual SalesRoute SalesRoute { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public int VisitOrder { get; set; }
    public string VisitFrequency { get; set; } = string.Empty; // "Weekly", "Biweekly", "Monthly"
    public string PreferredDay { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
