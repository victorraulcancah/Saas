namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ProofOfDelivery : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid DeliveryId { get; set; }
    public virtual Delivery Delivery { get; set; } = null!;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverDocument { get; set; } = string.Empty;
    public string ReceiverRelationship { get; set; } = string.Empty;
    public DateTime DeliveredAt { get; set; }
    public string SignatureUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
