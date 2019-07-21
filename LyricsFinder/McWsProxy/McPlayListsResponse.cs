using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MediaCenter.McWs
{

    /// <summary>
    /// Playlist list.
    /// </summary>
    [Serializable, XmlType("Response")]
    public class McPlayListsResponse
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
        public virtual Dictionary<string, McPlayListType> Items { get; set; }

        [XmlAttribute("Status")]
        public string Status
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
        /// Initializes a new instance of the <see cref="McPlayListsResponse"/> class.
        /// </summary>
        protected McPlayListsResponse()
        {
            Items = new Dictionary<string, McPlayListType>();
        }


        /// <summary>
        /// Creates the mc play lists response.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns><see cref="McPlayListsResponse"/> object.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<McPlayListsResponse> CreateMcPlayListsResponseAsync(string xml)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var ret = new McPlayListsResponse();
            var xDoc = new XmlDocument() { XmlResolver = null };

            using (var sReader = new StringReader(xml))
            using (var xReader = XmlReader.Create(sReader, new XmlReaderSettings() { XmlResolver = null }))
                xDoc.Load(xReader);

            var xmlRoot = xDoc.DocumentElement;

            ret.Status = xmlRoot.GetAttribute("Status");

            var xItems = xmlRoot.GetElementsByTagName("Item");

            foreach (XmlElement xItem in xItems)
            {
                var id = string.Empty;
                var name = string.Empty;
                var path = string.Empty;
                var type = string.Empty;
                var fields = xItem.GetElementsByTagName("Field");

                foreach (XmlElement field in fields)
                {
                    var fName = field.GetAttribute("Name");
                    var fValue = field.InnerText;

                    switch (fName.ToUpperInvariant())
                    {
                        case "ID":
                            id = fValue;
                            break;

                        case "NAME":
                            name = fValue;
                            break;

                        case "PATH":
                            path = fValue;
                            break;

                        case "TYPE":
                            type = fValue;
                            break;

                        default:
                            break;
                    }
                }

                var item = new McPlayListType
                {
                    Id = id,
                    Name = name,
                    Path = path,
                    Type = type
                };

                if (!type.Equals("GROUP", StringComparison.InvariantCultureIgnoreCase))
                    ret.Items.Add(id, item);
            }

            return ret;
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var rsp = obj as McPlayListsResponse;
            var ret = (rsp != null) &&
                (IsOk == rsp.IsOk) &&
                (Items.Count == rsp.Items.Count) &&
                (Status == rsp.Status);

            if (ret)
            {
                for (int i = 0; i < rsp.Items.Count; i++)
                {
                    if (Items.ElementAt(i).Key.Equals(rsp.Items.ElementAt(i).Key, StringComparison.InvariantCultureIgnoreCase)
                        && Items.ElementAt(i).Value.Equals(rsp.Items.ElementAt(i).Value))
                        continue;
                }
            }

            return ret;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 1337987077;

            hashCode = hashCode * -1521134295 + IsOk.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, McPlayListType>>.Default.GetHashCode(Items);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Status);

            return hashCode;
        }

    }



    /// <summary>
    /// Playlist information type.
    /// </summary>
    [Serializable]
    public class McPlayListType
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement]
        public string Id { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Path { get; set; }

        [XmlElement]
        public string Type { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        /// <summary>
        /// Gets the unsort key.
        /// </summary>
        /// <value>
        /// The unsort key.
        /// </value>
        public string Key
        {
            get
            {
                return Id;
            }
        }


        /// <summary>
        /// Gets the sort key.
        /// </summary>
        /// <value>
        /// The sort key.
        /// </value>
        public string SortKey
        {
            get
            {
                return string.Join("|", Path, Id);
            }
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is McPlayListType type &&
                   Id == type.Id &&
                   Name == type.Name &&
                   Path == type.Path &&
                   Type == type.Type;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 265471897;

            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);

            return hashCode;
        }

    }

}
