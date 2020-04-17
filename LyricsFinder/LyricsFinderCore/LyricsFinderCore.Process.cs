using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Forms;
using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.McWs;
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
        private async Task ConnectAsync()
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
                    await StatusMessageAsync($"Connecting to the MCWS {((cnt > 1) ? "failed " : string.Empty)}- attempt {cnt}...", true, true);

                    alive = await McRestService.GetAliveAsync();
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
                        Authentication = await McRestService.GetAuthenticationAsync();

                        _isConnectedToMc = true;
                        await StatusMessageAsync($"Connected successfully to the MCWS after {cnt} attempt(s).", true, true);
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
        public async Task InitCoreAsync()
        {
            if (_isDesignTime) return;

            var msg = string.Empty;

            try
            {
                EnableOrDisableToolStripItems(false, FileMenuItem, HelpMenuItem, ToolsMenuItem);
                await StatusMessageAsync("LyricsFinder initializes...", true, false); // Do NOT log here, before the InitLogging!

                // Init the log. This must be done as the very first thing, before trying to write to the log.
                msg = "LyricsFinder for JRiver Media Center is started" + (_isStandAlone ? " standalone." : " from Media Center.");
                await InitLoggingAsync(new[] { _logHeader, msg });

                msg = "initializing the application configuration handler";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                LyricsFinderCoreConfigurationSectionHandler.Init(Assembly.GetExecutingAssembly());

                msg = "initializing the local data";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                await InitLocalDataAsync();

                msg = "checking if private configuration is needed";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
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
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                InitKeyDownEvent();

                msg = "loading the form settings";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                LoadFormSettings();

                msg = "initializing the shortcuts";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                ShowShortcuts(_isStandAlone);

                msg = "initializing the start/stop button delegates";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                SearchAllStartStopButton.SetRunningState(false);
                SearchAllStartStopButton.Starting += SearchAllStartStopButton_StartingAsync;
                SearchAllStartStopButton.Stopping += SearchAllStartStopButton_StoppingAsync;
                ToolsSearchAllStartStopButton.SetRunningState(false);
                ToolsSearchAllStartStopButton.Starting += ToolsSearchAllStartStopButton_StartingAsync;
                ToolsSearchAllStartStopButton.Stopping += ToolsSearchAllStartStopButton_StoppingAsync;
                ToolsPlayStartStopButton.SetRunningState(false);
                ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_StartingAsync;
                ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_StoppingAsync;

                msg = "initializing the Media Center MCWS connection parameters";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                McRestService.Init(
                    LyricsFinderData.MainData.McAccessKey,
                    LyricsFinderData.MainData.McWsUrl,
                    LyricsFinderData.MainData.McWsUsername,
                    LyricsFinderData.MainData.McWsPassword);

                msg = "initializing the update check";
                await Logging.LogAsync(_progressPercentage, msg + "...", true);
                UpdateCheckTimer.Start();

                MainGridView.Select();

                _progressPercentage = 0;
                _noLyricsSearchList.AddRange(LyricsFinderData.MainData.NoLyricsSearchFilter.Split(',', ';'));

                EnableOrDisableToolStripItems(true);
                await ReloadPlaylistAsync(true);
            }
            catch (Exception ex)
            {
                EnableOrDisableToolStripItems(true);
                await StatusMessageAsync($"Error {msg} during initialization.", true, true);
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex, msg);
            }
        }


        /// <summary>
        /// Reloads the playlist.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> reconnect to MediaCenter; else do not.</param>
        /// <param name="menuItemName">Name of the menu item.</param>
        /// <returns></returns>
        private async Task ReloadPlaylistAsync(bool isReconnect = false, string menuItemName = null)
        {
            var msg = "connecting to the Media Center and/or loading the playlist";

            try
            {
                EnableOrDisableToolStripItems(false, FileMenuItem, ToolsPlayStartStopButton, ToolsSearchAllStartStopButton, SearchAllStartStopButton
                    , ToolsPlayJumpAheadLargeMenuItem, ToolsPlayJumpAheadSmallMenuItem, ToolsPlayJumpBackLargeMenuItem, ToolsPlayJumpBackSmallMenuItem);

                if (IsDataChanged)
                {
                    var result = MessageBox.Show("Data is changed and the changes will be lost if you continue.\r\n"
                        + "Do you want to continue anyway?"
                        , "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.No)
                    {
                        EnableOrDisableToolStripItems(true);
                        return;
                    }

                    if (result == DialogResult.Yes)
                        IsDataChanged = false;
                }

                if (!_isConnectedToMc || isReconnect)
                    await ConnectAsync();

                if (menuItemName.IsNullOrEmptyTrimmed())
                {
                    _currentMcPlaylist = await LoadPlaylistAsync();
                    _currentLyricsFinderPlaylist = _currentMcPlaylist.Clone(); // Don't just make a reference to _currentMcPlaylist, use a copy
                }
                else
                    _currentLyricsFinderPlaylist = await LoadPlaylistAsync(menuItemName);

                await FillDataGridAsync();

                PrepareItemStatesBeforeSearch();
                EnableOrDisableToolStripItems(true);

                _ = IsDataChanged; // Force disabling the save menu item
            }
            catch (Exception ex)
            {
                EnableOrDisableToolStripItems(true);
                EnableOrDisableToolStripItems(false, FileSelectPlaylistMenuItem, FileSaveMenuItem, ToolsPlayStartStopButton, ToolsSearchAllStartStopButton, SearchAllStartStopButton
                    , ToolsPlayJumpAheadLargeMenuItem, ToolsPlayJumpAheadSmallMenuItem, ToolsPlayJumpBackLargeMenuItem, ToolsPlayJumpBackSmallMenuItem);

                await StatusMessageAsync($"Error {msg}.", true, true);
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex, msg);
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

            var isCanceled = false;
            var isOk = false;
            var foundItemIndices = new List<int>();
            var searchItemIndices = new List<int>();
            var queue = new Queue<int>(Enumerable.Range(0, _currentLyricsFinderPlaylist.Items.Count));
            var workers = new List<Task>();

            try
            {
                _progressPercentage = 0;
                EnableOrDisableToolStripItems(false, FileMenuItem);
                await StatusMessageAsync($"Finding lyrics for the current playlist with {_currentLyricsFinderPlaylist.Items.Count} items...", true, true);
                PrepareItemStatesBeforeSearch();

                // Add the set of workers
                for (var i = 0; i < LyricsFinderData.MainData.MaxQueueLength; i++)
                {
                    workers.Add(SearchAllProcessWorkerAsync(i, queue, searchItemIndices, foundItemIndices, cancellationToken));
                }

                // Run the search on the set of workers
                await Task.WhenAll(workers);

                isCanceled = cancellationToken.IsCancellationRequested;
                isOk = true;
            }
            catch (Exception ex)
            {
                isOk = false;
                throw new Exception("Automatic search all failed.", ex);
            }
            finally
            {
                EnableOrDisableToolStripItems(true, FileMenuItem);
                SearchAllStartStopButton.SetRunningState(false);
                ToolsSearchAllStartStopButton.SetRunningState(false);

                var processedCount = (queue.Count > 0)
                    ? queue.Peek() + 1
                    : _currentLyricsFinderPlaylist.Items.Count;

                var result = (isOk)
                    ? (isCanceled)
                        ? "was canceled"
                        : "completed successfully"
                    : "failed";

                _progressPercentage = Convert.ToInt32(100 * processedCount / _currentLyricsFinderPlaylist.Items.Count);
                await StatusMessageAsync($"Finding lyrics for the current playlist {result} with {searchItemIndices.Count} items searched, "
                    + $"{foundItemIndices.Count} lyrics found and {processedCount} items processed."
                    , true, true);
            }
        }


        /// <summary>
        /// Asynchronous processes worker .
        /// </summary>
        /// <param name="workerNumber">The worker number.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="searchItemIndices">The search item indices.</param>
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
        private async Task SearchAllProcessWorkerAsync(int workerNumber, Queue<int> queue, List<int> searchItemIndices, List<int> foundItemIndices, CancellationToken cancellationToken)
        {
            if (queue == null) throw new ArgumentNullException(nameof(queue));
            if (searchItemIndices == null) throw new ArgumentNullException(nameof(searchItemIndices));
            if (foundItemIndices == null) throw new ArgumentNullException(nameof(foundItemIndices));
            if (cancellationToken == null) throw new ArgumentNullException(nameof(cancellationToken));

            var i = 0;
            var row = MainGridView.Rows[0];
            var msg = $"Process worker {workerNumber} failed. ";
            var lyricCell = row.Cells[(int)GridColumnEnum.Lyrics];
            var statusCell = row.Cells[(int)GridColumnEnum.Status];
            var lyricExceptions = new List<Exception>();

            try
            {
                while (queue.Count > 0)
                {
                    i = queue.Dequeue();

                    var found = false;

                    row = MainGridView.Rows[i];
                    msg = $"Process worker {workerNumber} failed at item {i}. ";

                    if (!int.TryParse(row.Cells[(int)GridColumnEnum.Key].Value?.ToString(), out var key))
                        throw new Exception($"{row.Cells[(int)GridColumnEnum.Key].Value} could not be parsed as an integer.");
#if DEBUG
                    var artist = row.Cells[(int)GridColumnEnum.Artist].Value?.ToString() ?? string.Empty;
                    var album = row.Cells[(int)GridColumnEnum.Album].Value?.ToString() ?? string.Empty;
                    var title = row.Cells[(int)GridColumnEnum.Title].Value?.ToString() ?? string.Empty;
#endif
                    lyricCell = row.Cells[(int)GridColumnEnum.Lyrics];
                    statusCell = row.Cells[(int)GridColumnEnum.Status];

                    var oldLyric = lyricCell.Value?.ToString() ?? string.Empty;
                    var oldStatus = statusCell.Value.ToString().ToLyricResultEnum();

                    if (cancellationToken.IsCancellationRequested)
                    {
                        if (oldStatus == LyricsResultEnum.Processing)
                            statusCell.Value = LyricsResultEnum.Canceled.ResultText();

                        break;
                    }

                    // Do we need to search this item?
                    if ((oldStatus == LyricsResultEnum.NotProcessedYet)
                        && (OverwriteMenuItem.Checked || oldLyric.IsNullOrEmptyTrimmed() || oldLyric.Contains(_noLyricsSearchList)))
                    {
                        statusCell.Value = $"{LyricsResultEnum.Processing.ResultText()}...";

                        // Try to get the first search hit
                        var resultServices = await LyricSearch.SearchAsync(LyricsFinderData, _currentLyricsFinderPlaylist.Items[key], lyricExceptions, cancellationToken, false);

                        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                        await _semaphoreSlim.WaitAsync();

                        try
                        {
                            searchItemIndices.Add(i);
                        }
                        finally
                        {
                            _semaphoreSlim.Release();
                        }

                        // Process the results
                        // The first lyric found by any service is used for each item
                        foreach (var service in resultServices)
                        {

                            if (service.LyricResult != LyricsResultEnum.Found) continue;
                            if (!service.FoundLyricList.IsNullOrEmpty())
                            {
                                found = true;

                                // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                                await _semaphoreSlim.WaitAsync();

                                try
                                {
                                    foundItemIndices.Add(i);
                                }
                                finally
                                {
                                    _semaphoreSlim.Release();
                                }

                                var newLyric = service.FoundLyricList.First().ToString();

                                lyricCell.Value = newLyric;
                                IsDataChanged = true;
                                break;
                            }
                        }

                        var processedCount = (queue.Count > 0)
                            ? queue.Peek() + 1
                            : _currentLyricsFinderPlaylist.Items.Count;

                        _progressPercentage = Convert.ToInt32(100 * processedCount / _currentLyricsFinderPlaylist.Items.Count);
                        await StatusMessageAsync($"Processed {processedCount} items, searched {searchItemIndices.Count} items and "
                            + $"found {foundItemIndices.Count} lyrics for the current playlist with {_currentLyricsFinderPlaylist.Items.Count} items..."
                            , true, true);

                        statusCell.Value = (found)
                            ? LyricsResultEnum.Found.ResultText()
                            : LyricsResultEnum.NotFound.ResultText();

                        if (lyricExceptions.Any())
                            throw new Exception("A lyric service failed.", lyricExceptions.First());
                    }
                    else if (!OverwriteMenuItem.Checked
                        && !oldLyric.IsNullOrEmptyTrimmed())
                        statusCell.Value = LyricsResultEnum.SkippedOldLyrics.ResultText();
                }
            }
            catch (Exception ex)
            {
                statusCell.Value = LyricsResultEnum.Error.ResultText();
                throw new Exception(msg, ex);
            }
        }

    }

}