using Backend.Domain.LossPrevention.Entities;

namespace Backend.Application.LossPrevention.Models;

public record CycleCountRequest(string CountNumber, Guid WarehouseId, string? Category, DateTime ScheduledDate, string? Notes);
public record CycleCountCloseRequest(int ItemsCounted, int DiscrepanciesFound, string? Notes);
public record CycleCountResponse(Guid Id, string CountNumber, Guid WarehouseId, string Category, DateTime ScheduledDate, CycleCount.CycleCountStatus Status, int ItemsCounted, int DiscrepanciesFound, string Notes);
