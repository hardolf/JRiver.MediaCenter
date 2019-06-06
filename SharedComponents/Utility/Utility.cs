using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// Utilities for the LyricsFinder.
    /// </summary>
    public static class Utility
    {

        /// <summary>
        /// Gets the total text from all the text boxes in the parent control.
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <returns>
        /// Appended string with all the text boxes' trimmed texts, with CR+LF as separators.
        /// </returns>
        /// <remarks>This routine can be used as a simple "serialization" function.</remarks>
        public static string GetAllTextBoxesText(this Control parentControl)
        {
            var ret = new StringBuilder();

            foreach (var ctl in parentControl.Controls)
            {
                if (ctl is TextBox txt) ret.AppendLine($"{txt.Name}: {txt.Text.Trim()}");
            }

            return ret.ToString();
        }


        /// <summary>
        /// Gets the linker time.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="target">The target.</param>
        /// <returns>The assembly build datetime.</returns>
        /// <remarks>
        /// <para>Sources:</para>
        /// <para>https://stackoverflow.com/questions/1600962/displaying-the-build-date</para>
        /// <para>https://blog.codinghorror.com/determining-build-date-the-hard-way/</para>
        /// </remarks>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }


        /// <summary>
        /// Joins the strings.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="strings">The strings.</param>
        /// <returns>Joined string.</returns>
        /// <remarks>
        /// <para>This function is an improved version of the <see cref="string.Join"/> method.</para>
        /// <para>It removes any duplicate trailing separators.</para>
        /// </remarks>
        public static string JoinTrimmedStrings(string separator, params string[] strings)
        {
            if (separator.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(separator));
            if (strings.IsNullOrEmpty()) throw new ArgumentNullException(nameof(separator));

            var ret = new StringBuilder();
            var trimmedSeparator = separator.Trim();

            foreach (var str in strings)
            {
                var s = str.Trim();

                if (ret.Length > 0)
                    ret.Append(separator);

                // Remove any trailing separators
                if (s.EndsWith(trimmedSeparator, StringComparison.InvariantCultureIgnoreCase))
                    s = s.Substring(0, s.LastIndexOf(trimmedSeparator, StringComparison.InvariantCultureIgnoreCase));

                ret.Append(s);
            }

            return ret.ToString();
        }


        /// <summary>
        /// Determines whether the input string contains one or more of the strings in the specified compare list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="compareList">The compare list.</param>
        /// <returns>
        ///   <c>true</c> if the input string contains one or more of the strings in the specified compare list; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string input, IEnumerable<string> compareList)
        {
            input = input.ToUpperInvariant();

            var ret = compareList.Any(s => input.Contains(s.ToUpperInvariant()));

            return ret;
        }


        /// <summary>
        /// DLLs the configuration file.
        /// </summary>
        /// <returns></returns>
        public static string DllConfigurationFile()
        {
            var assy = Assembly.GetExecutingAssembly();
            var ret = $"{assy.Location}.config";

            return ret;
        }


        /// <summary>
        /// Determines whether the array is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The array.</param>
        /// <returns>
        /// 	<c>true</c> if the array is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this T[] arr)
        {
            return arr == null || arr.Length == 0;
        }


        /// <summary>
        /// Determines whether the list is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr">The list.</param>
        /// <returns>
        /// 	<c>true</c> if the list is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IList<T> arr)
        {
            return arr == null || arr.Count == 0;
        }


        /// <summary>
        /// Determines whether the text is null or empty when trimmed.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>true</c> if the text is null or empty when trimmed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmptyTrimmed(
            this string text)
        {
            return string.IsNullOrEmpty(text?.Trim());
        }


        /// <summary>
        /// Converts Unix-style linefeeds to Windows-style line endings (LF to CR + LF)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string LfToCrLf(this string input)
        {
            var ret = new StringBuilder(input);

            for (int i = 0; i < ret.Length; i++)
            {
                if ((ret[i] == '\n') && ((i == 0) || (ret[i - 1] != '\r')))
                    ret.Insert(i, '\r');
            }

            return ret.ToString();
        }

    }

}
