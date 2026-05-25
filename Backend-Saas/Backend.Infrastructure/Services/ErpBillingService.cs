using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend.Domain.ERP.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class ErpBillingService : IErpBillingService
{
    private readonly AppDbContext _db;

    public ErpBillingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync() =>
        (await _db.Invoices.AsNoTracking().OrderByDescending(i => i.IssueDate).ToListAsync()).Select(MapInvoice);

    public async Task<InvoiceResponse> CreateInvoiceAsync(InvoiceRequest request)
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Series = request.Series,
            Correlative = request.Correlative,
            InvoiceNumber = $"{request.Series}-{request.Correlative}",
            CustomerName = request.CustomerName,
            CustomerTaxId = request.CustomerTaxId,
            IssueDate = DateTime.UtcNow,
            TotalAmount = request.TotalAmount,
            TaxAmount = request.TaxAmount,
            Currency = request.Currency ?? "PEN",
            Status = Invoice.InvoiceStatus.Draft
        };
        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();
        return MapInvoice(invoice);
    }

    public async Task<InvoiceResponse?> MarkInvoiceSentAsync(Guid id)
    {
        var invoice = await _db.Invoices.FindAsync(id);
        if (invoice is null) return null;
        invoice.Status = Invoice.InvoiceStatus.Sent;
        invoice.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapInvoice(invoice);
    }

    public async Task<IEnumerable<DispatchGuideResponse>> GetDispatchGuidesAsync() =>
        (await _db.DispatchGuides.AsNoTracking().OrderByDescending(g => g.IssueDate).ToListAsync()).Select(MapGuide);

    public async Task<DispatchGuideResponse> CreateDispatchGuideAsync(DispatchGuideRequest request)
    {
        var guide = new DispatchGuide
        {
            Id = Guid.NewGuid(),
            Series = request.Series,
            Correlative = request.Correlative,
            GuideNumber = $"{request.Series}-{request.Correlative}",
            ReasonCode = request.ReasonCode,
            ReasonDescription = request.ReasonDescription,
            SourceWarehouseId = request.SourceWarehouseId,
            DestinationAddress = request.DestinationAddress,
            DestinationUbigeo = request.DestinationUbigeo,
            TransportistName = request.TransportistName ?? string.Empty,
            TransportistDocument = request.TransportistDocument ?? string.Empty,
            Status = DispatchGuide.DispatchGuideStatus.Draft
        };

        foreach (var item in request.Items)
        {
            var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.ProductId)
                ?? throw new KeyNotFoundException($"Producto no encontrado: {item.ProductId}");
            guide.Items.Add(new DispatchGuideItem { Id = Guid.NewGuid(), ProductId = product.Id, ProductName = product.Name, Quantity = item.Quantity, UnitOfMeasure = item.UnitOfMeasure ?? "NIU" });
        }

        _db.DispatchGuides.Add(guide);
        await _db.SaveChangesAsync();
        return MapGuide(guide);
    }

    public async Task<DispatchGuideResponse?> IssueDispatchGuideAsync(Guid id)
    {
        var guide = await _db.DispatchGuides.FindAsync(id);
        if (guide is null) return null;
        guide.Status = DispatchGuide.DispatchGuideStatus.Issued;
        guide.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapGuide(guide);
    }

    private static InvoiceResponse MapInvoice(Invoice i) => new(i.Id, i.InvoiceNumber, i.Type, i.Series, i.Correlative, i.CustomerName, i.CustomerTaxId, i.IssueDate, i.TotalAmount, i.TaxAmount, i.Currency, i.Status);
    private static DispatchGuideResponse MapGuide(DispatchGuide g) => new(g.Id, g.GuideNumber, g.Series, g.Correlative, g.ReasonCode, g.ReasonDescription, g.SourceWarehouseId, g.DestinationAddress, g.Status);
}
