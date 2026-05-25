using Backend.Application.ERP.Models;

namespace Backend.Application.ERP.Services;

public interface IErpInventoryService
{
    Task<IEnumerable<InventoryMovementResponse>> GetMovementsAsync();
    Task<InventoryMovementResponse> CreateMovementAsync(InventoryMovementRequest request);
    Task<IEnumerable<GoodsReceiptResponse>> GetGoodsReceiptsAsync();
    Task<GoodsReceiptResponse> CreateGoodsReceiptAsync(GoodsReceiptRequest request);
    Task<GoodsReceiptResponse?> PostGoodsReceiptAsync(Guid id);
    Task<IEnumerable<WarehouseTransferResponse>> GetWarehouseTransfersAsync();
    Task<WarehouseTransferResponse> CreateWarehouseTransferAsync(WarehouseTransferRequest request);
    Task<WarehouseTransferResponse?> SendWarehouseTransferAsync(Guid id);
    Task<WarehouseTransferResponse?> ReceiveWarehouseTransferAsync(Guid id);
    Task<IEnumerable<StockAdjustmentResponse>> GetStockAdjustmentsAsync();
    Task<StockAdjustmentResponse> CreateStockAdjustmentAsync(StockAdjustmentRequest request);
}
