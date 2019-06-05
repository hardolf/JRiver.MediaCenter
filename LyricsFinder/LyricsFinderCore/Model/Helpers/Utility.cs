using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Utilities for the LyricsFinder.
    /// </summary>
    internal static class Utility
    {

        private const string UnInitializedPrivateSettingText = "YOUR_OWN_STRING";


        public const string AppSettingsSectionName = "appSettings";

        public const string PrivateConfigFileExt = ".private.config";

        public const string PrivateConfigTemplateFileExt = ".template.config";


        /// <summary>
        /// Gets the private settings file path.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="dataDir">The data dir.</param>
        /// <returns>
        /// String with the private settings file path.
        /// </returns>
        public static string GetPrivateSettingsFilePath(Assembly assembly, string dataDir)
        {
            var ret = Path.Combine(dataDir, Path.GetFileName(assembly.Location) + Utility.PrivateConfigFileExt);
            var appConfigFilePath = assembly.Location + Utility.PrivateConfigFileExt;
            var templateConfigFilePath = assembly.Location + Utility.PrivateConfigTemplateFileExt;

            // Create the private config. file if not found in the data dir.
            if (!File.Exists(ret))
            {
                if (File.Exists(appConfigFilePath))
                    File.Copy(appConfigFilePath, ret, false); // Upgrade from LyricsFinder 1.0
                else if (File.Exists(templateConfigFilePath))
                    File.Copy(templateConfigFilePath, ret, false); // First start
                else
                    ret = string.Empty; // No private settings file found / needed
            }

            return ret;
        }


        /// <summary>
        /// Determines whether the setting valye is initialized.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is private setting initialized] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrivateSettingInitialized(this string value)
        {
            return (!value.IsNullOrEmptyTrimmed() && !value.Equals(UnInitializedPrivateSettingText, StringComparison.InvariantCultureIgnoreCase));
        }


        /// <summary>
        /// Lyrics result text.
        /// </summary>
        /// <returns>Transformed text.</returns>
        public static string ResultText(this LyricResultEnum lyricsResult)
        {
            var ret = new StringBuilder(lyricsResult.ToString());

            // Convert CamelCase to normal sentence, e.g. "Camel case"
            for (int i = 0; i < ret.Length; i++)
            {
                var tst = ret[i].ToString(CultureInfo.InvariantCulture).ToUpperInvariant()[0];

                if ((i > 0) && (ret[i] == tst))
                {
                    ret[i] = ret[i].ToString(CultureInfo.InvariantCulture).ToLowerInvariant()[0];
                    ret.Insert(i, ' ');
                }
            }

            return ret.ToString();
        }

    }

}
