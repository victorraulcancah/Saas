using Backend.Domain.POS.Entities;

namespace Backend.Application.POS.Models;

public record PosSaleItemRequest(Guid ProductId, string ProductName, string? Sku, int Quantity, decimal UnitPrice, decimal DiscountAmount);

public record PosSaleRequest(
    string SaleNumber,
    string? CashRegisterCode,
    string? StoreCode,
    string? CustomerName,
    string? CustomerDocument,
    PosSale.PaymentMethod Method,
    List<PosSaleItemRequest> Items);

public record PosSaleItemResponse(Guid Id, Guid ProductId, string ProductName, string Sku, int Quantity, decimal UnitPrice, decimal DiscountAmount, decimal TotalAmount);

public record PosSaleResponse(
    Guid Id,
    string SaleNumber,
    DateTime SaleDate,
    string CashRegisterCode,
    string StoreCode,
    string CustomerName,
    string CustomerDocument,
    decimal SubTotal,
    decimal TaxAmount,
    decimal DiscountAmount,
    decimal TotalAmount,
    PosSale.PaymentMethod Method,
    PosSale.SaleStatus Status,
    IReadOnlyCollection<PosSaleItemResponse> Items);
