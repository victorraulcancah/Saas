using Backend.Domain.Common;

namespace Backend.Domain.Saas.Entities;

public class TenantCompanyProfile
{
    public Guid TenantId { get; set; }
    public string Ruc { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }
    public string? EmailFacturacion { get; set; }
    public string? Phone { get; set; }
    public string DireccionFiscal { get; set; } = string.Empty;
    public string Ubigeo { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string? RepresentanteLegal { get; set; }
    public string? TipoContribuyente { get; set; }
    public string? RegimenTributario { get; set; }
    public string? EstadoSunat { get; set; }
    public string? CondicionSunat { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
}
