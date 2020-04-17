using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MediaCenter.LyricsFinder.Forms;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.SharedComponents;

using Newtonsoft.Json;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Utilities for the LyricsFinder.
    /// </summary>
    [ComVisible(false)]
    public static class Utility
    {

        // Private constants
        private const string UnInitializedPrivateSettingText = "YOUR_OWN_STRING";
        private static readonly Uri LatestReleaseUrl = new UriBuilder("https://api.github.com/repos/hardolf/JRiver.MediaCenter/releases/latest").Uri;

        // Public constants
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string AppSettingsSectionName = "appSettings";
        public const string AppConfigFileExt = ".config";
        public const string PrivateConfigFileExt = ".private.config";
        public const string PrivateConfigTemplateFileExt = ".template.config";
        public static readonly Uri RepositoryUrl = new UriBuilder("https://github.com/hardolf/JRiver.MediaCenter").Uri;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member



        /// <summary>
        /// Gets the application settings file path.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// String with the application settings file path.
        /// </returns>
        /// <exception cref="ArgumentNullException">assembly</exception>
        public static string GetAppSettingsFilePath(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var ret = assembly.Location + AppConfigFileExt;

            return ret;
        }


        /// <summary>
        /// Gets the private settings file path.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="dataDir">The data dir.</param>
        /// <returns>
        /// String with the private settings file path.
        /// </returns>
        /// <exception cref="ArgumentNullException">assembly
        /// or
        /// dataDir</exception>
        /// <remarks>Copying is no longer done after v1.2</remarks>
        public static string GetPrivateSettingsFilePath(Assembly assembly, string dataDir)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (dataDir.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(dataDir));

            var ret = Path.Combine(dataDir, Path.GetFileName(assembly.Location) + PrivateConfigFileExt);
            //var appConfigFilePath = assembly.Location + PrivateConfigFileExt;
            //var templateConfigFilePath = assembly.Location + PrivateConfigTemplateFileExt;

            // Create the private config. file if not found in the data dir.
            //if (!File.Exists(ret))
            //{
            //    if (File.Exists(appConfigFilePath))
            //        File.Copy(appConfigFilePath, ret, false); // Upgrade from LyricsFinder 1.0
            //    else if (File.Exists(templateConfigFilePath))
            //        File.Copy(templateConfigFilePath, ret, false); // First start
            //    else
            //        ret = string.Empty; // No private settings file found / needed
            //}

            return ret;
        }


        /// <summary>
        /// Gets the property value from the target object.
        /// </summary>
        /// <param name="displayPropertyDictionary">The display property dictionary.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="ignorePropertyNameError">if set to <c>true</c> ignore property name error; else throw an exception on this error.</param>
        /// <returns>
        /// String value of the property from the target object.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        /// <exception cref="System.Exception">PropertyName</exception>
        public static object GetPropertyValue(this Dictionary<string, DisplayProperty> displayPropertyDictionary, string propertyName, bool ignorePropertyNameError = false)
        {
            if (displayPropertyDictionary == null) throw new ArgumentNullException(nameof(displayPropertyDictionary));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var isOk = displayPropertyDictionary.TryGetValue(propertyName, out var dp);
            object ret = null;

            if (isOk)
                ret = dp.Value;
            else if (!ignorePropertyNameError)
                throw new KeyNotFoundException($"The dictionary has no \"{propertyName}\" key.");

            return ret;
        }


        /// <summary>
        /// Shows the server error message.
        /// </summary>
        /// <param name="response">The HTTP Web response.</param>
        /// <returns>HTTP error message.</returns>
        /// <exception cref="ArgumentNullException">response</exception>
        public static string HttpWebServerErrorMessage(HttpWebResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            return $"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).";
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
        /// Gets the null responce message.
        /// </summary>
        /// <value>
        /// The null responce message.
        /// </value>
        public static string NullResponseMessage => "Response is null";


        /// <summary>
        /// Sets the property value on the target object.
        /// </summary>
        /// <param name="displayPropertyDictionary">The display property dictionary.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignorePropertyNameError">if set to <c>true</c> ignore property name error; else throw an exception on this error.</param>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The dictionary has no \"{propertyName}\" key.</exception>
        /// <exception cref="System.Exception">PropertyName</exception>
        public static void SetPropertyValue(this Dictionary<string, DisplayProperty> displayPropertyDictionary, string propertyName, object value, bool ignorePropertyNameError = false)
        {
            if (displayPropertyDictionary == null) throw new ArgumentNullException(nameof(displayPropertyDictionary));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var isOk = displayPropertyDictionary.TryGetValue(propertyName, out var dp);

            if (isOk)
                dp.Value = value;
            else if (!ignorePropertyNameError)
                throw new KeyNotFoundException($"The dictionary has no \"{propertyName}\" key.");
        }


        /// <summary>
        /// Check for updates.
        /// </summary>
        /// <param name="currentVersion">The current version.</param>
        /// <param name="maxWindowSize">Maximum size of the window.</param>
        /// <param name="isInteractive">if set to <c>true</c> [is interactive].</param>
        /// <returns>
        ///   <c>false</c> if a newer release is available; else <c>true</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">currentVersion</exception>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">Server error (HTTP {rsp.StatusCode}: {rsp.StatusDescription}</exception>
        public static async Task<bool> UpdateCheckAsync(Version currentVersion, Size maxWindowSize, bool isInteractive = false)
        {
            if (currentVersion == null) throw new ArgumentNullException(nameof(currentVersion));

            var req = WebRequest.Create(LatestReleaseUrl) as HttpWebRequest;
            var json = string.Empty;

            req.Method = "GET";
            req.UserAgent = "request";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Make the request for the latest release
            using (var rsp = req.GetResponse() as HttpWebResponse)
            {
                if (rsp == null)
                    throw new NullReferenceException(NullResponseMessage);
                if (rsp.StatusCode != HttpStatusCode.OK)
                    throw new Exception(HttpWebServerErrorMessage(rsp));

                using (var reader = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }
            }

            // Deserialize the returned JSON
            var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
            var tagName = (string)searchDyn.tag_name;
            var urlString = (string)searchDyn.html_url;
            var sb = new StringBuilder();

            for (int i = 0; i < tagName.Length; i++)
            {
                var ch = tagName[i];

                if ("0123456789.".Contains(ch))
                    sb.Append(ch);
            }

            // Compare the versions
            var latestVersion = Version.Parse(sb.ToString());
            var cmp = currentVersion.CompareTo(latestVersion);
            var ret = (cmp >= 0);

            if (!isInteractive)
                return ret;

            // This is the interactive part
            string msg;

            if (ret)
                msg = $"LyricsFinder v{currentVersion} is the latest release.\r\n"
                    + "No update is necessary.";
            else
                msg = $"LyricsFinder v{currentVersion} is not the latest release.\r\n"
                    + $"An update to v{latestVersion} is available.\r\n\r\n"
                    + $"You can visit the download site here:\r\n\r\n"
                    + $"{urlString}";

            await ErrorForm.ShowAsync(null, "Update information", msg, maxWindowSize);

            return ret;
        }


        /// <summary>
        /// Check for updates and retries 5 times.
        /// </summary>
        /// <param name="currentVersion">The current version.</param>
        /// <param name="maxWindowSize">Maximum size of the window.</param>
        /// <param name="isInteractive">if set to <c>true</c> [is interactive].</param>
        /// <param name="retryCount">The retry count.</param>
        /// <returns>
        ///   <c>false</c> if a newer release is available; else <c>true</c>.
        /// </returns>
        /// <exception cref="WebException">Error contacting the latest release site ({LatestReleaseUrl}): {ex.Message}</exception>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">Server error (HTTP {rsp.StatusCode}: {rsp.StatusDescription}</exception>
        public static async Task<bool> UpdateCheckWithRetriesAsync(Version currentVersion, Size maxWindowSize, bool isInteractive = false, int retryCount = 5)
        {
            const int retryInterval = 500; // Milliseconds

            var ret = true;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    ret = await UpdateCheckAsync(currentVersion, maxWindowSize, isInteractive);

                    break;
                }
                catch (WebException ex)
                {
                    // Should we ignore this exception for now?
                    if (i < retryCount - 1)
                        Thread.Sleep(retryInterval);
                    else
                        throw new WebException($"Error contacting the latest release site ({LatestReleaseUrl}): {ex.Message}");
                }
            }

            return ret;
        }

    }

}
