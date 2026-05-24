namespace Backend.Domain.HR.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class Attendance : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public bool IsAbsent { get; set; }
    public string Justification { get; set; } = string.Empty;
}
