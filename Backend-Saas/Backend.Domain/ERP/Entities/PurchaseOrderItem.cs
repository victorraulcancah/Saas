namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class PurchaseOrderItem : BaseEntity, ISoftDelete
{
    public Guid PurchaseOrderId { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
