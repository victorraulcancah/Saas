namespace Backend.Domain.OMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class OmnichannelOrderItem : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid OmnichannelOrderId { get; set; }
    public virtual OmnichannelOrder OmnichannelOrder { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
