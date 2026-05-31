namespace Backend.Domain.OMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class OmnichannelOrder : BaseEntity, ITenantEntity
{
    public enum OmnichannelOrderStatus
    {
        Pending,
        Assigned,
        Fulfilled,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string ExternalOrderNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public OmnichannelOrderStatus Status { get; set; } = OmnichannelOrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string DeliveryMethod { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public virtual ICollection<OmnichannelOrderItem> Items { get; set; } = new List<OmnichannelOrderItem>();
    public virtual ICollection<FulfillmentAssignment> FulfillmentAssignments { get; set; } = new List<FulfillmentAssignment>();
}
