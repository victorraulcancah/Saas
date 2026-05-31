namespace Backend.Application.RetailAnalytics.Models;

public record StoreTrafficReadingRequest(Guid BranchId, DateTime ReadingAt, int VisitorsIn, int VisitorsOut, int TicketsCount, decimal SalesAmount, string? Source);
public record StoreTrafficReadingResponse(Guid Id, Guid BranchId, DateTime ReadingAt, int VisitorsIn, int VisitorsOut, int TicketsCount, decimal SalesAmount, decimal ConversionRate, string Source);
