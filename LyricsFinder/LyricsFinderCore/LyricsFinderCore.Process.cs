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
        /// <returns><see cref="Task"/> object.</returns>
        /// <exception cref="System.Exception">The connected MediaCenter Web Service has AccessKey \"{alive.AccessKey}\" but \"{expectedAccessKey}\" was expected.
        /// or
        /// Failed to connect to MediaCenter Web Service \"{url}\". The service is {msg}.
        /// or
        /// Error in MediaCenter routine \"{typeNs}.{typeName}.{routineName}\".</exception>
        /// <exception cref="Exception">The connected MediaCenter Web Service has AccessKey \"{alive.AccessKey}\", ...
        /// or
        /// Failed to connect to MediaCenter Web Service \"{url}
        /// or
        /// Error in MediaCenter routine \"{typeNs}.{typeName}.{routineName}</exception>
        private async Task Connect()
        {
            var cnt = 0;
            var maxConnectAttempts = LyricsFinderData.MainData.MaxMcWsConnectAttempts;
            var url = LyricsFinderData.MainData.McWsUrl;
            McAliveResponse alive = null;

            _isConnectedToMc = false;

            // Try to get the MCWS connection
            do
            {
                cnt++;

                try
                {
                    StatusMessage($"Connecting to the MCWS {((cnt > 1) ? "failed " : string.Empty)}- attempt {cnt}...", true, true);

                    alive = await McRestService.GetAlive();
                }
                catch
                {
                    if (cnt < maxConnectAttempts)
                    {
                        // Ignore and wait ½ sec.
                        await Task.Delay(500);
                    }
                    else
                        throw;
                }
            } while (((alive == null) || (!alive.IsOk))
                && (cnt < maxConnectAttempts));

            // If we got the MCWS connection, let's try to authenticate
            if ((alive != null) && alive.IsOk)
            {
                var expectedAccessKey = LyricsFinderData.MainData.McAccessKey;

                if (alive.AccessKey == expectedAccessKey)
                {
                    try
                    {
                        Authentication = await McRestService.GetAuthentication();

                        _isConnectedToMc = true;
                        StatusMessage($"Connected successfully to the MCWS after {cnt} attempt(s).", true, true);
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    // We probably never get here
                    throw new Exception($"The connected JRiver MediaCenter Web Service (MCWS) has AccessKey \"{alive.AccessKey}\" but \"{expectedAccessKey}\" was expected.");
                }
            }
            else
            {
                // We probably never get here either
                var msg = (alive == null) ? "not alive" : "alive";

                if (alive != null)
                    msg += (alive.IsOk) ? " and OK" : " but not OK";

                throw new Exception($"Error connecting to the JRiver Media Center Web Service (MCWS) \"{url}\". The service is {msg}.");
            }
        }


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task InitCore()
        {
            if (_isDesignTime) return;

            var msg = string.Empty;

            try
            {
                EnableOrDisableMenuItems(false, FileMenuItem, HelpMenuItem, ToolsMenuItem);
                StatusMessage("LyricsFinder initializes...", true, false); // Do NOT log here, before the InitLogging!

                // Init the log. This must be done as the very first thing, before trying to write to the log.
                msg = "LyricsFinder for JRiver Media Center is started" + (_isStandAlone ? " standalone." : " from Media Center.");
                InitLogging(new[] { _logHeader, msg });

                msg = "initializing the application configuration handler";
                Logging.Log(_progressPercentage, msg + "...", true);
                LyricsFinderCoreConfigurationSectionHandler.Init(Assembly.GetExecutingAssembly());

                msg = "initializing the local data";
                Logging.Log(_progressPercentage, msg + "...", true);
                InitLocalData();

                msg = "initializing the private configuration handler";
                Logging.Log(_progressPercentage, msg + "...", true);
                LyricsFinderCorePrivateConfigurationSectionHandler.Init(Assembly.GetExecutingAssembly(), DataDirectory);

                msg = "checking if private configuration is needed";
                Logging.Log(_progressPercentage, msg + "...", true);
                if ((LyricsFinderData.MainData == null)
                    || LyricsFinderData.MainData.McAccessKey.IsNullOrEmptyTrimmed()
                    || LyricsFinderData.MainData.McWsUrl.IsNullOrEmptyTrimmed()
                    || LyricsFinderData.MainData.McWsUsername.IsNullOrEmptyTrimmed()
                    || LyricsFinderData.MainData.McWsPassword.IsNullOrEmptyTrimmed())
                {
                    using (var frm = new OptionForm("The LyricsFinder is not configured yet", LyricsFinderData))
                    {
                        frm.ShowDialog(this);
                    }
                }

                msg = "initializing the key events";
                Logging.Log(_progressPercentage, msg + "...", true);
                InitKeyDownEvent();

                msg = "loading the form settings";
                Logging.Log(_progressPercentage, msg + "...", true);
                LoadFormSettings();

                msg = "initializing the shortcuts";
                Logging.Log(_progressPercentage, msg + "...", true);
                ShowShortcuts(_isStandAlone);

                msg = "initializing the start/stop button delegates";
                Logging.Log(_progressPercentage, msg + "...", true);
                SearchAllStartStopButton.SetRunningState(false);
                SearchAllStartStopButton.Starting += SearchAllStartStopButton_Starting;
                SearchAllStartStopButton.Stopping += SearchAllStartStopButton_Stopping;
                ToolsSearchAllStartStopButton.SetRunningState(false);
                ToolsSearchAllStartStopButton.Starting += ToolsSearchAllStartStopButton_Starting;
                ToolsSearchAllStartStopButton.Stopping += ToolsSearchAllStartStopButton_Stopping;
                ToolsPlayStartStopButton.SetRunningState(false);
                ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_StartingAsync;
                ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_StoppingAsync;

                msg = "initializing the Media Center MCWS connection parameters";
                Logging.Log(_progressPercentage, msg + "...", true);
                McRestService.Init(
                    LyricsFinderData.MainData.McAccessKey,
                    LyricsFinderData.MainData.McWsUrl,
                    LyricsFinderData.MainData.McWsUsername,
                    LyricsFinderData.MainData.McWsPassword);

                msg = "initializing the update check";
                Logging.Log(_progressPercentage, msg + "...", true);
                UpdateCheckTimer.Start();

                MainGridView.Select();

                _progressPercentage = 0;
                _noLyricsSearchList.AddRange(LyricsFinderData.MainData.NoLyricsSearchFilter.Split(',', ';'));

                EnableOrDisableMenuItems(true);
                await ReloadPlaylist(true);

                CleanupObsoleteConfigurationFiles();
            }
            catch (Exception ex)
            {
                EnableOrDisableMenuItems(true);
                StatusMessage($"Error {msg} during initialization.", true, true);
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex, msg);
            }
        }


        /// <summary>
        /// Reloads the playlist.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> reconnect to MediaCenter; else do not.</param>
        /// <param name="menuItemName">Name of the menu item.</param>
        /// <returns></returns>
        private async Task ReloadPlaylist(bool isReconnect = false, string menuItemName = null)
        {
            var msg = "connecting to the Media Center and/or loading the playlist";

            try
            {
                EnableOrDisableToolStripItems(false, FileReloadMenuItem, FileSaveMenuItem, FileSelectPlaylistMenuItem, SearchAllStartStopButton);

                if (IsDataChanged)
                    throw new Exception("The item data is changed, you should save it before loading playlist.");

                if (!_isConnectedToMc || isReconnect)
                    await Connect();

                _currentPlaylist = (menuItemName.IsNullOrEmptyTrimmed())
                    ? await LoadPlaylist()
                    : await LoadPlaylist(menuItemName);

                await FillDataGrid();

                ResetItemStates();
                EnableOrDisableToolStripItems(true);
            }
            catch (Exception ex)
            {
                EnableOrDisableToolStripItems(true);
                EnableOrDisableToolStripItems(false, FileSaveMenuItem);

                StatusMessage($"Error {msg}.", true, true);
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex, msg);
            }
        }


        /// <summary>
        /// Tries to Find lyrics for all items in the current playlist.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This routine gets the first (if any) search results for each song from the lyric services.
        /// </remarks>
        private async Task SearchAllProcessAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken == null) throw new ArgumentNullException(nameof(cancellationToken));

            var isOk = false;
            var foundItemIndices = new List<int>();
            var queue = new Queue<int>(Enumerable.Range(0, _currentPlaylist.Items.Count));
            var workers = new List<Task>();

            try
            {
                _progressPercentage = 0;
                StatusMessage($"Finding lyrics for the current playlist with {_currentPlaylist.Items.Count} items...", true, true);
                ResetItemStates();
                cancellationToken.ThrowIfCancellationRequested();

                // Add the set of workers
                for (var i = 0; i < LyricsFinderData.MainData.MaxQueueLength; i++)
                {
                    workers.Add(SearchAllProcessWorkerAsync(i, queue, foundItemIndices, cancellationToken));
                }

                // Run the search on the set of workers
                await Task.WhenAll(workers);

                isOk = true;
            }
            finally
            {
                SearchAllStartStopButton.SetRunningState(false);
                ToolsSearchAllStartStopButton.SetRunningState(false);

                var processedCount = (queue.Count > 0)
                    ? queue.Peek() + 1
                    : _currentPlaylist.Items.Count;

                var result = (isOk)
                    ? (cancellationToken.IsCancellationRequested)
                        ? "was canceled"
                        : "completed successfully"
                    : "failed";

                _progressPercentage = Convert.ToInt32(100 * processedCount / _currentPlaylist.Items.Count);
                StatusMessage($"Finding lyrics for the current playlist {result} with {foundItemIndices.Count} lyrics found and {processedCount} processed.", true, true);
            }
        }


        /// <summary>
        /// Asynchronous processes worker .
        /// </summary>
        /// <param name="workerNumber">The worker number.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="foundItemIndices">The found item indices.</param>
        /// <param name="cancellationToken">The cancellation token source.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">queue
        /// or
        /// foundItemIndices
        /// or
        /// foundItemIndices</exception>
        /// <exception cref="Exception">Process worker failed at item {i}, Artist \"{artist}\", Album \"{album}\" and Title \"{title}\": {ex.Message}</exception>
        /// <exception cref="System.ArgumentNullException">queue
        /// or
        /// foundItemIndices
        /// or
        /// cancellationTokenSource</exception>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="LyricServiceCommunicationException"></exception>
        private async Task SearchAllProcessWorkerAsync(int workerNumber, Queue<int> queue, List<int> foundItemIndices, CancellationToken cancellationToken)
        {
            if (queue == null) throw new ArgumentNullException(nameof(queue));
            if (foundItemIndices == null) throw new ArgumentNullException(nameof(foundItemIndices));
            if (cancellationToken == null) throw new ArgumentNullException(nameof(cancellationToken));

            var i = 0;
            var row = MainGridView.Rows[0];
            var msg = $"Process worker {workerNumber} failed. ";

            try
            {
                while (queue.Count > 0)
                {
                    var found = false;

                    cancellationToken.ThrowIfCancellationRequested();
                    i = queue.Dequeue();
                    row = MainGridView.Rows[i];
                    msg = $"Process worker {workerNumber} failed at item {i}. ";

                    if (!int.TryParse(row.Cells[(int)GridColumnEnum.Key].Value?.ToString(), out var key))
                        throw new Exception($"{row.Cells[(int)GridColumnEnum.Key].Value} could not be parsed as an integer.");

                    var artist = row.Cells[(int)GridColumnEnum.Artist].Value?.ToString() ?? string.Empty;
                    var album = row.Cells[(int)GridColumnEnum.Album].Value?.ToString() ?? string.Empty;
                    var title = row.Cells[(int)GridColumnEnum.Title].Value?.ToString() ?? string.Empty;
                    var oldLyric = row.Cells[(int)GridColumnEnum.Lyrics].Value?.ToString() ?? string.Empty;

                    if (OverwriteMenuItem.Checked || oldLyric.IsNullOrEmptyTrimmed() || oldLyric.Contains(_noLyricsSearchList))
                    {
                        row.Cells[(int)GridColumnEnum.Status].Value = $"{LyricResultEnum.Processing.ResultText()}...";

                        // Try to get the first search hit
                        await LyricSearch.Search(LyricsFinderData, _currentPlaylist.Items[key], cancellationToken, false);

                        // Process the results
                        // The first lyric found by any service is used for each item
                        foreach (var service in LyricsFinderData.ActiveLyricServices)
                        {

                            if (service.LyricResult != LyricResultEnum.Found) continue;
                            if (!service.FoundLyricList.IsNullOrEmpty())
                            {
                                found = true;
                                foundItemIndices.Add(i);
                                row.Cells[(int)GridColumnEnum.Lyrics].Value = service.FoundLyricList.First().ToString();
                                IsDataChanged = true;
                                break;
                            }
                        }

                        var processedCount = (queue.Count > 0)
                            ? queue.Peek() + 1
                            : _currentPlaylist.Items.Count;

                        _progressPercentage = Convert.ToInt32(100 * processedCount / _currentPlaylist.Items.Count);
                        StatusMessage($"Processed {processedCount} items and found {foundItemIndices.Count} lyrics for the current playlist with {_currentPlaylist.Items.Count} items...", true, true);

                        row.Cells[(int)GridColumnEnum.Status].Value = (found)
                            ? LyricResultEnum.Found.ResultText()
                            : LyricResultEnum.NotFound.ResultText();
                    }
                    else if (!OverwriteMenuItem.Checked
                        && !oldLyric.IsNullOrEmptyTrimmed())
                        row.Cells[(int)GridColumnEnum.Status].Value = LyricResultEnum.SkippedOldLyrics.ResultText();
                }
            }
            catch (OperationCanceledException)
            {
                row.Cells[(int)GridColumnEnum.Status].Value = LyricResultEnum.Canceled.ResultText();
                throw;
            }
            catch (Exception ex)
            {
                row.Cells[(int)GridColumnEnum.Status].Value = LyricResultEnum.Error.ResultText();
                throw new Exception(msg, ex);
            }
        }

    }

}