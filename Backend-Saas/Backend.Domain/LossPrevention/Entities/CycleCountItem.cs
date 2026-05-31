namespace Backend.Domain.LossPrevention.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class CycleCountItem : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid CycleCountId { get; set; }
    public virtual CycleCount CycleCount { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public int ExpectedQuantity { get; set; }
    public int CountedQuantity { get; set; }
    public int Variance { get; set; }
    public decimal VarianceValue { get; set; }
    public string Notes { get; set; } = string.Empty;
}
