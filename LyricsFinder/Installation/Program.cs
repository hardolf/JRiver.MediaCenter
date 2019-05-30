using System;
using System.Collections.Generic;
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
            // Get the version text
            var assy = Assembly.GetExecutingAssembly();
            var version = assy.GetName().Version;
            var versionText = string.Join(".", version.Major, version.Minor, version.Build);

            // Delete the packed file if already there
            var zipPath = $@"..\..\Release\Setup{versionText}.zip";

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            // Pack the release files
            ZipFile.CreateFromDirectory(@"..\..\..\Documentation\Build\", zipPath, CompressionLevel.Fastest, false);

            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                // Pack the setup files
                foreach (var setupFilePath in Directory.GetFiles(@"..\..\Output\", "Setup*.exe", SearchOption.TopDirectoryOnly))
                {
                    var setupFileName = Path.GetFileName(setupFilePath);

                    zip.CreateEntryFromFile(setupFilePath, setupFileName, CompressionLevel.Fastest); 
                }
            }
        }

    }

}
