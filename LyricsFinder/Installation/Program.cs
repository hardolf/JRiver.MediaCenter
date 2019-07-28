using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Installation
{

    class Program
    {

        /// <summary>
        /// Defines the entry point of the application.
        /// Makes the release package.
        /// </summary>
        static void Main()
        {
            // Get the version
            var assy = Assembly.GetExecutingAssembly();
            var version = assy.GetName().Version;
            var versionText = string.Join(".", version.Major, version.Minor, version.Build);

            // Make the MJP files
            var currentDir = Path.GetDirectoryName(Path.GetFullPath(assy.Location));
            var subDir = currentDir.Substring(Path.GetDirectoryName(Path.GetDirectoryName(currentDir)).Length + 1);
            var instDir = Path.GetFullPath(Path.Combine(currentDir, $@"..\..\..\Installation"));
            var tmpDir = Path.Combine(currentDir, $@"..\..\..\..\SharedComponents", "MjpCreator", subDir);
            var exeDir = Path.GetFullPath(tmpDir);
            var exeFile = "MjpCreator.exe";
            var exePath = Path.GetFullPath(Path.Combine(exeDir, exeFile));
            var args = new StringBuilder();

            args.Append("-n \""+ "LyricsFinder" + "\" ");
            args.Append("-v \""+ $"{versionText}" + "\" ");
            args.Append("-url \"" + $"https://github.com/hardolf/JRiver.MediaCenter/releases/download/v{versionText}/Setup{versionText}.zip" + "\" ");
            args.Append("-dd \"" + $@"{instDir}\Output" + "\" ");
            args.Append("-sd \"" + $@"{instDir}\Build\Plugin;{instDir}\Build\Standalone;{instDir}\Build\LyricServices" + "\" ");
            args.Append("-com \"" + "LyricsFinderPlugin.dll;LyricsFinderCore.dll" + "\" ");

            if (File.Exists(exePath))
            {
                var psi = new ProcessStartInfo
                {
                    Arguments = args.ToString(),
                    FileName = exePath,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(exeDir),
                };

                Process.Start(psi);
            }

            // Pack the release files
            PackageSetupFile(new[] { Path.GetFullPath(@"..\..\..\Documentation\Build"), Path.GetFullPath(@"..\..\Output") }
                , Path.GetFullPath(@"..\..\Release"), "Setup", versionText);
        }


        /// <summary>
        /// Packages the setup.
        /// </summary>
        /// <param name="sourceDirectories">The source directories.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="destinationFileRootName">Name of the destination file root.</param>
        /// <param name="version">The version.</param>
        private static void PackageSetupFile(IEnumerable<string> sourceDirectories, string destinationDirectory, string destinationFileRootName, string versionText)
        {
            var zipPath = Path.Combine(destinationDirectory, $"{destinationFileRootName}.{versionText}.zip");

            // Delete the packed destination file if already there
            if (File.Exists(zipPath))
                File.Delete(zipPath);

            // Pack the release files
            foreach (var sourceDir in sourceDirectories)
            {
                if (File.Exists(zipPath))
                {
                    using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                    {
                        foreach (var setupFilePath in Directory.GetFiles(sourceDir))
                        {
                            var setupFileName = Path.GetFileName(setupFilePath);

                            zip.CreateEntryFromFile(setupFilePath, setupFileName, CompressionLevel.Fastest);
                        }
                    }
                }
                else
                    ZipFile.CreateFromDirectory(sourceDir, zipPath, CompressionLevel.Fastest, false);
            }
        }

    }

}
