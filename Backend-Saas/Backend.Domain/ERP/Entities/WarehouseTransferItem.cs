namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class WarehouseTransferItem : BaseEntity, ISoftDelete
{
    public Guid WarehouseTransferId { get; set; }
    public virtual WarehouseTransfer WarehouseTransfer { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? SourceLocationId { get; set; }
    public Guid? TargetLocationId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public DateTime? ExpirationDate { get; set; }
    public int Quantity { get; set; }
}
