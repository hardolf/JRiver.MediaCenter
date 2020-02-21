using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder
{

    class Program
    {

        /// <summary>
        /// Defines the entry point of the application.
        /// Makes the release package.
        /// </summary>
        static void Main()
        {
            const string packageRootFolder = "LyricsFinder"; // For installation in plugins sub-folder, use "LyricsFinder"

            // Get the version
            var assy = Assembly.GetExecutingAssembly();
            var version = assy.GetName().Version;
            var versionText = string.Join(".", version.Major, version.Minor, version.Build);

            // Make the MJP files
            MakeMcpFiles(assy, versionText);

            // Pack the release files
            PackageSetupFile(new[] { Path.GetFullPath(@"..\..\..\Documentation\Build")
                , Path.GetFullPath(@"..\..\Output") }, Path.GetFullPath(@"..\..\Release"), "Setup", versionText);

            PackageSetupFile(new[] { Path.GetFullPath(@"..\..\..\Installation\Build\LyricServices"), Path.GetFullPath(@"..\..\..\Installation\Build\Plugin"), Path.GetFullPath(@"..\..\..\Installation\Build\Standalone")
                , Path.GetFullPath(@"..\..\Output") }, Path.GetFullPath(@"..\..\Release"), "LyricsFinderFiles", versionText, packageRootFolder);
        }


        /// <summary>
        /// Makes the MCP files.
        /// </summary>
        /// <param name="assembly">The assy.</param>
        /// <param name="versionText">The version text.</param>
        /// <exception cref="System.Exception">MjpCreator failed creating MJP files.</exception>
        private static void MakeMcpFiles(Assembly assembly, string versionText, string packageRootFolder = "")
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));

            var currentDir = Path.GetDirectoryName(Path.GetFullPath(assembly.Location));
            var instDir = Path.GetFullPath(Path.Combine(currentDir, $@"..\..\..\Installation"));

            packageRootFolder = packageRootFolder?.Trim() ?? string.Empty;

            if ((!string.IsNullOrEmpty(packageRootFolder)) && (!packageRootFolder.EndsWith(@"\", StringComparison.InvariantCultureIgnoreCase)))
                packageRootFolder += @"\";

            var args = new List<string>
            {
                "-n", "LyricsFinder",
                "-v", $"{versionText}",
                "-url", $"https://github.com/hardolf/JRiver.MediaCenter/releases/download/v{versionText}/LyricsFinderFiles.{versionText}.zip",
                "-dd", $@"{instDir}\Release",
                //"-sd", $@"{instDir}\Build\Plugin;{instDir}\Build\Standalone;{instDir}\Build\LyricServices",
                "-com", $@"{packageRootFolder}LyricsFinderPlugin.dll;{packageRootFolder}LyricsFinderCore.dll"
            };

            if (0 != MjpCreator.MjpCreatorExecute(args.ToArray()))
                throw new Exception("MjpCreator failed creating MJP files.");
        }


        /// <summary>
        /// Packages the setup.
        /// </summary>
        /// <param name="sourceDirectories">The source directories.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="destinationFileRootName">Name of the destination file root.</param>
        /// <param name="versionText">The version text.</param>
        /// <param name="packageRootFolder">The package root folder.</param>
        private static void PackageSetupFile(IEnumerable<string> sourceDirectories, string destinationDirectory, string destinationFileRootName, string versionText, string packageRootFolder = "")
        {
            var zipPath = Path.Combine(destinationDirectory, $"{destinationFileRootName}.{versionText}.zip");

            // Delete the packed destination file if already there
            if (File.Exists(zipPath))
                File.Delete(zipPath);

            // Pack the release files
            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var sourceDir in sourceDirectories)
                {
                    foreach (var setupFilePath in Directory.GetFiles(sourceDir))
                    {
                        var setupFileName = Path.GetFileName(setupFilePath);

                        if (!string.IsNullOrEmpty(packageRootFolder))
                            setupFileName = Path.Combine(packageRootFolder, setupFileName);

                        zip.CreateEntryFromFile(setupFilePath, setupFileName, CompressionLevel.Fastest);
                    }
                }
            }
        }

    }

}
