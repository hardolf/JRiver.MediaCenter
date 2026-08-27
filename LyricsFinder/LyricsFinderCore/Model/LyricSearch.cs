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

        // Instantiate a Singleton of the Semaphore with a value of 1. 
        // This means that only 1 thread can be granted access at a time. 
        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

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
        /// <para>A failing service does not stop the search and does not throw: its exceptions are added to
        /// <paramref name="exceptions"/>, the same way whether the services were called serially or in parallel.
        /// Only the tasks that have completed when the search returns are inspected, so a service that is still
        /// running when another one finds a lyric may fail unnoticed.</para>
        /// </remarks>
        public static async Task<List<AbstractLyricService>> SearchAsync(LyricsFinderDataType lyricsFinderData, McMplItem mcItem, IList<Exception> exceptions, CancellationToken cancellationToken, bool isGetAll = false)
        {
            var ret = new List<AbstractLyricService>(); // List of service clones
            var services = new List<AbstractLyricService>(); // List of services in LyricsFinderData
            var tasks = new List<Task<AbstractLyricService>>(); // One search task per service clone

            // The arguments are checked before the try, because the finally block below uses them.
            if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));
            if (exceptions is null) throw new ArgumentNullException(nameof(exceptions));

            try
            {
                // Set up the tasks for the search
                foreach (var service in lyricsFinderData.ActiveLyricServices)
                {
                    var serviceClone = service.Clone() as AbstractLyricService ?? throw new Exception($"Error cloning service {service.Credit.ServiceName}.");
                    var task = serviceClone.ProcessAsyncWrapper(mcItem, cancellationToken, isGetAll);

                    await serviceClone.ResetTotalCountersAsync();

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
                        catch (Exception)
                        {
                            // Every failure is collected from the tasks themselves in the finally block below.
                        }
                    else
                    {
                        if (lyricsFinderData.MainData.SerialServiceRequestsDuringAutomaticSearch)
                        {
                            foreach (var task in tasks)
                            {
                                AbstractLyricService result = null;

                                try
                                {
                                    result = await task;
                                }
                                catch (Exception)
                                {
                                    // Collected in the finally block below, exactly as in the parallel path.
                                }

                                if (cancellationToken.IsCancellationRequested)
                                    break;

                                if (result?.LyricResult == LyricsResultEnum.Found)
                                    break;
                            }
                        }
                        else
                            _ = await tasks.WhenAny(t => t.Result.LyricResult == LyricsResultEnum.Found);
                    }
                }
            }
            finally
            {
                // Collect the failures of every completed task, so that a failing service is reported the same
                // way whether the search ran serially or in parallel.
                _ = tasks.CollectExceptions(exceptions);

                // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                await _semaphoreSlim.WaitAsync();

                try
                {
                    // Reload the lyric services' data
                    lyricsFinderData = LyricsFinderDataType.Load(lyricsFinderData.DataFilePath);

                    // Add the clone service counters and IsActive flag back to the original services
                    for (int i = 0; i < services.Count; i++)
                    {
                        var service = services[i];
                        var serviceClone = ret[i];
                        var reloadedService = lyricsFinderData.LyricServices.Find(s => s.Credit.ServiceName.Equals(service.Credit.ServiceName, StringComparison.CurrentCulture));

                        await service.IncrementRequestCountersAsync(serviceClone.RequestCountTotal);
                        await service.IncrementHitCountersAsync(serviceClone.HitCountTotal);

                        if (reloadedService?.IsActive ?? false)
                            service.IsActive = serviceClone.IsActive;
                    }

                }
                finally
                {
                    // Save the service counters etc.
                    try
                    {
                        await lyricsFinderData.SaveAsync();
                    }
                    finally
                    {
                        _semaphoreSlim.Release();
                    }
                }
            }

            return ret;
        }

    }

}
