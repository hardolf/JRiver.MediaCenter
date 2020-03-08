using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

using HtmlAgilityPack;

using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Lololyrics Lyrics API service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class LololyricsService : AbstractLyricService, ILyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LololyricsService"/> class.
        /// </summary>
        public LololyricsService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LololyricsService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public LololyricsService(LololyricsService source)
            : base(source)
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="LololyricsService" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new LololyricsService(this);

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Extracts the result text from a Uri and adds the found lyric text to the list.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// If found, the found lyric text string; else null.
        /// </returns>
        /// <exception cref="ArgumentNullException">uri</exception>
        /// <exception cref="System.ArgumentNullException">uri</exception>
        protected override async Task<string> ExtractOneLyricTextAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var ret = await base.ExtractOneLyricTextAsync(uri, cancellationToken).ConfigureAwait(false);
            var html = await base.HttpGetStringAsync(uri).ConfigureAwait(false);
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var docNode = doc.DocumentNode;

            if (docNode == null) throw new NullReferenceException("Document node not found in the lyric page.");

            var divs = docNode.SelectNodes("//div[@id]");

            if (divs == null) throw new NullReferenceException("Lyric \"id\" nodes not found.");

            HtmlNode divNode = null;

            foreach (var div in divs)
            {
                var idValue = div.GetAttributeValue("id", string.Empty);

                if (idValue.Equals("lyrics_txt", StringComparison.InvariantCultureIgnoreCase))
                {
                    divNode = div;
                    break;
                }
            }

            if (divNode == null) return string.Empty;

            ret = divNode.InnerText;
            ret = ret.Replace("\t", string.Empty); // Remove tab chars
            ret = ret.LfToCrLf(); // Ensure proper Windows CRLF line endings
            ret = ret.Trim('\r', '\n');

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                await AddFoundLyric(ret, new Uri(uri.AbsoluteUri)).ConfigureAwait(false);

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
        /// <exception cref="LyricServiceCommunicationException"></exception>
        /// <exception cref="GeneralLyricServiceException"></exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem item, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var xmlText = string.Empty;
            UriBuilder ub = null;

            try
            {
                await base.ProcessAsync(item, cancellationToken).ConfigureAwait(false); // Result: not found

                // Example GET request:
                // OK: http://api.lololyrics.com/0.5/getLyric?artist=Pattern+J&track=Chromozome
                // Not found: http://api.lololyrics.com/0.5/getLyric?artist=fakeartist&track=fakesong

                var itemName = item.Name.Trim();
                var uris = new List<Uri>();
                var uriText = Credit.ServiceUrl.ToString();

                if (itemName.EndsWith("?", StringComparison.InvariantCultureIgnoreCase))
                    itemName = itemName.Substring(0, itemName.Length - 1);

                if (uriText.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
                    uriText = uriText.Substring(0, uriText.Length - 1);

                // First we search for the artist and track title
                if (item.Artist.IsNullOrEmptyTrimmed())
                    ub = new UriBuilder($"{uriText}?track={itemName}");
                else
                    ub = new UriBuilder($"{uriText}?artist={item.Artist}&track={itemName}");

                xmlText = await base.HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                // Deserialize the returned XML
                var loadOptions = LoadOptions.PreserveWhitespace;
                var xElement = XElement.Parse(xmlText, loadOptions);
                var idElements = xElement.DescendantNodes()
                             .OfType<XElement>()
                             .Where(n => n.Name.LocalName == "status");

                var states = from xEl in idElements
                             select xEl.Value;

                var status = states.FirstOrDefault();

                if (!status.IsNullOrEmptyTrimmed() && (status == "OK"))
                {
                    idElements = xElement.DescendantNodes()
                                 .OfType<XElement>()
                                 .Where(n => n.Name.LocalName == "url");

                    foreach (var uriTxt in from xEl in idElements
                                           select xEl.Value)
                    {
                        uris.Add(new UriBuilder(uriTxt).Uri);
                    }

                    await ExtractAllLyricTextsAsync(uris, cancellationToken, isGetAll).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                if (!ex.Message.Contains("404")) // Ignore HTTP error 404
                    throw new GeneralLyricServiceException($"{Credit.ServiceName} process failed.", isGetAll, Credit, item, ex);
            }
            catch (Exception ex)
            {
                throw new GeneralLyricServiceException($"{Credit.ServiceName} process failed.", isGetAll, Credit, item, ex);
            }

            return this;
        }


        /// <summary>
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public override async Task RefreshServiceSettingsAsync()
        {
            await base.RefreshServiceSettingsAsync().ConfigureAwait(false);

            CreateDisplayProperties();
        }

    }

}
