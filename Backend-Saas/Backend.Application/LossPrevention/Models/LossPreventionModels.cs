using Backend.Domain.LossPrevention.Entities;

namespace Backend.Application.LossPrevention.Models;

// Cycle Counts
public record CycleCountRequest(string CountNumber, Guid WarehouseId, string? Category, DateTime ScheduledDate, string? Notes);
public record CycleCountResponse(Guid Id, string CountNumber, Guid WarehouseId, string Category, DateTime ScheduledDate, CycleCount.CycleCountStatus Status, int ItemsCounted, int DiscrepanciesFound, string Notes);
public record CycleCountCloseRequest(int ItemsCounted, int DiscrepanciesFound, string? Notes);

// Shrinkage Cases
public record ShrinkageCaseRequest(Guid WarehouseId, Guid? BranchId, ShrinkageCase.ShrinkageType Type, decimal EstimatedValue, string Description);
public record ShrinkageCaseResponse(Guid Id, string CaseNumber, Guid WarehouseId, ShrinkageCase.ShrinkageType Type, ShrinkageCase.CaseStatus Status, DateTime DiscoveredAt, decimal EstimatedValue, string Description);

// Suspicious Transaction Alerts
public record SuspiciousTransactionAlertRequest(SuspiciousTransactionAlert.AlertType Type, Guid TransactionId, string TransactionType, Guid? UserId, string UserName, decimal Amount, string Details);
public record SuspiciousTransactionAlertResponse(Guid Id, string AlertNumber, SuspiciousTransactionAlert.AlertType Type, SuspiciousTransactionAlert.AlertStatus Status, Guid TransactionId, string UserName, DateTime TransactionDate, decimal Amount, string RiskScore);

// Inventory Audits
public record InventoryAuditRequest(Guid WarehouseId, string AuditType, DateTime ScheduledDate, Guid? AuditorId);
public record InventoryAuditResponse(Guid Id, string AuditNumber, Guid WarehouseId, string AuditType, InventoryAudit.AuditStatus Status, DateTime ScheduledDate, int TotalItemsAudited, int DiscrepanciesFound, decimal TotalVarianceValue);
