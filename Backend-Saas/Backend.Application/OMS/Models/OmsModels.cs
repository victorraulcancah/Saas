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

public record SalesChannelRequest(string Name, string Code, SalesChannel.ChannelType Type, string? ApiEndpoint, string? ApiKey, string? Configuration);
public record SalesChannelResponse(Guid Id, string Name, string Code, SalesChannel.ChannelType Type, string ApiEndpoint, bool IsActive);

public record ChannelSyncLogRequest(Guid SalesChannelId, string SyncType);
public record ChannelSyncLogResponse(
    Guid Id,
    Guid SalesChannelId,
    string ChannelName,
    string SyncType,
    ChannelSyncLog.SyncStatus Status,
    int RecordsProcessed,
    int RecordsSuccess,
    int RecordsFailed,
    DateTime SyncStartedAt,
    DateTime? SyncCompletedAt,
    string ErrorMessage);

public record OrderRouteRequest(Guid OmnichannelOrderId, Guid? WarehouseId, Guid? BranchId, string RoutingStrategy, int Priority);
public record OrderRouteResponse(
    Guid Id,
    Guid OmnichannelOrderId,
    string OrderNumber,
    Guid? WarehouseId,
    Guid? BranchId,
    string RoutingStrategy,
    decimal Distance,
    decimal EstimatedCost,
    int Priority,
    OrderRoute.RouteStatus Status,
    DateTime? AssignedAt);
