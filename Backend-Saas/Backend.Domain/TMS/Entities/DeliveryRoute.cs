namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class DeliveryRoute : BaseEntity, ITenantEntity
{
    public enum DeliveryRouteStatus
    {
        Planned,
        InTransit,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string RouteNumber { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public DateTime PlannedDate { get; set; }
    public DeliveryRouteStatus Status { get; set; } = DeliveryRouteStatus.Planned;
    public decimal CapacityKg { get; set; }
    public string Notes { get; set; } = string.Empty;
}
