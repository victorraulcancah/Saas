namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class GpsTrackPoint : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid DeliveryRouteId { get; set; }
    public virtual DeliveryRoute DeliveryRoute { get; set; } = null!;
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public decimal Speed { get; set; }
    public decimal Heading { get; set; }
    public DateTime Timestamp { get; set; }
    public string Source { get; set; } = string.Empty; // "GPS", "Mobile", "Manual"
}
