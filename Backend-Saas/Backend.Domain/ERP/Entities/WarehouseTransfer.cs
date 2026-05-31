namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class WarehouseTransfer : BaseEntity, ITenantEntity
{
    public enum WarehouseTransferStatus
    {
        Draft,
        Sent,
        Received,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string TransferNumber { get; set; } = string.Empty;
    public Guid SourceWarehouseId { get; set; }
    public virtual Warehouse SourceWarehouse { get; set; } = null!;
    public Guid TargetWarehouseId { get; set; }
    public virtual Warehouse TargetWarehouse { get; set; } = null!;
    public WarehouseTransferStatus Status { get; set; } = WarehouseTransferStatus.Draft;
    public DateTime TransferDate { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; } = string.Empty;
    public virtual ICollection<WarehouseTransferItem> Items { get; set; } = new List<WarehouseTransferItem>();
}
