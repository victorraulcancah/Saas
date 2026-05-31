using Backend.Domain.SFA.Entities;

namespace Backend.Application.SFA.Models;

public record FieldOrderRequest(string OrderNumber, Guid CustomerId, Guid SalespersonId, DateTime VisitDate, decimal TotalAmount, bool IsOfflineCapture, string? Notes);
public record FieldOrderStatusRequest(FieldOrder.FieldOrderStatus Status);
public record FieldOrderResponse(Guid Id, string OrderNumber, Guid CustomerId, Guid SalespersonId, DateTime VisitDate, FieldOrder.FieldOrderStatus Status, decimal TotalAmount, bool IsOfflineCapture, string Notes);
