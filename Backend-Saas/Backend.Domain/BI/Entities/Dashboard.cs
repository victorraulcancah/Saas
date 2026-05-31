namespace Backend.Domain.BI.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Dashboard : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Layout { get; set; } = string.Empty; // JSON layout config
    public bool IsDefault { get; set; }
    public bool IsPublic { get; set; }
    public Guid CreatedBy { get; set; }
    public int RefreshIntervalMinutes { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<DashboardWidget> Widgets { get; set; } = new List<DashboardWidget>();
}
