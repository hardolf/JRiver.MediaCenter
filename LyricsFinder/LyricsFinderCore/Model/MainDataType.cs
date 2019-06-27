using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;

namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Main LyricsFinder data such as update info and connection data for the JRiver Media Center.
    /// </summary>
    [Serializable]
    public class MainDataType
    {

        /// <summary>
        /// Gets or sets the last update check date/time.
        /// </summary>
        /// <value>
        /// The last update check date/time.
        /// </value>
        [XmlElement]
        public DateTime LastUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the queue.
        /// </summary>
        /// <value>
        /// The maximum length of the queue.
        /// </value>
        [XmlElement]
        public int MaxQueueLength { get; set; }

        /// <summary>
        /// Gets or sets the Media Center web service (MCWS) access key.
        /// </summary>
        /// <value>
        /// The Media Center web service (MCWS) access key.
        /// </value>
        [XmlElement]
        public string McAccessKey { get; set; }

        /// <summary>
        /// Gets or sets the Media Center web service (MCWS) password.
        /// </summary>
        /// <value>
        /// The Media Center web service (MCWS) password.
        /// </value>
        [XmlElement]
        public string McWsPassword { get; set; }

        /// <summary>
        /// Gets or sets the Media Center web service (MCWS) URL.
        /// </summary>
        /// <value>
        /// The Media Center web service (MCWS) URL.
        /// </value>
        [XmlElement]
        public string McWsUrl { get; set; }

        /// <summary>
        /// Gets or sets the Media Center web service (MCWS) username.
        /// </summary>
        /// <value>
        /// The Media Center web service (MCWS) username.
        /// </value>
        [XmlElement]
        public string McWsUsername { get; set; }

        /// <summary>
        /// Gets or sets the update check interval days.
        /// </summary>
        /// <value>
        /// The update check interval days.
        /// </value>
        [XmlElement]
        public int UpdateCheckIntervalDays { get; set; }


        /// <summary>
        /// Creates a <see cref="MainDataType" /> object from configuration file.
        /// </summary>
        /// <returns>
        ///   <see cref="MainDataType" /> object.
        /// </returns>
        public static MainDataType CreateFromConfiguration()
        {
            var assy = Assembly.GetExecutingAssembly();
            var dataFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(LyricsFinderCoreConfigurationSectionHandler.LocalAppDataFile));
            var dataDirectory = Path.GetDirectoryName(dataFile);

            LyricsFinderCorePrivateConfigurationSectionHandler.Init(assy, dataDirectory);

            var ret = new MainDataType()
            {
                LastUpdateCheck = LyricsFinderCorePrivateConfigurationSectionHandler.LastUpdateCheck,
                MaxQueueLength = LyricsFinderCorePrivateConfigurationSectionHandler.MaxQueueLength,
                McAccessKey = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceAccessKey,
                McWsPassword = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServicePassword,
                McWsUrl = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl,
                McWsUsername = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUserName,
                UpdateCheckIntervalDays = LyricsFinderCorePrivateConfigurationSectionHandler.UpdateCheckIntervalDays
            };

            return ret;
        }

    }

}
