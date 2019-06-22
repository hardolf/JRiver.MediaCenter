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
        /// Initializes a new instance of the <see cref="MusiXmatchService"/> class.
        /// </summary>
        public MusiXmatchService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="getAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendent object of type <see cref="MusiXmatchService" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{req.RequestUri.ToString()}\".
        /// or
        /// \"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{req.RequestUri.ToString()}\".</exception>
        /// <remarks>
        /// This routine gets the first (if any) search results from the lyric service.
        /// </remarks>
        public override async Task<AbstractLyricService> Process(McMplItem item, bool getAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            await base.Process(item).ConfigureAwait(false); // Result: not found

            // Example requests:
            // http://api.musixmatch.com/ws/1.1/track.search?apikey=xxxxxxxxxxxxxxxxxxxxx&q_artist=Dire%20Straits&q_track=Lions
            // http://api.musixmatch.com/ws/1.1/track.lyrics.get?apikey=xxxxxxxxxxxxxxxxxxxxx&track_id=72952844&commontrack_id=61695

            var credit = Credit as CreditType;

            // First we search for the track
            var urlString = $"{credit.ServiceUrl}track.search?apikey={credit.Token}&q_artist={item.Artist}&q_track={item.Name}";
            var url = new SerializableUri(Uri.EscapeUriString(urlString));
            var json = string.Empty;

            try
            {
                json = await Helpers.Utility.HttpGetStringAsync(url).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{url.ToString()}\".", ex);
            }

            // Deserialize the returned JSON
            var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
            var trackDyns = searchDyn.message.body.track_list;

            // Now we get the lyrics for each search result
            try
            {
                foreach (var trackDyn in trackDyns)
                {
                    urlString = $"{credit.ServiceUrl}track.lyrics.get?apikey={credit.Token}&track_id={trackDyn?.track?.track_id ?? 0}&commontrack_id={trackDyn?.track?.commontrack_id ?? 0}";
                    url = new SerializableUri(Uri.EscapeUriString(urlString));

                    json = await Helpers.Utility.HttpGetStringAsync(url).ConfigureAwait(false);

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
                        continue;
                    }

                    var lyricText = (string)lyricsDyn?.lyrics_body ?? string.Empty;
                    var lyricUrl = (lyricsDyn?.backlink_url == null) ? null : new SerializableUri((string)lyricsDyn?.backlink_url);
                    var lyricTrackingUrl = (lyricsDyn?.html_tracking_url == null) ? null : new SerializableUri((string)lyricsDyn?.html_tracking_url);
                    var copyright = (string)lyricsDyn?.lyrics_copyright ?? string.Empty;

                    AddFoundLyric(lyricText, lyricUrl, lyricTrackingUrl, copyright);

                    if (!getAll)
                        break;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{url.ToString()}\".", ex);
            }

            return this;
        }

    }

}
