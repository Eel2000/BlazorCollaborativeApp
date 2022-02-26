URL = window.URL || window.webkitURL;
var voiceStream;
var rec;
var input;
var audioLink;

var AudioContext = window.AudioContext || window.webkitAudioContext;
var audioContext = new AudioContext;


var mediaRecorder
var chunks = [];

var constraints = {
    audio: true,
}



function startRecording() {
    navigator.mediaDevices.getUserMedia(constraints)
        .then(function (stream) {
            voiceStream = stream;
            input = audioContext.createMediaStreamSource(stream);

            rec = new Recorder(input, {
                numChannels: 1
            })

            rec.record()
            console.log("recording has started");
        }).catch((err) => {
            console.log(err);
        });
}

function stopRecording() {
    rec.stop();
    console.log("recording stopped");

    console.log("processing the records");
    const clipName = uuidv4();

    voiceStream.getAudioTracks()[0].stop();
    rec.exportWAV(createDownLoadLink);

    return audioLink + '.wav';
}

function createDownLoadLink(blob) {
    var url = URL.createObjectURL(blob);
    audioLink = url
    var au = document.getElementById('music');
    au.src = '';
    au.src = url;
}

function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}