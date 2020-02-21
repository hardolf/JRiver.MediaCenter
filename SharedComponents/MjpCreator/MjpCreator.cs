using MediaCenter.SharedComponents.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private static readonly Argument _argument = new Argument();


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
        public static MjpCreator CreateMjpCreator(string name, string version, string url, IEnumerable<string> sourceDirectories, IEnumerable<string> comRegisterFileNames, string packageAction = "UNZIPDIR|SAVE_DIR_STRUCT", string fileAction = "COPY_PLUGINDIR")
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (version is null) throw new ArgumentNullException(nameof(version));
            if (url is null) throw new ArgumentNullException(nameof(url));
            if (packageAction is null) throw new ArgumentNullException(nameof(packageAction));
            if (fileAction is null) throw new ArgumentNullException(nameof(fileAction));

            var ret = new MjpCreator();
            var package = ret.PackageFile.Package;
            var comFiles = new List<string>();
            var sourceFiles = new List<string>();
            var fileEntry = new MjpFileEntry("*.*");

            package.Action = packageAction;
            package.Name = name;
            package.Url = url;
            package.Version = version;
            package.HasActions = 0;

            // COM files
            if ((comRegisterFileNames != null) && (comRegisterFileNames.Any()))
            {
                foreach (var fileName in comRegisterFileNames)
                {
                    if (!comFiles.Contains(fileName))
                        comFiles.Add(fileName);
                }
            }

            // File entries
            if ((sourceDirectories == null) || (!sourceDirectories.Any()))
            {
                // All files (*.*) file entry
                fileEntry.Actions.Add(new MjpAction(fileAction));
                ret.PackageFile.FileEntries.Add(fileEntry);
                package.HasActions = 1;

                // COM file entries
                foreach (var fileName in comFiles)
                {
                    fileEntry = new MjpFileEntry(fileName);
                    fileEntry.Actions.Add(new MjpAction("REGISTER"));
                    ret.PackageFile.FileEntries.Add(fileEntry);
                }
            }
            else
            {
                // Separate file entries
                foreach (var sourceDir in sourceDirectories)
                {
                    foreach (var filePath in Directory.GetFiles(sourceDir))
                    {
                        var fileName = Path.GetFileName(filePath);

                        if (!sourceFiles.Contains(fileName))
                        {
                            sourceFiles.Add(fileName);
                            fileEntry = new MjpFileEntry(fileName);
                            fileEntry.Actions.Add(new MjpAction(fileAction));

                            if (comFiles.Contains(fileName))
                                fileEntry.Actions.Add(new MjpAction("REGISTER"));

                            ret.PackageFile.FileEntries.Add(fileEntry);
                            package.HasActions = 1;
                        }
                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// Initializes the <see cref="MjpCreator" /> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Execution errorlevel, 0: successfull, 1: failed.</returns>
        public static int MjpCreatorExecute(string[] args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));

            var fatalErrorText = "Fatal error:";
            var isOk = false;
            var beginTime = DateTime.Now;
            var endTime = beginTime;
            var assy = Assembly.GetExecutingAssembly();
            var assyName = assy.GetName().Name;

            try
            {
                Logging.Init(Settings.Default.LogHeader1, Settings.Default.LogHeader2);
                Logging.BeginLog();
                Logging.LogInfo(Utility.GetProgramInfo());
                Logging.DivideLog();
                Logging.LogInfo($"{assyName} started {beginTime.ToString(Settings.Default.TimeFormat, CultureInfo.InvariantCulture)}.");
                Logging.LogInfo(Argument.GetCommandString(args));
                Logging.DivideLog();

                _argument.Check(args);

                if (_argument.IsHelpRequired)
                    Console.WriteLine(_argument.GetSyntax());
                else
                {
                    var destDir = Path.GetFullPath(_argument.DestinationFileDirectory);
                    var destFile = _argument.Name + "." + _argument.Version;
                    var destExt = ".mjp";
                    var destPathLeft = Path.Combine(destDir, destFile);
                    var destPathIni = $"{destPathLeft}.plugin1{destExt}";
                    var destPathXml = $"{destPathLeft}.plugin2{destExt}";

                    var mjpCreator = MjpCreator.CreateMjpCreator(_argument.Name, _argument.Version, _argument.Url, _argument.SourceFileDirectories, _argument.ComRegisterFiles);

                    var iniText = mjpCreator.Serialize(1);
                    var xmlText = mjpCreator.Serialize(2);

                    Logging.LogInfo(iniText);
                    Logging.DivideLog();
                    Logging.LogInfo(xmlText);

                    File.WriteAllText(destPathIni, iniText);
                    File.WriteAllText(destPathXml, xmlText);

                    endTime = DateTime.Now;
                    isOk = true;

                    var mem = GC.GetTotalMemory(false);
                    var culture = CultureInfo.InvariantCulture;
                    var timeSpan = endTime - beginTime;
                    var timeMsg = string.Empty;

                    timeMsg += $"at {endTime.ToString(Settings.Default.TimeFormat, culture)} ";
                    timeMsg += $"after {timeSpan.ToString(Settings.Default.TimeSpanFormat, culture)} ";
                    timeMsg += $"({timeSpan.TotalSeconds.ToString(Settings.Default.TimeSpanSecondsFormat, culture)} seconds)";

                    var msg = (isOk)
                        ? $"{assyName} completed successfully {timeMsg}."
                        : $"{assyName} finished with error {timeMsg}." + fatalErrorText;

                    Logging.DivideLog();
                    Logging.LogInfo($"Memory used: {mem:N0} bytes.");
                    Logging.LogInfo(msg);
                }

                Logging.EndLog();
            }
            catch (Exception ex)
            {
                isOk = false;
                fatalErrorText += Environment.NewLine + ex.ToString();
                Logging.DivideLog();
                Logging.LogError(MethodBase.GetCurrentMethod(), ex, fatalErrorText);
            }

            return (isOk) ? 0 : 1;
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
