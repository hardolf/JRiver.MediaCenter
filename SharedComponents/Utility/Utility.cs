using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private static HttpClientHandler _httpClientHandlerAnonymous = new HttpClientHandler();
        private static HttpClientHandler _httpClientHandlerWithCredentials = new HttpClientHandler();
        private static HttpClient _httpClientAnonymous = new HttpClient(_httpClientHandlerAnonymous, true);
        private static HttpClient _httpClientWithCredentials = new HttpClient(_httpClientHandlerWithCredentials, true);
        private static readonly Dictionary<string, string> _httpHeaders = new Dictionary<string, string>
            {
                // { "Accept-Charset", "ISO-8859-1" }, // Avoid this, obsolete
                // { "Accept", "text/html,application/xhtml+xml,application/xml" }, // Not used yet...
                { "Accept-Encoding", "gzip, deflate" }, // Only use this if also using AutomaticDecompression on the handler, as it will otherwise make problems with some lyric services, e.g. CajunLyricsService
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident / 7.0; rv: 11.0) like Gecko"} // Internet Explorer 11 on Wondows 10
                // { "User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0" } // Not used, as it is browser version dependent
            };


        /// <summary>
        /// Initializes the <see cref="Utility"/> class.
        /// </summary>
        static Utility()
        {
            CreateHttpClient();
            CreateHttpClient("noname"); // Dummy creation
        }


        /// <summary>
        /// Make the TextBox fit its contents.
        /// </summary>
        /// <param name="control">The text box.</param>
        /// <param name="padding">The padding.</param>
        /// <exception cref="ArgumentNullException">control</exception>
        /// <remarks>
        /// Source: http://csharphelper.com/blog/2018/02/resize-a-textbox-to-fit-its-text-in-c/
        /// </remarks>
        public static void AutoSizeTextBox(this Control control, Padding? padding = null)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));

            const int x_margin = 0;
            const int y_margin = 2;

            if (!padding.HasValue)
                padding = Padding.Empty;

            Size size = TextRenderer.MeasureText(control.Text, control.Font);

            control.ClientSize = new Size(size.Width + x_margin + padding.Value.Left + padding.Value.Right,
                size.Height + y_margin + padding.Value.Top + padding.Value.Bottom);
        }


        /// <summary>
        /// Capitalizes the word using proper case.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>
        /// Capitalized word string.
        /// </returns>
        /// <exception cref="ArgumentNullException">word</exception>
        /// <exception cref="ArgumentException">The {nameof(word)} may not contain spaces: {word}.</exception>
        /// <remarks>
        /// For definition, <see href="https://www.computerhope.com/jargon/p/proper-case.htm" />.
        /// </remarks>
        public static string CapitalizeWord(this string word, CultureInfo cultureInfo = null)
        {
            if (word is null) throw new ArgumentNullException(nameof(word));

            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var ret = word;

            if (ret.Contains(' ')) throw new ArgumentException($"The {nameof(word)} may not contain spaces: \"{word}\".");

            if (ret.Length > 0)
                ret = char.ToUpper(ret[0], ci) + ret.Substring(1);

            return ret;
        }


        /// <summary>
        /// Capitalizes the word using title case.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>
        /// Capitalized word string.
        /// </returns>
        /// <exception cref="ArgumentNullException">word</exception>
        /// <exception cref="ArgumentException">The {nameof(word)} may not contain spaces: {word}.</exception>
        /// <remarks>
        /// For definition, <see href="https://www.computerhope.com/jargon/t/title-case.htm" />.
        /// </remarks>
        public static string CapitalizeWordTitle(this string word, CultureInfo cultureInfo = null)
        {
            if (word is null) throw new ArgumentNullException(nameof(word));

            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var ret = word;

            if (ret.Contains(' ')) throw new ArgumentException($"The {nameof(word)} may not contain spaces: \"{word}\".");
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
        /// Creates the HTTP client.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="timeoutMilliSeconds">The timeout in milliseconds. 0 or negative: no timeout.</param>
        /// <remarks>
        /// If <c>userName</c> is not specified, an anonymous HTTP client is created.
        /// </remarks>
        private static void CreateHttpClient(string userName = "", string password = "", int timeoutMilliSeconds = 0)
        {
            if (userName.IsNullOrEmptyTrimmed())
            {
                _httpClientAnonymous?.Dispose(); // Dispose the old HTTP client, if it was instantiated

                _httpClientHandlerAnonymous = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                _httpClientAnonymous = new HttpClient(_httpClientHandlerAnonymous, true);

                foreach (var httpHeader in _httpHeaders)
                {
                    _httpClientAnonymous.DefaultRequestHeaders.TryAddWithoutValidation(httpHeader.Key, httpHeader.Value);
                }

                if (timeoutMilliSeconds > 0)
                    _httpClientAnonymous.Timeout = new TimeSpan(timeoutMilliSeconds * 10000);
            }
            else
            {
                _httpClientWithCredentials?.Dispose(); // Dispose the old HTTP client, if it was instantiated

                _httpClientHandlerWithCredentials = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    Credentials = new NetworkCredential(userName, password)
                };

                _httpClientWithCredentials = new HttpClient(_httpClientHandlerWithCredentials, true);

                foreach (var httpHeader in _httpHeaders)
                {
                    _httpClientWithCredentials.DefaultRequestHeaders.TryAddWithoutValidation(httpHeader.Key, httpHeader.Value);
                }

                if (timeoutMilliSeconds > 0)
                    _httpClientWithCredentials.Timeout = new TimeSpan(timeoutMilliSeconds * 10000);
            }
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
            if (parentMenu is null) throw new ArgumentNullException(nameof(parentMenu));

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


        public static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;


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
            if (parentControl is null) throw new ArgumentNullException(nameof(parentControl));

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
        /// Gets the focused control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
        public static Control GetFocusedControl(Control control)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));

            var container = control as IContainerControl;

            while (container != null)
            {
                control = container.ActiveControl;
                container = control as IContainerControl;
            }

            return control;
        }


        /// <summary>
        /// Gets the size of the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
        public static Size GetControlTextSize(this Control control)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));

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
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));

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
                if (attr is AssemblyTitleAttribute titleAttr)
                    title = titleAttr.Title;

                // Check for the AssemblyDescription attribute.
                else if (attr is AssemblyDescriptionAttribute descAttr)
                    description = descAttr.Description;

                // Check for the AssemblyCompany attribute.
                // else if (attr is AssemblyCompanyAttribute companyAttr)
                //    company = companyAttr.Company;

                // Check for the AssemblyCompany attribute.
                else if (attr is AssemblyCopyrightAttribute crAttr)
                    copyright = crAttr.Copyright;
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
        /// <param name="timeoutMilliSeconds">The timeout in milliseconds. 0 or negative: no timeout.</param>
        /// <returns>
        /// Complete REST service Web request image.
        /// </returns>
        /// <exception cref="ArgumentNullException">requestUrl</exception>
        /// <exception cref="HttpRequestException">\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl.ToString()}\".</exception>
        public static async Task<Bitmap> HttpGetImageAsync(this Uri requestUrl, string userName = "", string password = "", int timeoutMilliSeconds = 0)
        {
            if (requestUrl is null) throw new ArgumentNullException(nameof(requestUrl));

            Bitmap ret = null;
            Stream st;

            try
            {
                if (userName.IsNullOrEmptyTrimmed())
                    st = await _httpClientAnonymous.GetStreamAsync(requestUrl).ConfigureAwait(false);
                else
                {
                    CreateHttpClient(userName, password, timeoutMilliSeconds);

                    st = await _httpClientWithCredentials.GetStreamAsync(requestUrl).ConfigureAwait(false);
                }

                using (st)
                {
                    ret = new Bitmap(st);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl}\".", ex);
            }

            return ret;
        }


        /// <summary>
        /// Sends the request to the MC server and reads the response.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="timeoutMilliSeconds">The timeout in milliseconds. 0 or negative: no timeout.</param>
        /// <returns>
        /// Complete REST service Web request string.
        /// </returns>
        /// <exception cref="ArgumentNullException">requestUrl</exception>
        /// <exception cref="HttpRequestException">\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl.ToString()}\".</exception>
        public static async Task<string> HttpGetStringAsync(this Uri requestUrl, string userName = "", string password = "", int timeoutMilliSeconds = 0)
        {
            if (requestUrl is null) throw new ArgumentNullException(nameof(requestUrl));

            string ret;

            try
            {
                if (userName.IsNullOrEmptyTrimmed())
                    ret = await _httpClientAnonymous.GetStringAsync(requestUrl).ConfigureAwait(false);
                else
                {
                    CreateHttpClient(userName, password, timeoutMilliSeconds);

                    ret = await _httpClientWithCredentials.GetStringAsync(requestUrl).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"\"The call to the service failed: \"{ex.Message}\". Request: \"{requestUrl}\".", ex);
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
        /// Determines whether the specified form is completely on screen.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>
        ///   <c>true</c> if the specified form is completely on screen; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOnScreenAll(this Form form)
        {
            if (form is null) throw new ArgumentNullException(nameof(form));

            Screen[] screens = Screen.AllScreens;

            foreach (Screen screen in screens)
            {
                Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);

                if (screen.WorkingArea.Contains(formRectangle))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Determines whether the top left point of the specified form is on screen.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>
        ///   <c>true</c> if the top left point of the specified form is on screen; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOnScreenTopLeft(this Form form)
        {
            if (form is null) throw new ArgumentNullException(nameof(form));

            Screen[] screens = Screen.AllScreens;

            foreach (Screen screen in screens)
            {
                Point formTopLeft = new Point(form.Left, form.Top);

                if (screen.WorkingArea.Contains(formTopLeft))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Determines whether parenthesized text is present in the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns><c>true</c> if parenthesized text is present in the specified text; else <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">text</exception>
        public static bool IsParenthesizedTextPresent(this string text)
        {
            if (text.IsNullOrEmptyTrimmed()) return false;

            var openingChars = new[] { '(', '[', '{' };
            var closingChars = new[] { ')', ']', '}' };
            var idx1 = text.IndexOfAny(openingChars);
            var idx2 = text.IndexOfAny(closingChars);

            return ((idx1 >= 0) || (idx2 >= 0));
        }


        /// <summary>
        /// Converts Unix-style linefeeds to Windows-style line endings (LF to CR + LF)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string LfToCrLf(this string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
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
        /// Removes the parenthesized text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// Original text with the parenthesized text removed.
        /// </returns>
        /// <exception cref="ArgumentNullException">text</exception>
        public static string RemoveParenthesizedText(this string text)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));

            var ret = text;
            var openingChars = new[] { '(', '[', '{' };
            var closingChars = new[] { ')', ']', '}' };

            var idx1 = ret.IndexOfAny(openingChars);
            var idx2 = ret.IndexOfAny(closingChars);

            while ((idx1 >= 0) && (idx2 > idx1))
            {
                ret = ret.Remove(idx1, idx2 - idx1 + 1);

                idx1 = ret.IndexOfAny(openingChars);
                idx2 = ret.IndexOfAny(closingChars);
            }

            return ret?.Trim() ?? string.Empty;
        }


        /// <summary>
        /// Sets the location and size of the target form.
        /// </summary>
        /// <param name="targetForm">The target form.</param>
        /// <param name="preferredLocation">The preferred location.</param>
        /// <param name="preferredSize">The preferred size.</param>
        public static void SetFormLocationAndSize(this Form targetForm, Point preferredLocation, Size preferredSize)
        {
            if (targetForm is null) throw new ArgumentNullException(nameof(targetForm));

            var activeWorkingArea = Screen.FromControl(targetForm).WorkingArea;
            var rect1 = new Rectangle(preferredLocation, preferredSize);
            var rect2 = rect1;
            var offset = 5;

            rect2.Location.Offset(offset, offset);

            if (activeWorkingArea.Contains(rect1.Location)
                && activeWorkingArea.Contains(rect2.Location)
                && (rect1.Height >= targetForm.MinimumSize.Height)
                && (rect1.Width >= targetForm.MinimumSize.Width))
            {
                // Place the form like the last time
                targetForm.Location = rect1.Location;
                targetForm.Size = rect1.Size;
            }
            else
            {
                // The last time the form was placed (in part) outside the screen, so reset the size and center on screen
                // targetForm.Size = targetForm.MinimumSize;

                var horOffset = (int)((activeWorkingArea.Width / 2) - (0.5 * targetForm.Width));
                var vertOffset = (int)((activeWorkingArea.Height / 2) - (0.5 * targetForm.Height));

                targetForm.Location = new Point(horOffset, vertOffset);
            }
        }


        /// <summary>
        /// Counts all of the string's lines.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Number of lines in the <c>input</c> string.</returns>
        public static int StringLineCount(this string input)
        {
            var ret = 0;

            using (var sr = new StringReader(input))
            {
                while (sr.ReadLine() != null)
                {
                    ret++;
                }
            }

            return ret;
        }


        public static string ToAmpersandCorrected(this string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var searchStrings = new[] { "&amp;amp;" };
            var ret = new StringBuilder(input);

            foreach (var s in searchStrings)
            {
                ret.Replace(s, "&");
            }

            return ret.ToString();
        }


        public static string ToBracketsCorrected(this string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ret = new StringBuilder(input);

            ret.Replace("&lt;", "<");
            ret.Replace("&gt;", ">");

            return ret.ToString();
        }


        /// <summary>
        /// Capitalize letter after each period.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string ToCapitalizedAfterPeriod(this string input, CultureInfo cultureInfo = null)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var isPeriod = false;
            var ret = new StringBuilder();
            var ws = new[] { ' ', '\t' };

            for (int i = 0; i < input.Length; i++)
            {
                var ch = input[i];

                if (ch == '.')
                    isPeriod = true;
                else if (isPeriod && !ws.Contains(ch))
                {
                    isPeriod = false;
                    ch = char.ToUpper(ch, ci);
                }

                ret.Append(ch);
            }

            return ret.ToString();
        }


        /// <summary>
        /// Converts to to a string where single '\r' and '\n' are replaced by '\r\n'.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string ToNormalizedLineEndings(this string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            input = input.Trim(' ', '\t');

            var len = input.Length;
            var ret = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                var ch = input[i];

                if ((ch == '\r') && (i < len - 1))
                {
                    if (input[i + 1] == '\n')
                        ret.Append(ch);
                    else
                        ret.Append(Constants.NewLine);
                }
                else if ((ch == '\n') && (i > 0))
                {
                    if (input[i - 1] == '\r')
                        ret.Append(ch);
                    else
                        ret.Append(Constants.NewLine);
                }
                else
                    ret.Append(ch);
            }

            return ret.ToString();
        }


        /// <summary>
        /// Converts input string to a string without double spaces and with no more than 2 consecutive line endings.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// Normalized input string.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string ToNormalizedString(this string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ret = new StringBuilder(input.Trim(' ', '\t'));

            ret.Replace("  ", " ");
            ret.Replace(Constants.TripleNewLine, Constants.DoubleNewLine);
            ret.Replace("\n\n\n", Constants.DoubleNewLine);
            ret.Replace("\n\n", Constants.DoubleNewLine);
            ret.Replace("\r\r\r", Constants.DoubleNewLine);
            ret.Replace("\r\r", Constants.DoubleNewLine);

            return ret.ToString();
        }


        /// <summary>
        /// Converts input string to proper case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>
        /// Proper case version of the input string.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string ToProperCase(this string input, CultureInfo cultureInfo = null)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var ti = ci.TextInfo;
            var ret = input;

            ret = ti.ToTitleCase(ret); // Microsoft's version of Title Case is really a Proper Case

            return ret;
        }


        /// <summary>
        /// Converts input string to sentence case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>
        /// Sentence case version of the input string.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        /// <remarks>
        /// The first letter of each line is made uppercase, the remaining is left as it is.
        /// </remarks>
        public static string ToSentenceCase(this string input, CultureInfo cultureInfo = null)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var ret = input.ToNormalizedString();
            var lines = ret.Split(new[] { Constants.NewLine }, StringSplitOptions.None);
            var skippedFirstCharacters = new[] { '\'', '(', '[', '{' };

            // Transform the lines
            for (var i = 0; i < lines.Length; i++)
            {
                var tmp = lines[i];
                var firstCharIdx = 0;

                if (tmp.IsNullOrEmptyTrimmed()) continue;

                if (tmp.Length > 1)
                {
                    foreach (var ch in tmp)
                    {
                        // Find the index of the first character that should not be skipped
                        if (skippedFirstCharacters.Contains(ch))
                            firstCharIdx++;
                        else
                            break;
                    }

                    // Make the first 'real' character uppercase and preserve the rest of the string
                    tmp = ((firstCharIdx > 0) ? tmp.Substring(0, firstCharIdx) : string.Empty)
                        + tmp[firstCharIdx].ToString(ci).ToUpper(ci) + tmp.Substring(firstCharIdx + 1);
                }
                else
                    tmp = tmp.ToUpper(ci);

                lines[i] = tmp.ToCapitalizedAfterPeriod(cultureInfo);
            }

            // Assemble the complete text
            ret = string.Join(Constants.NewLine, lines);

            return ret;
        }


        /// <summary>
        /// Converts to singlelineendings.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string ToSingleLineEndings(this string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ret = new StringBuilder(input.ToNormalizedString());

            ret.Replace(Constants.DoubleNewLine, Constants.NewLine);

            return ret.ToString();
        }


        /// <summary>
        /// Converts input string to title case.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>
        /// Title case version of the input string.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public static string ToTitleCase(this string input, CultureInfo cultureInfo = null)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var ci = cultureInfo ?? CultureInfo.CurrentCulture;
            var ret = input.ToNormalizedString();
            var lines = ret.Split(new[] { Constants.NewLine }, StringSplitOptions.None);

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
            ret = string.Join(Constants.NewLine, lines);

            return ret;
        }


        /// <summary>
        /// Trims all of the string's lines.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The <c>input</c> string with each of it's lines trimmed.</returns>
        public static string TrimStringLines(this string input)
        {
            if (input.IsNullOrEmptyTrimmed()) return input;

            var ret = new StringBuilder();

            using (var sr = new StringReader(input))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    ret.AppendLine(line.Trim());
                }
            }

            // Remove the last line-end if the input string does not end with a line-end
            if (!input.EndsWith(Constants.NewLine, StringComparison.InvariantCultureIgnoreCase))
                ret.Length -= Constants.NewLine.Length;

            return ret.ToString();
        }


        /// <summary>
        /// Trims all of the string's lines.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="numberOfLines">The number of lines.</param>
        /// <returns>
        /// The first <c>numberOfLines</c> lines of the <c>input</c> string.
        /// </returns>
        public static string TruncateStringLines(this string input, int numberOfLines)
        {
            var cnt = 0;
            var ret = new StringBuilder();

            using (var sr = new StringReader(input))
            {
                string line;

                while ((cnt < numberOfLines) && ((line = sr.ReadLine()) != null))
                {
                    cnt++;
                    ret.AppendLine(line.Trim());
                }
            }

            return ret.ToString();
        }

    }

}
