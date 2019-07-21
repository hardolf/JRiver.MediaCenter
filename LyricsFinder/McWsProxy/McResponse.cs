using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace MediaCenter.McWs
{

    /// <summary>
    /// JRiver MediaCenter Web Service base response type.
    /// </summary>
    [Serializable, XmlType("Response")]
    public class McResponse
    {

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ok; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public virtual bool IsOk { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [XmlIgnore]
        public virtual Dictionary<string, string> Items { get; set; }

        /// <summary>
        /// Gets or sets the XML root element.
        /// </summary>
        /// <value>
        /// The XML root element.
        /// </value>
        [XmlIgnore]
        private XmlElement XmlRoot { get; set; }

        [XmlAttribute("Status")]
        private string Status
        {
            get
            {
                return (IsOk) ? "OK" : "";
            }
            set
            {
                IsOk = value.Equals("OK", StringComparison.InvariantCultureIgnoreCase) ? true : false;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McResponse"/> class.
        /// </summary>
        protected McResponse()
        {
            Items = new Dictionary<string, string>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McResponse"/> class.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public McResponse(string xml)
            : this()
        {
            var xDoc = new XmlDocument() { XmlResolver = null };

            using (var sReader = new StringReader(xml))
            using (var xReader = XmlReader.Create(sReader, new XmlReaderSettings() { XmlResolver = null }))
                xDoc.Load(xReader); 

            XmlRoot = xDoc.DocumentElement;
            Status = XmlRoot.GetAttribute("Status");

            var xItems = XmlRoot.GetElementsByTagName("Item");

            foreach (XmlElement xItem in xItems)
            {
                var name = xItem.GetAttribute("Name");

                Items.Add(name, xItem.InnerXml);
            }
        }


        /// <summary>
        /// Fills the properties from the <see cref="Items"/> dictionary.
        /// </summary>
        public virtual void FillPropertiesFromItems()
        {
            foreach (var item in this.Items)
            {
                var typ = this.GetType();
                var props = typ.GetProperties();

                foreach (var prop in props)
                {
                    if (prop.Name == item.Key)
                        prop.SetValue(this, item.Value);
                }
            }
        }

    }

}
