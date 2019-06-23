using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// lyricsFinderData
        /// or
        /// mcItem
        /// </exception>
        /// <exception cref="Exception"></exception>
        public static async Task Search(LyricsFinderDataType lyricsFinderData, McMplItem mcItem)
        {
            if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            // Set up the tasks for the search
            var tasks = new List<Task<AbstractLyricService>>();
            var services = new List<AbstractLyricService>();

            foreach (var service in lyricsFinderData.ActiveServices)
            {
                var task = service.ProcessAsync(mcItem, true);

                services.Add(service);
                tasks.Add(task);
            }

            // Run the search and wait for all the results
            List<Exception> exceptions = new List<Exception>();

            try
            {
                var resultServices = await Task.WhenAll(tasks);

                foreach (var service in resultServices)
                {
                    exceptions.AddRange(service.Exceptions);
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            // Save the service counters
            lyricsFinderData.Save();

            if (exceptions.Count > 0)
            {
                var msg = new StringBuilder();

                msg.AppendLine($"Lyrics search for Artist \"{mcItem.Artist}\",  Album \"{mcItem.Album}\" and Song \"{mcItem.Name}\" failed with {exceptions.Count} errors:");

                for (int i = 0; i < exceptions.Count; i++)
                {
                    if (i > 0)
                        msg.AppendLine();

                    msg.Append(exceptions[i].Message);
                }

                throw new Exception(msg.ToString());
            }
        }

    }

}
