﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;


namespace MediaCenter.LyricsFinder.Model.McRestService
{

    /// <summary>
    /// JRiver MediaCenter REST Web service base response type for the MPL commands.
    /// </summary>
    [Serializable, XmlType("MPL")]
    internal class McMplResponse
    {

        /// <summary>
        /// Gets or sets the path separator.
        /// </summary>
        /// <value>
        /// The path separator.
        /// </value>
        [XmlAttribute]
        public string PathSeparator { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [XmlAttribute]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [XmlAttribute]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [XmlIgnore]
        public virtual Dictionary<int, McMplItem> Items { get; set; }

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
        protected McMplResponse()
        {
            Items = new Dictionary<int, McMplItem>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McMplResponse"/> class.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public McMplResponse(string xml)
            : this()
        {
            XmlDocument xDoc = new XmlDocument() { XmlResolver = null };
            McMplItem item = null;
            XmlNodeList xItems = null;

            var sr = new StringReader(xml);
            var reader = XmlReader.Create(sr, new XmlReaderSettings() { XmlResolver = null });
            xDoc.Load(reader);

            XmlRoot = xDoc.DocumentElement;
            PathSeparator = XmlRoot.GetAttribute("PathSeparator");
            Title = XmlRoot.GetAttribute("Title");
            Version = XmlRoot.GetAttribute("Version");
            xItems = XmlRoot.GetElementsByTagName("Item");

            foreach (XmlElement xItem in xItems)
            {
                item = new McMplItem(xItem);

                var task = Task.Run(async () => { await item.FillPropertiesFromFields(); });

                task.Wait();

                if (!Items.Keys.Contains(item.Key))
                    Items.Add(item.Key, item);
            }
        }

    }

}
