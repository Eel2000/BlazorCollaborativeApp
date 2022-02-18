using BlazorCollaborativeApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using BlazorCollaborativeApp.Shared.Services.Intefaces;

namespace BlazorCollaborativeApp.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] ICachingService caching { get; set; }
        const string sId = "1";
        private string? Text { get; set; }
        private string? change { get; set; }

        private ObservableCollection<Sheet> Sheets { get; set; } = default!;
        private SheetModel? SheetModel = new();


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

            hubConnection.On<string, Sheet>("OnNoteAdded", (sId, Data) =>
             {
                 var sheet = Data;
                 Sheets.Add(sheet);

                 StateHasChanged();
                 Console.WriteLine("New Note received");
             });

            hubConnection.On<string, Sheet>("OnNoteDeleted", (sId, sheet) =>
            {
                var toDelete = sheet;
                Sheets.Remove(Sheets.Where(x => x.Id == toDelete.Id).First());
                StateHasChanged();
                Console.WriteLine("A sheet was removed");
            });

            await hubConnection.StartAsync();

            //initilizing sheets
            //FilledData();
            Sheets = new ObservableCollection<Sheet>();
        }

        private void FilledData()
        {
            var pjUser = new User
            {
                Username = "Mr Oh!"
            };
            var pjUser2 = new User
            {
                Username = "Mr Bean"
            };

            var pj = new Project
            {
                Description = "The project to create sticky note on blazor",
                Id = Guid.NewGuid().ToString(),
                Name = "Blazor Brain storming project.",
                OwnerId = pjUser.Id,
            };
            var pj2 = new Project
            {
                Description = "Enumerates sorting Algorithms.",
                Id = Guid.NewGuid().ToString(),
                Name = "Sorting Algorithms",
                OwnerId = pjUser2.Id,
            };

            var pjSheet = new Sheet
            {
                EditDate = DateTime.Now,
                Project = pj,
                ProjectId = pj.Id,
                SessionId = sId,
                Title = "Code Algorithms."
            };
            var pjSheet2 = new Sheet
            {
                EditDate = DateTime.Now,
                Project = pj2,
                ProjectId = pj2.Id,
                SessionId = sId,
                Title = "Find a suitable name"
            };

            var pjSheetContents = new List<Content>
            {
                new Content
                {
                    Data="Good morning world.",
                    Line=1,
                    SheetId = pjSheet.Id,
                    Sheet = pjSheet,
                    User=pjUser,
                    UserId=pjUser.Id,
                }, new Content
                {
                    Data="Good morning world.",
                    Line=2,
                    SheetId = pjSheet.Id,
                    Sheet = pjSheet,
                    User=pjUser,
                    UserId=pjUser.Id,
                }, new Content
                {
                    Data="Aero Call.",
                    Line=3,
                    SheetId = pjSheet.Id,
                    Sheet = pjSheet,
                    User=pjUser,
                    UserId=pjUser.Id,
                }, new Content
                {
                    Data="Engine call.",
                    Line=4,
                    SheetId = pjSheet.Id,
                    Sheet = pjSheet,
                    User=pjUser,
                    UserId=pjUser.Id,
                },
            };
            var pjSheetContents2 = new List<Content>
            {
                new Content
                {
                    Data="Sort bubbles",
                    Line=1,
                    SheetId = pjSheet2.Id,
                    Sheet = pjSheet2,
                    User=pjUser2,
                    UserId=pjUser2.Id,
                }, new Content
                {
                    Data="Binary search",
                    Line=2,
                    SheetId = pjSheet2.Id,
                    Sheet = pjSheet2,
                    User=pjUser2,
                    UserId=pjUser2.Id,
                }, new Content
                {
                    Data="Sort general",
                    Line=3,
                    SheetId = pjSheet2.Id,
                    Sheet = pjSheet2,
                    User=pjUser2,
                    UserId=pjUser2.Id,
                }, new Content
                {
                    Data="Engine call.",
                    Line=4,
                    SheetId = pjSheet2.Id,
                    Sheet = pjSheet2,
                    User=pjUser2,
                    UserId=pjUser2.Id,
                },
            };

            pjSheet.Contents = pjSheetContents;
            pjSheet2.Contents = pjSheetContents2;
            Sheets = new ObservableCollection<Sheet>
            {
                pjSheet,pjSheet2
            };

        }

        private async void Send()
        {
            var sheet = new Sheet();
            sheet.Title = SheetModel?.Title;
            sheet.Contents = new ObservableCollection<Content>
            {
                new Content
                {
                    SheetId = sheet.Id,
                    Data = SheetModel?.Data,
                    Line = SheetModel.Line
                }
            };

            var data = await caching.GetAsync<Sheet>("1");
            Sheets.Add(sheet);

            await hubConnection.InvokeAsync("AddNoteAsync", sId, sheet);
        }

        private async void Remove(Sheet sheet)
        {
            Sheets.Remove(sheet);
            await hubConnection.InvokeAsync("RemoveNoteAsync", sId, sheet);
        }

        private async void OnChangeAsync(ChangeEventArgs change)
        {
            if (change != null)
            {
                Text = change.Value.ToString();
                await hubConnection.InvokeAsync("NotifyChangesAsync", sId, change.Value);
            }
        }
    }

    public class SheetModel
    {
        public string? Title { get; set; }
        public string? Data { get; set; }
        public int Line { get; set; }
    }
}
