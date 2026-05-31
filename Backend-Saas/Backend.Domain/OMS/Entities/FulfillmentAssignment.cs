namespace Backend.Domain.OMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class FulfillmentAssignment : BaseEntity, ITenantEntity
{
    public enum FulfillmentAssignmentStatus
    {
        Assigned,
        Picking,
        Ready,
        Dispatched,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public Guid OmnichannelOrderId { get; set; }
    public virtual OmnichannelOrder OmnichannelOrder { get; set; } = null!;
    public Guid WarehouseId { get; set; }
    public Guid? BranchId { get; set; }
    public string FulfillmentType { get; set; } = string.Empty;
    public FulfillmentAssignmentStatus Status { get; set; } = FulfillmentAssignmentStatus.Assigned;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; } = string.Empty;
}
