using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// DateTime structure with time zone.
    /// </summary>
    /// <remarks>
    /// Source inspired by: <see href="https://stackoverflow.com/questions/246498/creating-a-datetime-in-a-specific-time-zone-in-c-sharp"/>
    /// </remarks>
    [Serializable]
    public class ServiceDateTimeWithZone
    {

        /// <summary>
        /// Gets the universal time.
        /// </summary>
        /// <value>
        /// The universal time.
        /// </value>
        [XmlElement]
        public DateTime UniversalTime { get; set; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        [XmlIgnore]
        public TimeZoneInfo ServiceTimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId); }
            set { TimeZoneId = value?.Id ?? TimeZoneInfo.Utc.ToString(); }
        }

        /// <summary>
        /// Gets or sets the time zone information serialized.
        /// </summary>
        /// <value>
        /// The time zone information serialized.
        /// </value>
        [XmlElement]
        public string TimeZoneId { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDateTimeWithZone"/> struct.
        /// </summary>
        public ServiceDateTimeWithZone()
            : this(DateTime.Now, TimeZoneInfo.Local)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDateTimeWithZone"/> struct.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="timeZone">The time zone.</param>
        public ServiceDateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
        {
            var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

            UniversalTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);
            ServiceTimeZone = timeZone;
        }


        /// <summary>
        /// Adds the number of days to the <see cref="UniversalTime" property./>.
        /// </summary>
        /// <param name="days">The number of days.</param>
        public void AddDays(int days)
        {
            UniversalTime = UniversalTime.AddDays(days);
        }


        /// <summary>
        /// Gets the local time for the client (this program).
        /// </summary>
        /// <value>
        /// The local time for the client (this program).
        /// </value>
        public DateTime ClientLocalTime
        {
            get
            {
                return UniversalTime.ToLocalTime();
            }
        }


        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static ServiceDateTimeWithZone Parse(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            var zone = string.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                char c = (char)s[i];

                if (!":0123456789".Contains(c))
                {
                    zone = s.Substring(i).ToUpperInvariant();
                    break;
                }
            }

            var dt = DateTime.Parse(s, CultureInfo.InvariantCulture);
            var zoneId = TimeZoneInfo.FindSystemTimeZoneById(zone);
            var ret = new ServiceDateTimeWithZone(dt, zoneId);

            return ret;
        }


        /// <summary>
        /// Gets the local time for the service.
        /// </summary>
        /// <value>
        /// The local time for the service.
        /// </value>
        public DateTime ServiceLocalTime
        {
            get
            {
                return TimeZoneInfo.ConvertTime(UniversalTime, ServiceTimeZone);
            }
        }

    }

}
