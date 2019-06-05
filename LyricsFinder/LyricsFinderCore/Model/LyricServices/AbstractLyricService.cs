using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using log4net;

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
        /// Gets or sets the daily quota.
        /// </summary>
        /// <value>
        /// The daily quota.
        /// </value>
        [XmlElement]
        public virtual int DailyQuota { get; set; }

        /// <summary>
        /// Gets or sets the data directory.
        /// </summary>
        /// <value>
        /// The data directory.
        /// </value>
        [XmlIgnore]
        public virtual string DataDirectory { get; set; }

        /// <summary>
        /// Gets or sets the internal found lyric list.
        /// </summary>
        /// <value>
        /// The internal found lyric list.
        /// </value>
        [XmlIgnore]
        private FoundLyricListType<FoundLyricType> InternalFoundLyricList { get; set; }

        /// <summary>
        /// Gets or sets a list of the found lyric texts.
        /// </summary>
        /// <value>
        /// The list of found lyric texts.
        /// </value>
        [ComVisible(false)]
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
        /// Gets or sets a value indicating whether this service quota is exceeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this service quota is exceeded; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public virtual bool IsQuotaExceeded { get; set; }

        /// <summary>
        /// Gets or sets the quota reset time, with time zone of the lyric server.
        /// </summary>
        /// <value>
        /// The quota reset time.
        /// </value>
        [XmlElement]
        public virtual ServiceDateTimeWithZone QuotaResetTime { get; set; }

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
            DailyQuota = 0;
            InternalFoundLyricList = new FoundLyricListType<FoundLyricType>();
            FoundLyricList = new ReadOnlyCollection<FoundLyricType>(InternalFoundLyricList);
            IsActive = false;
            IsImplemented = false;
            IsQuotaExceeded = false;
            LyricResult = LyricResultEnum.NotProcessedYet;
            QuotaResetTime = new ServiceDateTimeWithZone(DateTime.Now.Date, TimeZoneInfo.Local); // Default is midnight in the client time zone
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
        public virtual FoundLyricType AddFoundLyric(string lyricText, SerializableUri lyricUrl, SerializableUri trackingUrl = null, string copyright = null)
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
        /// Checks the quota and adjusts the counters if necessary.
        /// </summary>
        public void CheckQuota()
        {
            // UTC date / time calculations for the quota
            var nowDate = DateTime.UtcNow.Date;
            var quotaDate = QuotaResetTime.UniversalTime.Date;
            var quotaDiffDays = (int)Math.Ceiling(nowDate.Subtract(quotaDate).TotalDays);

            if (quotaDiffDays > 0)
            {
                IsQuotaExceeded = false;
                QuotaResetTime.AddDays(quotaDiffDays);
                RequestCountToday = 0;
                HitCountToday = 0;

                Logging.Log(0, $"A new quota-day has begun for lyric service \"{Credit.ServiceName}\", request counters are reset.");
            }
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
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="getAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns></returns>
        public virtual AbstractLyricService Process(McMplItem item, bool getAll = false)
        {
            InternalFoundLyricList.Clear();
            LyricResult = LyricResultEnum.NotFound;
            CheckQuota();

            // Skip if we are over the daily limit
            if ((DailyQuota > 0) && (RequestCountToday > DailyQuota))
            {
                IsActive = false;
                IsQuotaExceeded = true;

                var msg = $"Lyric service \"{Credit.ServiceName}\" is over the daily limit of {DailyQuota} requests per day. \"{Credit.ServiceName}\" is now disabled in LyricsFinder and no more requests will be sent to this service today.";

                throw new LyricsQuotaExceededException(msg);
            }
            else
            {
                // We will query the service shortly, so we increment the counters now
                RequestCountToday++;
                RequestCountTotal++;
            }

            return this;
        }


        /// <summary>
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public virtual void RefreshServiceSettings()
        {
            var assy = Assembly.GetAssembly(GetType());
            var config = ConfigurationManager.OpenExeConfiguration(assy.Location);
            var settings = config.AppSettings.Settings;
            var privateSettings = LyricServicesPrivateConfigurationSectionHandler.CreateLyricServicesPrivateConfigurationSectionHandler(assy, DataDirectory);

            if (!IsImplemented)
                IsActive = false;

            Credit = new CreditType
            {
                Company = ServiceSettingsValue(settings, "Company"),
                CreditDate = DateTime.Now,
                CreditTextFormat = ServiceSettingsValue(settings, "CreditTextFormat"),
                CreditUrl = new SerializableUri(ServiceSettingsValue(settings, "CreditUrl")),
                DateFormat = ServiceSettingsValue(settings, "DateFormat"),
                ServiceName = ServiceSettingsValue(settings, "ServiceName"),
                ServiceUrl = new SerializableUri(ServiceSettingsValue(settings, "ServiceUrl")),
                Token = privateSettings.Token,
                UserId = privateSettings.UserId
            };

            var dailyQuotaString = privateSettings.DailyQuota;

            DailyQuota = (dailyQuotaString.IsNullOrEmptyTrimmed())
                ? 0
                : int.Parse(dailyQuotaString, CultureInfo.InvariantCulture);

            QuotaResetTime = new ServiceDateTimeWithZone(DateTime.Now.Date, TimeZoneInfo.FindSystemTimeZoneById(ServiceSettingsValue(settings, "QuotaResetTimeZone")));

            CheckQuota();
        }


        /// <summary>
        /// Gets the lyric service settings value.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// String representing the value of the setting.
        /// </returns>
        /// <exception cref="ArgumentException">Argument '{nameof(key)}</exception>
        /// <exception cref="IndexOutOfRangeException">Configuration value for key '{key}</exception>
        private string ServiceSettingsValue(
            KeyValueConfigurationCollection settings,
            string key)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (key.IsNullOrEmptyTrimmed()) throw new ArgumentException(nameof(settings));

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
