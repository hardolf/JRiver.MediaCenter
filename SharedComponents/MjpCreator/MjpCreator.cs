using System;
using System.Collections.Generic;
using System.IO;
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
        /// Creates the MJP creator.
        /// </summary>
        /// <param name="name">The product name.</param>
        /// <param name="version">The product version.</param>
        /// <param name="url">The product package download URL.</param>
        /// <param name="sourceDirectories">The source file directories.</param>
        /// <param name="comRegisterFileNames">The COM register file names.</param>
        /// <param name="packageAction">The package action.</param>
        /// <param name="fileAction">The file action.</param>
        /// <returns>A <see cref="MjpCreator"/> object with thespecified properties.</returns>
        public static MjpCreator CreateMjpCreator(string name, string version, string url, IEnumerable<string> sourceDirectories, IEnumerable<string> comRegisterFileNames, string packageAction = "UNZIPDIR", string fileAction = "COPY_PLUGINDIR")
        {
            var ret = new MjpCreator();
            var package = ret.PackageFile.Package;
            var comFiles = new List<string>();
            var sourceFiles = new List<string>();

            package.Action = packageAction;
            package.Name = name;
            package.Url = url;
            package.Version = version;
            package.HasActions = 0;

            foreach (var sourceDir in sourceDirectories)
            {
                foreach (var filePath in Directory.GetFiles(sourceDir))
                {
                    if (!sourceFiles.Contains(filePath))
                        sourceFiles.Add(filePath);
                }
            }

            foreach (var fileName in comRegisterFileNames)
            {
                if (!comFiles.Contains(fileName))
                    comFiles.Add(fileName);
            }

            foreach (var sourceFile in sourceFiles)
            {
                var fileName = Path.GetFileName(sourceFile);
                var fileEntry = new MjpFileEntry(fileName);

                fileEntry.Actions.Add(new MjpAction(fileAction));

                if (comFiles.Contains(fileName))
                    fileEntry.Actions.Add(new MjpAction("REGISTER"));

                ret.PackageFile.FileEntries.Add(fileEntry);
                package.HasActions = 1;
            }

            return ret;
        }


        /// <summary>
        /// Serializes the <see cref="PackageFile" />.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Serialized object string.
        /// </returns>
        /// <exception cref="System.ArgumentException">Destination file path must contain \".v1.\" (INI style) or \".v2.\" (XML style). - destinationFilePath</exception>
        public string Serialize(short version)
        {
            var ret = string.Empty;

            switch (version)
            {
                case 1:
                    ret = PackageFile.SerializeIniStyle();
                    break;

                case 2:
                    PackageFile.Version = "2.0";

                    var ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);

                    ret = PackageFile.XmlSerializeToString(ns, new[] { typeof(MjpPackageFile), typeof(MjpPackage), typeof(MjpFileEntry), typeof(MjpAction) });
                    ret = ret.Replace("encoding=\"utf-16\"", "standalone=\"yes\" ");
                    break;

                default:
                    throw new ArgumentException($"Version must be either 1 (INI style) or 2 (XML style).", nameof(version));
            }

            return ret;
        }

    }

}
