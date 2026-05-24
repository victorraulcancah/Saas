namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class Warehouse : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public virtual ICollection<WarehouseLocation> WarehouseLocations { get; set; } = new List<WarehouseLocation>();
}
