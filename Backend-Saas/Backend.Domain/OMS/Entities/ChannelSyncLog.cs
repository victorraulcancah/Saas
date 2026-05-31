namespace Backend.Domain.OMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ChannelSyncLog : BaseEntity, ITenantEntity
{
    public enum SyncStatus
    {
        Success,
        Failed,
        Partial
    }

    public Guid? TenantId { get; set; }
    public Guid SalesChannelId { get; set; }
    public virtual SalesChannel SalesChannel { get; set; } = null!;
    public string SyncType { get; set; } = string.Empty; // "Orders", "Products", "Inventory"
    public SyncStatus Status { get; set; }
    public int RecordsProcessed { get; set; }
    public int RecordsSuccess { get; set; }
    public int RecordsFailed { get; set; }
    public DateTime SyncStartedAt { get; set; }
    public DateTime? SyncCompletedAt { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty; // JSON details
}
