using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Private configuration section handler for LyricsFinderCore.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ComVisible(false)]
    [Serializable]
    public static class LyricsFinderCoreConfigurationSectionHandler
    {

        private const string _localAppDataFilePropertyName = "localAppDataFile";
        private const string _mcNoLyricsSearchListPropertyName = "mcNoLyricsSearchList";
        private const string _MaxMcWsConnectAttemptsPropertyName = "MaxMcWsConnectAttempts";
        private const string _mouseMoveOpenLyricsFormPropertyName = "mouseMoveOpenLyricsForm";

        private static Configuration _configuration;
        private static AppSettingsSection _configurationSection;
        private static Assembly _assembly;


        /// <summary>
        /// Gets the Media Center MCWS service password.
        /// </summary>
        /// <returns>
        /// The Media Center MCWS service password.
        /// </returns>
        public static string LocalAppDataFile => Instance?.Settings[_localAppDataFilePropertyName]?.Value ?? string.Empty;

        /// <summary>
        /// Gets the Media Center MCWS service URL.
        /// </summary>
        /// <value>
        /// The Media Center MCWS service URL.
        /// </value>
        public static string McNoLyricsSearchList => Instance?.Settings[_mcNoLyricsSearchListPropertyName]?.Value ?? string.Empty;

        /// <summary>
        /// Gets a value indicating whether [mouse move open lyrics form].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mouse move open lyrics form]; otherwise, <c>false</c>.
        /// </value>
        public static bool MouseMoveOpenLyricsForm
        {
            get
            {
                var valueString = Instance?.Settings[_mouseMoveOpenLyricsFormPropertyName]?.Value ?? string.Empty;

                if (!bool.TryParse(valueString, out var ret))
                    ret = false;

                return ret;
            }
        }

        /// <summary>
        /// Gets the mc ws connect attempts.
        /// </summary>
        /// <value>
        /// The mc ws connect attempts.
        /// </value>
        public static int MaxMcWsConnectAttempts
        {
            get
            {
                var valueString = Instance?.Settings[_MaxMcWsConnectAttemptsPropertyName]?.Value ?? string.Empty;

                if (!int.TryParse(valueString, out var ret))
                    ret = 5;

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
                // Get calling method name
                var stackTrace = new StackTrace();
                var callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

                if (!callingMethod.Contains(nameof(LocalAppDataFile)))
                    IsUsed = true;

                // Get the configuration section handler fom file only once
                if (_configurationSection == null)
                {
                    var configFile = Utility.GetAppSettingsFilePath(_assembly);

                    try
                    {
                        var map = new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = configFile
                        };

                        _configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                        using (new AddinCustomConfigResolveHelper(_assembly))
                        {
                            _configurationSection = _configuration.GetSection(Utility.AppSettingsSectionName) as AppSettingsSection;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error getting configuration section \"{Utility.AppSettingsSectionName}\" in \"{configFile}\".", ex);
                    }

                    if ((LicenseManager.UsageMode != LicenseUsageMode.Designtime) && !(_configurationSection != null))
                        throw new ConfigurationErrorsException($"Configuration section \"{Utility.AppSettingsSectionName}\" is not defined in \"{configFile}\".");
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
        /// Initializes the LyricsFinder private configuration handler.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void Init(Assembly assembly)
        {
            _assembly = assembly;
        }

    }

}
