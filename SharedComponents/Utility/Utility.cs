using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        // We don't dispose of these objects
        private static HttpClientHandler _httpClientHandler = new HttpClientHandler();
        private static HttpClient _httpClientWithCredentials = new HttpClient(_httpClientHandler, true);
        private static readonly HttpClient _httpClientAnonymous = new HttpClient();


        /// <summary>
        /// Make the TextBox fit its contents.
        /// </summary>
        /// <param name="control">The text box.</param>
        /// <remarks>
        /// Source: http://csharphelper.com/blog/2018/02/resize-a-textbox-to-fit-its-text-in-c/ 
        /// </remarks>
        public static void AutoSizeTextBox(this Control control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            const int x_margin = 0;
            const int y_margin = 2;

            Size size = TextRenderer.MeasureText(control.Text, control.Font);

            control.ClientSize = new Size(size.Width + x_margin, size.Height + y_margin);
        }


        /// <summary>
        /// Capitalizes the word using proper case.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        /// Capitalized word string.
        /// </returns>
        /// <exception cref="ArgumentNullException">word</exception>
        /// <exception cref="ArgumentException">The {nameof(word)} may not contain spaces: {word}.</exception>
        /// <remarks>For definition, <see href="https://www.computerhope.com/jargon/p/proper-case.htm"/>.</remarks>
        public static string CapitalizeWord(this string word)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            var ci = CultureInfo.CurrentCulture;
            var ret = word.Trim();

            if (ret.Contains(' ')) throw new ArgumentException($"The {nameof(word)} may not contain spaces: {word}.");

            if (ret.Length > 0)
                ret = char.ToUpper(ret[0], ci) + ret.Substring(1);

            return ret;
        }


        /// <summary>
        /// Capitalizes the word using title case.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        /// Capitalized word string.
        /// </returns>
        /// <exception cref="ArgumentNullException">word</exception>
        /// <exception cref="ArgumentException">The {nameof(word)} may not contain spaces: {word}.</exception>
        /// <remarks>For definition, <see href="https://www.computerhope.com/jargon/t/title-case.htm"/>.</remarks>
        public static string CapitalizeWordTitle(this string word)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            var ci = CultureInfo.CurrentCulture;
            var ret = word.Trim();

            if (ret.Contains(' ')) throw new ArgumentException($"The {nameof(word)} may not contain spaces: {word}.");
            if (ret.Length > 3)
                ret = ret.CapitalizeWord();
            else
                ret = ret.ToLower(ci);

            return ret;
        }


        /// <summary>
        /// Determines whether the input string contains one or more of the strings in the specified compare list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="compareList">The compare list.</param>
        /// <returns>
        ///   <c>true</c> if the input string contains one or more of the strings in the specified compare list; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static bool Contains(this string input, IEnumerable<string> compareList)
        {
            if (input.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(input));

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
        /// Enables or disables the tool strip items named in the toolStripItems.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c>, the tool strip items are enabled; else they are disabled.</param>
        /// <param name="toolStripItems">The tool strip items.</param>
        /// <remarks>
        /// <para>If empty tool strip item list, all tool strip items are enabled or disabled.</para>
        /// </remarks>
        public static void EnableOrDisableToolStripItems(bool isEnabled, params ToolStripItem[] toolStripItems)
        {
            if (toolStripItems.IsNullOrEmpty()) throw new ArgumentNullException(nameof(toolStripItems));

            foreach (var item in toolStripItems)
            {
                if (item is ToolStripMenuItem menu)
                    EnableOrDisableToolStripMenuItems(menu, isEnabled);
                else if (item is ToolStripItem itm)
                    itm.Enabled = isEnabled;
            }
        }


        /// <summary>
        /// Enables or disables ALL the menu items under the parent menu.
        /// </summary>
        /// <param name="parentMenu">The parent menu.</param>
        /// <param name="isEnabled">If set to <c>true</c>, the menus are enabled; else they are disabled.</param>
        /// <remarks>
        /// <para>The process is recursive, i.e. if a menu is enabled, all its sub-menus are enabled too - and vice versa</para>
        /// </remarks>
        public static void EnableOrDisableToolStripMenuItems(ToolStripMenuItem parentMenu, bool isEnabled)
        {
            if (parentMenu == null) throw new ArgumentNullException(nameof(parentMenu));

            parentMenu.Enabled = isEnabled;

            foreach (var item in parentMenu.DropDownItems)
            {
                if (item is ToolStripMenuItem menu)
                {
                    menu.Enabled = isEnabled;

                    if (menu.DropDownItems.Count > 0)
                        EnableOrDisableToolStripMenuItems(menu, isEnabled);
                }
                else if (item is ToolStripItem subItem)
                    EnableOrDisableToolStripItems(isEnabled, subItem);
            }
        }


        public static string GetActualAsyncMethodName([CallerMemberName]string name = null) => name;


        /// <summary>
        /// Gets the total text from all the text boxes in the parent control.
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <returns>
        /// Appended string with all the text boxes' trimmed texts, with CR+LF as separators.
        /// </returns>
        /// <remarks>This routine can be used as a simple "serialization" function.</remarks>
        public static string GetAllControlText(this Control parentControl)
        {
            if (parentControl == null) throw new ArgumentNullException(nameof(parentControl));

            var ret = new StringBuilder();

            ret.AppendLine($"{parentControl.Name}: {parentControl.Text.Trim()}");

            foreach (var ctl in parentControl.Controls)
            {
                if (ctl is TextBox txt)
                    ret.AppendLine($"{txt.Name}: {txt.Text.Trim()}");
                else if (ctl is NumericUpDown ud)
                    ret.AppendLine($"{ud.Name}: {ud.Value.ToString(CultureInfo.InvariantCulture).Trim()}");
                else if (ctl is CheckBox chk)
                    ret.AppendLine($"{chk.Name}: {chk.Checked}");
            }

            return ret.ToString();
        }


        /// <summary>
        /// Gets the size of the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
        public static Size GetControlTextSize(this Control control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            Size ret = TextRenderer.MeasureText(control.Text, control.Font);

            return ret;
        }


        /// <summary>
        /// Gets the linker time.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// The assembly build datetime.
        /// </returns>
        /// <exception cref="ArgumentNullException">assembly</exception>
        /// <remarks>
        /// <para>Sources:</para>
        /// <para>https://stackoverflow.com/questions/1600962/displaying-the-build-date</para>
        /// <para>https://blog.codinghorror.com/determining-build-date-the-hard-way/</para>
        /// </remarks>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var filePath = assembly.Location;
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
        /// Gets the program info text.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// Program info text.
        /// </returns>
        public static string GetProgramInfo(Assembly assembly = null)
        {
            var assy = assembly ?? Assembly.GetCallingAssembly();
            //var company = string.Empty;
            var copyright = string.Empty;
            var description = string.Empty;
            var title = string.Empty;
            var v = assy?.GetName()?.Version ?? new Version();
            var version = v.ToString();
            var ret = new StringBuilder();

            foreach (var attr in Attribute.GetCustomAttributes(assy))
            {
                // Check for the AssemblyTitle attribute.
                if (attr is AssemblyTitleAttribute)
                    title = ((AssemblyTitleAttribute)attr).Title;

                // Check for the AssemblyDescription attribute.
                else if (attr is AssemblyDescriptionAttribute)
                    description = ((AssemblyDescriptionAttribute)attr).Description;

                // Check for the AssemblyCompany attribute.
                //else if (attr is AssemblyCompanyAttribute)
                //    company = ((AssemblyCompanyAttribute)attr).Company;

                // Check for the AssemblyCompany attribute.
                else if (attr is AssemblyCopyrightAttribute)
                    copyright = ((AssemblyCopyrightAttribute)attr).Copyright;
            }

            ret.AppendFormat(CultureInfo.InvariantCulture, "{0} version {1}", title, version);
            ret.AppendLine();
            ret.AppendLine(description);
            ret.Append(copyright);

            return ret.ToString();
        }



        /// <summary>
        /// Sends the request to the MC server and reads the response.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Complete REST service Web request image.
        /// </returns>
        /// <exception cref="ArgumentNullException">requestUrl</exception>
        /// <exception cref="HttpRequestException">\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl.ToString()}\".</exception>
        public static async Task<Bitmap> HttpGetImageAsync(this Uri requestUrl, string userName = "", string password = "")
        {
            if (requestUrl == null) throw new ArgumentNullException(nameof(requestUrl));

            Bitmap ret = null;
            Stream st;

            try
            {
                if (userName.IsNullOrEmptyTrimmed())
                    st = await _httpClientAnonymous.GetStreamAsync(requestUrl).ConfigureAwait(false);
                else
                {
                    _httpClientWithCredentials.Dispose();
                    _httpClientHandler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                        Credentials = new NetworkCredential(userName, password)
                    };
                    _httpClientWithCredentials = new HttpClient(_httpClientHandler, true);

                    st = await _httpClientWithCredentials.GetStreamAsync(requestUrl).ConfigureAwait(false);
                }

                using (st)
                {
                    ret = new Bitmap(st);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl.ToString()}\".", ex);
            }

            return ret;
        }


        /// <summary>
        /// Sends the request to the MC server and reads the response.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Complete REST service Web request string.
        /// </returns>
        public static async Task<string> HttpGetStringAsync(this Uri requestUrl, string userName = "", string password = "")
        {
            if (requestUrl == null) throw new ArgumentNullException(nameof(requestUrl));

            string ret;

            try
            {
                if (userName.IsNullOrEmptyTrimmed())
                    ret = await _httpClientAnonymous.GetStringAsync(requestUrl).ConfigureAwait(false);
                else
                {
                    _httpClientWithCredentials.Dispose();
                    _httpClientHandler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                        Credentials = new NetworkCredential(userName, password)
                    };
                    _httpClientWithCredentials = new HttpClient(_httpClientHandler, true);

                    ret = await _httpClientWithCredentials.GetStringAsync(requestUrl).ConfigureAwait(false);
                }

            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl.ToString()}\".", ex);
            }

            return ret;
        }


        /// <summary>
        /// Joins the strings.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="strings">The strings.</param>
        /// <returns>
        /// Joined string.
        /// </returns>
        /// <exception cref="ArgumentNullException">separator
        /// or
        /// separator</exception>
        /// <remarks>
        /// <para>This function is an improved version of the <see cref="string.Join" /> method.</para>
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
        /// <exception cref="ArgumentNullException">input</exception>
        public static string LfToCrLf(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.IsNullOrEmptyTrimmed()) return input;

            var ret = new StringBuilder(input);

            for (int i = 0; i < ret.Length; i++)
            {
                if ((ret[i] == '\n') && ((i == 0) || (ret[i - 1] != '\r')))
                    ret.Insert(i, '\r');
            }

            return ret.ToString();
        }


        /// <summary>
        /// Determines the minimum of the parameters.
        /// </summary>
        /// <param name="integers">The integers.</param>
        /// <returns>The lowest of the specified integers.</returns>
        public static int Min(params int[] integers)
        {
            if (integers.IsNullOrEmpty()) throw new ArgumentException("You must specify 1 or more integers.");

            var ret = integers[0];

            for (int i = 1; i < integers.Length; i++)
            {
                if (integers[i] < ret)
                    ret = integers[i];
            }

            return ret;
        }


        /// <summary>
        /// Converts input string to a string without double spaces and with no more than 2 consecutive line endings.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>Normalized input string.</returns>
        public static string ToNormalizedString(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var ret = new StringBuilder(input.Trim());

            ret.Replace("  ", " ");
            ret.Replace("\r\n\r\n\r\n", "\r\n\r\n");
            ret.Replace("\n\n\n", "\r\n\r\n");
            ret.Replace("\n\n", "\r\n\r\n");

            return ret.ToString();
        }


        /// <summary>
        /// Converts input string to proper case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>Proper case version of the input string.</returns>
        public static string ToProperCase(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var ci = CultureInfo.CurrentCulture;
            var ti = ci.TextInfo;
            var ret = input;

            ret = ti.ToTitleCase(ret); // Microsoft's version of Title Case is really a Proper Case

            return ret;
        }


        /// <summary>
        /// Converts input string to sentence case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>Sentence case version of the input string.</returns>
        public static string ToSentenceCase(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var ci = CultureInfo.CurrentCulture;
            var isNextUpper = true; // Start with upper case
            var ret = new StringBuilder(input.ToNormalizedString());

            for (var i = 0; i < ret.Length; i++)
            {
                if ((new[] { '\r', '\n', '.' }).Contains(ret[i]))
                    isNextUpper = true;
                else
                {
                    if (ret[i] != ' ')
                    {
                        ret[i] = isNextUpper ? char.ToUpper(ret[i], ci) : char.ToLower(ret[i], ci);
                        isNextUpper = false;
                    }
                }
            }

            return ret.ToString();
        }


        /// <summary>
        /// Converts input string to title case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>Title case version of the input string.</returns>
        public static string ToTitleCase(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var ci = CultureInfo.CurrentCulture;
            var ret = input.ToNormalizedString();
            var lines = ret.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // Transform the lines
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].IsNullOrEmptyTrimmed()) continue;

                var words = lines[i].Trim().Split(' ');

                // Transform the words of the line
                for (var j = 0; j < words.Length; j++)
                {
                    words[j] = words[j].CapitalizeWordTitle();
                }

                // Assemble the line, with the first letter in upper case
                lines[i] = string.Join(" ", words);
                lines[i] = char.ToUpper(lines[i][0], ci) + lines[i].Substring(1);
            }

            // Assemble the complete text
            ret = string.Join(Environment.NewLine, lines);

            return ret;
        }

    }

}
