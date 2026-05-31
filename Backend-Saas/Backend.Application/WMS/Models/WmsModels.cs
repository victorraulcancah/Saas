using Backend.Domain.WMS.Entities;

namespace Backend.Application.WMS.Models;

public record WarehouseTaskRequest(
    string TaskNumber,
    WarehouseTask.WarehouseTaskType Type,
    Guid WarehouseId,
    string? Zone,
    string? LocationCode,
    Guid? AssignedUserId,
    string? Notes);

public record WarehouseTaskStatusRequest(WarehouseTask.WarehouseTaskStatus Status);

public record WarehouseTaskResponse(
    Guid Id,
    string TaskNumber,
    WarehouseTask.WarehouseTaskType Type,
    WarehouseTask.WarehouseTaskStatus Status,
    Guid WarehouseId,
    string Zone,
    string LocationCode,
    Guid? AssignedUserId,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    string Notes);
