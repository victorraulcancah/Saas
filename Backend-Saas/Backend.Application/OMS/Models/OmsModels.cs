using Backend.Domain.OMS.Entities;

namespace Backend.Application.OMS.Models;

public record OmnichannelOrderItemRequest(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record OmnichannelOrderRequest(
    string OrderNumber,
    string Channel,
    string? ExternalOrderNumber,
    Guid? CustomerId,
    string DeliveryMethod,
    string? DeliveryAddress,
    string? Notes,
    List<OmnichannelOrderItemRequest> Items);

public record OmnichannelOrderItemResponse(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);

public record FulfillmentAssignmentRequest(Guid WarehouseId, Guid? BranchId, string FulfillmentType, string? Notes);

public record FulfillmentAssignmentResponse(
    Guid Id,
    Guid OmnichannelOrderId,
    Guid WarehouseId,
    Guid? BranchId,
    string FulfillmentType,
    FulfillmentAssignment.FulfillmentAssignmentStatus Status,
    DateTime AssignedAt,
    string Notes);

public record OmnichannelOrderResponse(
    Guid Id,
    string OrderNumber,
    string Channel,
    string ExternalOrderNumber,
    Guid? CustomerId,
    DateTime OrderDate,
    OmnichannelOrder.OmnichannelOrderStatus Status,
    decimal TotalAmount,
    string DeliveryMethod,
    string DeliveryAddress,
    string Notes,
    IReadOnlyCollection<OmnichannelOrderItemResponse> Items,
    IReadOnlyCollection<FulfillmentAssignmentResponse> FulfillmentAssignments);
