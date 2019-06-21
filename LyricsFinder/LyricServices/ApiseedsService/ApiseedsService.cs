using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Apiseeds Lyrics API service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class ApiseedsService : AbstractLyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiseedsService"/> class.
        /// </summary>
        public ApiseedsService()
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
        ///   <see cref="AbstractLyricService" /> descendent object of type <see cref="Stands4Service" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{req.RequestUri.ToString()}\".</exception>
        /// <remarks>
        /// This routine gets the first (if any) search results from the lyric service.
        /// </remarks>
        public override async Task<AbstractLyricService> Process(McMplItem item, bool getAll = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            await base.Process(item).ConfigureAwait(false); // Result: not found

            // Example GET request:
            // https://orion.apiseeds.com/api/music/lyric/dire straits/brothers in arms?apikey=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            var credit = Credit as CreditType;

            // First we search for the track
            var urlString = $"{credit.ServiceUrl}{item.Artist}/{item.Name}?apikey={credit.Token}";
            var url = new SerializableUri(Uri.EscapeUriString(urlString));
            var json = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    json = await client.GetStringAsync(url).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                // Is this a normal situation? i.e. if the song was not found, the remote server returns HTML error 404
                if (ex.Message.Contains("404"))
                    return this;
                else
                    throw new Exception($"\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{url.ToString()}\".", ex);
            }

            // Deserialize the returned JSON
            var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
            var lyricText = (string)searchDyn.result.track.text;
            var copyright = searchDyn.result.copyright;
            var copyrightText = SharedComponents.Utility.JoinTrimmedStrings(".\r\n", (string)copyright.notice, (string)copyright.artist, (string)copyright.text) + ".";

            AddFoundLyric(lyricText, null, null, copyrightText);

            return this;
        }

    }

}
