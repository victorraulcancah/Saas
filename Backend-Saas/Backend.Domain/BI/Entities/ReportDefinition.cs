namespace Backend.Domain.BI.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ReportDefinition : BaseEntity, ITenantEntity
{
    public enum ReportType
    {
        Sales,
        Inventory,
        Financial,
        HR,
        Custom
    }

    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ReportType Type { get; set; }
    public string DataSource { get; set; } = string.Empty;
    public string QueryDefinition { get; set; } = string.Empty; // JSON query config
    public string VisualizationType { get; set; } = string.Empty; // "Table", "Chart", "Dashboard"
    public string Parameters { get; set; } = string.Empty; // JSON parameters
    public bool IsPublic { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
