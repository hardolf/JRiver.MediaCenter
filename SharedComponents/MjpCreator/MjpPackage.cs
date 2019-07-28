using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// MJP Package element type.
    /// </summary>
    [Serializable]
    public class MjpPackage
    {

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        [XmlElement(IsNullable = false, Order = 1)]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has actions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has actions; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(IsNullable = false, Order = 4)]
        public int HasActions { get; set; } = 0;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement(IsNullable = false, Order = 0)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [XmlElement("URL", IsNullable = false, Order = 2)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [XmlElement(IsNullable = false, Order = 3)]
        public string Version { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpPackage"/> class.
        /// </summary>
        public MjpPackage()
        {
        }


        /// <summary>
        /// Serializes this object INI style.
        /// </summary>
        /// <returns>INI style string representation of this object.</returns>
        public string SerializeIniStyle()
        {
            var ret = new StringBuilder();

            ret.AppendLine($"Name={Name}");
            ret.AppendLine($"Action={Action}");
            ret.AppendLine($"Version={Version}");
            ret.AppendLine($"URL={Url}");

            return ret.ToString();
        }

    }

}
