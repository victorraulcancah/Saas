using Backend.Domain.TMS.Entities;

namespace Backend.Application.TMS.Models;

// Delivery Routes
public record DeliveryRouteRequest(string RouteNumber, string? VehiclePlate, string? DriverName, DateTime PlannedDate, decimal CapacityKg, string? Notes);
public record DeliveryRouteResponse(Guid Id, string RouteNumber, string VehiclePlate, string DriverName, DateTime PlannedDate, DeliveryRoute.DeliveryRouteStatus Status, decimal CapacityKg, string Notes);
public record DeliveryRouteStatusRequest(DeliveryRoute.DeliveryRouteStatus Status, string? Notes);

// Vehicles
public record VehicleRequest(string Plate, string Brand, string Model, int Year, Vehicle.VehicleType Type, decimal CapacityKg, decimal CapacityM3, string FuelType, decimal FuelConsumption);
public record VehicleResponse(Guid Id, string Plate, string Brand, string Model, int Year, Vehicle.VehicleType Type, Vehicle.VehicleStatus Status, decimal CapacityKg, decimal CapacityM3, bool IsActive);

// Drivers
public record DriverRequest(Guid EmployeeId, string LicenseNumber, string LicenseType, DateTime LicenseExpiryDate);
public record DriverResponse(Guid Id, Guid EmployeeId, string LicenseNumber, string LicenseType, DateTime LicenseExpiryDate, Driver.DriverStatus Status, decimal Rating, int TotalDeliveries, bool IsActive);

// Route Plans
public record RoutePlanRequest(DateTime PlannedDate, string OptimizationCriteria, string? Notes);
public record RoutePlanResponse(Guid Id, string PlanNumber, DateTime PlannedDate, RoutePlan.PlanStatus Status, int TotalStops, decimal TotalDistance, decimal EstimatedDuration, decimal EstimatedCost, string OptimizationCriteria);

// Route Stops
public record RouteStopRequest(Guid DeliveryRouteId, int StopOrder, Guid? OrderId, string CustomerName, string Address, string Latitude, string Longitude, DateTime PlannedArrival);
public record RouteStopResponse(Guid Id, Guid DeliveryRouteId, int StopOrder, string CustomerName, string Address, DateTime PlannedArrival, DateTime? ActualArrival, DateTime? ActualDeparture, RouteStop.StopStatus Status);

// Deliveries
public record DeliveryRequest(Guid OrderId, string OrderType, string CustomerName, string DeliveryAddress, string ContactPhone, DateTime ScheduledDate);
public record DeliveryResponse(Guid Id, string DeliveryNumber, Guid OrderId, string OrderType, Guid? DeliveryRouteId, Guid? VehicleId, Guid? DriverId, Delivery.DeliveryStatus Status, string CustomerName, string DeliveryAddress, DateTime ScheduledDate, DateTime? DeliveredAt);

// Proof of Delivery
public record ProofOfDeliveryRequest(Guid DeliveryId, string ReceiverName, string ReceiverDocument, string ReceiverRelationship, string SignatureUrl, string PhotoUrl, string Latitude, string Longitude, string? Notes);
public record ProofOfDeliveryResponse(Guid Id, Guid DeliveryId, string ReceiverName, string ReceiverDocument, DateTime DeliveredAt, string SignatureUrl, string PhotoUrl);
