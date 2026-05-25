namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class GoodsReceiptItem : BaseEntity, ISoftDelete
{
    public Guid GoodsReceiptId { get; set; }
    public virtual GoodsReceipt GoodsReceipt { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? WarehouseLocationId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public DateTime? ExpirationDate { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
}
