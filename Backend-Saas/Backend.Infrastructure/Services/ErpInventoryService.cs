using Backend.Application.Common.Interfaces;
using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend.Domain.ERP.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class ErpInventoryService : IErpInventoryService
{
    private readonly AppDbContext _db;
    private readonly ErpStockService _stock;
    private readonly ICurrentUserService _currentUser;
    private readonly IRealtimeNotificationService _realtime;

    public ErpInventoryService(AppDbContext db, ErpStockService stock, ICurrentUserService currentUser, IRealtimeNotificationService realtime)
    {
        _db = db;
        _stock = stock;
        _currentUser = currentUser;
        _realtime = realtime;
    }

    public async Task<IEnumerable<InventoryMovementResponse>> GetMovementsAsync() =>
        (await _db.InventoryMovements.AsNoTracking().Include(m => m.Product).Include(m => m.Warehouse).OrderByDescending(m => m.CreatedAt).ToListAsync()).Select(MapMovement);

    public async Task<InventoryMovementResponse> CreateMovementAsync(InventoryMovementRequest request)
    {
        var product = await _db.Products.FindAsync(request.ProductId) ?? throw new KeyNotFoundException("Producto no encontrado");
        if (!await _db.Warehouses.AnyAsync(w => w.Id == request.WarehouseId)) throw new KeyNotFoundException("Almacén no encontrado");
        var movement = new InventoryMovement
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            WarehouseLocationId = request.WarehouseLocationId,
            Type = request.Type,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Reference = request.Reference ?? string.Empty,
            Notes = request.Notes ?? string.Empty,
            Reason = request.Reason ?? "Movimiento manual",
            LotNumber = request.LotNumber ?? string.Empty,
            ExpirationDate = request.ExpirationDate
        };
        if (request.Type == InventoryMovement.MovementType.Entry)
            await _stock.AddStockAsync(product, request.WarehouseId, request.WarehouseLocationId, request.LotNumber, request.ExpirationDate, request.Quantity);
        else if (request.Type == InventoryMovement.MovementType.Exit)
            await _stock.RemoveStockAsync(product, request.WarehouseId, request.WarehouseLocationId, request.LotNumber, request.ExpirationDate, request.Quantity);
        _db.InventoryMovements.Add(movement);
        await _db.SaveChangesAsync();
        await NotifyAsync("InventoryUpdated", new { type = "inventory_movement", movement.Id });
        return MapMovement(movement);
    }

    public async Task<IEnumerable<GoodsReceiptResponse>> GetGoodsReceiptsAsync() =>
        (await _db.GoodsReceipts.AsNoTracking().OrderByDescending(r => r.ReceiptDate).ToListAsync()).Select(MapReceipt);

    public async Task<GoodsReceiptResponse> CreateGoodsReceiptAsync(GoodsReceiptRequest request)
    {
        var receipt = new GoodsReceipt { Id = Guid.NewGuid(), ReceiptNumber = request.ReceiptNumber, PurchaseOrderId = request.PurchaseOrderId, WarehouseId = request.WarehouseId, Notes = request.Notes ?? string.Empty, Status = GoodsReceipt.GoodsReceiptStatus.Draft };
        foreach (var item in request.Items)
        {
            var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.ProductId) ?? throw new KeyNotFoundException($"Producto no encontrado: {item.ProductId}");
            receipt.Items.Add(new GoodsReceiptItem { Id = Guid.NewGuid(), ProductId = item.ProductId, ProductName = product.Name, WarehouseLocationId = item.WarehouseLocationId, LotNumber = item.LotNumber ?? string.Empty, ExpirationDate = item.ExpirationDate, Quantity = item.Quantity, UnitCost = item.UnitCost });
        }
        _db.GoodsReceipts.Add(receipt);
        await _db.SaveChangesAsync();
        return MapReceipt(receipt);
    }

    public async Task<GoodsReceiptResponse?> PostGoodsReceiptAsync(Guid id)
    {
        var receipt = await _db.GoodsReceipts.Include(r => r.Items).FirstOrDefaultAsync(r => r.Id == id);
        if (receipt is null) return null;
        if (receipt.Status != GoodsReceipt.GoodsReceiptStatus.Draft) throw new InvalidOperationException("Solo se puede publicar una recepción en borrador.");
        foreach (var item in receipt.Items)
        {
            var product = await _db.Products.FindAsync(item.ProductId) ?? throw new KeyNotFoundException($"Producto no encontrado: {item.ProductId}");
            await _stock.AddStockAsync(product, receipt.WarehouseId, item.WarehouseLocationId, item.LotNumber, item.ExpirationDate, item.Quantity);
            _db.InventoryMovements.Add(new InventoryMovement { Id = Guid.NewGuid(), ProductId = item.ProductId, WarehouseId = receipt.WarehouseId, WarehouseLocationId = item.WarehouseLocationId, Type = InventoryMovement.MovementType.Entry, Quantity = item.Quantity, UnitPrice = item.UnitCost, Reference = receipt.ReceiptNumber, Reason = "Recepción de mercadería", LotNumber = item.LotNumber, ExpirationDate = item.ExpirationDate });
        }
        receipt.Status = GoodsReceipt.GoodsReceiptStatus.Posted;
        receipt.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await NotifyAsync("InventoryUpdated", new { type = "goods_receipt", receipt.Id, receipt.ReceiptNumber });
        return MapReceipt(receipt);
    }

    public async Task<IEnumerable<WarehouseTransferResponse>> GetWarehouseTransfersAsync() =>
        (await _db.WarehouseTransfers.AsNoTracking().OrderByDescending(t => t.TransferDate).ToListAsync()).Select(MapTransfer);

    public async Task<WarehouseTransferResponse> CreateWarehouseTransferAsync(WarehouseTransferRequest request)
    {
        if (request.SourceWarehouseId == request.TargetWarehouseId) throw new InvalidOperationException("El almacén origen y destino deben ser diferentes.");
        var transfer = new WarehouseTransfer { Id = Guid.NewGuid(), TransferNumber = request.TransferNumber, SourceWarehouseId = request.SourceWarehouseId, TargetWarehouseId = request.TargetWarehouseId, Notes = request.Notes ?? string.Empty, Status = WarehouseTransfer.WarehouseTransferStatus.Draft };
        foreach (var item in request.Items)
        {
            var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.ProductId) ?? throw new KeyNotFoundException($"Producto no encontrado: {item.ProductId}");
            transfer.Items.Add(new WarehouseTransferItem { Id = Guid.NewGuid(), ProductId = item.ProductId, ProductName = product.Name, SourceLocationId = item.SourceLocationId, TargetLocationId = item.TargetLocationId, LotNumber = item.LotNumber ?? string.Empty, ExpirationDate = item.ExpirationDate, Quantity = item.Quantity });
        }
        _db.WarehouseTransfers.Add(transfer);
        await _db.SaveChangesAsync();
        return MapTransfer(transfer);
    }

    public Task<WarehouseTransferResponse?> SendWarehouseTransferAsync(Guid id) => MoveTransferAsync(id, true);
    public Task<WarehouseTransferResponse?> ReceiveWarehouseTransferAsync(Guid id) => MoveTransferAsync(id, false);

    public async Task<IEnumerable<StockAdjustmentResponse>> GetStockAdjustmentsAsync() =>
        (await _db.StockAdjustments.AsNoTracking().OrderByDescending(a => a.CreatedAt).ToListAsync()).Select(MapAdjustment);

    public async Task<StockAdjustmentResponse> CreateStockAdjustmentAsync(StockAdjustmentRequest request)
    {
        var product = await _db.Products.FindAsync(request.ProductId) ?? throw new KeyNotFoundException("Producto no encontrado");
        var adjustment = new StockAdjustment { Id = Guid.NewGuid(), AdjustmentNumber = request.AdjustmentNumber, ProductId = request.ProductId, WarehouseId = request.WarehouseId, WarehouseLocationId = request.WarehouseLocationId, LotNumber = request.LotNumber ?? string.Empty, ExpirationDate = request.ExpirationDate, Type = request.Type, Quantity = request.Quantity, Reason = request.Reason ?? string.Empty };
        var isEntry = request.Type is StockAdjustment.StockAdjustmentType.Entry or StockAdjustment.StockAdjustmentType.CustomerReturn or StockAdjustment.StockAdjustmentType.CountCorrection;
        if (isEntry) await _stock.AddStockAsync(product, request.WarehouseId, request.WarehouseLocationId, request.LotNumber, request.ExpirationDate, request.Quantity);
        else await _stock.RemoveStockAsync(product, request.WarehouseId, request.WarehouseLocationId, request.LotNumber, request.ExpirationDate, request.Quantity);
        _db.StockAdjustments.Add(adjustment);
        _db.InventoryMovements.Add(new InventoryMovement { Id = Guid.NewGuid(), ProductId = request.ProductId, WarehouseId = request.WarehouseId, WarehouseLocationId = request.WarehouseLocationId, Type = isEntry ? InventoryMovement.MovementType.Entry : InventoryMovement.MovementType.Exit, Quantity = request.Quantity, Reference = request.AdjustmentNumber, Reason = request.Type.ToString(), Notes = request.Reason ?? string.Empty, LotNumber = request.LotNumber ?? string.Empty, ExpirationDate = request.ExpirationDate });
        await _db.SaveChangesAsync();
        await NotifyAsync("InventoryUpdated", new { type = "stock_adjustment", adjustment.Id, adjustment.AdjustmentNumber });
        return MapAdjustment(adjustment);
    }

    private async Task<WarehouseTransferResponse?> MoveTransferAsync(Guid id, bool send)
    {
        var transfer = await _db.WarehouseTransfers.Include(t => t.Items).FirstOrDefaultAsync(t => t.Id == id);
        if (transfer is null) return null;
        if (send && transfer.Status != WarehouseTransfer.WarehouseTransferStatus.Draft) throw new InvalidOperationException("Solo se puede enviar un traslado en borrador.");
        if (!send && transfer.Status != WarehouseTransfer.WarehouseTransferStatus.Sent) throw new InvalidOperationException("Solo se puede recibir un traslado enviado.");
        foreach (var item in transfer.Items)
        {
            var product = await _db.Products.FindAsync(item.ProductId) ?? throw new KeyNotFoundException($"Producto no encontrado: {item.ProductId}");
            if (send)
            {
                await _stock.RemoveStockAsync(product, transfer.SourceWarehouseId, item.SourceLocationId, item.LotNumber, item.ExpirationDate, item.Quantity);
                _db.InventoryMovements.Add(new InventoryMovement { Id = Guid.NewGuid(), ProductId = item.ProductId, WarehouseId = transfer.SourceWarehouseId, WarehouseLocationId = item.SourceLocationId, Type = InventoryMovement.MovementType.Exit, Quantity = item.Quantity, Reference = transfer.TransferNumber, Reason = "Traslado entre almacenes", LotNumber = item.LotNumber, ExpirationDate = item.ExpirationDate });
            }
            else
            {
                await _stock.AddStockAsync(product, transfer.TargetWarehouseId, item.TargetLocationId, item.LotNumber, item.ExpirationDate, item.Quantity);
                _db.InventoryMovements.Add(new InventoryMovement { Id = Guid.NewGuid(), ProductId = item.ProductId, WarehouseId = transfer.TargetWarehouseId, WarehouseLocationId = item.TargetLocationId, Type = InventoryMovement.MovementType.Entry, Quantity = item.Quantity, Reference = transfer.TransferNumber, Reason = "Recepción de traslado", LotNumber = item.LotNumber, ExpirationDate = item.ExpirationDate });
            }
        }
        transfer.Status = send ? WarehouseTransfer.WarehouseTransferStatus.Sent : WarehouseTransfer.WarehouseTransferStatus.Received;
        transfer.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await NotifyAsync("InventoryUpdated", new { type = send ? "warehouse_transfer_sent" : "warehouse_transfer_received", transfer.Id, transfer.TransferNumber });
        return MapTransfer(transfer);
    }

    private async Task NotifyAsync(string eventName, object payload)
    {
        if (_currentUser.TenantId is { } tenantId) await _realtime.NotifyTenantAsync(tenantId, eventName, payload);
    }

    private static InventoryMovementResponse MapMovement(InventoryMovement m) => new(m.Id, m.ProductId, m.Product?.Name ?? string.Empty, m.WarehouseId, m.Warehouse?.Name ?? string.Empty, m.WarehouseLocationId, m.Type, m.Quantity, m.UnitPrice, m.Reference, m.Notes, m.Reason, m.LotNumber, m.ExpirationDate, m.CreatedAt);
    private static GoodsReceiptResponse MapReceipt(GoodsReceipt r) => new(r.Id, r.ReceiptNumber, r.PurchaseOrderId, r.WarehouseId, r.Status, r.ReceiptDate, r.Notes);
    private static WarehouseTransferResponse MapTransfer(WarehouseTransfer t) => new(t.Id, t.TransferNumber, t.SourceWarehouseId, t.TargetWarehouseId, t.Status, t.TransferDate, t.Notes);
    private static StockAdjustmentResponse MapAdjustment(StockAdjustment a) => new(a.Id, a.AdjustmentNumber, a.ProductId, a.WarehouseId, a.WarehouseLocationId, a.Type, a.Quantity, a.Reason);
}
