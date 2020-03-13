using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Lyric search type.
    /// </summary>
    [ComVisible(false)]
    public static class LyricSearch
    {

        /// <summary>
        /// Searches for lyrics in all lyric services.
        /// </summary>
        /// <param name="lyricsFinderData">The lyrics finder data.</param>
        /// <param name="mcItem">The Media Center item.</param>
        /// <param name="exceptions">The exceptions.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> [get all].</param>
        /// <returns>
        /// Listof service clones.
        /// </returns>
        /// <exception cref="ArgumentNullException">lyricsFinderData
        /// or
        /// mcItem</exception>
        /// <exception cref="Exception">Error cloning service {service.Credit.ServiceName}.</exception>
        /// <remarks>
        /// <para>We clone each active service before using the clone to the search.</para>
        /// <para>This is done in order to avoid duplicate lyrics during concurrent searches with the same service.</para>
        /// </remarks>
        public static async Task<List<AbstractLyricService>> SearchAsync(LyricsFinderDataType lyricsFinderData, McMplItem mcItem, IList<Exception> exceptions, CancellationToken cancellationToken, bool isGetAll = false)
        {
            var ret = new List<AbstractLyricService>(); // List of service clones
            var services = new List<AbstractLyricService>(); // List of services in LyricsFinderData
            LyricServiceBaseException lyricServiceException = null;

            try
            {
                if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
                if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));
                if (exceptions is null) throw new ArgumentNullException(nameof(exceptions));

                // Set up the tasks for the search
                var tasks = new List<Task<AbstractLyricService>>();

                foreach (var service in lyricsFinderData.ActiveLyricServices)
                {
                    var serviceClone = service.Clone() as AbstractLyricService ?? throw new Exception($"Error cloning service {service.Credit.ServiceName}.");
                    var task = serviceClone.ProcessAsyncWrapper(mcItem, cancellationToken, isGetAll);

                    services.Add(service);
                    ret.Add(serviceClone);
                    tasks.Add(task);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    if (isGetAll)
                        try
                        {
                            _ = await Task.WhenAll(tasks);
                        }
                        catch (LyricServiceBaseException ex)
                        {
                            lyricServiceException = ex;
                        }
                    else
                    {
                        if (lyricsFinderData.MainData.SerialServiceRequestsDuringAutomaticSearch)
                        {
                            foreach (var task in tasks)
                            {
                                try
                                {
                                    _ = await task;
                                }
                                catch (LyricServiceBaseException ex)
                                {
                                    lyricServiceException = ex;
                                }

                                if (task.Result.LyricResult == LyricsResultEnum.Found)
                                    break;
                            }
                        }
                        else
                        {
                            try
                            {
                                _ = await tasks.WhenAny(t => t.Result.LyricResult == LyricsResultEnum.Found);
                            }
                            catch (LyricServiceBaseException ex)
                            {
                                lyricServiceException = ex;
                            }
                        }
                    }
                }
            }
            finally
            {
                try
                {
                    // Add the clone service counters and IsActive flag back to the original services
                    for (int i = 0; i < services.Count; i++)
                    {
                        var service = services[i];
                        var serviceClone = ret[i];

                        await service.IncrementRequestCountersAsync(serviceClone.RequestCountTotal - service.RequestCountTotal);
                        await service.IncrementHitCountersAsync(serviceClone.HitCountTotal - service.HitCountTotal);
                        service.IsActive = serviceClone.IsActive;
                    }

                }
                finally
                {
                    // Save the service counters etc.
                    lyricsFinderData.SaveAsync();
                }
            }

            if (lyricServiceException != null)
                exceptions.Add(lyricServiceException);

            return ret;
        }

    }

}
