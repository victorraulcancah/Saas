using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantBranch
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? SunatEstablishmentCode { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Ubigeo { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public ICollection<InvoiceSeries> InvoiceSeries { get; set; } = new List<InvoiceSeries>();
}
