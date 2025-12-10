using Microsoft.AspNetCore.SignalR;

namespace CriptoApi.Api.Hubs
{
    // SignalR Hub for sending cryptocurrency updates.
    // No server-side methods are required unless clients must call the server.
    public class CryptoHub : Hub
    {
        // Optional: You can add client → server actions later
        // (chat, alert, subscriptions, filters, commands, etc.)        
    }
}
