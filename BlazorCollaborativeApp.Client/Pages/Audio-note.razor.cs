using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCollaborativeApp.Client.Pages
{
    public partial class Audio_note : ComponentBase
    {
        [Inject] public IJSRuntime JS { get; set; }


        [Parameter]
        public string SourceAudio { get; set; } = default!;

        public string classNamePause = "fa-solid fa-pause";
        public string classNameMic = "fa-solid fa-microphone";

        private string Icon = default!;

        bool isRecording = false;

        protected override void OnInitialized()
        {
            Icon = classNameMic;
            base.OnInitialized();
        }

        private async void PlayAsync()
        {
            await JS.InvokeVoidAsync("startRecording");
            Icon = classNamePause;
            isRecording = true;
        }

        private async void StopAsync()
        {
            if (isRecording)
            {
                var url = await JS.InvokeAsync<string>("stopRecording");
                if(!string.IsNullOrWhiteSpace(url))
                {
                    SourceAudio = url;
                }
            }
        }
    }
}
