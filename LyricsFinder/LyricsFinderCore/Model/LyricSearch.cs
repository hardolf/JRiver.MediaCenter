using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Lyric search type.
    /// </summary>
    public static class LyricSearch
    {

        /// <summary>
        /// Searches for lyrics in all lyric services.
        /// </summary>
        /// <param name="lyricsFinderData">The lyrics finder data.</param>
        /// <param name="mcItem">The Media Center item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> [get all].</param>
        /// <returns>Listof service clones.</returns>
        /// <exception cref="ArgumentNullException">lyricsFinderData
        /// or
        /// mcItem</exception>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// <para>We clone each active service before using the clone to the search.</para>
        /// <para>This is done in order to avoid duplicate lyrics during concurrent searches with the same service.</para>
        /// </remarks>
        public static async Task<List<AbstractLyricService>> SearchAsync(LyricsFinderDataType lyricsFinderData, McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            cancellationToken.ThrowIfCancellationRequested();

            // Set up the tasks for the search
            var tasks = new List<Task<AbstractLyricService>>();
            var services = new List<AbstractLyricService>();
            var ret = new List<AbstractLyricService>(); // List of service clones

            foreach (var service in lyricsFinderData.ActiveLyricServices)
            {
                var serviceClone = service.Clone();
                var task = serviceClone.ProcessAsync(mcItem, cancellationToken, isGetAll);

                services.Add(service);
                ret.Add(serviceClone);
                tasks.Add(task);
            }

            if (isGetAll)
                _ = await Task.WhenAll(tasks);
            else
                _ = await tasks.WhenAny(t => t.Result.LyricResult == LyricResultEnum.Found);

            // Add the clone service counters back to the original services
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                var serviceClone = ret[i];

                service.IncrementHitCounters(serviceClone.HitCountTotal);
                service.IncrementRequestCounters(serviceClone.RequestCountTotal);
            }

            // Save the service counters
            lyricsFinderData.Save();

            return ret;
        }

    }

}
