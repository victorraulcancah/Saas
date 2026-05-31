namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class DispatchGuide : BaseEntity, ITenantEntity
{
    public enum DispatchGuideStatus
    {
        Draft,
        Issued,
        Accepted,
        Rejected,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string Series { get; set; } = string.Empty;
    public string Correlative { get; set; } = string.Empty;
    public string GuideNumber { get; set; } = string.Empty;
    public string ReasonCode { get; set; } = string.Empty;
    public string ReasonDescription { get; set; } = string.Empty;
    public Guid SourceWarehouseId { get; set; }
    public virtual Warehouse SourceWarehouse { get; set; } = null!;
    public string DestinationAddress { get; set; } = string.Empty;
    public string DestinationUbigeo { get; set; } = string.Empty;
    public string TransportistName { get; set; } = string.Empty;
    public string TransportistDocument { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DispatchGuideStatus Status { get; set; } = DispatchGuideStatus.Draft;
    public virtual ICollection<DispatchGuideItem> Items { get; set; } = new List<DispatchGuideItem>();
}
