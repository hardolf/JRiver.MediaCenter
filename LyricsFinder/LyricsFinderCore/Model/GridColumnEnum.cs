using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Enumeration of the grid column names.
    /// </summary>
    internal enum GridColumnEnum : int
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        PlayImage,
        Index,
        Sequence,
        Key,
        Cover,
        Artist,
        Album,
        Title,
        Lyrics,
        Status
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
