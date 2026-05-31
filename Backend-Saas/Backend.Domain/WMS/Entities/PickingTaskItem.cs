namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PickingTaskItem : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid PickingTaskId { get; set; }
    public virtual PickingTask PickingTask { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public int QuantityRequested { get; set; }
    public int QuantityPicked { get; set; }
    public bool IsCompleted { get; set; }
}
