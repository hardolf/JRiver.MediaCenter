
function convertToHtml(s) {
    var ret = s;

    //for (i = 0; i < s.length; i++) {
    //    var ch = s[i];
    //    var n = s.charCodeAt(i);
    //    // if (ch === 'å') { ret = '&aring;'; } // 0229
    //    switch (ch) {
    //        case 'ä':
    //            ret += '&auml;';
    //            break;
    //        case 'å':
    //            ret += '&aring;';
    //            break;
    //        default:
    //            ret += ch;
    //    }
    //    //ret += ch + '|' + n + ' ';
    //}

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
