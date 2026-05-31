namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class WarehouseZone : BaseEntity, ITenantEntity
{
    public enum ZoneType
    {
        Receiving,
        Storage,
        Picking,
        Packing,
        Shipping,
        Returns,
        Quarantine
    }

    public Guid? TenantId { get; set; }
    public Guid WarehouseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public ZoneType Type { get; set; }
    public decimal Area { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;
}
