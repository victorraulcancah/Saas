namespace Backend.Domain.BI.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class MetricSnapshot : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string MetricKey { get; set; } = string.Empty;
    public string MetricName { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal Value { get; set; }
    public string Dimension { get; set; } = string.Empty;
    public string SourceSystem { get; set; } = string.Empty;
}
