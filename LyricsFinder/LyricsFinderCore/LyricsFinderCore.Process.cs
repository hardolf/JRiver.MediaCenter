using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    partial class LyricsFinderCore
    {

        /************************************/
        /***** Private process routines *****/
        /************************************/

        /// <summary>
        /// Connects to the MediaCenter.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <exception cref="ArgumentNullException">worker</exception>
        /// <exception cref="Exception">The connected MediaCenter Web Service has AccessKey \"{alive.AccessKey}\", ...
        /// or
        /// Failed to connect to MediaCenter Web Service \"{url}
        /// or
        /// Error in MediaCenter routine \"{typeNs}.{typeName}.{routineName}</exception>
        private void Connect(BackgroundWorker worker)
        {
            if (worker == null)
                throw new ArgumentNullException(nameof(worker));

            var workerState = new WorkerUserState();

            try
            {
                // ErrorTest();

                var url = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl;
                var connAttempts = Properties.Settings.Default.McWebServiceConnectAttempts;
                McAliveResponse alive = null;

                workerState.Message = $"Connecting to MediaCenter Web Service...";

                if (!worker.CancellationPending)
                    worker.ReportProgress(0, workerState);

                // Try to get the MC WS connection
                for (int i = 0; i < connAttempts; i++)
                {
                    try
                    {
                        alive = McRestService.GetAlive();

                        if (alive != null)
                            break;
                    }
                    catch (Exception)
                    {
                        // Ignore and wait ½ sec.
                        Thread.Sleep(500);
                    }
                }

                if ((alive != null) && alive.IsOk)
                {
                    var expectedAccessKey = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceAccessKey;

                    if (alive.AccessKey == expectedAccessKey)
                        Authentication = McRestService.GetAuthentication();
                    else
                        throw new Exception($"The connected MediaCenter Web Service has AccessKey \"{alive.AccessKey}\" but \"{expectedAccessKey}\" was expected.");
                }
                else
                {
                    var msg = (alive == null) ? "not alive" : "alive";

                    if (alive != null)
                        msg += (alive.IsOk) ? " and OK" : " but not OK";

                    throw new Exception($"Failed to connect to MediaCenter Web Service \"{url}\". The service is {msg}.");
                }

                _isConnectedToMc = true;

                // Get the current playlist items
                workerState.Message = $"Collecting the current playlist...";

                if (!worker.CancellationPending)
                    worker.ReportProgress(0, workerState);

                // Get the current Media Center info
                McInfo();
                var playList = McRestService.GetPlayList();

                workerState.Message = $"Connected to MediaCenter, the current playlist has {playList.Items.Count} items.";
                workerState.Items = playList.Items;
                workerState.CurrentItemIndex = (_playingIndex >= 0) ? _playingIndex : 0;

                if (!worker.CancellationPending)
                    worker.ReportProgress(0, workerState);

                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                var type = this.GetType();
                var typeNs = type?.Namespace ?? "?";
                var typeName = type?.Name ?? "?";
                var routineName = MethodBase.GetCurrentMethod()?.Name ?? "?";

                throw new Exception($"Error in MediaCenter routine \"{typeNs}.{typeName}.{routineName}\".", ex);
            }
        }


        /// <summary>
        /// Processes the current MediaCenter playlist items.
        /// </summary>
        private void Process(BackgroundWorker worker, DoWorkEventArgs e)
        {
            var isOk = false;
            var foundCount = 0;
            var lastIdx = 0;
            var workerState = new WorkerUserState();

            try
            {
                // ErrorTest();

                // Get the current playlist items
                workerState.Message = $"Collecting lyrics for the current playlist...";

                if (!worker.CancellationPending)
                    worker.ReportProgress(0, workerState);

                var playList = McRestService.GetPlayList();

                // Iterate the music file items
                workerState.Message = $"Finding lyrics for the current playlist with {playList.Items.Count} items...";
                workerState.Items = playList.Items;
                workerState.CurrentItemIndex = 0;
                workerState.OverwriteLyrics = OverwriteMenuItem.Checked;

                if (!worker.CancellationPending)
                    worker.ReportProgress(0, workerState);

                foreach (var kvp in playList.Items)
                {
                    workerState.CurrentItem = kvp.Value;

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        workerState.CurrentItemIndex = lastIdx++; // workerState.CurrentItemIndex is always 1 less thant the lastIdx
                        _progressPercentage = Convert.ToInt32(100 * lastIdx / playList.Items.Count);

                        if (ProcessItem(worker, workerState))
                            foundCount++;
                    }
                }

                isOk = true;

                if (!e.Cancel)
                    _progressPercentage = 100;

                LyricsFinderData.Save();
            }
            catch (LyricsQuotaExceededException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var type = this.GetType();
                var typeNs = type?.Namespace ?? "?";
                var typeName = type?.Name ?? "?";
                var routineName = MethodBase.GetCurrentMethod()?.Name ?? "?";

                isOk = false;

                throw new Exception($"Error in MediaCenter routine \"{typeNs}.{typeName}.{routineName}\".", ex);
            }
            finally
            {
                var result = (isOk)
                    ? (e.Cancel)
                        ? "was canceled"
                        : "completed successfully"
                    : "failed";

                workerState.Message = $"Finding lyrics for the current playlist {result} with {foundCount} lyrics found.";

                if (!worker.CancellationPending)
                    worker.ReportProgress(_progressPercentage, workerState);
            }
        }


        private bool ProcessItem(BackgroundWorker worker, WorkerUserState workerState)
        {
            var item = workerState.CurrentItem;
            var itemText = $"\"{item.Artist}\" - \"{item.Album}\" - \"{item.Name}\"";
            var ret = false;

            // Do we only look at items with no lyrics?
            if (workerState.OverwriteLyrics || item.Lyrics.IsNullOrEmptyTrimmed() || item.Lyrics.Contains(_noLyricsSearchList))
                workerState.LyricsStatus = LyricResultEnum.NotProcessedYet;
            else
                workerState.LyricsStatus = LyricResultEnum.SkippedOldLyrics;

            var doSearch = !(new[] { LyricResultEnum.Found, LyricResultEnum.ManuallyEdited, LyricResultEnum.SkippedOldLyrics }).Contains(workerState.LyricsStatus);

            if (doSearch)
            {
                // Search for lyrics in each service and stop if lyrics is found
                foreach (var service in LyricsFinderData.Services)
                {
                    if (!service.IsImplemented || !service.IsActive || service.IsQuotaExceeded) continue;

                    service.Process(item);

                    workerState.LyricsStatus = service.LyricResult;
                    workerState.Message = $"{service.LyricResultMessage.PadRight(19, ' ')}: {itemText}";
                    workerState.LyricsTextList.Clear();

                    if (service.LyricResult == LyricResultEnum.Found)
                    {
                        foreach (var foundLyrics in service.FoundLyricList)
                        {
                            var txt = foundLyrics.ToString();

                            workerState.LyricsTextList.Add(txt);
                        }

                        ret = true;
                        break;
                    }
                }
            }
            else
            {
                workerState.Message = $"{workerState.LyricsStatus.ResultText().PadRight(19, ' ')}: {itemText}";
            }

            if (!worker.CancellationPending)
                worker.ReportProgress(_progressPercentage, workerState);

            // We may need this in order to give the datagrid enough time to update (don't know why...)
            Thread.Sleep(50);

            return ret;
        }

    }

}