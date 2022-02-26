using BlazorCollaborativeApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using BlazorCollaborativeApp.Shared.Services.Intefaces;
using System.Net.Http.Json;

namespace BlazorCollaborativeApp.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] HttpClient client { get; set; }
        [Inject] ICachingService cachingService { get; set; }

        const int MAX_CHARACTERS = 100;
        int left = MAX_CHARACTERS;

        bool isBlocked = false;

        private string? sId;
        private string? Text { get; set; }
        private string? change { get; set; }

        private ObservableCollection<Sheet> Sheets { get; set; } = default!;
        private SheetModel? SheetModel = new();


        private HubConnection? hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7012/canal").WithAutomaticReconnect().Build();

            //initilizing sheets
            //FilledData();
            Sheets = new ObservableCollection<Sheet>();

            hubConnection.On<string, object>("OnChange", (sId, Data) =>
             {
                 var chg = Data.ToString();
                 change = chg;
                 left = left > 0 ? MAX_CHARACTERS - change.Count() : left + change.Count();
                 StateHasChanged();
                 Console.WriteLine(change);
             });

            hubConnection.On<string>("OnNoteAdded", async (sId) =>
             {
                 //var sheet = Data;
                 var sheet = await client.GetFromJsonAsync<Sheet>("https://localhost:7012/api/Caching/get-cached-data/" + sId);
                 if (sheet is not null)
                 {
                     Sheets.Add(sheet);
                     StateHasChanged();

                     Console.WriteLine("New Note received");
                     return;
                 }
                 Console.WriteLine("Noting for this session");
             });

            hubConnection.On<string, Sheet>("OnNoteDeleted", (sId, sheet) =>
            {
                var toDelete = sheet;
                Sheets.Remove(Sheets.Where(x => x.Id == toDelete.Id).First());
                StateHasChanged();
                Console.WriteLine("A sheet was removed");
            });

            hubConnection.On<string, Sheet>("OnNoteUpdated", (sId, sheet) =>
             {
                 var toUpdate = Sheets.FirstOrDefault(x => x.Id == sheet.Id);
                 if (toUpdate is not null)
                 {
                     toUpdate.ProjectId = sheet.ProjectId;
                     toUpdate.SessionId = sheet.SessionId;
                     toUpdate.EditDate = sheet.EditDate;
                     toUpdate.Contents = sheet.Contents;
                     toUpdate.Title = sheet.Title;

                     StateHasChanged();
                     Console.WriteLine("Sheet has been updated");
                     return;
                 }

                 Console.WriteLine("Unable to update this sheet, beause it was not found");
                 return;
             });

            await hubConnection.StartAsync();


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
            sheet.Title = string.IsNullOrWhiteSpace(SheetModel?.Title) ? "Untitled Note" : SheetModel?.Title;
            sheet.Contents = new ObservableCollection<Content>
            {
                new Content
                {
                    SheetId = sheet.Id,
                    Data = string.IsNullOrWhiteSpace( SheetModel?.Data)?string.Empty:SheetModel?.Data,
                    Line = SheetModel.Line
                }
            };

            Sheets.Add(sheet);
            var res = await client.PostAsJsonAsync("https://localhost:7012/api/Caching/cache-data", sheet);
            if (res.IsSuccessStatusCode)
                sId = await res.Content.ReadAsStringAsync();

            await hubConnection.InvokeAsync("AddNoteAsync", sheet.Id);
            SheetModel = new();
            left = MAX_CHARACTERS;
            StateHasChanged();
        }

        private async void Remove(Sheet sheet)
        {
            Sheets.Remove(sheet);
            await hubConnection.InvokeAsync("RemoveNoteAsync", sId, sheet);
            await client.DeleteAsync("https://localhost:7012/api/Caching/remove-from-cache/" + sId);
        }

        private async void UpdateAsync()
        {
            var toUpdate = Sheets.FirstOrDefault(x => x.Id == SheetModel!.Id);
            if (toUpdate != null)
            {
                toUpdate.Title = SheetModel!.Title;
                toUpdate.Contents.FirstOrDefault()!.Data = SheetModel.Data;

                await client.PutAsJsonAsync("https://localhost:7012/api/Caching/update-cached-data", toUpdate);
                await hubConnection.InvokeAsync("UpdateNoteAsync", sId, toUpdate);
                Console.WriteLine("Updating");
                SheetModel = new();
                StateHasChanged();
                return;
            }
            Console.WriteLine("Failed to update");
        }

        private async void OnChangeAsync(ChangeEventArgs change)
        {
            if (change != null)
            {
                Text = change.Value.ToString();
                left = left > 0 ? MAX_CHARACTERS - change.Value.ToString().Count() :
                 left + change.Value.ToString().Count();
                await hubConnection.InvokeAsync("NotifyChangesAsync", sId, change.Value);
            }
        }

        private void OnTyping(ChangeEventArgs change)
        {
            left = left > 0 ? MAX_CHARACTERS - change.Value.ToString().Count() :
                 left + change.Value.ToString().Count();
        }

        private void OnEdit(Sheet sheet)
        {
            SheetModel.Id = sheet.Id;
            SheetModel.Title = sheet.Title;
            SheetModel.Data = sheet.Contents?.SingleOrDefault()?.Data;
            left = MAX_CHARACTERS - SheetModel.Data.Count();
            StateHasChanged();
        }

        private void OnCancel() => SheetModel = new();

    }

    public class SheetModel
    {
        public string? Title { get; set; }
        public string? Data { get; set; }
        public int Line { get; set; }
        public string? Id { get; set; }
    }
}
