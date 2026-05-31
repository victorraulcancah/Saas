namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Driver : BaseEntity, ITenantEntity
{
    public enum DriverStatus
    {
        Available,
        OnRoute,
        OnBreak,
        Inactive
    }

    public Guid? TenantId { get; set; }
    public Guid EmployeeId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string LicenseType { get; set; } = string.Empty;
    public DateTime LicenseExpiryDate { get; set; }
    public DriverStatus Status { get; set; } = DriverStatus.Available;
    public decimal Rating { get; set; }
    public int TotalDeliveries { get; set; }
    public bool IsActive { get; set; } = true;
}
