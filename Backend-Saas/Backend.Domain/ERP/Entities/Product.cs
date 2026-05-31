namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Product : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal CostPrice { get; set; }
    public Guid? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public int MaxStock { get; set; }
    public bool IsActive { get; set; } = true;
}
