using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

//using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.McWs
{

    /// <summary>
    /// JRiver MediaCenter playlist type.
    /// </summary>
    [Serializable, XmlType("Item")]
    public class McMplItem
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement()]
        public int Key { get; set; }

        [XmlElement()]
        public string Filename { get; set; }

        [XmlElement()]
        public string Artist { get; set; }

        [XmlElement()]
        public string Album { get; set; }

        [XmlElement()]
        public string Name { get; set; }

        [XmlElement("File Type")]
        public string FileType { get; set; }

        [XmlElement()]
        public string Genre { get; set; }

        [XmlElement()]
        public string Bitrate { get; set; }

        [XmlElement("Image File")]
        public string ImageFile { get; set; }

        [XmlElement("Media Type")]
        public string MediaType { get; set; }

        [XmlElement("Last Played")]
        public string LastPlayed { get; set; }

        [XmlElement("File Size")]
        public string FileSize { get; set; }

        [XmlElement()]
        public string Duration { get; set; }

        [XmlElement("Number Plays")]
        public string NumberPlays { get; set; }

        [XmlElement("Track #")]
        public string TrackNumber { get; set; }

        [XmlElement("Date Created")]
        public string DateCreated { get; set; }

        [XmlElement("Date Modified")]
        public string DateModified { get; set; }

        [XmlElement("Date Imported")]
        public string DateImported { get; set; }

        [XmlElement()]
        public string BPM { get; set; }

        [XmlElement()]
        public string Lyrics { get; set; }

        [XmlElement()]
        public string Date { get; set; }

        [XmlElement()]
        public string Bookmark { get; set; }

        [XmlElement("Sample Rate")]
        public string SampleRate { get; set; }

        [XmlElement()]
        public string Channels { get; set; }

        [XmlElement("Bit Depth")]
        public string BitDepth { get; set; }

        [XmlElement()]
        public string Compression { get; set; }

        [XmlElement("Skip Count")]
        public string SkipCount { get; set; }

        [XmlElement("Last Skipped")]
        public string LastSkipped { get; set; }

        [XmlElement("Get Cover Art Info")]
        public string GetCoverArtInfo { get; set; }

        [XmlElement("Peak Level (R128)")]
        public string PeakLevelR128 { get; set; }

        [XmlElement("Peak Level (Sample)")]
        public string PeakLevelSample { get; set; }

        [XmlElement("Volume Level (R128)")]
        public string VolumeLevelR128 { get; set; }

        [XmlElement("Volume Level (ReplayGain)")]
        public string VolumeLevelReplayGain { get; set; }

        [XmlElement("Dynamic Range (R128)")]
        public string DynamicRangeR128 { get; set; }

        [XmlElement("Dynamic Range (DR)")]
        public string DynamicRangeDr { get; set; }

        [XmlElement("DateFirstRated")]
        public string DateFirstRated { get; set; }

        [XmlElement("Total Tracks")]
        public string TotalTracks { get; set; }

        [XmlElement]
        public Bitmap Image { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [XmlIgnore]
        public virtual Dictionary<string, string> Fields { get; }

        /// <summary>
        /// Gets the date fields.
        /// </summary>
        /// <value>
        /// The date fields.
        /// </value>
        [XmlIgnore]
        public static IEnumerable<string> DateFields { get; } = new string[] { "Date" };

        /// <summary>
        /// Gets the date/time fields.
        /// </summary>
        /// <value>
        /// The date/time fields.
        /// </value>
        [XmlIgnore]
        public static IEnumerable<string> DateTimeFields { get; } = new string[] { "Date Created", "Date First Rated", "Date Imported",
                "Date Last Opened", "Date Modified", "Date Tagged", "Last Played", "Last Skipped" };

        /// <summary>
        /// Gets the duration fields.
        /// </summary>
        /// <value>
        /// The duration fields.
        /// </value>
        [XmlIgnore]
        public static IEnumerable<string> DurationFields { get; } = new string[] { "Duration" };

        [XmlIgnore]
        public static IEnumerable<string> NoFormatFields { get; } = new string[] { "Date (readable)", "Date (year)" };


        /// <summary>
        /// Initializes a new instance of the <see cref="McMplResponse" /> class.
        /// </summary>
        public McMplItem()
        {
            Fields = new Dictionary<string, string>();
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Deep copy of the current McMplItem object.</returns>
        public McMplItem Clone()
        {
            var ret = Clone(this);

            return ret;
        }


        /// <summary>
        /// Clones this instance and removes any parenthesized text from Artist, Album and Name properties.
        /// </summary>
        /// <returns>Deep copy of the current McMplItem object, with any parenthesized text removed from Artist, Album and Name properties.</returns>
        public McMplItem CloneAndRemoveParenthesizedText()
        {
            var ret = CloneAndRemoveParenthesizedText(this);

            return ret;
        }


        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>Deep copy of the source McMplItem object.</returns>
        /// <exception cref="ArgumentNullException">src</exception>
        public static McMplItem Clone(McMplItem src)
        {
            if (src is null) throw new ArgumentNullException(nameof(src));

            var ret = new McMplItem
            {
                Album = src.Album,
                Artist = src.Artist,
                BitDepth = src.BitDepth,
                Bitrate = src.Bitrate,
                Bookmark = src.Bookmark,
                BPM = src.BPM,
                Channels = src.Channels,
                Compression = src.Compression,
                Date = src.Date,
                DateCreated = src.DateCreated,
                DateFirstRated = src.DateFirstRated,
                DateImported = src.DateImported,
                DateModified = src.DateModified,
                Duration = src.Duration,
                DynamicRangeDr = src.DynamicRangeDr,
                DynamicRangeR128 = src.DynamicRangeR128,
                Filename = src.Filename,
                FileSize = src.FileSize,
                FileType = src.FileType,
                Genre = src.Genre,
                GetCoverArtInfo = src.GetCoverArtInfo,
                // Image = src.Image,
                ImageFile = src.ImageFile,
                Key = src.Key,
                LastPlayed = src.LastPlayed,
                LastSkipped = src.LastSkipped,
                Lyrics = src.Lyrics,
                MediaType = src.MediaType,
                Name = src.Name,
                NumberPlays = src.NumberPlays,
                PeakLevelR128 = src.PeakLevelR128,
                PeakLevelSample = src.PeakLevelSample,
                SampleRate = src.SampleRate,
                SkipCount = src.SkipCount,
                TotalTracks = src.TotalTracks,
                TrackNumber = src.TrackNumber,
                VolumeLevelR128 = src.VolumeLevelR128,
                VolumeLevelReplayGain = src.VolumeLevelReplayGain
            };

            foreach (var field in src.Fields)
            {
                ret.Fields.Add(field.Key, field.Value);
            }

            return ret;
        }


        /// <summary>
        /// Clones the source and removes any parenthesized text from Artist, Album and Name properties.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>Deep copy of the source McMplItem object, with any parenthesized text removed from Artist, Album and Name properties.</returns>
        /// <exception cref="ArgumentNullException">src</exception>
        public static McMplItem CloneAndRemoveParenthesizedText(McMplItem src)
        {
            if (src is null) throw new ArgumentNullException(nameof(src));

            var ret = Clone(src);

            ret.Artist = ret.Artist.RemoveParenthesizedText();
            ret.Album = ret.Album.RemoveParenthesizedText();
            ret.Name = ret.Name.RemoveParenthesizedText();

            return ret;
        }


        /// <summary>
        /// Creates the mc MPL item.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns>
        ///   <see cref="McMplItem" /> object.
        /// </returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<McMplItem> CreateMcMplItemAsync(XmlElement root)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (root == null) throw new ArgumentNullException(nameof(root));

            var ret = new McMplItem();
            var xmlRoot = root;
            var xFields = xmlRoot.GetElementsByTagName("Field");

            foreach (XmlElement xField in xFields)
            {
                var name = xField.GetAttribute("Name");

                ret.Fields.Add(name, xField.InnerXml);
            }

            return ret;
        }


        /// <summary>
        /// Fields to string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// String representation of the field value.
        /// </returns>
        public static string FieldToString(string key, string value)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));

            var ret = value;

            if (!double.TryParse(ret, out var dblVal)) dblVal = -1;
            if (!long.TryParse(ret, out var longVal)) longVal = -1;

            if (DateFields.Contains(key, StringComparer.InvariantCultureIgnoreCase))
            {
                if (longVal > 0)
                    ret = DateTime.FromOADate(longVal).Year.ToString(CultureInfo.InvariantCulture);
            }
            else if (DateTimeFields.Contains(key, StringComparer.InvariantCultureIgnoreCase))
            {
                if (longVal > 0)
                {
                    var dt = Constants.MediaCenterZeroUtcDate.AddSeconds(longVal).ToLocalTime();

                    ret = dt.ToString(Constants.DateTimeFormat, CultureInfo.CurrentCulture);
                }
            }
            else if (DurationFields.Contains(key, StringComparer.InvariantCultureIgnoreCase))
            {
                if (dblVal > 0)
                {
                    var ts = TimeSpan.FromSeconds(dblVal);
                    var format = (ts.Hours > 0) ? Constants.TimeSpanFormatLong : Constants.TimeSpanFormatShort;

                    ret = ts.ToString(format, CultureInfo.CurrentCulture);
                }
            }
            else if (!NoFormatFields.Contains(key, StringComparer.InvariantCultureIgnoreCase))
            {
                if (long.TryParse(ret, out var tmp))
                    ret = tmp.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture); 
            }

            return ret;
        }


        /// <summary>
        /// Field to string.
        /// </summary>
        /// <param name="key">The field key.</param>
        /// <returns>
        /// String representation of the field value.
        /// </returns>
        public string FieldToString(string key)
        {
            if (!Fields.TryGetValue(key, out var ret)) ret = string.Empty;

            return FieldToString(key, ret);
        }


        /// <summary>
        /// Fills the properties from the <see cref="Fields"/> dictionary.
        /// </summary>
        public virtual async Task FillPropertiesFromFieldsAsync()
        {
            foreach (var field in this.Fields)
            {
                var typ = this.GetType();
                var props = typ.GetProperties();

                foreach (var prop in props)
                {
                    var fieldKey = field.Key;
                    var propKey = prop.Name;
                    var atts = Attribute.GetCustomAttributes(prop, typeof(XmlElementAttribute));

                    if ((atts != null) && (atts.Length > 0))
                    {
                        if ((atts[0] is XmlElementAttribute att) && !att.ElementName.IsNullOrEmptyTrimmed())
                            propKey = att.ElementName;
                    }

                    if (propKey == fieldKey)
                    {
                        if (propKey == "Key")
                        {
                            if (int.TryParse(field.Value, out int key))
                                prop.SetValue(this, key);
                            else
                                throw new Exception($"Non-integer item key: \"{field.Value}\".");
                        }
                        else
                            prop.SetValue(this, field.Value);

                        if ((propKey == "Image File") && field.Value.Equals("internal", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var rsp = await McRestService.GetImageAsync(Key).ConfigureAwait(false);

                            Image = rsp.Image;
                        }
                    }
                }
            }
        }

    }

}
