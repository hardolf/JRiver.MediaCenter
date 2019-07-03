using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;

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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">lyricsFinderData
        /// or
        /// mcItem</exception>
        /// <exception cref="Exception"></exception>
        public static async Task SearchAsync(LyricsFinderDataType lyricsFinderData, McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            cancellationToken.ThrowIfCancellationRequested();

            // Set up the tasks for the search
            var tasks = new List<Task<AbstractLyricService>>();
            var services = new List<AbstractLyricService>();

            foreach (var service in lyricsFinderData.ActiveLyricServices)
            {
                var task = service.ProcessAsync(mcItem, cancellationToken, isGetAll);

                services.Add(service);
                tasks.Add(task);
            }

            if (isGetAll)
                _ = await Task.WhenAll(tasks);
            else
                _ = await Task.WhenAny(tasks);

            // Save the service counters
            lyricsFinderData.Save();
        }

    }

}
