
function convertNationalCharsToHtml(ch) {
    var ret = ch;

    if (ch === 'å') { ret = '&aring;'; }

    return ret;
}


function onBodyLoad() {
    // alert("onLoad");

    var lyric = document.getElementById("lyricText");

    if (lyric.textContent.trim() === "") {
        // alert("Empty");

        var lyricTextDiv = document.getElementById("lyricTextDiv");

        // lyricTextDiv.remove();
    }
}
