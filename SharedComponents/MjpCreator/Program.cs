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

            if (Debugger.IsAttached || !isOk)
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