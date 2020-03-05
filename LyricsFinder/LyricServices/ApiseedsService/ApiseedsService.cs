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

using MediaCenter.McWs;
using MediaCenter.SharedComponents;

using Newtonsoft.Json;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Apiseeds Lyrics API service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class ApiseedsService : AbstractLyricService, ILyricService
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
        /// Initializes a new instance of the <see cref="ApiseedsService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public ApiseedsService(ApiseedsService source)
            : base(source)
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="ApiseedsService" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new ApiseedsService(this)
            {
                Token = Token
            };

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Refreshes the display properties.
        /// </summary>
        public override void CreateDisplayProperties()
        {
            base.CreateDisplayProperties();

            DisplayProperties.Add(nameof(Token), Token, null, isEditAllowed: true);
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

                var itemName = item.Name.Trim();

                if (itemName.EndsWith("?", StringComparison.InvariantCultureIgnoreCase))
                    itemName = itemName.Substring(0, itemName.Length - 1);

                ub = new UriBuilder($"{Credit.ServiceUrl}/{item.Artist}/{itemName}?apikey={Token}");

                // First we search for the track
                json = await base.HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                // Deserialize the returned JSON
                var searchDyn = JsonConvert.DeserializeObject<dynamic>(json);
                var lyricText = (string)searchDyn.result.track.text;
                var copyright = searchDyn.result.copyright;
                var copyrightText = SharedComponents.Utility.JoinTrimmedStrings(".\r\n", (string)copyright.notice, (string)copyright.artist, (string)copyright.text) + ".";

                await AddFoundLyric(lyricText, null, null, copyrightText).ConfigureAwait(false);
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
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public override async Task RefreshServiceSettingsAsync()
        {
            await base.RefreshServiceSettingsAsync().ConfigureAwait(false);

            CreateDisplayProperties();
        }

    }

}
