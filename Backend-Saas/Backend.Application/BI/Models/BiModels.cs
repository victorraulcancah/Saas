using Backend.Domain.BI.Entities;

namespace Backend.Application.BI.Models;

// Metric Snapshots
public record MetricSnapshotRequest(string MetricKey, string MetricName, DateTime PeriodStart, DateTime PeriodEnd, decimal Value, string? Dimension, string? SourceSystem);
public record MetricSnapshotResponse(Guid Id, string MetricKey, string MetricName, DateTime PeriodStart, DateTime PeriodEnd, decimal Value, string Dimension, string SourceSystem);

// KPI Definitions
public record KpiDefinitionRequest(string Name, string Description, KpiDefinition.KpiCategory Category, string Formula, string Unit, decimal? TargetValue);
public record KpiDefinitionResponse(Guid Id, string Name, string Description, KpiDefinition.KpiCategory Category, string Unit, decimal? TargetValue, bool IsActive);

// Dashboards
public record DashboardRequest(string Name, string Description, string Layout, bool IsDefault, bool IsPublic, int RefreshIntervalMinutes);
public record DashboardResponse(Guid Id, string Name, string Description, bool IsDefault, bool IsPublic, int RefreshIntervalMinutes, bool IsActive);

// Report Definitions
public record ReportDefinitionRequest(string Name, string Description, ReportDefinition.ReportType Type, string DataSource, string QueryDefinition, bool IsPublic);
public record ReportDefinitionResponse(Guid Id, string Name, string Description, ReportDefinition.ReportType Type, string DataSource, bool IsPublic, bool IsActive);

// Data Export Jobs
public record DataExportJobRequest(Guid? ReportDefinitionId, string DataSource, DataExportJob.ExportFormat Format, DateTime ScheduledAt);
public record DataExportJobResponse(Guid Id, string JobNumber, DataExportJob.ExportFormat Format, DataExportJob.ExportStatus Status, DateTime ScheduledAt, DateTime? CompletedAt, string FileUrl, int RecordsExported);
