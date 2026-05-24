namespace Backend.Domain.HR.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class Payroll : BaseEntity, ITenantEntity
{
    public enum PayrollStatus
    {
        Draft,
        Calculated,
        Paid
    }

    public Guid? TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal Bonuses { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public PayrollStatus Status { get; set; }
    public DateTime? PaymentDate { get; set; }
}
