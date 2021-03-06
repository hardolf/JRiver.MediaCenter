﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using HtmlAgilityPack;

using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// AZLyrics lyric service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class AZLyricsService : AbstractLyricService, ILyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AZLyricsService"/> class.
        /// </summary>
        public AZLyricsService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AZLyricsService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public AZLyricsService(AZLyricsService source)
            : base(source)
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="AZLyricsService" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new AZLyricsService(this);

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Extracts the result text and sets the FoundLyricsText.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// If found, the found lyric text string; else null.
        /// </returns>
        /// <exception cref="ArgumentNullException">uri</exception>
        /// <exception cref="NullReferenceException">Document node not found in the lyric page.
        /// or
        /// Lyric \"div\" nodes not found.
        /// or
        /// Lyric main \"div\" node not found.
        /// or
        /// Lyric sub \"div\" nodes not found.</exception>
        protected override async Task<string> ExtractOneLyricTextAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            // AZLyrics is prone to ban my IP, so I insert an extra delay here
            await AsyncUtility.RandomizedDelayAsync(LyricsFinderData.MainData.DelayMilliSecondsBetweenSearches).ConfigureAwait(false);

            var ret = await base.ExtractOneLyricTextAsync(uri, cancellationToken).ConfigureAwait(false);
            var html = await HttpGetStringAsync(uri).ConfigureAwait(false);
            var doc = new HtmlDocument();

            TestIpBanWarning(html);

            doc.LoadHtml(html);

            var docNode = doc.DocumentNode;

            if (docNode == null) throw new NullReferenceException("Document node not found in the lyric page.");

            var divs = docNode.SelectNodes("//div[@class]");

            if (divs == null) throw new NullReferenceException("Lyric \"div\" nodes not found.");

            HtmlNode divNode = null;

            foreach (var div in divs)
            {
                var classValue = div.GetAttributeValue("class", string.Empty);

                if (classValue.Equals("container main-page", StringComparison.InvariantCultureIgnoreCase))
                {
                    divNode = div;
                    break;
                }
            }

            if (divNode == null) throw new NullReferenceException("Lyric main \"div\" node not found.");

            divs = divNode.SelectNodes("./div/div/div");

            if (divs == null) throw new NullReferenceException("Lyric sub \"div\" nodes not found.");

            foreach (var div in divs)
            {
                // The final div element has no attributes
                if (!div.HasAttributes)
                {
                    divNode = div;
                    break;
                }
            }

            if (divNode == null) return string.Empty;

            ret = divNode.InnerText;
            ret = ret.LfToCrLf(); // Ensure proper Windows CRLF line endings
            ret = ret.Trim('\r', '\n');

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                await AddFoundLyric(ret, new Uri(uri.AbsoluteUri)).ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Gets the first result URL.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>
        /// URL to the first result Web page.
        /// </returns>
        /// <exception cref="ArgumentNullException">html</exception>
        protected virtual List<Uri> GetResultUris(string html)
        {
            if (html.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(html));

            var ret = new List<Uri>();
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var docNode = doc.DocumentNode;

            if (docNode == null) throw new NullReferenceException("Document node not found in the search results page.");

            var tableNode = docNode.SelectSingleNode("//table[@class]");

            if (tableNode == null) return ret; // No results

            if (!tableNode.GetAttributeValue("class", string.Empty).Equals("table table-condensed", StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Result table node with attribute class=\"table table-condensed\" not found.");

            var anchors = tableNode.SelectNodes("./tr/td/a");

            foreach (var a in anchors)
            {
                // Skip page anchors
                if (!a.GetAttributeValue("class", string.Empty).IsNullOrEmptyTrimmed())
                    continue;

                var hrefValue = a.GetAttributeValue("href", string.Empty);

                try
                {
                    var uri = new UriBuilder(hrefValue).Uri;

                    ret.Add(uri);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error parsing URI string from 'href' attribute: \"{hrefValue}\".", ex);
                }
            }

            return ret;
        }


        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AZLyricsService" /> descendant object.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricServiceIpBanWarningException"></exception>
        /// <exception cref="LyricServiceIpBannedException">
        /// Lyric service \"{Credit.ServiceName}\" is in danger of a ban of your IP address. \r\n"
        ///                         + "The service is now disabled in LyricsFinder. \r\n"
        ///                         + "No more requests will be sent to this service until corrected. \r\n"
        ///                         + "You should try the AZLyrics site in a browser (https://azlyrics.com/) and tick the checkbox telling the site that you are no robot.
        /// or
        /// Lyric service \"{Credit.ServiceName}\" experienced a \"{exx.Message}\" error. \r\n"
        ///                             + "This is possibly a temporary ban of your IP address and the service is now disabled in LyricsFinder. \r\n"
        ///                             + "No more requests will be sent to this service until corrected. \r\n"
        ///                             + "You could try the AZLyrics site in a browser (https://azlyrics.com/) and tick the checkbox telling the site that you are no robot.
        /// </exception>
        /// <exception cref="LyricServiceCommunicationException"></exception>
        /// <exception cref="LyricServiceBaseException"></exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            var html = string.Empty;
            var ub = new UriBuilder(Credit.ServiceUrl);

            try
            {
                await base.ProcessAsync(mcItem, cancellationToken).ConfigureAwait(false); // Result: not found

                // First we try a rigorous query
                ub.Query = $"q={mcItem.Artist} {mcItem.Album} {mcItem.Name}";
                html = await HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                TestIpBanWarning(html, mcItem, isGetAll);

                // We force serial search in order to avoid IP address banning from the service
                await ExtractAllLyricTextsAsync(GetResultUris(html), cancellationToken, isGetAll, true).ConfigureAwait(false);

                // If not found or if we want all possible results, we next try a more lax query without the album
                if (IsActive && !LyricsFinderData.MainData.StrictSearchOnly
                    && (isGetAll || (LyricResult != LyricsResultEnum.Found)))
                {
                    ub.Query = $"q={mcItem.Artist} {mcItem.Name}";
                    html = await HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                    TestIpBanWarning(html, mcItem, isGetAll);

                    // We force serial search in order to avoid IP address banning from the service
                    await ExtractAllLyricTextsAsync(GetResultUris(html), cancellationToken, isGetAll, true).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                // Get the first exception
                var exx = (Exception)ex;

                while (exx.InnerException != null)
                    exx = exx.InnerException;

                if (exx.Message.Contains("403"))
                {
                    IsActive = false;

                    throw new LyricServiceIpBannedException(Constants.NewLine
                        + $"Lyric service \"{Credit.ServiceName}\" experienced a \"{exx.Message}\" error. " + Constants.NewLine
                        + "This is possibly a temporary ban of your IP address and the service is now disabled in LyricsFinder. " + Constants.NewLine
                        + "No more requests will be sent to this service until corrected. " + Constants.NewLine
                        + "You should wait a couple of hours and then try the AZLyrics site in a browser (https://azlyrics.com/)"
                        + "In the browser, tick the checkbox telling the site that you are no robot.",
                            isGetAll, Credit, mcItem, ex);
                }
                else
                    throw new LyricServiceCommunicationException($"{Credit.ServiceName} request failed for: " +
                        Constants.NewLine + $"\"{mcItem.Artist}\" - \"{mcItem.Album}\" - \"{mcItem.Name}\".",
                        isGetAll, Credit, mcItem, ub.Uri, ex);
            }
            catch (Exception ex)
            {
                throw new LyricServiceBaseException($"{Credit.ServiceName} process failed for: " +
                    Constants.NewLine + $"\"{mcItem.Artist}\" - \"{mcItem.Album}\" - \"{mcItem.Name}\".",
                    isGetAll, Credit, mcItem, ex);
            }

            return this;
        }


        /// <summary>
        /// Tests if the <c>html</c> contains an IP ban warning.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <exception cref="ArgumentNullException">html</exception>
        /// <exception cref="LyricServiceIpBanWarningException">..."</exception>
        private void TestIpBanWarning(string html, McMplItem mcItem = null, bool isGetAll = false)
        {
            if (html.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(html));

            if (html.ToUpperInvariant().Contains("<FORM ID=\"AZ_UNBLOCK\""))
            {
                IsActive = false;

                throw new LyricServiceIpBanWarningException(Constants.NewLine
                    + $"Lyric service \"{Credit.ServiceName}\" is in danger of a ban of your IP address. " + Constants.NewLine
                    + "The service is now disabled in LyricsFinder. " + Constants.NewLine
                    + "No more requests will be sent to this service until corrected. " + Constants.NewLine
                    + "You should wait a couple of minutes and then try the AZLyrics site in a browser (https://azlyrics.com/)"
                    + "In the browser, tick the checkbox telling the site that you are no robot.",
                    isGetAll, Credit, mcItem);
            }
        }

    }

}
