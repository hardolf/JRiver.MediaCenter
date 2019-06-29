using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;

namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Main LyricsFinder data such as update info and connection data for the JRiver Media Center.
    /// </summary>
    [Serializable]
    public class MainDataType
    {

        private Size _lyricFormSize;


        /// <summary>
        /// Gets or sets the last update check date/time.
        /// </summary>
        /// <value>
        /// The last update check date/time.
        /// </value>
        [XmlElement]
        public DateTime LastUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets the size of the lyric form.
        /// </summary>
        /// <value>
        /// The size of the lyric form.
        /// </value>
        [XmlElement]
        public Size LyricFormSize
        {
            get => (_lyricFormSize.Height == 0 || _lyricFormSize.Width == 0) ? new Size(400, 600) : _lyricFormSize;
            set => _lyricFormSize = value;
        }

        /// <summary>
        /// Gets or sets the number of attempts to connect with the Media Center web service (MCWS) before showing an error.
        /// </summary>
        /// <value>
        /// The Media Center web service (MCWS) connect attempts (default: 10).
        /// </value>
        [XmlElement]
        public int MaxMcWsConnectAttempts { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the queue during automatic search all.
        /// </summary>
        /// <value>
        /// The maximum length of the queue (default: 10).
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
        /// The Media Center web service (MCWS) URL (default: http://localhost:52199/MCWS/v1).
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
        /// Gets or sets a value indicating whether a mouse move in the lyrics column should open the lyrics form.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a mouse move in the lyrics column should open the lyrics form; otherwise, <c>false</c> (default).
        /// </value>
        [XmlElement]
        public bool MouseMoveOpenLyricsForm { get; set; }

        /// <summary>
        /// Gets or sets the lyrics search filter, a comma or semicolon separated list of strings.
        /// </summary>
        /// <value>
        /// The lyrics search filter.
        /// </value>
        [XmlElement]
        public string NoLyricsSearchFilter { get; set; }

        /// <summary>
        /// Gets or sets the update check interval days.
        /// </summary>
        /// <value>
        /// The update check interval days (default: 0, i.e. at each Media Center start).
        /// </value>
        [XmlElement]
        public int UpdateCheckIntervalDays { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MainDataType"/> class.
        /// </summary>
        public MainDataType()
        {
            // Set defaults
            MaxQueueLength = 10;
            MaxMcWsConnectAttempts = 10;
            McWsUrl = "http://localhost:52199/MCWS/v1";
            UpdateCheckIntervalDays = 0; // Default: 0, i.e. at each Media Center start
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MainDataType" /> class.
        /// </summary>
        /// <param name="maxQueueLength">Maximum length of the queue.</param>
        /// <param name="maxMcWsConnectAttempts">The mc ws connect attempts.</param>
        /// <param name="mcWsUrl">The mc ws URL.</param>
        /// <param name="mouseMoveOpenLyricsForm">if set to <c>true</c> [mouse move open lyrics form].</param>
        /// <param name="updateCheckIntervalDays">The update check interval days.</param>
        /// <exception cref="ArgumentNullException">mcWsUrl</exception>
        public MainDataType(int maxQueueLength, int maxMcWsConnectAttempts, string mcWsUrl, bool mouseMoveOpenLyricsForm, int updateCheckIntervalDays)
        {
            MaxQueueLength = maxQueueLength;
            MaxMcWsConnectAttempts = maxMcWsConnectAttempts;
            McWsUrl = mcWsUrl ?? throw new ArgumentNullException(nameof(mcWsUrl));
            MouseMoveOpenLyricsForm = mouseMoveOpenLyricsForm;
            UpdateCheckIntervalDays = updateCheckIntervalDays;
        }


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
                MaxMcWsConnectAttempts = LyricsFinderCoreConfigurationSectionHandler.MaxMcWsConnectAttempts,
                McWsPassword = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServicePassword,
                McWsUrl = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl,
                McWsUsername = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUserName,
                MouseMoveOpenLyricsForm = LyricsFinderCoreConfigurationSectionHandler.MouseMoveOpenLyricsForm,
                NoLyricsSearchFilter = LyricsFinderCoreConfigurationSectionHandler.McNoLyricsSearchList,
                UpdateCheckIntervalDays = LyricsFinderCorePrivateConfigurationSectionHandler.UpdateCheckIntervalDays
            };

            return ret;
        }

    }

}
