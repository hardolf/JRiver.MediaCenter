using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Private configuration section handler for all LyricServices.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [Serializable]
    public class LyricServicesPrivateConfigurationSectionHandler
    {

        private const string _dailyQuotaPropertyName = "dailyQuota";
        private const string _tokenPropertyName = "token";
        private const string _UserIdPropertyName = "userId";

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
        public string DailyQuota => Instance?.Settings[_dailyQuotaPropertyName]?.Value ?? string.Empty;

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
                // Get the configuration section handler fom file only once
                if (_configurationSection == null)
                {
                    var privateConfigFile = Utility.GetPrivateSettingsFilePath(LyricServiceAssembly, DataDirectory);

                    if (privateConfigFile.IsNullOrEmptyTrimmed())
                        return null;

                    // Avoid analyzer warning CA1812
                    _ = new ValueConfigurationElement();

                    try
                    {
                        var map = new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = privateConfigFile
                        };
                        var privateConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                        using (new AddinCustomConfigResolveHelper(LyricServiceAssembly))
                        {
                            _configurationSection = privateConfig.GetSection(Utility.AppSettingsSectionName) as AppSettingsSection;
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

    }



    /// <summary>
    /// Private configuration section handler for LyricsFinderCore.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [Serializable]
    public static class LyricsFinderCorePrivateConfigurationSectionHandler
    {

        private const string _mcWebServiceAccessKeyPropertyName = "mcWebServiceAccessKey";
        private const string _mcWebServicePasswordPropertyName = "mcWebServicePassword";
        private const string _mcWebServiceUrlPropertyName = "mcWebServiceUrl";
        private const string _mcWebServiceUserNamePropertyName = "mcWebServiceUserName";

        private static Configuration _privateConfiguration;
        private static AppSettingsSection _configurationSection;
        private static Assembly _assembly;
        private static string _dataDirectory;


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

                    // Avoid analyzer warning CA1812
                    _ = new ValueConfigurationElement();

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
        public static void Save(string accessKey, string serviceUrl, string username, string password)
        {
            _privateConfiguration.AppSettings.Settings.Remove(_mcWebServiceAccessKeyPropertyName);
            _privateConfiguration.AppSettings.Settings.Remove(_mcWebServicePasswordPropertyName);
            _privateConfiguration.AppSettings.Settings.Remove(_mcWebServiceUrlPropertyName);
            _privateConfiguration.AppSettings.Settings.Remove(_mcWebServiceUserNamePropertyName);

            _privateConfiguration.AppSettings.Settings.Add(_mcWebServiceAccessKeyPropertyName, accessKey);
            _privateConfiguration.AppSettings.Settings.Add(_mcWebServicePasswordPropertyName, password);
            _privateConfiguration.AppSettings.Settings.Add(_mcWebServiceUrlPropertyName, serviceUrl);
            _privateConfiguration.AppSettings.Settings.Add(_mcWebServiceUserNamePropertyName, username);

            _privateConfiguration.Save(ConfigurationSaveMode.Modified);
        }

    }



    /// <summary>
    /// Value configuration element.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    [Serializable]
    public class ValueConfigurationElement : ConfigurationElement
    {

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [ConfigurationProperty("value", IsRequired = true, DefaultValue = "")]
        public string Value => (string)this["value"];

    }

}
