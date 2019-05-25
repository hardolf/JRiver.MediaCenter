﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.McRestService
{

    /// <summary>
    /// JRiver MediaCenter playlist type.
    /// </summary>
    [Serializable, XmlType("Item")]
    public class McMplItem
    {

        [XmlElement()]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
        /// Gets or sets the XML root element.
        /// </summary>
        /// <value>
        /// The XML root element.
        /// </value>
        [XmlIgnore]
        private XmlElement XmlRoot { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="McMplResponse"/> class.
        /// </summary>
        public McMplItem()
        {
            Fields = new Dictionary<string, string>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McMplResponse" /> class.
        /// </summary>
        /// <param name="root">The root.</param>
        internal McMplItem(XmlElement root)
            : this()
        {
            XmlRoot = root;

            var xFields = XmlRoot.GetElementsByTagName("Field");

            foreach (XmlElement xField in xFields)
            {
                var name = xField.GetAttribute("Name");

                Fields.Add(name, xField.InnerXml);
            }
        }


        /// <summary>
        /// Fills the properties from the <see cref="Fields"/> dictionary.
        /// </summary>
        public virtual void FillPropertiesFromFields()
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
                            Image = McRestService.GetImage(Key).Image;
                        }
                    }
                }
            }
        }

    }

}