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

using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// CajunLyrics Lyrics API service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class CajunLyricsService : AbstractLyricService, ILyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CajunLyricsService"/> class.
        /// </summary>
        public CajunLyricsService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CajunLyricsService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public CajunLyricsService(CajunLyricsService source)
            : base(source)
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="CajunLyricsService" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new CajunLyricsService(this);

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Extracts all lyric IDs.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <returns>List of found lyric IDs.</returns>
        private List<string> ExtractAllLyricIds(string xmlText)
        {
            var ret = new List<string>();

            // Deserialize the returned XML
            var loadOptions = LoadOptions.PreserveWhitespace;
            var xElement = XElement.Parse(xmlText, loadOptions);
            var idElements = xElement.DescendantNodes()
                         .OfType<XElement>()
                         .Where(n => n.Name.LocalName == "Id");

            ret.AddRange(from xEl in idElements
                         where (int.TryParse(xEl.Value, out int _))
                         select xEl.Value);
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
            var xmlText = await HttpGetStringAsync(uri).ConfigureAwait(false);

            // Deserialize the returned XML
            var loadOptions = LoadOptions.PreserveWhitespace;
            var xElement = XElement.Parse(xmlText, loadOptions);
            var lyrics = xElement.DescendantNodes()
                         .OfType<XElement>()
                         .Where(n => n.Name.LocalName == "Lyric");

            if (lyrics.Any() && (lyrics.First().Value != "Not found"))
                ret = lyrics.First().Value;

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                await AddFoundLyric(ret, null, null, null).ConfigureAwait(false);

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
        /// <exception cref="LyricServiceBaseException"></exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem item, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var xmlText = string.Empty;
            UriBuilder ub = null;

            try
            {
                await base.ProcessAsync(item, cancellationToken).ConfigureAwait(false); // Result: not found

                // Example GET requests:
                // http://api.cajunlyrics.com/LyricSearchList.php?artist=Bruce%20Daigrepont&title=La%20Jalouserie
                // http://api.cajunlyrics.com/LyricDirectSearch.php?artist=Bruce%20Daigrepont&title=La%20Jalouserie
                // http://api.cajunlyrics.com/LyricDirectSearch.php?ID=64

                var itemName = item.Name.Trim();
                var uris = new List<Uri>();
                var uriText = Credit.ServiceUrl.ToString();

                if (itemName.EndsWith("?", StringComparison.InvariantCultureIgnoreCase))
                    itemName = itemName.Substring(0, itemName.Length - 1);

                if (uriText.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
                    uriText = uriText.Substring(0, uriText.Length - 1);

                // First we search for the artist and track title
                ub = new UriBuilder($"{uriText}/LyricSearchList.php?artist={item.Artist}&title={itemName}");

                xmlText = await HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                // Deserialize the returned XML
                var lyricIds = ExtractAllLyricIds(xmlText);

                // If no results where found, repeat the search with the track title alone (no artist)
                if (IsActive && !LyricsFinderData.MainData.StrictSearchOnly && !lyricIds.Any())
                {
                    ub = new UriBuilder($"{uriText}/LyricSearchList.php?title={itemName}");

                    xmlText = await HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                    // Deserialize the returned XML
                    lyricIds = ExtractAllLyricIds(xmlText);
                }

                foreach (var lyricId in lyricIds)
                {
                    ub = new UriBuilder($"{uriText}/LyricDirectSearch.php?id={lyricId}");
                    uris.Add(ub.Uri);
                }

                await ExtractAllLyricTextsAsync(uris, cancellationToken, isGetAll).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new LyricServiceBaseException($"{Credit.ServiceName} process failed.", isGetAll, Credit, item, ex);
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
