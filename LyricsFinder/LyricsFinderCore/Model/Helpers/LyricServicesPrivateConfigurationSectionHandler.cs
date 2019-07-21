using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Private configuration section handler for all LyricServices.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ComVisible(false)]
    [Serializable]
    public class LyricServicesPrivateConfigurationSectionHandler
    {

        private const string _dailyQuotaPropertyName = "dailyQuota";
        private const string _tokenPropertyName = "token";
        private const string _UserIdPropertyName = "userId";

        [NonSerialized]
        private Configuration _privateConfiguration;

        [NonSerialized]
        private AppSettingsSection _configurationSection;


        /// <summary>
        /// Gets or sets the assembly of the lyric service.
        /// </summary>
        /// <value>
        /// The assembly location of the lyric service.
        /// </value>
        private Assembly LyricServiceAssembly { get; set; }

        /// <summary>
        /// Gets the Media Center MCWS service daily quota.
        /// </summary>
        /// <value>
        /// The Media Center MCWS service daily quota.
        /// </value>
        public int DailyQuota
        {
            get
            {
                var valueString = Instance?.Settings[_dailyQuotaPropertyName]?.Value ?? string.Empty;

                if (!int.TryParse(valueString, out var ret))
                    ret = 0;

                return ret;
            }
        }

        /// <summary>
        /// Gets or sets the data directory.
        /// </summary>
        /// <value>
        /// The data directory.
        /// </value>
        private string DataDirectory { get; set; }

        /// <summary>
        /// Gets the Media Center MCWS service token.
        /// </summary>
        /// <value>
        /// The Media Center MCWS service token.
        /// </value>
        public string Token => Instance?.Settings[_tokenPropertyName]?.Value ?? string.Empty;

        /// <summary>
        /// Gets the Media Center MCWS service user ID.
        /// </summary>
        /// <returns>
        /// The Media Center MCWS service user ID.
        /// </returns>
        public string UserId => Instance?.Settings[_UserIdPropertyName]?.Value ?? string.Empty;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException">Configuration section not defined.</exception>
        /// <value>
        /// The instance.
        /// </value>
        private AppSettingsSection Instance
        {
            get
            {
                IsUsed = true;

                // Get the configuration section handler fom file only once
                if (_configurationSection == null)
                {
                    var privateConfigFile = Utility.GetPrivateSettingsFilePath(LyricServiceAssembly, DataDirectory);

                    if (privateConfigFile.IsNullOrEmptyTrimmed()
                        || !File.Exists(privateConfigFile))
                        return null;

                    // Avoid analyzer warning CA1812
                    //_ = new ValueConfigurationElement();

                    try
                    {
                        var map = new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = privateConfigFile
                        };

                       _privateConfiguration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                        using (new AddinCustomConfigResolveHelper(LyricServiceAssembly))
                        {
                            _configurationSection = _privateConfiguration.GetSection(Utility.AppSettingsSectionName) as AppSettingsSection;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error getting configuration section \"{Utility.AppSettingsSectionName}\" in \"{privateConfigFile}\". Calling assembly: {LyricServiceAssembly.FullName}.", ex);
                    }

                    if ((LicenseManager.UsageMode != LicenseUsageMode.Designtime) && !(_configurationSection != null))
                        throw new ConfigurationErrorsException($"Configuration section \"{Utility.AppSettingsSectionName}\" is not defined in \"{privateConfigFile}\".");
                }

                return _configurationSection;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance have been used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance have been; otherwise, <c>false</c>.
        /// </value>
        public static bool IsUsed { get; set; } = false;


        /// <summary>
        /// Checks if this handler have been used and if not, delete the config file.
        /// </summary>
        public static void CheckUse()
        {
            if (!IsUsed)
            {
                // Delete the config file
            }
        }


        /// <summary>
        /// Creates the lyric services private configuration section handler.
        /// </summary>
        /// <param name="assembly">The lyric service assembly.</param>
        /// <param name="dataDirectory">The data directory.</param>
        /// <returns>
        ///   <see cref="LyricServicesPrivateConfigurationSectionHandler" /> object.
        /// </returns>
        public static LyricServicesPrivateConfigurationSectionHandler CreateLyricServicesPrivateConfigurationSectionHandler(Assembly assembly, string dataDirectory)
        {
            return new LyricServicesPrivateConfigurationSectionHandler
            {
                DataDirectory = dataDirectory,
                LyricServiceAssembly = assembly
            };
        }


        /*
        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="dailyQuota">The daily quota.</param>
        /// <param name="token">The token.</param>
        /// <param name="userId">The user identifier.</param>
        public void Save(int? dailyQuota = null, string token = null, string userId = null)
        {
            var intString = (dailyQuota == null)
                ? string.Empty
                : dailyQuota.Value.ToString(CultureInfo.InvariantCulture);

            SaveProperty(_dailyQuotaPropertyName, intString);
            SaveProperty(_tokenPropertyName, token);
            SaveProperty(_UserIdPropertyName, userId);

            _privateConfiguration.Save(ConfigurationSaveMode.Modified);
        }


        /// <summary>
        /// Saves the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void SaveProperty(string key, string value)
        {
            if (!value.IsNullOrEmptyTrimmed())
            {
                _privateConfiguration.AppSettings.Settings.Remove(key);
                _privateConfiguration.AppSettings.Settings.Add(key, value);
            }
        }
        */

    }

}
