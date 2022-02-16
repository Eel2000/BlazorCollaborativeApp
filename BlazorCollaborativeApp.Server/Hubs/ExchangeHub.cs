using Microsoft.AspNetCore.SignalR;

namespace BlazorCollaborativeApp.Server.Hubs
{
    public class ExchangeHub : Hub
    {
        public async Task NotifyChangesAsync(string sId, object data)
        {
            await Clients.All.SendAsync("OnChange", sId, data);
            System.Diagnostics.Debug.WriteLine("A change occured", "OnChange");
        }
    }
}
