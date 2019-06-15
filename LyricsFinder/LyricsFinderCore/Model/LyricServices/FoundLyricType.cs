using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// List type with overridden Add methods, removing duplicates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{FoundLyricType}" />
    internal class FoundLyricListType<T> : List<FoundLyricType> where T : FoundLyricType, new()
    {

        /// <summary>
        /// Adds the specified found lyric.
        /// </summary>
        /// <param name="foundLyric">The found lyric.</param>
        public void Add(T foundLyric)
        {
            if (Exists(foundLyric.LyricText, foundLyric.LyricUrl?.ToString() ?? string.Empty, foundLyric.TrackingUrl?.ToString() ?? string.Empty))
                return;

            base.Add(foundLyric);
        }


        /// <summary>
        /// Adds the specified lyric text.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="lyricText">The lyric text.</param>
        /// <param name="lyricCreditText">The lyric credit text.</param>
        /// <param name="lyricUrl">The lyric URL.</param>
        /// <param name="trackingUrl">The tracking URL.</param>
        /// <returns>
        /// Added object.
        /// </returns>
        public T Add(AbstractLyricService service, string lyricText, string lyricCreditText, SerializableUri lyricUrl, SerializableUri trackingUrl)
        {
            var ret = new T
            {
                LyricCreditText = lyricCreditText,
                LyricText = lyricText,
                LyricUrl = lyricUrl,
                Service = service,
                TrackingUrl = trackingUrl
            };

            Add(ret);

            return ret;
        }


        /// <summary>
        /// Tests if the specified lyric text is in the collection.
        /// </summary>
        /// <param name="lyricText">The lyric text.</param>
        /// <param name="lyricUrl">The lyric URL.</param>
        /// <param name="trackingUrl">The tracking URL.</param>
        /// <returns>
        ///   <c>true</c> if the lyric text is in the collection; otherwise, <c>false</c>.
        /// </returns>
        public bool Exists(string lyricText, string lyricUrl = null, string trackingUrl = null)
        {
            if (this.Any(l => l.LyricText == lyricText))
                return true;

            if (!lyricUrl.IsNullOrEmptyTrimmed() && this.Any(l => l.LyricUrl.Equals(lyricUrl)))
                return true;

            if (!trackingUrl.IsNullOrEmptyTrimmed() && this.Any(l => l.TrackingUrl.Equals(trackingUrl)))
                return true;

            return false;
        }

    }



    /// <summary>
    /// Lyrics search result type.
    /// </summary>
    [Serializable]
    public class FoundLyricType
    {

        private string _lyricCreditText = string.Empty;
        private string _lyricText = string.Empty;


        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        [XmlIgnore]
        public AbstractLyricService Service { get; set; }


        /// <summary>
        /// Gets or sets the found lyrics credit text.
        /// </summary>
        /// <value>
        /// The found lyrics credit text.
        /// </value>
        [XmlElement]
        public virtual string LyricCreditText
        {
            get
            {
                var ret = new StringBuilder(_lyricCreditText);

                ret.Replace("{LyricUrl}", LyricUrl?.ToString() ?? string.Empty);
                ret.Replace("{TrackingUrl}", TrackingUrl?.ToString() ?? string.Empty);
                ret.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);

                return ret.ToString();
            }
            set { _lyricCreditText = value?.Trim().LfToCrLf() ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the found lyrics text.
        /// </summary>
        /// <value>
        /// The found lyrics text.
        /// </value>
        [XmlElement]
        public virtual string LyricText
        {
            get { return _lyricText; }
            set { _lyricText = value?.Trim().LfToCrLf() ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the found lyrics URL.
        /// </summary>
        /// <value>
        /// The found lyrics text.
        /// </value>
        [XmlElement]
        public virtual SerializableUri LyricUrl { get; set; }

        /// <summary>
        /// Gets or sets the tracking URL.
        /// </summary>
        /// <value>
        /// The tracking URL.
        /// </value>
        [XmlElement]
        public virtual SerializableUri TrackingUrl { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FoundLyricType"/> class.
        /// </summary>
        public FoundLyricType()
        {
        }


        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var ret = new StringBuilder(LyricText);

            ret.AppendLine();
            ret.AppendLine();
            ret.Append(LyricCreditText);

            return ret.ToString();
        }

    }

}
