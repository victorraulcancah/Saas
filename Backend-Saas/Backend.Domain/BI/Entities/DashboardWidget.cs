namespace Backend.Domain.BI.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class DashboardWidget : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid DashboardId { get; set; }
    public virtual Dashboard Dashboard { get; set; } = null!;
    public Guid? ReportDefinitionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string WidgetType { get; set; } = string.Empty; // "Chart", "KPI", "Table", "Gauge"
    public string Configuration { get; set; } = string.Empty; // JSON config
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int DisplayOrder { get; set; }
}
