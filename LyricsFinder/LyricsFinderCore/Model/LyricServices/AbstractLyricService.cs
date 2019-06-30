using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Abstract lyric service type.
    /// </summary>
    [Serializable]
    public abstract class AbstractLyricService
    {

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>
        /// The credit.
        /// </value>
        [XmlElement("Credit")]
        public virtual CreditType Credit { get; set; }

        /// <summary>
        /// Gets or sets the data directory.
        /// </summary>
        /// <value>
        /// The data directory.
        /// </value>
        [XmlIgnore]
        public virtual string DataDirectory { get; set; }

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
        /// Gets or sets a value indicating whether the obsolete configurations have been used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the obsolete configurations have been used; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public static bool IsObsoleteConfigurationsUsed { get; set; } = false;

        /// <summary>
        /// Gets or sets the exceptions.
        /// </summary>
        /// <value>
        /// The exceptions.
        /// </value>
        [XmlIgnore]
        public List<Exception> Exceptions { get; private set; }

        /// <summary>
        /// Gets or sets a list of the found lyric texts.
        /// </summary>
        /// <value>
        /// The list of found lyric texts.
        /// </value>
        [XmlIgnore]
        public ReadOnlyCollection<FoundLyricType> FoundLyricList { get; }

        /// <summary>
        /// Gets or sets the lyric result. If set to <c>Found</c> increments the hit counters.
        /// </summary>
        /// <value>
        /// The lyric result.
        /// </value>
        [XmlIgnore]
        public virtual LyricResultEnum LyricResult { get; set; }

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
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        [XmlIgnore]
        protected virtual KeyValueConfigurationCollection Settings { get; private set; }

        /// <summary>
        /// Gets or sets the private settings.
        /// </summary>
        /// <value>
        /// The private settings.
        /// </value>
        [XmlIgnore]
        public virtual LyricServicesPrivateConfigurationSectionHandler PrivateSettings { get; set; }

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

                if (LyricResult == LyricResultEnum.Found)
                    ret.Append($" by service \"{Credit.ServiceName}\"");

                return ret.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the request count today.
        /// </summary>
        /// <value>
        /// The request count today.
        /// </value>
        [XmlElement]
        public virtual int RequestCountToday { get; set; }

        /// <summary>
        /// Gets or sets the hit count today.
        /// </summary>
        /// <value>
        /// The hit count today.
        /// </value>
        [XmlElement]
        public virtual int HitCountToday { get; set; }

        /// <summary>
        /// Gets or sets the request count total.
        /// </summary>
        /// <value>
        /// The request count total.
        /// </value>
        [XmlElement]
        public virtual int RequestCountTotal { get; set; }

        /// <summary>
        /// Gets or sets the hit count total.
        /// </summary>
        /// <value>
        /// The hit count total.
        /// </value>
        [XmlElement]
        public virtual int HitCountTotal { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLyricService"/> class.
        /// </summary>
        public AbstractLyricService()
        {
            Credit = new CreditType();
            DisplayProperties = new Dictionary<string, DisplayProperty>();
            Exceptions = new List<Exception>();
            InternalFoundLyricList = new FoundLyricListType<FoundLyricType>();
            FoundLyricList = new ReadOnlyCollection<FoundLyricType>(InternalFoundLyricList);
            IsActive = false;
            IsImplemented = false;
            LyricResult = LyricResultEnum.NotProcessedYet;
        }


        /// <summary>
        /// Adds the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="request">Optional request.</param>
        public void AddException(Exception exception, string request = null)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            var msg = $"\"{Credit.ServiceName}\" call failed: \"{exception.Message}\"";

            if (!request.IsNullOrEmptyTrimmed())
                msg += $" Request: \"{request}\".";

            Exceptions.Add(new Exception(msg, exception));
        }


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
        public virtual FoundLyricType AddFoundLyric(string lyricText, Uri lyricUrl, Uri trackingUrl = null, string copyright = null)
        {
            if (!copyright.IsNullOrEmptyTrimmed())
                Credit.Copyright = copyright;

            var beforeCount = InternalFoundLyricList.Count;
            var ret = InternalFoundLyricList.Add(this, lyricText, Credit.ToString(), lyricUrl, trackingUrl);
            var afterCount = InternalFoundLyricList.Count;
            var diff = (afterCount - beforeCount);

            // This construct ensures that we don't count duplicates
            HitCountToday += diff;
            HitCountTotal += diff;

            LyricResult = LyricResultEnum.Found;

            return ret;
        }


        /// <summary>
        /// Creates the service client.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName">Name of the configuration.</param>
        /// <returns>
        /// Service client.
        /// </returns>
        /// <remarks>
        /// Source inspired by: https://www.codeproject.com/Articles/1060520/Centralizing-WCF-Client-Configuration-in-a-Class-L
        /// </remarks>
        protected static T CreateServiceClient<T>(string configName)
        {
            var assemblyLocation = string.Empty;
            Configuration config = null;
            ConfigurationChannelFactory<T> channelFactory = null;
            T ret;

            try
            {
                assemblyLocation = Assembly.GetCallingAssembly().Location;
                config = ConfigurationManager.OpenExeConfiguration(assemblyLocation);
                channelFactory = new ConfigurationChannelFactory<T>(configName, config, null);

                ret = channelFactory.CreateChannel();

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating the service client\r\n\r\nAssembly location: {assemblyLocation}\r\nConfiguration file path: {config?.FilePath}\r\nEndpoint configuration name: {configName}.\r\n\r\n", ex);
            }
        }


        /// <summary>
        /// Extracts all the lyrics from all the Uris.
        /// </summary>
        /// <param name="uris">The uris.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> extracts all lyrics from all the Uris; else exits after the first hit.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">uris</exception>
        protected virtual async Task ExtractAllLyricTextsAsync(IEnumerable<Uri> uris, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (uris == null) throw new ArgumentNullException(nameof(uris));

            if (isGetAll)
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
                // Serial search, probably hits on the first try
                foreach (var uri in uris)
                {
                    var lyricText = await ExtractOneLyricTextAsync(uri, cancellationToken).ConfigureAwait(false);

                    if (!lyricText.IsNullOrEmptyTrimmed())
                        break;
                }
            }
        }


        /// <summary>
        /// Get string asynchronous from HTTP request to the lyric service.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>Response text from lyric service.</returns>
        /// <remarks>We use a central routine here, so that the request counters may be properly updated.</remarks>
        protected virtual async Task<string> HttpGetStringAsync(Uri requestUri)
        {
            // One more request...
            RequestCountToday++;
            RequestCountTotal++;

            var ret = await Helpers.Utility.HttpGetStringAsync(requestUri).ConfigureAwait(false);

            return ret;
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

            cancellationToken.ThrowIfCancellationRequested();

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
        /// Gets value indicating whether this service's quota is exceeded.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this service's quota is exceeded; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsQuotaExceeded()
        {
            return false;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricsQuotaExceededException">Lyric service \"{Credit.ServiceName}\" is exceeding its quota and is now disabled in LyricsFinder, "
        /// + "no more requests will be sent to this service until corrected.</exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<AbstractLyricService> ProcessAsync(McMplItem item, CancellationToken cancellationToken, bool isGetAll = false)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            cancellationToken.ThrowIfCancellationRequested();

            Exceptions.Clear();
            InternalFoundLyricList.Clear();
            LyricResult = LyricResultEnum.NotFound;


            // Skip if we are over the quota limit
            if (IsQuotaExceeded())
            {
                IsActive = false;

                throw new LyricsQuotaExceededException($"Lyric service \"{Credit.ServiceName}\" is exceeding its quota and is now disabled in LyricsFinder, "
                    + "no more requests will be sent to this service until corrected.");
            }

            return this;
        }


        /// <summary>
        /// Refreshes the display properties.
        /// </summary>
        public virtual void RefreshDisplayProperties()
        {
            DisplayProperties.Clear();

            foreach (var dp in Credit.DisplayProperties)
            {
                DisplayProperties.Add(dp.Key, dp.Value); 
            }

            DisplayProperties.Add(nameof(RequestCountToday), new DisplayProperty("Requests, today", RequestCountToday.ToString(Constants.IntegerFormat, CultureInfo.InvariantCulture)));
            DisplayProperties.Add(nameof(HitCountToday), new DisplayProperty("Hits, today", HitCountToday.ToString(Constants.IntegerFormat, CultureInfo.InvariantCulture)));
            DisplayProperties.Add(nameof(RequestCountTotal), new DisplayProperty("Requests, total", RequestCountTotal.ToString(Constants.IntegerFormat, CultureInfo.InvariantCulture)));
            DisplayProperties.Add(nameof(HitCountTotal), new DisplayProperty("Hits, total", HitCountTotal.ToString(Constants.IntegerFormat, CultureInfo.InvariantCulture)));
        }


        /// <summary>
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public virtual void RefreshServiceSettings()
        {
            var assy = Assembly.GetAssembly(GetType());
            var config = ConfigurationManager.OpenExeConfiguration(assy.Location);

            if (!IsImplemented)
                IsActive = false;

            IsObsoleteConfigurationsUsed = ((Credit == null)
                || ((Credit.CreditUrl == null) || Credit.CreditUrl.AbsoluteUri.ToUpper(CultureInfo.InvariantCulture).Contains("LOCALHOST"))
                || ((Credit.ServiceUrl == null) || Credit.ServiceUrl.AbsoluteUri.ToUpper(CultureInfo.InvariantCulture).Contains("LOCALHOST"))
                || Credit.Company.IsNullOrEmptyTrimmed()
                || Credit.CreditTextFormat.IsNullOrEmptyTrimmed()
                || Credit.DateFormat.IsNullOrEmptyTrimmed()
                || Credit.ServiceName.IsNullOrEmptyTrimmed());

            if (IsObsoleteConfigurationsUsed)
            {
                Settings = config.AppSettings.Settings;
                PrivateSettings = LyricServicesPrivateConfigurationSectionHandler.CreateLyricServicesPrivateConfigurationSectionHandler(assy, DataDirectory);

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
            Credit.RefreshDisplayProperties();
            RefreshDisplayProperties();
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

    }

}
