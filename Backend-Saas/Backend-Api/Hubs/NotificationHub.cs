using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend_Api
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var tenantId = Context.User?.FindFirst("TenantId")?.Value;
            if (tenantId is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"tenant_{tenantId}");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var tenantId = Context.User?.FindFirst("TenantId")?.Value;
            if (tenantId is not null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"tenant_{tenantId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}