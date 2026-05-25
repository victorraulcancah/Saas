using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend.Domain.ERP.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class ErpPurchasingService : IErpPurchasingService
{
    private readonly AppDbContext _db;

    public ErpPurchasingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrdersAsync() =>
        (await _db.PurchaseOrders.AsNoTracking().OrderByDescending(o => o.CreatedAt).ToListAsync()).Select(Map);

    public async Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(PurchaseOrderRequest request)
    {
        var supplier = await _db.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == request.SupplierId)
            ?? throw new KeyNotFoundException("Proveedor no encontrado");

        var order = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = request.OrderNumber,
            SupplierId = request.SupplierId,
            SupplierName = string.IsNullOrWhiteSpace(request.SupplierName) ? supplier.Name : request.SupplierName,
            Status = PurchaseOrder.PurchaseOrderStatus.Draft,
            TotalAmount = request.TotalAmount,
            Notes = request.Notes ?? string.Empty
        };

        _db.PurchaseOrders.Add(order);
        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<PurchaseOrderResponse?> ApprovePurchaseOrderAsync(Guid id)
    {
        var order = await _db.PurchaseOrders.FindAsync(id);
        if (order is null) return null;
        order.Status = PurchaseOrder.PurchaseOrderStatus.Approved;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(order);
    }

    private static PurchaseOrderResponse Map(PurchaseOrder o) => new(o.Id, o.OrderNumber, o.SupplierId, o.SupplierName, o.Status, o.TotalAmount, o.Notes);
}
