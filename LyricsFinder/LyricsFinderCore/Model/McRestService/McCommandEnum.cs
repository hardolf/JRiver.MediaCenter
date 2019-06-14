using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.LyricsFinder.Model.McRestService
{

    /// <summary>
    /// Enumeration of the JRiver MediaCenter commands used in the LyricsFinder.
    /// </summary>
    internal enum McCommandEnum
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        AddToPlayingNow,
        Alive,
        Authenticate,
        GetImage,
        Info,
        PlayByIndex,
        PlayByKey,
        Playlist,
        PlaylistFiles,
        PlaylistList,
        PlayPause,
        PlayPlaylist,
        Stop,
        SetInfo
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }

}
