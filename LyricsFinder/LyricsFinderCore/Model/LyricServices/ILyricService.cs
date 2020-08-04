using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Lyric service interface.
    /// </summary>
    [ComVisible(false)]
    public interface ILyricService
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        /**********************/
        /***** Properties *****/
        /**********************/

        string Comment { get; set; }

        CreditType Credit { get; set; }

        int DelayMilliSecondsBetweenSearches { get; set; }

        Dictionary<string, DisplayProperty> DisplayProperties { get; }

        ReadOnlyCollection<FoundLyricType> FoundLyricList { get; }

        int HitCountToday { get; set; }

        int HitCountTotal { get; set; }

        bool IsActive { get; set; }

        bool IsImplemented { get; set; }

        DateTime LastRequest { get; set; }

        DateTime LastSearchStart { get; set; }

        DateTime LastSearchStop { get; set; }

        LyricsResultEnum LyricResult { get; set; }

        string LyricResultMessage { get; }

        LyricsFinderDataType LyricsFinderData { get; set; }

        int RequestCountToday { get; set; }

        int RequestCountTotal { get; set; }

        int TimeoutMilliSeconds { get; set; }


        /*********************/
        /****** Methods ******/
        /*********************/

        Task<FoundLyricType> AddFoundLyric(string lyricText, Uri lyricUrl, Uri trackingUrl = null, string copyright = null);

        ILyricService Clone();

        void CreateDisplayProperties();

        Task IncrementHitCountersAsync(int count = 1);

        Task IncrementRequestCountersAsync(int count = 1);

        Task<bool> IsQuotaExceededAsync();

        Task<AbstractLyricService> ProcessAsync(McMplItem item, CancellationToken cancellationToken, bool isGetAll = false);

        Task RefreshServiceSettingsAsync();

        Task ResetTodayCountersAsync();

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}