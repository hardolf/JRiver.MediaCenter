using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Enumeration of the lyric services results.
    /// </summary>
    public enum LyricResultEnum
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        NotProcessedYet,
        Found,
        NotFound,
        SkippedOldLyrics,
        ManuallyEdited,
        Error,
        Processing
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
