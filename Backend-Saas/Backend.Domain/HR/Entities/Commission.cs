namespace Backend.Domain.HR.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Commission : BaseEntity, ITenantEntity
{
    public enum CommissionStatus
    {
        Pending,
        Calculated,
        Paid
    }

    public Guid? TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public string SourceType { get; set; } = string.Empty; // "Sale", "Order", "Target"
    public Guid? SourceId { get; set; } // ID de la venta, pedido, etc.
    public decimal BaseAmount { get; set; } // Monto base sobre el que se calcula
    public decimal CommissionRate { get; set; } // Porcentaje de comisión
    public decimal CommissionAmount { get; set; } // Monto calculado
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public CommissionStatus Status { get; set; } = CommissionStatus.Pending;
    public DateTime? PaidAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}
