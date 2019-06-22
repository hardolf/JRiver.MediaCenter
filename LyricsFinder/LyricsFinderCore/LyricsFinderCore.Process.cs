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
            var maxConnectAttempts = LyricsFinderCoreConfigurationSectionHandler.McWsConnectAttempts;
            var url = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl;
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
            } while (cnt < maxConnectAttempts);

            // If we got the MCWS connection, let's try to authenticate
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
                StatusMessage("LyricsFinder initializes...", true, false);

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
                ToolsSearchAllStartStopButton.Starting += ToolsSearchAllStartStopButton_StartingAsync;
                ToolsSearchAllStartStopButton.Stopping += ToolsSearchAllStartStopButton_StoppingAsync;
                SearchAllStartStopButton.Starting += StartStopButton_StartingAsync;
                SearchAllStartStopButton.Stopping += StartStopButton_StoppingAsync;

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

                EnableOrDisableMenuItems(true);
                await ReloadPlaylist(true);
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
                EnableOrDisableMenuItems(false, FileReloadMenuItem, FileSaveMenuItem, FileSelectPlaylistMenuItem);

                if (!_isConnectedToMc || isReconnect)
                    await Connect();

                _currentPlaylist = await LoadPlaylist(menuItemName);
                await FillDataGrid();

                EnableOrDisableMenuItems(true);
            }
            catch (Exception ex)
            {
                EnableOrDisableMenuItems(true);
                EnableOrDisableMenuItems(false, FileSaveMenuItem);

                StatusMessage($"Error {msg}.", true, true);
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex, msg);
            }
        }

    }

}