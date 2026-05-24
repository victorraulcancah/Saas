namespace Backend.Domain.HR.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class Employee : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public string BankAccount { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
