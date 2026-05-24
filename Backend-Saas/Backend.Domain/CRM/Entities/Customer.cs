namespace Backend.Domain.CRM.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class Customer : BaseEntity, ITenantEntity
{
    public enum CustomerType
    {
        Person,
        Company
    }

    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; } = true;
    public string Notes { get; set; } = string.Empty;
}
