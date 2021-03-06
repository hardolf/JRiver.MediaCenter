﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Abstract lyric service type.
    /// </summary>
    [ComVisible(false)]
    [Serializable]
    public abstract class AbstractLyricService : ILyricService
    {

        // Instantiate a Singleton of the Semaphore with a value of 1. 
        // This means that only 1 thread can be granted access at a time. 
        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);


        /**********************/
        /***** Properties *****/
        /**********************/

        /// <summary>
        /// Gets or sets the lyric service comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        [XmlElement]
        public virtual string Comment { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>
        /// The credit.
        /// </value>
        [XmlElement("Credit")]
        public virtual CreditType Credit { get; set; }

        /// <summary>
        /// Gets or sets the delay milli seconds between searches.
        /// </summary>
        /// <value>
        /// The delay milli seconds between searches.
        /// </value>
        [XmlIgnore]
        public virtual int DelayMilliSecondsBetweenSearches { get; set; }

        /// <summary>
        /// Gets or sets the display properties.
        /// </summary>
        /// <value>
        /// The display properties.
        /// </value>
        [XmlIgnore]
        public virtual Dictionary<string, DisplayProperty> DisplayProperties { get; private set; }

        /// <summary>
        /// Gets or sets the internal found lyric list.
        /// </summary>
        /// <value>
        /// The internal found lyric list.
        /// </value>
        [XmlIgnore]
        private FoundLyricListType<FoundLyricType> InternalFoundLyricList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration file have been used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the configuration file have been used; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public static bool IsConfigurationFileUsed { get; set; } = false;

        /// <summary>
        /// Gets a list of the found lyric texts.
        /// </summary>
        /// <value>
        /// The public list of found lyric texts.
        /// </value>
        [XmlIgnore]
        public virtual ReadOnlyCollection<FoundLyricType> FoundLyricList { get; }

        /// <summary>
        /// Gets or sets the hit count today.
        /// </summary>
        /// <value>
        /// The hit count today.
        /// </value>
        [XmlElement]
        public virtual int HitCountToday { get; set; }

        /// <summary>
        /// Gets or sets the hit count total.
        /// </summary>
        /// <value>
        /// The hit count total.
        /// </value>
        [XmlElement]
        public virtual int HitCountTotal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AbstractLyricService"/> is active, i.e. should be part of the lyric search.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is implemented.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is implemented; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public virtual bool IsImplemented { get; set; }

        /// <summary>
        /// Gets or sets the last request.
        /// </summary>
        /// <value>
        /// The last request.
        /// </value>
        [XmlElement]
        public virtual DateTime LastRequest { get; set; }

        /// <summary>
        /// Gets or sets the last search start time.
        /// </summary>
        /// <value>
        /// The last search start time.
        /// </value>
        [XmlIgnore]
        public virtual DateTime LastSearchStart { get; set; }

        /// <summary>
        /// Gets or sets the last search stop time.
        /// </summary>
        /// <value>
        /// The last search stop time.
        /// </value>
        [XmlIgnore]
        public virtual DateTime LastSearchStop { get; set; }

        /// <summary>
        /// Gets or sets the lyric result. If set to <c>Found</c> increments the hit counters.
        /// </summary>
        /// <value>
        /// The lyric result.
        /// </value>
        [XmlIgnore]
        public virtual LyricsResultEnum LyricResult { get; set; }

        /// <summary>
        /// Gets the result text.
        /// </summary>
        /// <returns>Result message for the status.</returns>
        [XmlIgnore]
        public virtual string LyricResultMessage
        {
            get
            {
                var ret = new StringBuilder(LyricResult.ResultText());

                if (LyricResult == LyricsResultEnum.Found)
                    ret.Append($" by service \"{Credit.ServiceName}\"");

                return ret.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the lyrics finder data.
        /// </summary>
        /// <value>
        /// The lyrics finder data.
        /// </value>
        [XmlIgnore]
        public virtual LyricsFinderDataType LyricsFinderData { get; set; }

        /// <summary>
        /// Gets or sets the request count today.
        /// </summary>
        /// <value>
        /// The request count today.
        /// </value>
        [XmlElement]
        public virtual int RequestCountToday { get; set; }

        /// <summary>
        /// Gets or sets the request count total.
        /// </summary>
        /// <value>
        /// The request count total.
        /// </value>
        [XmlElement]
        public virtual int RequestCountTotal { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        [XmlIgnore]
        protected virtual KeyValueConfigurationCollection Settings { get; private set; }

        /// <summary>
        /// Gets or sets the timeout in milliseconds.
        /// </summary>
        /// <value>
        /// The timeout milli seconds.
        /// </value>
        [XmlElement]
        public virtual int TimeoutMilliSeconds { get; set; } = 0; // Default: no timeout


        /************************/
        /***** Constructors *****/
        /************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLyricService"/> class.
        /// </summary>
        public AbstractLyricService()
        {
            Comment = string.Empty;
            Credit = new CreditType();
            DisplayProperties = new Dictionary<string, DisplayProperty>();
            InternalFoundLyricList = new FoundLyricListType<FoundLyricType>();
            FoundLyricList = new ReadOnlyCollection<FoundLyricType>(InternalFoundLyricList);
            IsActive = false;
            IsImplemented = false;
            LyricResult = LyricsResultEnum.NotProcessedYet;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLyricService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public AbstractLyricService(AbstractLyricService source)
            : this()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comment = source.Comment;
            Credit = source.Credit.Clone();
            DelayMilliSecondsBetweenSearches = source.DelayMilliSecondsBetweenSearches;
            IsActive = source.IsActive;
            IsImplemented = source.IsImplemented;
            LyricsFinderData = source.LyricsFinderData;
            LyricResult = LyricsResultEnum.NotProcessedYet;

            HitCountToday = source.HitCountToday;
            HitCountTotal = source.HitCountTotal;
            RequestCountToday = source.RequestCountToday;
            RequestCountTotal = source.RequestCountTotal;
        }


        /*********************/
        /****** Methods ******/
        /*********************/

        /// <summary>
        /// Adds the found lyric.
        /// </summary>
        /// <param name="lyricText">The lyric text.</param>
        /// <param name="lyricUrl">The lyric URL.</param>
        /// <param name="trackingUrl">The tracking URL.</param>
        /// <param name="copyright">The copyright.</param>
        /// <returns>
        ///   <see cref="FoundLyricType" /> object.
        /// </returns>
        public virtual async Task<FoundLyricType> AddFoundLyric(string lyricText, Uri lyricUrl, Uri trackingUrl = null, string copyright = null)
        {
            if (lyricText.IsNullOrEmptyTrimmed()) return null;

            if (!copyright.IsNullOrEmptyTrimmed())
                Credit.Copyright = copyright;

            lyricText = WebUtility.HtmlDecode(lyricText).Trim();

            var beforeCount = InternalFoundLyricList.Count;
            var ret = InternalFoundLyricList.Add(this, lyricText, Credit.ToString(), lyricUrl, trackingUrl);
            var afterCount = InternalFoundLyricList.Count;
            var diff = (afterCount - beforeCount);

            // This construct ensures that we don't count duplicates
            await IncrementHitCountersAsync(diff);

            LyricResult = LyricsResultEnum.Found;

            return ret;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns><see cref="AbstractLyricService"/> descendant object.</returns>
        public virtual ILyricService Clone()
        {
            return null;
        }


        /// <summary>
        /// Creates the display properties.
        /// </summary>
        public virtual void CreateDisplayProperties()
        {
            DisplayProperties.Clear();

            Credit.CreateDisplayProperties();

            DisplayProperties.Add(nameof(Comment), Comment,
                "Service comment", "Special comments for the lyric service", true);
            DisplayProperties.Add(nameof(TimeoutMilliSeconds), TimeoutMilliSeconds.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture),
                "Timeout", "Lyric service call timeout in milliseconds, 0 or negative: no timeout", true);
            DisplayProperties.Add(nameof(RequestCountToday), RequestCountToday.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture),
                "Requests, today", "Number of requests to the lyric service since midnight (local client time)", false);
            DisplayProperties.Add(nameof(HitCountToday), HitCountToday.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture),
                "Hits, today", "Number of successful results (hits) from the lyric service since midnight (local client time)", false);
            DisplayProperties.Add(nameof(RequestCountTotal), RequestCountTotal.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture),
                "Requests, total", "Total number of requests to the lyric service since the creation of the local client datafile", false);
            DisplayProperties.Add(nameof(HitCountTotal), HitCountTotal.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture),
                "Hits, total", "Total number of successful results (hits) from the lyric service since the creation of the local client datafile", false);
        }


        /// <summary>
        /// Extracts all the lyrics from all the Uris.
        /// </summary>
        /// <param name="uris">The uris.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> extracts all lyrics from all the Uris deafult using parallell search; else exits after the first hit, default using serial search.</param>
        /// <param name="isSerialSearchForced">if set to <c>true</c> serial search is forced even when getting all hits; else default to parallell search when getting all hits.</param>
        /// <exception cref="ArgumentNullException">uris</exception>
        protected virtual async Task ExtractAllLyricTextsAsync(IEnumerable<Uri> uris, CancellationToken cancellationToken, bool isGetAll = false, bool isSerialSearchForced = false)
        {
            if (uris == null) throw new ArgumentNullException(nameof(uris));

            if (isGetAll && !isSerialSearchForced)
            {
                // Parallel search
                var tasks = new List<Task>();

                foreach (var uri in uris)
                {
                    tasks.Add(ExtractOneLyricTextAsync(uri, cancellationToken));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            else
            {
                // Serial search, breaks on the first hit if not getting all hits
                foreach (var uri in uris)
                {
                    var lyricText = await ExtractOneLyricTextAsync(uri, cancellationToken).ConfigureAwait(false);

                    if (!lyricText.IsNullOrEmptyTrimmed() && !isGetAll)
                        break;
                }
            }
        }


        /// <summary>
        /// Extracts the result text from a Uri and adds the found lyric text to the list.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// If found, the found lyric text string; else null.
        /// </returns>
        /// <exception cref="ArgumentNullException">uri</exception>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task<string> ExtractOneLyricTextAsync(Uri uri, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var ret = string.Empty;

            /**************************************************
             * This must be done in every descendant routine: *
             **************************************************
             * At the top:
             **************************************************
            var ret = await base.ExtractOneLyricTextAsync(uri).ConfigureAwait(false);

             **************************************************
             * At the bottom:
             **************************************************

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                AddFoundLyric(ret, new UriBuilder(uri.AbsoluteUri).Uri);
            */

            return ret;
        }


        /// <summary>
        /// Get string asynchronous from HTTP request to the lyric service.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>Response text from lyric service.</returns>
        /// <remarks>We use a central routine here, so that the request counters may be properly updated.</remarks>
        protected virtual async Task<string> HttpGetStringAsync(Uri requestUri)
        {
            if (IsActive && await IsQuotaExceededAsync())
                QuotaError();

            var ret = string.Empty;

            await AsyncUtility.RandomizedDelayAsync(LyricsFinderData.MainData.DelayMilliSecondsBetweenSearches);

            try
            {
                ret = await SharedComponents.Utility.HttpGetStringAsync(requestUri, timeoutMilliSeconds: TimeoutMilliSeconds).ConfigureAwait(false);
            }
            finally
            {
                await IncrementRequestCountersAsync();
            }

            return ret;
        }


        /// <summary>
        /// Increments the hit counters.
        /// </summary>
        /// <param name="count">The count.</param>
        public virtual async Task IncrementHitCountersAsync(int count = 1)
        {
            // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
            await _semaphoreSlim.WaitAsync();

            try
            {
                HitCountToday += count;
                HitCountTotal += count;

                CreateDisplayProperties();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }


        /// <summary>
        /// Increments the request counters.
        /// </summary>
        /// <param name="count">The count.</param>
        public virtual async Task IncrementRequestCountersAsync(int count = 1)
        {
            // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
            await _semaphoreSlim.WaitAsync();

            try
            {
                LastRequest = DateTime.Now;
                RequestCountToday += count;
                RequestCountTotal += count;

                CreateDisplayProperties();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }


        /// <summary>
        /// Gets value indicating whether this service's quota is exceeded.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this service's quota is exceeded; otherwise, <c>false</c>.
        /// </returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<bool> IsQuotaExceededAsync() => false;
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricsQuotaExceededException">Lyric service \"{Credit.ServiceName}\" is exceeding its quota and is now disabled in LyricsFinder, "
        /// + "no more requests will be sent to this service until corrected.</exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual async Task<AbstractLyricService> ProcessAsync(McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            InternalFoundLyricList.Clear();
            LyricResult = LyricsResultEnum.NotFound;

            // Skip if we are over the quota limit
            if (IsActive && await IsQuotaExceededAsync())
                QuotaError(mcItem, isGetAll);

            return this;
        }


        /// <summary>
        /// Processes the specified MediaCenter item, wrapper for ProcessAsync.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricsQuotaExceededException">Lyric service \"{Credit.ServiceName}\" is exceeding its quota and is now disabled in LyricsFinder, "
        /// + "no more requests will be sent to this service until corrected.</exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual async Task<AbstractLyricService> ProcessAsyncWrapper(McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            LastSearchStart = DateTime.Now;
            LastSearchStop = LastSearchStart;

            var ret = await ProcessAsync(mcItem, cancellationToken, isGetAll);

            // If failed search and still active, retry with any parenthesized text removed
            if (IsActive
                && (ret.LyricResult == LyricsResultEnum.NotFound)
                && (mcItem.Artist.IsParenthesizedTextPresent() || mcItem.Album.IsParenthesizedTextPresent() || mcItem.Name.IsParenthesizedTextPresent()))
            {
                var mcItemClone = mcItem.CloneAndRemoveParenthesizedText();

                ret = await ProcessAsync(mcItemClone, cancellationToken, isGetAll);
            }

            LastSearchStop = DateTime.Now;

            return ret;
        }


        /// <summary>
        /// Sets the IsActive property to <c>false</c> and throws a quota error.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <exception cref="LyricsQuotaExceededException">Lyric service ... is exceeding its quota...</exception>
        protected virtual void QuotaError(McMplItem mcItem = null, bool isGetAll = false)
        {
            IsActive = false;

            throw new LyricsQuotaExceededException(Constants.NewLine
                + $"Lyric service \"{Credit.ServiceName}\" is exceeding its quota. " + Constants.NewLine
                + "The service is now disabled in LyricsFinder. " + Constants.NewLine
                + "Check the service in the lyric service form. " + Constants.NewLine
                + "No more requests will be sent to this service until corrected.",
                isGetAll, Credit, mcItem);
        }


        /// <summary>
        /// Refreshes the service settings from the service configuration file, if needed.
        /// </summary>
        public virtual async Task RefreshServiceSettingsAsync()
        {
            var assy = Assembly.GetAssembly(GetType());
            var config = ConfigurationManager.OpenExeConfiguration(assy.Location);

            if (!IsImplemented)
                IsActive = false;

            IsConfigurationFileUsed = (Credit == null)
                || (Comment.IsNullOrEmptyTrimmed()
                    && ((Credit.CreditUrl == null) || Credit.CreditUrl.AbsoluteUri.ToUpperInvariant().Contains("LOCALHOST"))
                    && ((Credit.ServiceUrl == null) || Credit.ServiceUrl.AbsoluteUri.ToUpperInvariant().Contains("LOCALHOST"))
                    && Credit.Company.IsNullOrEmptyTrimmed()
                    && Credit.ServiceName.IsNullOrEmptyTrimmed());

            if (IsConfigurationFileUsed)
            {
                Settings = config.AppSettings.Settings;

                Comment = ServiceSettingsValue(Settings, "Comment");

                Credit = new CreditType
                {
                    Company = ServiceSettingsValue(Settings, "Company"),
                    CreditDate = DateTime.Now,
                    CreditTextFormat = ServiceSettingsValue(Settings, "CreditTextFormat"),
                    CreditUrl = new UriBuilder(ServiceSettingsValue(Settings, "CreditUrl")).Uri,
                    DateFormat = ServiceSettingsValue(Settings, "DateFormat"),
                    ServiceName = ServiceSettingsValue(Settings, "ServiceName"),
                    ServiceUrl = new UriBuilder(ServiceSettingsValue(Settings, "ServiceUrl")).Uri,
                };
            }

            Credit.CreditDate = DateTime.Now;
            DelayMilliSecondsBetweenSearches = LyricsFinderData.MainData.DelayMilliSecondsBetweenSearches;

            if (LastRequest.Date < DateTime.Now.Date)
                await ResetTodayCountersAsync();

            CreateDisplayProperties();

            if (IsActive && await IsQuotaExceededAsync())
                QuotaError();
        }


        /// <summary>
        /// Resets the today counters.
        /// </summary>
        public virtual async Task ResetTodayCountersAsync()
        {
            // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
            await _semaphoreSlim.WaitAsync();

            try
            {
                HitCountToday = 0;
                RequestCountToday = 0;

                CreateDisplayProperties();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }


        /// <summary>
        /// Resets the total counters.
        /// </summary>
        public virtual async Task ResetTotalCountersAsync()
        {
            // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
            await _semaphoreSlim.WaitAsync();

            try
            {
                HitCountTotal = 0;
                RequestCountTotal = 0;

                CreateDisplayProperties();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }


        /// <summary>
        /// Gets the lyric service settings value.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// String representing the value of the setting.
        /// </returns>
        /// <exception cref="ArgumentNullException">settings</exception>
        /// <exception cref="IndexOutOfRangeException">Configuration value for key '{key}</exception>
        /// <exception cref="ArgumentException">Argument '{nameof(key)}</exception>
        protected static string ServiceSettingsValue(
            KeyValueConfigurationCollection settings,
            string key)
        {
            if ((settings == null) || (key.IsNullOrEmptyTrimmed()))
                throw new ArgumentNullException(nameof(settings));

            var ret = string.Empty;

            try
            {
                ret = settings[key].Value;
            }
            catch (Exception ex)
            {
                throw new IndexOutOfRangeException($"Configuration value for key '{key}' not found.", ex);
            }

            return ret;
        }


        /// <summary>
        /// Validates the display properties.
        /// </summary>
        public virtual void ValidateDisplayProperties()
        {
            var dps = new Dictionary<string, DisplayProperty>();

            Credit.ValidateDisplayProperties();

            // Test initialization
            dps.Add(nameof(Comment), Comment);
            dps.Add(nameof(RequestCountToday), RequestCountToday, null, RequestCountToday.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture));
            dps.Add(nameof(HitCountToday), HitCountToday, null, HitCountToday.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture));
            dps.Add(nameof(RequestCountTotal), RequestCountTotal, null, RequestCountTotal.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture));
            dps.Add(nameof(HitCountTotal), HitCountTotal, null, HitCountTotal.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture));
            dps.Add(nameof(TimeoutMilliSeconds), TimeoutMilliSeconds, null, TimeoutMilliSeconds.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture));
        }

    }

}
