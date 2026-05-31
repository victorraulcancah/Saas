namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Delivery : BaseEntity, ITenantEntity
{
    public enum DeliveryStatus
    {
        Pending,
        Assigned,
        InTransit,
        Delivered,
        Failed,
        Returned
    }

    public Guid? TenantId { get; set; }
    public string DeliveryNumber { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public Guid? DeliveryRouteId { get; set; }
    public virtual DeliveryRoute? DeliveryRoute { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? DriverId { get; set; }
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverDocument { get; set; } = string.Empty;
    public string SignatureUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
