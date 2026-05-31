using Backend.Application.POS.Models;
using Backend.Application.POS.Services;
using Backend.Domain.POS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class PosSalesService : IPosSalesService
{
    private const decimal DefaultTaxRate = 0.18m;
    private readonly AppDbContext _db;

    public PosSalesService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PosSaleResponse>> GetSalesAsync()
    {
        var sales = await _db.PosSales
            .AsNoTracking()
            .Include(s => s.Items)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

        return sales.Select(MapSale);
    }

    public async Task<PosSaleResponse> CreateSaleAsync(PosSaleRequest request)
    {
        if (request.Items.Count == 0)
            throw new InvalidOperationException("La venta debe tener al menos un item.");

        var sale = new PosSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = request.SaleNumber,
            SaleDate = DateTime.UtcNow,
            CashRegisterCode = request.CashRegisterCode ?? string.Empty,
            StoreCode = request.StoreCode ?? string.Empty,
            CustomerName = request.CustomerName ?? string.Empty,
            CustomerDocument = request.CustomerDocument ?? string.Empty,
            Method = request.Method,
            Status = PosSale.SaleStatus.Draft
        };

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            if (item.UnitPrice < 0)
                throw new InvalidOperationException("El precio unitario no puede ser negativo.");

            var lineTotal = item.Quantity * item.UnitPrice - item.DiscountAmount;
            if (lineTotal < 0)
                throw new InvalidOperationException("El descuento no puede superar el importe del item.");

            sale.Items.Add(new PosSaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Sku = item.Sku ?? string.Empty,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                TotalAmount = lineTotal
            });
        }

        sale.SubTotal = sale.Items.Sum(i => i.TotalAmount);
        sale.TaxAmount = Math.Round(sale.SubTotal * DefaultTaxRate, 2);
        sale.DiscountAmount = sale.Items.Sum(i => i.DiscountAmount);
        sale.TotalAmount = sale.SubTotal + sale.TaxAmount;

        _db.PosSales.Add(sale);
        await _db.SaveChangesAsync();
        return MapSale(sale);
    }

    public async Task<PosSaleResponse?> CompleteSaleAsync(Guid id)
    {
        var sale = await _db.PosSales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        if (sale is null) return null;
        if (sale.Status != PosSale.SaleStatus.Draft)
            throw new InvalidOperationException("Solo se puede completar una venta en borrador.");

        sale.Status = PosSale.SaleStatus.Completed;
        sale.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapSale(sale);
    }

    public async Task<PosSaleResponse?> CancelSaleAsync(Guid id)
    {
        var sale = await _db.PosSales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        if (sale is null) return null;
        if (sale.Status == PosSale.SaleStatus.Cancelled)
            return MapSale(sale);

        sale.Status = PosSale.SaleStatus.Cancelled;
        sale.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapSale(sale);
    }

    private static PosSaleResponse MapSale(PosSale sale) => new(
        sale.Id,
        sale.SaleNumber,
        sale.SaleDate,
        sale.CashRegisterCode,
        sale.StoreCode,
        sale.CustomerName,
        sale.CustomerDocument,
        sale.SubTotal,
        sale.TaxAmount,
        sale.DiscountAmount,
        sale.TotalAmount,
        sale.Method,
        sale.Status,
        sale.Items.Select(MapItem).ToList());

    private static PosSaleItemResponse MapItem(PosSaleItem item) => new(
        item.Id,
        item.ProductId,
        item.ProductName,
        item.Sku,
        item.Quantity,
        item.UnitPrice,
        item.DiscountAmount,
        item.TotalAmount);
}
