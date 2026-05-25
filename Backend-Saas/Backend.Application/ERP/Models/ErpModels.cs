using Backend.Domain.ERP.Entities;

namespace Backend.Application.ERP.Models;

public record CategoryRequest(string Name, string? Description, Guid? ParentId);
public record CategoryResponse(Guid Id, string Name, string Description, Guid? ParentId);

public record ProductRequest(string Name, string? Description, string Sku, string? Barcode, decimal UnitPrice, decimal CostPrice, Guid? CategoryId, int Stock, int MinStock, int MaxStock);
public record ProductResponse(Guid Id, string Name, string Description, string Sku, string Barcode, decimal UnitPrice, decimal CostPrice, Guid? CategoryId, string? CategoryName, int Stock, int MinStock, int MaxStock, bool IsActive);

public record SupplierRequest(string Name, string TaxId, string? Email, string? Phone, string? Address);
public record SupplierResponse(Guid Id, string Name, string TaxId, string Email, string Phone, string Address, bool IsActive);

public record WarehouseRequest(string Name, string Code, string? Address, string? City);
public record WarehouseResponse(Guid Id, string Name, string Code, string Address, string City, bool IsActive);

public record WarehouseLocationRequest(Guid WarehouseId, string Code, string? Aisle, string? Rack, string? Level);
public record WarehouseLocationResponse(Guid Id, Guid WarehouseId, string Code, string Aisle, string Rack, string Level);

public record ProductStockResponse(Guid Id, Guid ProductId, string ProductName, Guid WarehouseId, string WarehouseName, Guid? WarehouseLocationId, string? LocationCode, string LotNumber, DateTime? ExpirationDate, int Quantity, ProductStock.StockCondition Condition);

public record InventoryMovementRequest(Guid ProductId, Guid WarehouseId, Guid? WarehouseLocationId, InventoryMovement.MovementType Type, int Quantity, decimal UnitPrice, string? Reference, string? Notes, string? Reason, string? LotNumber, DateTime? ExpirationDate);
public record InventoryMovementResponse(Guid Id, Guid ProductId, string ProductName, Guid WarehouseId, string WarehouseName, Guid? WarehouseLocationId, InventoryMovement.MovementType Type, int Quantity, decimal UnitPrice, string Reference, string Notes, string Reason, string LotNumber, DateTime? ExpirationDate, DateTime CreatedAt);

public record GoodsReceiptItemRequest(Guid ProductId, Guid? WarehouseLocationId, string? LotNumber, DateTime? ExpirationDate, int Quantity, decimal UnitCost);
public record GoodsReceiptRequest(string ReceiptNumber, Guid? PurchaseOrderId, Guid WarehouseId, string? Notes, List<GoodsReceiptItemRequest> Items);
public record GoodsReceiptResponse(Guid Id, string ReceiptNumber, Guid? PurchaseOrderId, Guid WarehouseId, GoodsReceipt.GoodsReceiptStatus Status, DateTime ReceiptDate, string Notes);

public record WarehouseTransferItemRequest(Guid ProductId, Guid? SourceLocationId, Guid? TargetLocationId, string? LotNumber, DateTime? ExpirationDate, int Quantity);
public record WarehouseTransferRequest(string TransferNumber, Guid SourceWarehouseId, Guid TargetWarehouseId, string? Notes, List<WarehouseTransferItemRequest> Items);
public record WarehouseTransferResponse(Guid Id, string TransferNumber, Guid SourceWarehouseId, Guid TargetWarehouseId, WarehouseTransfer.WarehouseTransferStatus Status, DateTime TransferDate, string Notes);

public record StockAdjustmentRequest(string AdjustmentNumber, Guid ProductId, Guid WarehouseId, Guid? WarehouseLocationId, string? LotNumber, DateTime? ExpirationDate, StockAdjustment.StockAdjustmentType Type, int Quantity, string? Reason);
public record StockAdjustmentResponse(Guid Id, string AdjustmentNumber, Guid ProductId, Guid WarehouseId, Guid? WarehouseLocationId, StockAdjustment.StockAdjustmentType Type, int Quantity, string Reason);

public record DispatchGuideItemRequest(Guid ProductId, int Quantity, string? UnitOfMeasure);
public record DispatchGuideRequest(string Series, string Correlative, string ReasonCode, string ReasonDescription, Guid SourceWarehouseId, string DestinationAddress, string DestinationUbigeo, string? TransportistName, string? TransportistDocument, List<DispatchGuideItemRequest> Items);
public record DispatchGuideResponse(Guid Id, string GuideNumber, string Series, string Correlative, string ReasonCode, string ReasonDescription, Guid SourceWarehouseId, string DestinationAddress, DispatchGuide.DispatchGuideStatus Status);

public record InvoiceRequest(Invoice.InvoiceType Type, string Series, string Correlative, string CustomerName, string CustomerTaxId, decimal TotalAmount, decimal TaxAmount, string? Currency);
public record InvoiceResponse(Guid Id, string InvoiceNumber, Invoice.InvoiceType Type, string Series, string Correlative, string CustomerName, string CustomerTaxId, DateTime IssueDate, decimal TotalAmount, decimal TaxAmount, string Currency, Invoice.InvoiceStatus Status);

public record PurchaseOrderRequest(string OrderNumber, Guid SupplierId, string? SupplierName, decimal TotalAmount, string? Notes);
public record PurchaseOrderResponse(Guid Id, string OrderNumber, Guid SupplierId, string SupplierName, PurchaseOrder.PurchaseOrderStatus Status, decimal TotalAmount, string Notes);
