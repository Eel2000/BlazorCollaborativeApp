using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorCollaborativeApp.Client.Pages
{
    public partial class Index : ComponentBase
    {
        const string sId = "1";
        private string? Text { get; set; }
        private string? change { get; set; }


        private HubConnection? hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7012/canal").WithAutomaticReconnect().Build();

            hubConnection.On<string, object>("OnChange", (sId, Data) =>
             {
                 var chg = Data.ToString();
                 change = chg;
                 StateHasChanged();
                 Console.WriteLine(change);
             });

            await hubConnection.StartAsync();
        }

        private async void Send()
        {
            await hubConnection.InvokeAsync("NotifyChangesAsync", sId, Text);
            Console.WriteLine(Text);
            System.Diagnostics.Debug.WriteLine(Text, "typing-zone");
        }

        private async void OnChangeAsync(ChangeEventArgs change)
        {
            if (change != null)
            {
                Text = change.Value.ToString();
                await hubConnection.InvokeAsync("NotifyChangesAsync", sId, change.Value);
                System.Diagnostics.Debug.WriteLine(change.Value, "typing-zone");
            }
        }
    }
}
