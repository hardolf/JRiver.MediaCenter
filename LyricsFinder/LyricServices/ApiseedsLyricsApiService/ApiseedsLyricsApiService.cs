﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.LyricServices.Properties;
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
    public class ApiseedsLyricsApiService : AbstractLyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiseedsLyricsApiService"/> class.
        /// </summary>
        public ApiseedsLyricsApiService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="getAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// This routine gets the first (if any) search results from the lyric service.
        /// </remarks>
        public override AbstractLyricService Process(McMplItem item, bool getAll = false)
        {
            base.Process(item); // Result: not found

            // Example GET request:
            // https://orion.apiseeds.com/api/music/lyric/dire straits/brothers in arms?apikey=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            var credit = Credit as CreditType;

            // First we search for the track
            var urlString = $"{credit.ServiceUrl}{item.Artist}/{item.Name}?apikey={credit.Token}";
            var url = new SerializableUri(Uri.EscapeUriString(urlString));

            var req = WebRequest.Create(url) as HttpWebRequest;
            var json = string.Empty;
            HttpWebResponse rsp = null;

            using (rsp = req.GetResponse() as HttpWebResponse)
            {
                if (rsp == null)
                    throw new NullReferenceException("Response is null");
                if (rsp.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Server error (HTTP {rsp.StatusCode}: {rsp.StatusDescription}).");

                using (var rspStream = rsp.GetResponseStream())
                {
                    var reader = new StreamReader(rspStream, Encoding.UTF8);

                    json = reader.ReadToEnd();
                }
            }

            // Deserialize the returned JSON
            var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
            var tracks = searchDyn.message.body.track_list;

            // Now we get the lyrics for each search result
            foreach (var track in tracks)
            {
                // Deserialize the returned JSON
                var lyricDyn = JsonConvert.DeserializeObject<dynamic>(json);
                var lyricDynBody = lyricDyn.message.body;

                if ((lyricDynBody == null) || (lyricDynBody.Count == 0))
                    continue;

                var lyricDynEl = lyricDynBody.lyrics;
                var lyricText = (string)lyricDynEl.lyrics_body;

                AddFoundLyric(lyricText, lyricDynEl.backlink_url, lyricDynEl.html_tracking_url, (string)lyricDynEl.lyrics_copyright);

                if (!getAll)
                    break;
            }

            return this;
        }

    }

}
