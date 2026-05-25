namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class StockAdjustment : BaseEntity, ITenantEntity
{
    public enum StockAdjustmentType
    {
        Entry,
        Exit,
        Loss,
        Damaged,
        Expired,
        CustomerReturn,
        SupplierReturn,
        CountCorrection
    }

    public Guid? TenantId { get; set; }
    public string AdjustmentNumber { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public Guid? WarehouseLocationId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public DateTime? ExpirationDate { get; set; }
    public StockAdjustmentType Type { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}
