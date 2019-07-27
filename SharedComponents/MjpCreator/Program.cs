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

namespace MediaCenter.SharedComponents
{

    public class Program
    {

        private static readonly Argument _argument = new Argument();


        public static int Main(string[] args)
        {
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
                Logging.LogInfo($"{assyName} started {beginTime.ToString(Settings.Default.TimeFormat)}.");
                Logging.LogInfo(Argument.GetCommandString(args));
                Logging.DivideLog();

                _argument.Check(args);

                if (_argument.IsHelpRequired)
                    Console.WriteLine(_argument.GetSyntax());
                else
                {
                    var mjpCreator = new MjpCreator();
                    var packageFile = mjpCreator.PackageFile;
                    var package = packageFile.Package;
                    var url = "https://github.com/hardolf/JRiver.MediaCenter/releases/download/v1.2.2/Setup1.2.2.zip";
                    var tmp = "/download/v";
                    var idx1 = url.IndexOf(tmp) + tmp.Length;
                    var idx2 = url.IndexOf("/", idx1);
                    var versionText = url.Substring(idx1, idx2 - idx1);

                    package.Action = "UNZIPDIR";
                    package.HasActions = 1;
                    package.Name = "LyricsFinder plug-in installation package";
                    package.Url = url;
                    package.Version = versionText;

                    var comFiles = _argument.ComRegisterFiles;
                    var sourceDirs = _argument.SourceFileDirectories;
                    var sourceFiles = new List<string>();

                    foreach (var sourceDir in sourceDirs)
                    {
                        sourceFiles.AddRange(Directory.GetFiles(sourceDir));
                    }

                    foreach (var sourceFile in sourceFiles)
                    {
                        var fileName = Path.GetFileName(sourceFile);
                        var fileEntry = new MjpFileEntry(fileName);

                        fileEntry.Actions.Add(new MjpAction("COPY_PLUGINDIR"));

                        if (comFiles.Contains(fileName))
                            fileEntry.Actions.Add(new MjpAction("REGISTER"));

                        mjpCreator.PackageFile.FileEntries.Add(fileEntry); 
                    }

                    var destPath = Path.GetFullPath(_argument.DestinationFilePath);
                    var destDir = Path.GetDirectoryName(destPath);
                    var destFile = Path.GetFileNameWithoutExtension(destPath);
                    var destExt = Path.GetExtension(destPath);
                    var destPathLeft = Path.Combine(destDir, destFile + versionText);
                    var destPathIni = destPathLeft + ".v1" + destExt;
                    var destPathXml = destPathLeft + ".v2" + destExt;
                    var iniText = mjpCreator.Serialize(destPathIni);
                    var xmlText = mjpCreator.Serialize(destPathXml);

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

                    timeMsg += $"at {endTime.ToString(Settings.Default.TimeFormat)} ";
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

            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.Write("Press a key to close: ");
                Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Closing...");
            }

            return (isOk)
                ? 0
                : 1;
        }

    }

}