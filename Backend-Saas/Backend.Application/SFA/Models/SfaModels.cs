using Backend.Domain.SFA.Entities;

namespace Backend.Application.SFA.Models;

// Field Orders
public record FieldOrderRequest(string OrderNumber, Guid CustomerId, Guid SalespersonId, DateTime VisitDate, decimal TotalAmount, bool IsOfflineCapture, string? Notes);
public record FieldOrderResponse(Guid Id, string OrderNumber, Guid CustomerId, Guid SalespersonId, DateTime VisitDate, FieldOrder.FieldOrderStatus Status, decimal TotalAmount, bool IsOfflineCapture, string Notes);
public record FieldOrderStatusRequest(FieldOrder.FieldOrderStatus Status, string? Notes);

// Field Visits
public record FieldVisitRequest(Guid SalesRepId, Guid CustomerId, DateTime ScheduledDate, string Purpose, string? Notes);
public record FieldVisitResponse(Guid Id, string VisitNumber, Guid SalesRepId, Guid CustomerId, DateTime ScheduledDate, DateTime? CheckInTime, DateTime? CheckOutTime, FieldVisit.VisitStatus Status, string Purpose, string Notes);

// Pre-Orders
public record PreOrderItemRequest(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal Discount);
public record PreOrderRequest(Guid SalesRepId, Guid CustomerId, Guid? FieldVisitId, string PaymentTerms, DateTime? DeliveryDate, List<PreOrderItemRequest> Items, string? Notes);
public record PreOrderItemResponse(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal Discount, decimal TotalPrice);
public record PreOrderResponse(Guid Id, string PreOrderNumber, Guid SalesRepId, Guid CustomerId, DateTime OrderDate, PreOrder.PreOrderStatus Status, decimal TotalAmount, string PaymentTerms, DateTime? DeliveryDate, IReadOnlyCollection<PreOrderItemResponse> Items);

// Field Collections
public record FieldCollectionRequest(Guid SalesRepId, Guid CustomerId, Guid? FieldVisitId, decimal Amount, string PaymentMethod, string? ReferenceNumber, string? Notes);
public record FieldCollectionResponse(Guid Id, string CollectionNumber, Guid SalesRepId, Guid CustomerId, decimal Amount, string PaymentMethod, DateTime CollectionDate, FieldCollection.CollectionStatus Status);

// Sales Routes
public record SalesRouteRequest(string Name, string Code, Guid SalesRepId, string? Description);
public record SalesRouteResponse(Guid Id, string Name, string Code, Guid SalesRepId, bool IsActive);
