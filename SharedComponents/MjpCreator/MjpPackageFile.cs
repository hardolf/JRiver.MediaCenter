using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// MJP Package file type.
    /// </summary>
    [Serializable]
    [XmlRoot("MJPF", Namespace = "")]
    public class MjpPackageFile
    {

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [XmlAttribute("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the package.
        /// </summary>
        /// <value>
        /// The package.
        /// </value>
        [XmlElement(IsNullable = false)]
        public MjpPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the file entries.
        /// </summary>
        /// <value>
        /// The file entries.
        /// </value>
        [XmlArray("Actions", IsNullable = false)]
        [XmlArrayItem("FileEntry")]
        public List<MjpFileEntry> FileEntries { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpPackageFile"/> class.
        /// </summary>
        public MjpPackageFile()
        {
            FileEntries = new List<MjpFileEntry>();
            Package = new MjpPackage();
        }


        /// <summary>
        /// Serializes this object INI style.
        /// </summary>
        /// <returns>INI style string representation of this object.</returns>
        public string SerializeIniStyle()
        {
            var ret = new StringBuilder();

            ret.AppendLine("[Package]");
            ret.AppendLine(Package.SerializeIniStyle());

            ret.AppendLine("[Action]");

            for (int i = 0; i < FileEntries.Count; i++)
            {
                var fileEntry = FileEntries[i];

                ret.AppendLine(fileEntry.SerializeIniStyle(i));
            }

            return ret.ToString();
        }

    }

}
