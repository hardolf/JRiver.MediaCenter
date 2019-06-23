using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;

using MediaCenter.LyricsFinder.Model.Helpers;
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
        /// Extracts the result text and sets the FoundLyricsText.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// If found, the found lyric text string; else null.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Document node not found in the lyric page.
        /// or
        /// Lyric \"div\" nodes not found.
        /// or
        /// Lyric main \"div\" node not found.
        /// or
        /// Lyric sub \"div\" nodes not found.
        /// </exception>
        protected override async Task<string> ExtractOneLyricTextAsync(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var ret = await base.ExtractOneLyricTextAsync(uri).ConfigureAwait(false);
            var html = await Helpers.Utility.HttpGetStringAsync(uri).ConfigureAwait(false);
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
                AddFoundLyric(ret, new SerializableUri(uri.AbsoluteUri));

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

            var hrefs = tableNode.SelectNodes("./tr/td/a[@href]");

            if (hrefs == null) return ret;

            foreach (var href in hrefs)
            {
                var uri = new Uri(href.GetAttributeValue("href", string.Empty));

                ret.Add(uri);
            }

            return ret;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="getAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendent object of type <see cref="Stands4Service" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{req.RequestUri.ToString()}\".</exception>
        /// <remarks>
        /// This routine gets the first (if any) search results from the lyric service.
        /// </remarks>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem item, bool getAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            await base.ProcessAsync(item).ConfigureAwait(false); // Result: not found

            var credit = Credit as CreditType;
            var html = string.Empty;
            var urlString = string.Empty;
            SerializableUri uri;

            try
            {
                // First we try a rigorous test
                urlString = $"{credit.ServiceUrl}?q={item.Artist} {item.Album} {item.Name}";
                uri = new SerializableUri(Uri.EscapeUriString(urlString));
                html = await Helpers.Utility.HttpGetStringAsync(uri).ConfigureAwait(false);

                await ExtractAllLyricTextsAsync(GetResultUris(html), getAll).ConfigureAwait(false);

                // If not found or if we want all possible results, we next try a more lax test without the album
                if (getAll || (LyricResult != LyricResultEnum.Found))
                {
                    urlString = $"{credit.ServiceUrl}?q={item.Artist} {item.Name}";
                    uri = new SerializableUri(Uri.EscapeUriString(urlString));
                    html = await Helpers.Utility.HttpGetStringAsync(uri).ConfigureAwait(false);

                    await ExtractAllLyricTextsAsync(GetResultUris(html), getAll).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{urlString}\".", ex);
            }

            return this;
        }

    }

}
