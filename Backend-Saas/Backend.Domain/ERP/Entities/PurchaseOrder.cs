namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PurchaseOrder : BaseEntity, ITenantEntity
{
    public enum PurchaseOrderStatus
    {
        Draft,
        Sent,
        Approved,
        Received,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public PurchaseOrderStatus Status { get; set; }
    public virtual ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
}
