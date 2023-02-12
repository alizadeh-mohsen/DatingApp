using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            this.presenceTracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
            await presenceTracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            var onlineUsers=presenceTracker.GetOnlineUsers();
            await Clients.Others.SendAsync("GetOnlineUsers", onlineUsers);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await presenceTracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            
            var onlineUsers = presenceTracker.GetOnlineUsers();
            await Clients.Others.SendAsync("GetOnlineUsers", onlineUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
