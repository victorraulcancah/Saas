namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ReplenishmentOrderItem : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid ReplenishmentOrderId { get; set; }
    public virtual ReplenishmentOrder ReplenishmentOrder { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantityRequested { get; set; }
    public int QuantityApproved { get; set; }
    public int QuantityShipped { get; set; }
}
