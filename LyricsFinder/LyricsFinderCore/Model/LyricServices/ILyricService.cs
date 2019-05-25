using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.Shared;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Lyric service interface.
    /// </summary>
    public interface ILyricService
    {

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>
        /// The credit.
        /// </value>
        [XmlElement("Credit")]
        CreditType Credit { get; set; }

        /// <summary>
        /// Gets or sets the daily quota.
        /// </summary>
        /// <value>
        /// The daily quota.
        /// </value>
        [XmlElement]
        int DailyQuota { get; set; }

        /// <summary>
        /// Gets or sets a list of the found lyrics texts.
        /// </summary>
        /// <value>
        /// The list of found lyrics texts.
        /// </value>
        [ComVisible(false)]
        [XmlIgnore]
        ReadOnlyCollection<FoundLyricType> FoundLyricsList { get; set; }

        /// <summary>
        /// Gets or sets the lyrics result. If set to <c>Found</c> increments the hit counters.
        /// </summary>
        /// <value>
        /// The lyrics result.
        /// </value>
        [XmlIgnore]
        LyricsResultEnum LyricsResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AbstractLyricService"/> is active, i.e. should be part of the lyrics search.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        [XmlElement]
        bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is implemented.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is implemented; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        bool IsImplemented { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this service quota is exceeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this service quota is exceeded; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        bool IsQuotaExceeded { get; set; }

        /// <summary>
        /// Gets or sets the quota reset time, with time zone of the lyrics server.
        /// </summary>
        /// <value>
        /// The quota reset time.
        /// </value>
        [XmlElement]
        DateTimeWithZone QuotaResetTime { get; set; }

        /// <summary>
        /// Gets the result text.
        /// </summary>
        /// <returns>Result message for the status.</returns>
        [XmlIgnore]
        string LyricsResultMessage { get; }

        /// <summary>
        /// Gets or sets the request count today.
        /// </summary>
        /// <value>
        /// The request count today.
        /// </value>
        [XmlElement]
        int RequestCountToday { get; set; }

        /// <summary>
        /// Gets or sets the hit count today.
        /// </summary>
        /// <value>
        /// The hit count today.
        /// </value>
        [XmlElement]
        int HitCountToday { get; set; }

        /// <summary>
        /// Gets or sets the request count total.
        /// </summary>
        /// <value>
        /// The request count total.
        /// </value>
        [XmlElement]
        int RequestCountTotal { get; set; }

        /// <summary>
        /// Gets or sets the hit count total.
        /// </summary>
        /// <value>
        /// The hit count total.
        /// </value>
        [XmlElement]
        int HitCountTotal { get; set; }

    }

}
