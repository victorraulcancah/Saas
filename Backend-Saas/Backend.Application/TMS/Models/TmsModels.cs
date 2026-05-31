using Backend.Domain.TMS.Entities;

namespace Backend.Application.TMS.Models;

public record DeliveryRouteRequest(string RouteNumber, string? VehiclePlate, string? DriverName, DateTime PlannedDate, decimal CapacityKg, string? Notes);
public record DeliveryRouteStatusRequest(DeliveryRoute.DeliveryRouteStatus Status);
public record DeliveryRouteResponse(Guid Id, string RouteNumber, string VehiclePlate, string DriverName, DateTime PlannedDate, DeliveryRoute.DeliveryRouteStatus Status, decimal CapacityKg, string Notes);
