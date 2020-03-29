using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
    [ComVisible(false)]
    [Serializable]
    public class MainDataType
    {

        private Size _lyricFormSize;


        /// <summary>
        /// Gets or sets the delay milliseconds between item searches.
        /// </summary>
        /// <value>
        /// The delay milliseconds between searches.
        /// </value>
        [XmlElement]
        public int DelayMilliSecondsBetweenSearches { get; set; }

        /// <summary>
        /// Gets or sets the last Media Center status check.
        /// </summary>
        /// <value>
        /// The last Media Center status check.
        /// </value>
        [XmlElement]
        public DateTime LastMcStatusCheck { get; set; }

        /// <summary>
        /// Gets or sets the last update check date/time.
        /// </summary>
        /// <value>
        /// The last update check date/time.
        /// </value>
        [XmlElement]
        public DateTime LastUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets the lyric form location.
        /// </summary>
        /// <value>
        /// The lyric form location.
        /// </value>
        [XmlElement]
        public Point LyricFormLocation { get; set; }

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
        /// Gets or sets a value indicating whether service requests should be sent in sequence during automatic search.
        /// </summary>
        /// <value>
        ///   <c>true</c> if service requests should be sent in sequence during automatic search; otherwise, <c>false</c>, the requests are sent in parallel.
        /// </value>
        /// <remarks>
        /// <para>This property is typically set to <c>true</c> when service requests should be kept at a minimum due to service request quota limits.</para>
        /// <para>The downside with serial requests is longer automatic search times.</para>
        /// </remarks>
        [XmlElement]
        public bool SerialServiceRequestsDuringAutomaticSearch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether strict search only is to be used.
        /// In a strict search, both artist and song name must mach; else a match on song name is sufficient.
        /// </summary>
        /// <value>
        ///   <c>true</c> if strict search only is to be used; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        public bool StrictSearchOnly { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MainDataType"/> class.
        /// </summary>
        public MainDataType()
        {
            // Set defaults
            MaxQueueLength = 10;
            MaxMcWsConnectAttempts = 10;
            McAccessKey = string.Empty;
            McWsPassword = string.Empty;
            McWsUrl = "http://localhost:52199/MCWS/v1";
            McWsUsername = string.Empty;
            NoLyricsSearchFilter = string.Empty;
            UpdateCheckIntervalDays = 0; // Default: 0, i.e. at each Media Center start
        }

    }

}
