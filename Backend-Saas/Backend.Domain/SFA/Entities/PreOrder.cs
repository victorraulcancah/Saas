namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PreOrder : BaseEntity, ITenantEntity
{
    public enum PreOrderStatus
    {
        Draft,
        Submitted,
        Approved,
        Rejected,
        Converted
    }

    public Guid? TenantId { get; set; }
    public string PreOrderNumber { get; set; } = string.Empty;
    public Guid SalesRepId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? FieldVisitId { get; set; }
    public DateTime OrderDate { get; set; }
    public PreOrderStatus Status { get; set; } = PreOrderStatus.Draft;
    public decimal TotalAmount { get; set; }
    public string PaymentTerms { get; set; } = string.Empty;
    public DateTime? DeliveryDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid? ConvertedOrderId { get; set; }
    public DateTime? ConvertedAt { get; set; }

    public virtual ICollection<PreOrderItem> Items { get; set; } = new List<PreOrderItem>();
}
