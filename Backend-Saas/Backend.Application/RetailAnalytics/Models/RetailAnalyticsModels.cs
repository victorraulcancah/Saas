namespace Backend.Application.RetailAnalytics.Models;

// Store Traffic Readings
public record StoreTrafficReadingRequest(Guid BranchId, DateTime ReadingAt, int VisitorsIn, int VisitorsOut, int TicketsCount, decimal SalesAmount, string? Source);
public record StoreTrafficReadingResponse(Guid Id, Guid BranchId, DateTime ReadingAt, int VisitorsIn, int VisitorsOut, int TicketsCount, decimal SalesAmount, decimal ConversionRate, string Source);

// Conversion Metrics
public record ConversionMetricRequest(Guid BranchId, DateTime Date, int TotalVisitors, int TotalTransactions, decimal AverageTicketValue);
public record ConversionMetricResponse(Guid Id, Guid BranchId, DateTime Date, int TotalVisitors, int TotalTransactions, decimal ConversionRate, decimal AverageTicketValue, decimal TotalRevenue);

// Product Performance
public record ProductPerformanceRequest(Guid ProductId, string ProductName, Guid? BranchId, DateTime PeriodStart, DateTime PeriodEnd, int UnitsSold, decimal Revenue, decimal Margin);
public record ProductPerformanceResponse(Guid Id, Guid ProductId, string ProductName, DateTime PeriodStart, DateTime PeriodEnd, int UnitsSold, decimal Revenue, decimal Margin, decimal TurnoverRate);

// Heatmap Readings
public record HeatmapReadingRequest(Guid BranchId, string Zone, string ZoneType, int VisitorCount, decimal DwellTimeSeconds);
public record HeatmapReadingResponse(Guid Id, Guid BranchId, string Zone, string ZoneType, DateTime ReadingTime, int VisitorCount, decimal DwellTimeSeconds, decimal Temperature);
