using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.McWs
{

    /// <summary>
    /// Enumeration of the JRiver MediaCenter commands used in the LyricsFinder.
    /// </summary>
    public enum McCommandEnum
    {

        AddToPlayingNow,
        Alive,
        Authenticate,
        GetImage,
        GetInfo,
        GetInfoFull,
        Info,
        PlayByIndex,
        PlayByKey,
        Playlist,
        PlaylistFiles,
        PlaylistList,
        PlaylistListForItem,
        PlayPause,
        PlayPlaylist,
        Position,
        Stop,
        SetInfo

    }

}
