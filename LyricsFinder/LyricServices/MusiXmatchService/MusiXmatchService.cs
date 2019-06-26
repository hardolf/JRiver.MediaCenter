using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;

using Newtonsoft.Json;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// MusiXmatchService lyrics service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class MusiXmatchService : AbstractLyricService
    {

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [XmlIgnore]
        public string Token { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MusiXmatchService"/> class.
        /// </summary>
        public MusiXmatchService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Extracts the result text from a Uri and adds the found lyric text to the list.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// If found, the found lyric text string; else null.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">uri</exception>
        protected override async Task<string> ExtractOneLyricTextAsync(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var ret = await base.ExtractOneLyricTextAsync(uri).ConfigureAwait(false);
            var json = await Helpers.Utility.HttpGetStringAsync(uri).ConfigureAwait(false);

            // Deserialize the returned JSON
            var lyricDyn = JsonConvert.DeserializeObject<dynamic>(json);
            var lyricDynBody = lyricDyn.message.body;
            dynamic lyricsDyn;

            try
            {
                lyricsDyn = lyricDynBody?.lyrics;
            }
            catch
            {
                return null;
            }

            ret = (string)lyricsDyn?.lyrics_body ?? string.Empty;

            var lyricUrl = (lyricsDyn?.backlink_url == null) ? null : new SerializableUri((string)lyricsDyn?.backlink_url);
            var lyricTrackingUrl = (lyricsDyn?.html_tracking_url == null) ? null : new SerializableUri((string)lyricsDyn?.html_tracking_url);
            var copyright = (string)lyricsDyn?.lyrics_copyright ?? string.Empty;

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                AddFoundLyric(ret, lyricUrl, lyricTrackingUrl, copyright);

            return ret;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object of type <see cref="MusiXmatchService" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">item</exception>
        /// <exception cref="LyricServiceCommunicationException"></exception>
        /// <exception cref="GeneralLyricServiceException"></exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem item, bool isGetAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var json = string.Empty;
            var ub = new UriBuilder($"{Credit.ServiceUrl}/track.search?apikey={Token}&q_artist={item.Artist}&q_track={item.Name}");

            try
            {
                await base.ProcessAsync(item).ConfigureAwait(false); // Result: not found

                // Example requests:
                // http://api.musixmatch.com/ws/1.1/track.search?apikey=xxxxxxxxxxxxxxxxxxxxx&q_artist=Dire%20Straits&q_track=Lions
                // http://api.musixmatch.com/ws/1.1/track.lyrics.get?apikey=xxxxxxxxxxxxxxxxxxxxx&track_id=72952844&commontrack_id=61695

                // First we search for the track
                json = await Helpers.Utility.HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                if (Exceptions.Count > 0)
                    return this;

                // Deserialize the returned JSON
                var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
                var trackDyns = searchDyn.message.body.track_list;

                // Now we get the lyrics for each search result
                var uris = new List<Uri>();

                foreach (var trackDyn in trackDyns)
                {
                    ub = new UriBuilder($"{Credit.ServiceUrl}/track.lyrics.get?apikey={Token}&track_id={trackDyn?.track?.track_id ?? 0}&commontrack_id={trackDyn?.track?.commontrack_id ?? 0}");

                    uris.Add(ub.Uri);
                }

                await ExtractAllLyricTextsAsync(uris, isGetAll).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                throw new LyricServiceCommunicationException($"{Credit.ServiceName} request failed.", isGetAll, Credit, item, ub.Uri, ex);
            }
            catch (Exception ex)
            {
                throw new GeneralLyricServiceException($"{Credit.ServiceName} process failed.", isGetAll, Credit, item, ex);
            }

            return this;
        }


        /// <summary>
        /// Refreshes the display properties.
        /// </summary>
        public override void RefreshDisplayProperties()
        {
            base.RefreshDisplayProperties();

            DisplayProperties.Add(nameof(Token), new DisplayProperty("Token", Token, null, nameof(Token), true));
        }


        /// <summary>
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public override void RefreshServiceSettings()
        {
            base.RefreshServiceSettings();

            Token = PrivateSettings.Token;

            RefreshDisplayProperties();
        }

    }

}
