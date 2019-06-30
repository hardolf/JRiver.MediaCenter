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
using System.Xml.Serialization;

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
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [XmlElement]
        public string Token { get; set; }


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

            var json = string.Empty;
            UriBuilder ub = null;

            try
            {
                await base.ProcessAsync(item, cancellationToken).ConfigureAwait(false); // Result: not found

                // Example GET request:
                // https://orion.apiseeds.com/api/music/lyric/dire straits/brothers in arms?apikey=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                ub = new UriBuilder($"{Credit.ServiceUrl}/{item.Artist}/{item.Name}?apikey={Token}");

                // First we search for the track
                json = await base.HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                if (Exceptions.Count > 0)
                    return this;

                // Deserialize the returned JSON
                var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
                var lyricText = (string)searchDyn.result.track.text;
                var copyright = searchDyn.result.copyright;
                var copyrightText = SharedComponents.Utility.JoinTrimmedStrings(".\r\n", (string)copyright.notice, (string)copyright.artist, (string)copyright.text) + ".";

                AddFoundLyric(lyricText, null, null, copyrightText);
            }
            catch (HttpRequestException ex)
            {
                // We assume this is a normal situation, i.e. if the song was not found, the remote server returns HTML error 404
                if (ex.Message.Contains("404"))
                    return this;
                else
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

            if (Token.IsNullOrEmptyTrimmed())
                Token = PrivateSettings.Token;

            RefreshDisplayProperties();
        }

    }

}
