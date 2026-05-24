namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class InventoryMovement : BaseEntity, ITenantEntity
{
    public enum MovementType
    {
        Entry,
        Exit,
        Transfer
    }

    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public MovementType Type { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
