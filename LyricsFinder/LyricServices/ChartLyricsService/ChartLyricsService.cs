using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;
using System.Net.Http;
using System.Threading;

namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Chart Lyrics service type.
    /// </summary>
    [Serializable]
    public class ChartLyricsService : AbstractLyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLyricService"/> class.
        /// </summary>
        public ChartLyricsService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="ChartLyricsService" /> object.
        /// </returns>
        public override AbstractLyricService Clone()
        {
            var ret = new ChartLyricsService
            {
                Comment = Comment,
                Credit = Credit.Clone(),
                IsActive = IsActive,
                IsImplemented = IsImplemented,
                LyricResult = LyricResultEnum.NotProcessedYet,
                LyricsFinderData = LyricsFinderData,

            };

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">If set to <c>true</c> get all search hits; else get the first one only.</param>
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

            apiv1Soap client = null;
            var msg = string.Empty;
            var ub = new UriBuilder(Credit.ServiceUrl);

            try
            {
                await base.ProcessAsync(item, cancellationToken).ConfigureAwait(false); // Result: not found

                msg = "CreateServiceClient";
                client = CreateServiceClient<apiv1Soap>("apiv1Soap");
                msg = "SearchLyric";

                var rsp1 = client.SearchLyric(item.Artist, item.Name);

                IncrementRequestCounters(); // We need to do this here because the request is done with SOAP and not through HttpGetStringAsync

                if ((rsp1 != null) && (rsp1.Length > 0))
                {
                    foreach (var rspLyricResult in rsp1)
                    {
                        if (rspLyricResult == null) continue;
                        if (rspLyricResult.LyricId == 0) continue;

                        cancellationToken.ThrowIfCancellationRequested();

                        msg = "GetLyric";
                        var rsp2 = client.GetLyric(rspLyricResult.LyricId, rspLyricResult.LyricChecksum);

                        AddFoundLyric(rsp2.Lyric, new Uri(rsp2.LyricUrl));
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new LyricServiceCommunicationException($"{Credit.ServiceName} request failed on {msg}.", isGetAll, Credit, item, ub.Uri, ex);
            }
            catch (Exception ex)
            {
                try
                {
                    if (client != null)
                        ((ICommunicationObject)client).Abort();
                }
                catch { /* I don't care */ }

                throw new GeneralLyricServiceException($"{Credit.ServiceName} process failed on {msg}.", isGetAll, Credit, item, ex);
            }

            return this;
        }

    }

}
