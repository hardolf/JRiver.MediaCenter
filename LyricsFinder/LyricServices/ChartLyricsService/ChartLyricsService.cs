using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Chart Lyrics service type.
    /// </summary>
    [Serializable]
    public class ChartLyricsService : AbstractLyricService, ILyricService
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
        /// Initializes a new instance of the <see cref="ChartLyricsService" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public ChartLyricsService(ChartLyricsService source)
            : base(source)
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="ChartLyricsService" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new ChartLyricsService(this);

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

                // We need to do the following delay and search code here because the service request is done with SOAP and not through HttpGetStringAsync
                await DelayRandomizedAsync(LyricsFinderData.MainData.DelayMilliSecondsBetweenSearches).ConfigureAwait(false);

                var rsp1 = Array.Empty<SearchLyricResult>();

                try
                {
                    rsp1 = client.SearchLyric(item.Artist, item.Name);
                }
                finally
                {
                    await IncrementRequestCountersAsync().ConfigureAwait(false);
                }

                if ((rsp1 != null) && (rsp1.Length > 0))
                {
                    foreach (var rspLyricResult in rsp1)
                    {
                        if (rspLyricResult == null) continue;
                        if (rspLyricResult.LyricId == 0) continue;

                        await DelayRandomizedAsync(LyricsFinderData.MainData.DelayMilliSecondsBetweenSearches).ConfigureAwait(false);

                        msg = "GetLyric";
                        var rsp2 = client.GetLyric(rspLyricResult.LyricId, rspLyricResult.LyricChecksum);

                        await AddFoundLyric(rsp2.Lyric, new Uri(rsp2.LyricUrl)).ConfigureAwait(false);
                    }
                }
            }
            catch (FaultException ex)
            {
                if (ex.Message.Contains("No valid words left in contains list")) // Ignore error if empty contains list
                {
                    try
                    {
                        if (client != null)
                            ((ICommunicationObject)client).Abort();
                    }
                    catch { /* I don't care */ }
                }
                else
                    throw;
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
