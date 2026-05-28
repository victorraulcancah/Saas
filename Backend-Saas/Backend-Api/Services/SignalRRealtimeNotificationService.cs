using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Backend_Api
{
    public class SignalRRealtimeNotificationService : IRealtimeNotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRRealtimeNotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task NotifyTenantAsync(Guid tenantId, string eventName, object payload)
        {
            return _hub.Clients.Group($"tenant_{tenantId}").SendAsync(eventName, payload);
        }
    }
}