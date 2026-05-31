namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class RouteStop : BaseEntity, ITenantEntity
{
    public enum StopStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Skipped
    }

    public Guid? TenantId { get; set; }
    public Guid DeliveryRouteId { get; set; }
    public virtual DeliveryRoute DeliveryRoute { get; set; } = null!;
    public int StopOrder { get; set; }
    public Guid? OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public DateTime PlannedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public DateTime? ActualDeparture { get; set; }
    public StopStatus Status { get; set; } = StopStatus.Pending;
    public string Notes { get; set; } = string.Empty;
}
