using Backend.Domain.WMS.Entities;

namespace Backend.Application.WMS.Models;

// Warehouse Tasks
public record WarehouseTaskRequest(Guid WarehouseId, WarehouseTask.WarehouseTaskType Type, string TaskNumber, string? Zone, string? LocationCode, Guid? AssignedUserId, string? Notes);
public record WarehouseTaskResponse(Guid Id, string TaskNumber, WarehouseTask.WarehouseTaskType Type, WarehouseTask.WarehouseTaskStatus Status, Guid WarehouseId, string Zone, string LocationCode, Guid? AssignedUserId, DateTime? StartedAt, DateTime? CompletedAt, string Notes);
public record WarehouseTaskStatusRequest(WarehouseTask.WarehouseTaskStatus Status, string? Notes);

// Picking Tasks
public record PickingTaskItemRequest(Guid ProductId, string ProductName, string LocationCode, int QuantityRequested);
public record PickingTaskRequest(Guid OrderId, string OrderType, Guid WarehouseId, int Priority, List<PickingTaskItemRequest> Items);
public record PickingTaskItemResponse(Guid Id, Guid ProductId, string ProductName, string LocationCode, int QuantityRequested, int QuantityPicked, bool IsCompleted);
public record PickingTaskResponse(Guid Id, string PickingNumber, Guid OrderId, string OrderType, Guid WarehouseId, Guid? AssignedUserId, PickingTask.PickingStatus Status, int Priority, DateTime? AssignedAt, DateTime? StartedAt, DateTime? CompletedAt, IReadOnlyCollection<PickingTaskItemResponse> Items);

// Packing Tasks
public record PackingTaskRequest(Guid PickingTaskId, int TotalBoxes, decimal TotalWeight);
public record PackingTaskResponse(Guid Id, string PackingNumber, Guid PickingTaskId, Guid? AssignedUserId, PackingTask.PackingStatus Status, int TotalBoxes, decimal TotalWeight, DateTime? StartedAt, DateTime? CompletedAt);

// Replenishment
public record ReplenishmentOrderItemRequest(Guid ProductId, string ProductName, int QuantityRequested);
public record ReplenishmentOrderRequest(Guid WarehouseId, Guid? BranchId, List<ReplenishmentOrderItemRequest> Items, string? Notes);
public record ReplenishmentOrderItemResponse(Guid Id, Guid ProductId, string ProductName, int QuantityRequested, int QuantityApproved, int QuantityShipped);
public record ReplenishmentOrderResponse(Guid Id, string ReplenishmentNumber, Guid WarehouseId, Guid? BranchId, ReplenishmentOrder.ReplenishmentStatus Status, DateTime RequestedDate, DateTime? ApprovedAt, DateTime? CompletedAt, IReadOnlyCollection<ReplenishmentOrderItemResponse> Items);

// Warehouse Zones
public record WarehouseZoneRequest(Guid WarehouseId, string Name, string Code, WarehouseZone.ZoneType Type, decimal Area, int Capacity);
public record WarehouseZoneResponse(Guid Id, Guid WarehouseId, string Name, string Code, WarehouseZone.ZoneType Type, decimal Area, int Capacity, bool IsActive);

// Storage Bins
public record StorageBinRequest(Guid WarehouseId, string Code, string Zone, string Aisle, string Level, string Position, decimal Capacity);
public record StorageBinResponse(Guid Id, Guid WarehouseId, string Code, string Zone, string Aisle, string Level, string Position, StorageBin.BinStatus Status, decimal Capacity, decimal CurrentOccupancy, bool IsActive);
