namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class WarehouseLocation : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public string Code { get; set; } = string.Empty;
    public string Aisle { get; set; } = string.Empty;
    public string Rack { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
}
