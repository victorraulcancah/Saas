namespace Backend.Domain.ERP.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class DispatchGuideItem : BaseEntity, ISoftDelete
{
    public Guid DispatchGuideId { get; set; }
    public virtual DispatchGuide DispatchGuide { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = "NIU";
}
