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
            var connectAttempts = 0;
            var maxConnectAttempts = LyricsFinderCoreConfigurationSectionHandler.McWsConnectAttempts;
            var url = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl;
            McAliveResponse alive = null;

            _isConnectedToMc = false;
            _statusWarning = string.Empty;

            // Try to get the MC WS connection
            do
            {
                connectAttempts++;

                try
                {
                    alive = await McRestService.GetAlive();
                }
                catch
                {
                    if (connectAttempts < maxConnectAttempts)
                    {
                        await StatusMessage($"Connection to the MCWS failed - retrying {connectAttempts}...", true, true);

                        // Ignore and wait ½ sec.
                        // Do not use await or Task.Delay here!
                        Thread.Sleep(500);
                    }
                    else
                        throw;
                }
            } while (connectAttempts < maxConnectAttempts);

            if ((alive != null) && alive.IsOk)
            {
                var expectedAccessKey = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceAccessKey;

                if (alive.AccessKey == expectedAccessKey)
                {
                    try
                    {
                        Authentication = await McRestService.GetAuthentication();

                        _isConnectedToMc = true;
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    throw new Exception($"The connected JRiver MediaCenter Web Service (MCWS) has AccessKey \"{alive.AccessKey}\" but \"{expectedAccessKey}\" was expected.");
                }
            }
            else
            {
                // We probably never get here
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
                _statusWarning = string.Empty;

                await StatusMessage("LyricsFinder initializes...", true, false);

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
                if (!(Model.Helpers.Utility.IsPrivateSettingInitialized(LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceAccessKey)
                    && Model.Helpers.Utility.IsPrivateSettingInitialized(LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl)
                    && Model.Helpers.Utility.IsPrivateSettingInitialized(LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUserName)
                    && Model.Helpers.Utility.IsPrivateSettingInitialized(LyricsFinderCorePrivateConfigurationSectionHandler.McWebServicePassword)))
                {
                    using (var frm = new OptionForm("The LyricsFinder is not configured yet"))
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
                ToolsSearchAllStartStopButton.Starting += ToolsSearchAllStartStopButton_Starting;
                ToolsSearchAllStartStopButton.Stopping += ToolsSearchAllStartStopButton_Stopping;
                SearchAllStartStopButton.Starting += StartStopButton_Starting;
                SearchAllStartStopButton.Stopping += StartStopButton_Stopping;

                msg = "initializing the Media Center MCWS connection parameters";
                Logging.Log(_progressPercentage, msg + "...", true);
                McRestService.Init(
                    LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceAccessKey,
                    LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl,
                    LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUserName,
                    LyricsFinderCorePrivateConfigurationSectionHandler.McWebServicePassword);

                msg = "initializing the update check";
                Logging.Log(_progressPercentage, msg + "...", true);
                UpdateCheckTimer.Start();

                MainDataGridView.Select();

                _progressPercentage = 0;
                _noLyricsSearchList.AddRange(LyricsFinderCoreConfigurationSectionHandler.McNoLyricsSearchList.Split(',', ';'));

                msg = "connecting to the MediaCenter Web Service (MCWS)";
                await StatusMessage(msg + "...", true, true);
                await Connect();

                msg = "loading the current \"Playing Now\" playlist";
                await StatusMessage(msg + "...", true, true);
                await LoadCurrentPlaylist();
                await FillDataGrid();
            }
            catch (Exception ex)
            {
                await StatusMessage($"Error {msg} during initialization.", true, true);
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex, msg);
            }
        }


        /// <summary>
        /// Loads the current playlist.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        private async Task LoadCurrentPlaylist()
        {
            await StatusMessage("Collecting the current playlist...");

            if ((_currentPlaylist == null) || (_currentPlaylist.Id == 0))
                _currentPlaylist = await McRestService.GetPlayNowList().ConfigureAwait(false);
            else
                _currentPlaylist = await McRestService.GetPlaylistFiles(_currentPlaylist.Id, _currentPlaylist.Name).ConfigureAwait(false);

            await StatusMessage((_currentPlaylist.Name.IsNullOrEmptyTrimmed())
                ? $"Connected to MediaCenter, the current playlist has {_currentPlaylist.Items.Count} items."
                : $"Connected to MediaCenter, the current playlist \"{_currentPlaylist.Name}\" has {_currentPlaylist.Items.Count} items.");

            //TODO: BackgroundWorker: workerState.CurrentItemIndex = (_playingIndex >= 0) ? _playingIndex : 0;
        }


        /*
        /// <summary>
        /// Processes the current MediaCenter playlist items.
        /// </summary>
        private async Task Process(BackgroundWorker worker, DoWorkEventArgs e)
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

                var playList = await McRestService.GetPlayNowList().ConfigureAwait(false);

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

                _progressPercentage = 100;
                isOk = true;
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
                var routineName = SharedComponents.Utility.GetActualAsyncMethodName()?.Name ?? "?";

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
        */


        /*
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
        */

    }

}