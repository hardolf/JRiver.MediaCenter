using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Private configuration section handler for all LyricServices.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [Serializable]
    public class LyricServicesPrivateConfigurationSectionHandler
    {

        private const string _privateConfigFileExt = ".private.config";
        private const string _sectionName = "appSettings";
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
                    var privateConfigFile = (LyricServiceAssembly?.Location ?? "") + _privateConfigFileExt;

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
                            _configurationSection = privateConfig.GetSection(_sectionName) as AppSettingsSection;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error getting configuration section \"{_sectionName}\" in \"{privateConfigFile}\". Calling assembly: {LyricServiceAssembly.FullName}.", ex);
                    }

                    if ((LicenseManager.UsageMode != LicenseUsageMode.Designtime) && !(_configurationSection != null))
                        throw new ConfigurationErrorsException($"Configuration section \"{_sectionName}\" is not defined in \"{privateConfigFile}\".");
                }

                return _configurationSection;
            }
        }


        /// <summary>
        /// Creates the lyric services private configuration section handler.
        /// </summary>
        /// <param name="assembly">The lyric service assembly.</param>
        /// <returns>
        ///   <see cref="LyricServicesPrivateConfigurationSectionHandler" /> object.
        /// </returns>
        public static LyricServicesPrivateConfigurationSectionHandler CreateLyricServicesPrivateConfigurationSectionHandler(Assembly assembly)
        {
            return new LyricServicesPrivateConfigurationSectionHandler
            {
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

        private const string _privateConfigFileExt = ".private.config";
        private const string _sectionName = "appSettings";
        private const string _mcWebServiceAccessKeyPropertyName = "mcWebServiceAccessKey";
        private const string _mcWebServicePasswordPropertyName = "mcWebServicePassword";
        private const string _mcWebServiceUrlPropertyName = "mcWebServiceUrl";
        private const string _mcWebServiceUserNamePropertyName = "mcWebServiceUserName";

        private static AppSettingsSection _configurationSection;


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
                    var assy = Assembly.GetExecutingAssembly();
                    var privateConfigFile = (assy?.Location ?? "") + _privateConfigFileExt;

                    // Avoid analyzer warning CA1812
                    _ = new ValueConfigurationElement();

                    try
                    {
                        var map = new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = privateConfigFile
                        };
                        var privateConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                        using (new AddinCustomConfigResolveHelper(assy))
                        {
                            _configurationSection = privateConfig.GetSection(_sectionName) as AppSettingsSection;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error getting configuration section \"{_sectionName}\" in \"{privateConfigFile}\".", ex);
                    }

                    if ((LicenseManager.UsageMode != LicenseUsageMode.Designtime) && !(_configurationSection != null))
                        throw new ConfigurationErrorsException($"Configuration section \"{_sectionName}\" is not defined in \"{privateConfigFile}\".");
                }

                return _configurationSection;
            }
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
