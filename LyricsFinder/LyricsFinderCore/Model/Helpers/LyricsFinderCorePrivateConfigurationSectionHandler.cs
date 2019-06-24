using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Private configuration section handler for LyricsFinderCore.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [Serializable]
    public static class LyricsFinderCorePrivateConfigurationSectionHandler
    {

        private const string _lastUpdateCheckPropertyName = "lastUpdateCheck";
        private const string _mcWebServiceAccessKeyPropertyName = "mcWebServiceAccessKey";
        private const string _mcWebServicePasswordPropertyName = "mcWebServicePassword";
        private const string _mcWebServiceUrlPropertyName = "mcWebServiceUrl";
        private const string _mcWebServiceUserNamePropertyName = "mcWebServiceUserName";
        private const string _updateCheckIntervalDaysPropertyName = "updateCheckIntervalDays";
        private const string _lyricFormSizePropertyName = "lyricFormSize";
        private const string _maxQueueLengthPropertyName = "maximumQueueLength";

        private static Configuration _privateConfiguration;
        private static AppSettingsSection _configurationSection;
        private static Assembly _assembly;
        private static string _dataDirectory;


        /// <summary>
        /// Gets the last update check.
        /// </summary>
        /// <value>
        /// The last update check.
        /// </value>
        public static DateTime LastUpdateCheck
        {
            get
            {
                var valueString = Instance?.Settings[_lastUpdateCheckPropertyName]?.Value ?? string.Empty;

                if (!DateTime.TryParse(valueString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var ret))
                    ret = DateTime.MinValue;

                return ret;
            }
        }

        /// <summary>
        /// Gets the Media Center MCWS service access key.
        /// </summary>
        /// <value>
        /// The Media Center MCWS service access key.
        /// </value>
        public static string McWebServiceAccessKey => Instance?.Settings[_mcWebServiceAccessKeyPropertyName]?.Value ?? string.Empty;

        /// <summary>
        /// Gets the Media Center MCWS service password.
        /// </summary>
        /// <returns>
        /// The Media Center MCWS service password.
        /// </returns>
        public static string McWebServicePassword => Instance?.Settings[_mcWebServicePasswordPropertyName]?.Value ?? "";

        /// <summary>
        /// Gets the Media Center MCWS service URL.
        /// </summary>
        /// <value>
        /// The Media Center MCWS service URL.
        /// </value>
        public static string McWebServiceUrl => Instance?.Settings[_mcWebServiceUrlPropertyName]?.Value ?? "";

        /// <summary>
        /// Gets the Media Center MCWS service user name.
        /// </summary>
        /// <returns>
        /// The Media Center MCWS service user name.
        /// </returns>
        public static string McWebServiceUserName => Instance?.Settings[_mcWebServiceUserNamePropertyName]?.Value ?? "";

        /// <summary>
        /// Gets the update check interval days.
        /// </summary>
        /// <value>
        /// The update check interval days.
        /// </value>
        public static int UpdateCheckIntervalDays
        {
            get
            {
                var valueString = Instance?.Settings[_updateCheckIntervalDaysPropertyName]?.Value ?? string.Empty;

                if (!int.TryParse(valueString, out var ret))
                    ret = 0;

                return ret;
            }
        }

        /// <summary>
        /// Gets the maximum length of the queue.
        /// </summary>
        /// <value>
        /// The maximum length of the queue.
        /// </value>
        public static int MaxQueueLength
        {
            get
            {
                var valueString = Instance?.Settings[_maxQueueLengthPropertyName]?.Value ?? string.Empty;

                if (!int.TryParse(valueString, out var ret))
                    ret = 10;

                return ret;
            }
        }

        /// <summary>
        /// Gets the size of the lyrics form.
        /// </summary>
        /// <value>
        /// The size of the lyrics form.
        /// </value>
        public static Size LyricFormSize
        {
            get
            {
                var valueString = Instance?.Settings[_lyricFormSizePropertyName]?.Value;
                var valueArray = Array.Empty<string>();

                if (!valueString.IsNullOrEmptyTrimmed())
                    valueArray = valueString.Replace(" ", string.Empty).Split(',');

                if (valueArray.Length == 0)
                    valueArray = new string[] { "400", "600" };

                if (valueArray.Length != 2)
                    throw new ConfigurationErrorsException($"Wrong size format. Expected: \"x,y\", actual value; \"{valueString}\".");

                foreach (var s in valueArray)
                {
                    if (!int.TryParse(s, out var _))
                        throw new ConfigurationErrorsException($"Wrong size format. Expected numbers: \"x,y\", actual value; \"{valueString}\".");
                }

                var ret = new Size(int.Parse(valueArray[0], CultureInfo.InvariantCulture), int.Parse(valueArray[1], CultureInfo.InvariantCulture));

                return ret;
            }
        }


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        /// <exception cref="ConfigurationErrorsException">Configuration section not defined.</exception>
        private static AppSettingsSection Instance
        {
            get
            {
                // Get the configuration section handler fom file only once
                if (_configurationSection == null)
                {
                    var privateConfigFile = Utility.GetPrivateSettingsFilePath(_assembly, _dataDirectory);

                    try
                    {
                        var map = new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = privateConfigFile
                        };

                        _privateConfiguration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                        using (new AddinCustomConfigResolveHelper(_assembly))
                        {
                            _configurationSection = _privateConfiguration.GetSection(Utility.AppSettingsSectionName) as AppSettingsSection;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error getting configuration section \"{Utility.AppSettingsSectionName}\" in \"{privateConfigFile}\".", ex);
                    }

                    if ((LicenseManager.UsageMode != LicenseUsageMode.Designtime) && !(_configurationSection != null))
                        throw new ConfigurationErrorsException($"Configuration section \"{Utility.AppSettingsSectionName}\" is not defined in \"{privateConfigFile}\".");
                }

                return _configurationSection;
            }
        }


        /// <summary>
        /// Initializes the LyricsFinder private configuration handler.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="dataDirectory">The data directory.</param>
        public static void Init(Assembly assembly, string dataDirectory)
        {
            _assembly = assembly;
            _dataDirectory = dataDirectory;
        }


        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="accessKey">The access key.</param>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="lastUpdateCheck">The last update check.</param>
        /// <param name="updateCheckIntervalDays">The update check interval days.</param>
        /// <param name="maximumQueueLength">Maximum length of the queue.</param>
        /// <param name="lyricFormSize">Size of the lyric form.</param>
        public static void Save(string accessKey = null, string serviceUrl = null, string username = null, string password = null
            , DateTime? lastUpdateCheck = null, int? updateCheckIntervalDays = null, int? maximumQueueLength = null, Size? lyricFormSize = null)
        {
            var intStringMax = (maximumQueueLength == null)
                ? string.Empty
                : maximumQueueLength.Value.ToString(CultureInfo.InvariantCulture);

            var intStringUpd = (updateCheckIntervalDays == null)
                ? string.Empty
                : updateCheckIntervalDays.Value.ToString(CultureInfo.InvariantCulture);

            var dateString = (lastUpdateCheck == null)
                ? string.Empty
                : lastUpdateCheck.Value.ToString(CultureInfo.CurrentCulture);

            var sizeString = (lyricFormSize == null)
                ? string.Empty
                : $"{lyricFormSize.Value.Width},{lyricFormSize.Value.Height}";

            SaveProperty(_lastUpdateCheckPropertyName, dateString);
            SaveProperty(_mcWebServiceAccessKeyPropertyName, accessKey);
            SaveProperty(_mcWebServiceUrlPropertyName, serviceUrl);
            SaveProperty(_mcWebServiceUserNamePropertyName, username);
            SaveProperty(_mcWebServicePasswordPropertyName, password);
            SaveProperty(_maxQueueLengthPropertyName, intStringMax);
            SaveProperty(_updateCheckIntervalDaysPropertyName, intStringUpd);
            SaveProperty(_lyricFormSizePropertyName, sizeString);

            _privateConfiguration.Save(ConfigurationSaveMode.Modified);
        }


        /// <summary>
        /// Saves the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private static void SaveProperty(string key, string value)
        {
            if (!value.IsNullOrEmptyTrimmed())
            {
                _privateConfiguration.AppSettings.Settings.Remove(key);
                _privateConfiguration.AppSettings.Settings.Add(key, value);
            }
        }

    }

}
