namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class ProductStock : BaseEntity, ITenantEntity
{
    public enum StockCondition
    {
        Available,
        Reserved,
        Damaged,
        Expired,
        Quarantine
    }

    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public Guid? WarehouseLocationId { get; set; }
    public virtual WarehouseLocation? WarehouseLocation { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public DateTime? ExpirationDate { get; set; }
    public int Quantity { get; set; }
    public StockCondition Condition { get; set; } = StockCondition.Available;
}
