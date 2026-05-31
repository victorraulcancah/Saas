using Backend.Application.BI.Models;

namespace Backend.Application.BI.Services;

public interface IBiMetricsService
{
    Task<IEnumerable<MetricSnapshotResponse>> GetMetricsAsync(DateTime? from = null, DateTime? to = null);
    Task<MetricSnapshotResponse> CreateMetricSnapshotAsync(MetricSnapshotRequest request);
}
