using System;
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
        /// Gets or sets the playlist ID, optional.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the playlist name, optional.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlIgnore]
        public string Name { get; set; }

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
        /// Initializes a new instance of the <see cref="McMplResponse"/> class.
        /// </summary>
        protected McMplResponse()
        {
            Items = new Dictionary<int, McMplItem>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McMplResponse" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        protected McMplResponse(int id = -1, string name = null)
            : this()
        {
            Id = id;
            Name = name;
        }


        /// <summary>
        /// Creates the mc MPL response.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns><see cref="McMplResponse"/> object.</returns>
        public static async Task<McMplResponse> CreateMcMplResponse(string xml, int id = -1, string name = null)
        {
            var ret = new McMplResponse(id, name);

            XmlDocument xDoc = new XmlDocument() { XmlResolver = null };

            using (var sReader = new StringReader(xml))
            using (var xReader = XmlReader.Create(sReader, new XmlReaderSettings() { XmlResolver = null }))
                xDoc.Load(xReader);

            var xmlRoot = xDoc.DocumentElement;

            ret.PathSeparator = xmlRoot.GetAttribute("PathSeparator");
            ret.Title = xmlRoot.GetAttribute("Title");
            ret.Version = xmlRoot.GetAttribute("Version");

            var xItems = xmlRoot.GetElementsByTagName("Item");

            foreach (XmlElement xItem in xItems)
            {
                var item = await McMplItem.CreateMcMplItem(xItem).ConfigureAwait(false);

                await item.FillPropertiesFromFields().ConfigureAwait(false);

                if (!ret.Items.Keys.Contains(item.Key))
                    ret.Items.Add(item.Key, item);
            }

            return ret;
        }

    }

}
