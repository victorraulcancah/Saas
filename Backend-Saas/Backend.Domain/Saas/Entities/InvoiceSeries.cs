using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class InvoiceSeries
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? BranchId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public long CurrentNumber { get; set; }
    public bool IsElectronic { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public TenantBranch? Branch { get; set; }
}
