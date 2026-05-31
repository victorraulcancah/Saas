namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

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
    public Guid? WarehouseLocationId { get; set; }
    public virtual WarehouseLocation? WarehouseLocation { get; set; }
    public MovementType Type { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string LotNumber { get; set; } = string.Empty;
    public DateTime? ExpirationDate { get; set; }
}
