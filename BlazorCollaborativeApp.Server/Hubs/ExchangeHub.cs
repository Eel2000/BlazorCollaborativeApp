using BlazorCollaborativeApp.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazorCollaborativeApp.Server.Hubs
{
    public class ExchangeHub : Hub
    {
        public async Task NotifyChangesAsync(string sId, object data)
        {
            await Clients.Others.SendAsync("OnChange", sId, data);
            System.Diagnostics.Debug.WriteLine("A change occured", "OnChange");
        }

        public async Task AddNoteAsync(string sId)
        {
            await Clients.Others.SendAsync("OnNoteAdded", sId);
            System.Diagnostics.Debug.WriteLine("New Note has been added", "OnNoteAdded");
        }

        public async Task RemoveNoteAsync(string sId, Sheet sheet)
        {
            await Clients.Others.SendAsync("OnNoteDeleted", sId, sheet);
            System.Diagnostics.Debug.WriteLine("a note was removed", "OnRemove");
        }

        public async Task UpdateNoteAsync(string sId, Sheet sheet)
        {
            await Clients.Others.SendAsync("OnNoteUpdated", sId, sheet);
            System.Diagnostics.Debug.WriteLine("A note was updated", "OnUpdate");
        }
    }
}
