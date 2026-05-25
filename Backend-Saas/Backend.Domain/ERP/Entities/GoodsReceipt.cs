namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class GoodsReceipt : BaseEntity, ITenantEntity
{
    public enum GoodsReceiptStatus
    {
        Draft,
        Posted,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid? PurchaseOrderId { get; set; }
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;
    public GoodsReceiptStatus Status { get; set; } = GoodsReceiptStatus.Draft;
    public string Notes { get; set; } = string.Empty;
    public virtual ICollection<GoodsReceiptItem> Items { get; set; } = new List<GoodsReceiptItem>();
}
