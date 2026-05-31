namespace Backend.Domain.BI.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class KpiDefinition : BaseEntity, ITenantEntity
{
    public enum KpiCategory
    {
        Sales,
        Inventory,
        Financial,
        Operations,
        HR,
        Customer
    }

    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public KpiCategory Category { get; set; }
    public string Formula { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty; // "%", "$", "units", etc.
    public decimal? TargetValue { get; set; }
    public decimal? WarningThreshold { get; set; }
    public decimal? CriticalThreshold { get; set; }
    public string DataSource { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
