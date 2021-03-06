﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.McWs;
using MediaCenter.SharedComponents;

using Newtonsoft.Json;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// MusiXmatchService lyrics service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class MusiXmatchService : AbstractLyricService, ILyricService
    {

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [XmlElement]
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
        /// Initializes a new instance of the <see cref="MusiXmatchService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public MusiXmatchService(MusiXmatchService source)
            : base(source)
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="MusiXmatchService" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new MusiXmatchService(this)
            {
                Token = Token
            };

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Creates the display properties.
        /// </summary>
        public override void CreateDisplayProperties()
        {
            base.CreateDisplayProperties();

            DisplayProperties.Add(nameof(Token), Token, null, isEditAllowed: true);
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
            var json = await HttpGetStringAsync(uri).ConfigureAwait(false);

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

            var lyricUrl = (lyricsDyn?.backlink_url == null) ? null : new Uri((string)lyricsDyn?.backlink_url);
            var lyricTrackingUrl = (lyricsDyn?.html_tracking_url == null) ? null : new Uri((string)lyricsDyn?.html_tracking_url);
            var copyright = (string)lyricsDyn?.lyrics_copyright ?? string.Empty;

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                await AddFoundLyric(ret, lyricUrl, lyricTrackingUrl, copyright).ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object of type <see cref="MusiXmatchService" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricServiceCommunicationException"></exception>
        /// <exception cref="LyricServiceBaseException"></exception>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            var json = string.Empty;
            var ub = new UriBuilder($"{Credit.ServiceUrl}/track.search?apikey={Token}&q_artist={mcItem.Artist}&q_track={mcItem.Name}");

            try
            {
                await base.ProcessAsync(mcItem, cancellationToken).ConfigureAwait(false); // Result: not found

                // Example requests:
                // http://api.musixmatch.com/ws/1.1/track.search?apikey=xxxxxxxxxxxxxxxxxxxxx&q_artist=Dire%20Straits&q_track=Lions
                // http://api.musixmatch.com/ws/1.1/track.lyrics.get?apikey=xxxxxxxxxxxxxxxxxxxxx&track_id=72952844&commontrack_id=61695

                // First we search for the track
                json = await HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

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

                await ExtractAllLyricTextsAsync(uris, cancellationToken, isGetAll).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
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
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public override async Task RefreshServiceSettingsAsync()
        {
            await base.RefreshServiceSettingsAsync().ConfigureAwait(false);

            CreateDisplayProperties();
        }


        /// <summary>
        /// Validates the display properties.
        /// </summary>
        public override void ValidateDisplayProperties()
        {
            base.ValidateDisplayProperties();

            // Test initialization
            var dps = new Dictionary<string, DisplayProperty>
            {
                { nameof(Token), Token }
            };
        }

    }

}
