using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// MJP Creator type.
    /// </summary>
    public class MjpCreator
    {

        /// <summary>
        /// Gets or sets the package file.
        /// </summary>
        /// <value>
        /// The package file.
        /// </value>
        public MjpPackageFile PackageFile { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpCreator" /> class.
        /// </summary>
        public MjpCreator()
        {
            PackageFile = new MjpPackageFile();
        }


        /// <summary>
        /// Serializes the <see cref="PackageFile" />.
        /// </summary>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <returns>Serialized object string.</returns>
        public string Serialize(string destinationFilePath)
        {
            var ret = string.Empty;

            if (destinationFilePath.Contains(".v1."))
            {
                ret = PackageFile.SerializeIniStyle();
            }
            else if (destinationFilePath.Contains(".v2."))
            {
                PackageFile.Version = "2.0";

                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                ret = PackageFile.XmlSerializeToString(ns, new[] { typeof(MjpPackageFile), typeof(MjpPackage), typeof(MjpFileEntry), typeof(MjpAction) });
                ret = ret.Replace("encoding=\"utf-16\"", "standalone=\"yes\" ");
            }
            else
                throw new ArgumentException($"Destination file path must contain \".v1.\" (INI style) or \".v2.\" (XML style).", nameof(destinationFilePath));

            return ret;
        }

    }

}
