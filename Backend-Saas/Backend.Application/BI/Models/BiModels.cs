namespace Backend.Application.BI.Models;

public record MetricSnapshotRequest(string MetricKey, string MetricName, DateTime PeriodStart, DateTime PeriodEnd, decimal Value, string? Dimension, string? SourceSystem);
public record MetricSnapshotResponse(Guid Id, string MetricKey, string MetricName, DateTime PeriodStart, DateTime PeriodEnd, decimal Value, string Dimension, string SourceSystem);
