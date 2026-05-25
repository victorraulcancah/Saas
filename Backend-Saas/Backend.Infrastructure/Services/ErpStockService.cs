using Backend.Domain.ERP.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class ErpStockService
{
    private readonly AppDbContext _db;

    public ErpStockService(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddStockAsync(Product product, Guid warehouseId, Guid? locationId, string? lotNumber, DateTime? expirationDate, int quantity, ProductStock.StockCondition condition = ProductStock.StockCondition.Available)
    {
        var normalizedLot = lotNumber ?? string.Empty;
        var stock = await _db.ProductStocks.FirstOrDefaultAsync(s =>
            s.ProductId == product.Id &&
            s.WarehouseId == warehouseId &&
            s.WarehouseLocationId == locationId &&
            s.LotNumber == normalizedLot &&
            s.ExpirationDate == expirationDate &&
            s.Condition == condition);

        if (stock is null)
        {
            stock = new ProductStock
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                WarehouseId = warehouseId,
                WarehouseLocationId = locationId,
                LotNumber = normalizedLot,
                ExpirationDate = expirationDate,
                Quantity = 0,
                Condition = condition
            };
            _db.ProductStocks.Add(stock);
        }

        stock.Quantity += quantity;
        product.Stock += quantity;
    }

    public async Task RemoveStockAsync(Product product, Guid warehouseId, Guid? locationId, string? lotNumber, DateTime? expirationDate, int quantity)
    {
        var normalizedLot = lotNumber ?? string.Empty;
        var stock = await _db.ProductStocks.FirstOrDefaultAsync(s =>
            s.ProductId == product.Id &&
            s.WarehouseId == warehouseId &&
            s.WarehouseLocationId == locationId &&
            s.LotNumber == normalizedLot &&
            s.ExpirationDate == expirationDate &&
            s.Condition == ProductStock.StockCondition.Available);

        if (stock is null || stock.Quantity < quantity)
            throw new InvalidOperationException("Stock insuficiente para la operación solicitada.");

        stock.Quantity -= quantity;
        product.Stock -= quantity;
    }
}
