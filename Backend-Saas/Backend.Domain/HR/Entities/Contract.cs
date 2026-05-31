namespace Backend.Domain.HR.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Contract : BaseEntity, ITenantEntity
{
    public enum ContractType
    {
        FullTime,
        PartTime,
        Temporary,
        Freelance,
        Internship
    }

    public enum ContractStatus
    {
        Active,
        Expired,
        Terminated,
        Suspended
    }

    public Guid? TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public string ContractNumber { get; set; } = string.Empty;
    public ContractType Type { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Active;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Salary { get; set; }
    public string Currency { get; set; } = "PEN";
    public int WorkHoursPerWeek { get; set; }
    public string Terms { get; set; } = string.Empty;
    public DateTime? TerminationDate { get; set; }
    public string? TerminationReason { get; set; }
}
