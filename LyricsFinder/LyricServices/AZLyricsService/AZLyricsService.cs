using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using HtmlAgilityPack;

using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// AZLyrics lyric service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class AZLyricsService : AbstractLyricService
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
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="AZLyricsService" /> object.
        /// </returns>
        public override AbstractLyricService Clone()
        {
            var ret = new AZLyricsService
            {
                Comment = Comment,
                Credit = Credit.Clone(),
                IsActive = IsActive,
                IsImplemented = IsImplemented,
                LyricResult = LyricResultEnum.NotProcessedYet,
                LyricsFinderData = LyricsFinderData,

            };

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

            var ret = await base.ExtractOneLyricTextAsync(uri, cancellationToken).ConfigureAwait(false);
            var html = await base.HttpGetStringAsync(uri).ConfigureAwait(false);
            var doc = new HtmlDocument();

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

            ret = divNode.InnerText;
            ret = ret.LfToCrLf(); // Ensure proper Windows CRLF line endings
            ret = ret.Trim('\r', '\n');

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                AddFoundLyric(ret, new Uri(uri.AbsoluteUri));

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
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object of type <see cref="Stands4Service" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricsQuotaExceededException">Lyric service \"{Credit.ServiceName}\" experienced a \"{chk}\" error. "
        ///                         + "This is possibly a temporary ban of your IP address "
        ///                         + $"and the service is now disabled in LyricsFinder. No more requests will be sent to this service until corrected.</exception>
        /// <exception cref="LyricServiceCommunicationException"></exception>
        /// <exception cref="GeneralLyricServiceException"></exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem item, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var html = string.Empty;
            var ub = new UriBuilder(Credit.ServiceUrl);

            try
            {
                await base.ProcessAsync(item, cancellationToken).ConfigureAwait(false); // Result: not found

                // First we try a rigorous query
                ub.Query = $"q={item.Artist} {item.Album} {item.Name}";
                html = await base.HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                await ExtractAllLyricTextsAsync(GetResultUris(html), cancellationToken, isGetAll).ConfigureAwait(false);

                // If not found or if we want all possible results, we next try a more lax query without the album
                if (isGetAll || (LyricResult != LyricResultEnum.Found))
                {
                    ub.Query = $"q={item.Artist} {item.Name}";
                    html = await base.HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                    await ExtractAllLyricTextsAsync(GetResultUris(html), cancellationToken, isGetAll).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                // Get the first exception
                var exx = (Exception)ex;

                while (exx.InnerException != null)
                    exx = exx.InnerException;

                var chk = "The underlying connection was closed: The connection was closed unexpectedly";

                if (exx.Message.Contains(chk))
                {
                    IsActive = false;

                    throw new LyricsQuotaExceededException($"Lyric service \"{Credit.ServiceName}\" experienced a \"{chk}\" error. "
                            + "This is possibly a temporary ban of your IP address "
                            + $"and the service is now disabled in LyricsFinder. No more requests will be sent to this service until corrected.", ex);
                }
                else
                    throw new LyricServiceCommunicationException($"{Credit.ServiceName} request failed.", isGetAll, Credit, item, ub.Uri, ex);
            }
            catch (Exception ex)
            {
                throw new GeneralLyricServiceException($"{Credit.ServiceName} process failed.", isGetAll, Credit, item, ex);
            }

            return this;
        }

    }

}
