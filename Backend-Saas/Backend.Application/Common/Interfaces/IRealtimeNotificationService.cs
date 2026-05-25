namespace Backend.Application.Common.Interfaces;

public interface IRealtimeNotificationService
{
    Task NotifyTenantAsync(Guid tenantId, string eventName, object payload);
}
