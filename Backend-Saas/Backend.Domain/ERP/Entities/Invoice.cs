namespace Backend.Domain.ERP.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Invoice : BaseEntity, ITenantEntity
{
    public enum InvoiceType
    {
        Factura,
        Boleta,
        NotaCredito,
        NotaDebito
    }

    public enum InvoiceStatus
    {
        Draft,
        Sent,
        Accepted,
        Rejected,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public InvoiceType Type { get; set; }
    public string Series { get; set; } = string.Empty;
    public string Correlative { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerTaxId { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string CdrCode { get; set; } = string.Empty;
    public string CdrDescription { get; set; } = string.Empty;
    public InvoiceStatus Status { get; set; }
    public string XmlContent { get; set; } = string.Empty;
    public string PdfPath { get; set; } = string.Empty;
    public string? SatConfig { get; set; }
}
