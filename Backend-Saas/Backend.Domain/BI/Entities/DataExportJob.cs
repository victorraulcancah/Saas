namespace Backend.Domain.BI.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class DataExportJob : BaseEntity, ITenantEntity
{
    public enum ExportStatus
    {
        Scheduled,
        Processing,
        Completed,
        Failed
    }

    public enum ExportFormat
    {
        Excel,
        CSV,
        PDF,
        JSON
    }

    public Guid? TenantId { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public Guid? ReportDefinitionId { get; set; }
    public string DataSource { get; set; } = string.Empty;
    public ExportFormat Format { get; set; }
    public ExportStatus Status { get; set; } = ExportStatus.Scheduled;
    public DateTime ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public int RecordsExported { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public Guid RequestedBy { get; set; }
}
