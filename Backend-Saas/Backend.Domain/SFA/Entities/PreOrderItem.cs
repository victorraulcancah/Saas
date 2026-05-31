namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PreOrderItem : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid PreOrderId { get; set; }
    public virtual PreOrder PreOrder { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}
