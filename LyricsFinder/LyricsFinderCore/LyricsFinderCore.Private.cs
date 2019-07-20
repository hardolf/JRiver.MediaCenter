using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;
using log4net.Util;

using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    partial class LyricsFinderCore
    {

        /***********************************/
        /***** Private assembly-wide   *****/
        /***** constants and variables *****/
        /***********************************/

        const string _menuNameDelim = "_";

        private bool _isConnectedToMc = false;
        private bool _isDesignTime = false;
        private bool _isGridDataChanged = false; // Do not change this in code, always change the <c>IsDataChanged</c> property instead.
        private bool _isOnHandleDestroyedDone = false;
        private readonly bool _isStandAlone = true;

        private int _currentMouseColumnIndex = -1;
        private int _currentMouseRowIndex = -1;
        private int _playingIndex = -1;
        private int _playingKey = -1;
        private int _selectedKey = -1;
        private static int _progressPercentage = -1;
        private static int _mcStatusIntervalNormal = 500; // ½ second
        private static int _mcStatusIntervalError = 5000; // 5 seconds

        private readonly string _logHeader = "".PadRight(80, '-');

        private DateTime _lastUpdateCheck = DateTime.MinValue;

        private BitmapForm _bitmapForm = null;
        private LyricForm _lyricsForm = null;
        private List<string> _noLyricsSearchList = new List<string>();
        private SortedDictionary<string, McPlayListType> _currentSortedMcPlaylists = new SortedDictionary<string, McPlayListType>();
        private McPlayListsResponse _currentUnsortedMcPlaylistsResponse = null;
        private McMplResponse _currentLyricsFinderPlaylist = null;
        private McMplResponse _currentMcPlaylist = null;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static Bitmap _emptyCoverImage = new Bitmap(400, 400);
        private static Bitmap _emptyPlayPauseImage = new Bitmap(16, 16);

        // Instantiate a Singleton of the Semaphore with a value of 1. 
        // This means that only 1 thread can be granted access at a time. 
        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);


        /**********************************/
        /***** Private misc. routines *****/
        /**********************************/


        #region ErrorTest

        /// <summary>
        /// Test routine for the error handling.
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="Exception">Test error</exception>
        private static void ErrorTest()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                throw new Exception("Test error", ex);
            }
        }

        #endregion


        /// <summary>
        /// Blanks the play status bitmaps.
        /// </summary>
        /// <param name="exceptionIndex">Index of the exception song that should not be blanked.</param>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task BlankPlayStatusBitmapsAsync(int exceptionIndex = -1)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var rows = MainGridView.Rows;

            _emptyPlayPauseImage.MakeTransparent();

            // Clear all other bitmaps than the one in exceptionIndex row
            foreach (DataGridViewRow r in rows)
            {
                if (r.Index == exceptionIndex)
                    continue;

                var c = r.Cells[(int)GridColumnEnum.PlayImage] as DataGridViewImageCell;

                if (c.Value != _emptyPlayPauseImage)
                    c.Value = _emptyPlayPauseImage;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether any obsolete configurations have been used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if any obsolete configurations have been used; otherwise, <c>false</c>.
        /// </value>
        public void CleanupObsoleteConfigurationFiles()
        {
            var isUsed = (AbstractLyricService.IsObsoleteConfigurationsUsed
                || LyricServicesPrivateConfigurationSectionHandler.IsUsed
                || LyricsFinderCoreConfigurationSectionHandler.IsUsed
                || LyricsFinderCorePrivateConfigurationSectionHandler.IsUsed);

            if (!isUsed)
            {
                var isCleanupDone = true;
                var assy = Assembly.GetExecutingAssembly();
                var sb = new StringBuilder();
                var files = new List<FileInfo>();
                var path = Model.Helpers.Utility.GetPrivateSettingsFilePath(assy, DataDirectory);

                files.Add(new FileInfo(path));

                foreach (var service in LyricsFinderData.LyricServices)
                {
                    assy = Assembly.GetAssembly(service.GetType());
                    path = Model.Helpers.Utility.GetPrivateSettingsFilePath(assy, DataDirectory);

                    if (!path.IsNullOrEmptyTrimmed())
                    {
                        var file = new FileInfo(path);

                        files.Add(file);

                        if (file.Exists)
                            isCleanupDone = false;
                    }
                }

                if (isCleanupDone) return;

                sb.Append("Your private local configuration files are now obsolete and are no longer used, ");
                sb.AppendLine("as the information is now copied to the central \"LyricsFinder.xml\" file.");
                sb.AppendLine("These files are no longer needed:");
                sb.AppendLine();

                foreach (var file in files)
                {
                    sb.AppendLine(file.FullName);
                }

                sb.AppendLine();
                sb.AppendLine("Is it OK to delete them now?");

                var result = MessageBox.Show(this, sb.ToString(), "Clean up obsolete files?"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    foreach (var file in files)
                    {
                        file.Delete();
                    }
                }
            }
        }


        /// <summary>
        /// Enables or disables the tool strip items named in the toolStripItems.
        /// </summary>
        /// <param name="isEnable">if set to <c>true</c>, the tool strip items are enabled; else they are disabled.</param>
        /// <param name="toolStripItems">The tool strip items.</param>
        /// <remarks>
        /// <para>If empty tool strip item list, all tool strip items are enabled or disabled.</para>
        /// </remarks>
        private void EnableOrDisableToolStripItems(bool isEnable, params ToolStripItem[] toolStripItems)
        {
            if (toolStripItems.IsNullOrEmpty())
            {
                // Set ALL the items to isEnable value
                foreach (var item in TopMenu.Items)
                {
                    if (item is ToolStripMenuItem menu)
                        EnableOrDisableToolStripMenuItems(menu, isEnable);
                    else if (item is ToolStripButton button)
                        button.Enabled = isEnable;
                }
                foreach (var item in BottomMenu.Items)
                {
                    if (item is ToolStripMenuItem menu)
                        EnableOrDisableToolStripMenuItems(menu, isEnable);
                    else if (item is ToolStripButton button)
                        button.Enabled = isEnable;
                }
            }
            else
            {
                // Set only the specified items to isEnable value
                foreach (var item in toolStripItems)
                {
                    if (item is ToolStripMenuItem menu)
                        EnableOrDisableToolStripMenuItems(menu, isEnable);
                    else if (item is ToolStripButton button)
                        button.Enabled = isEnable;
                }
            }
        }


        /// <summary>
        /// Enables or disables ALL the menu items under the parent menu.
        /// </summary>
        /// <param name="parentMenu">The parent menu.</param>
        /// <param name="isEnable">If set to <c>true</c>, the menus are enabled; else they are disabled.</param>
        /// <remarks>
        /// <para>The process is recursive, i.e. if a menu is enabled, all its sub-menus are enabled too - and vice versa</para>
        /// </remarks>
        private void EnableOrDisableToolStripMenuItems(ToolStripMenuItem parentMenu, bool isEnable)
        {
            parentMenu.Enabled = isEnable;

            foreach (var item in parentMenu.DropDownItems)
            {
                if (item is ToolStripMenuItem menu)
                {
                    menu.Enabled = isEnable;

                    if (menu.DropDownItems.Count > 0)
                        EnableOrDisableToolStripMenuItems(menu, isEnable);
                }
                else if (item is ToolStripItem subItem)
                    EnableOrDisableToolStripItems(isEnable, subItem);
            }
        }


        /// <summary>
        /// Show and log a fatal error report.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// Fatal error reporting, normally called from an event routine.
        /// </remarks>
        private async Task ErrorReportAsync(string methodName, Exception exception, string message = null)
        {
            // Stop the timers
            McStatusTimer.Stop();
            UpdateCheckTimer.Stop();

            message = message?.Trim() ?? string.Empty;
            message += " ";

            await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error {message}in {methodName} event.", exception, _progressPercentage);

            // Start the timers
            McStatusTimer.Start();
            UpdateCheckTimer.Start();
        }


        /// <summary>
        /// Fills the data grid.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Current playlist is not initializes yet.</exception>
        private async Task FillDataGridAsync()
        {
            if ((_currentLyricsFinderPlaylist == null) || ((_currentLyricsFinderPlaylist.Items?.Count ?? -1) < 0))
                throw new Exception("Current playlist is not initialized yet.");

            McStatusTimer.Stop();

            // ErrorTest();

            var idx = 0;
            var dgv = MainGridView;

            // Clean up the previous list
            dgv.Rows.Clear();
            IsDataChanged = false;

            foreach (var item in _currentLyricsFinderPlaylist.Items)
            {
                var initStatus = LyricResultEnum.NotProcessedYet.ResultText();
                var row = new DataGridViewRow();
                var value = item.Value;
                var coverImage = GetCoverImage(value);

                _emptyPlayPauseImage.MakeTransparent();

                // Create the row and make it's height equal to the width of the bitmap img
                row.CreateCells(dgv, _emptyPlayPauseImage, idx, ++idx, value.Key, coverImage, WebUtility.HtmlDecode(value.Artist), WebUtility.HtmlDecode(value.Album), WebUtility.HtmlDecode(value.Name), value.Lyrics, initStatus);
                row.Height = dgv.Columns[(int)GridColumnEnum.Cover].Width;

                dgv.Rows.Add(row);
            }

            await SetPlayingImagesAndMenusAsync();

            // Scroll if necessary
            if ((_playingIndex >= 0) && (dgv.Rows.Count >= _playingIndex + 1))
            {
                dgv.Rows[_playingIndex].Selected = true;
                dgv.FirstDisplayedScrollingRowIndex = (_playingIndex > 2) ? _playingIndex - 3 : 0;
                _playingKey = (int)dgv.Rows[_playingIndex].Cells[(int)GridColumnEnum.Key].Value;
            }

            if (ToolsPlayStartStopButton.GetStartingEventSubscribers().Length == 0)
                ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_StartingAsync;

            if (ToolsPlayStartStopButton.GetStoppingEventSubscribers().Length == 0)
                ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_StoppingAsync;

            McStatusTimer.Start();

            _ = IsDataChanged; // Force display of data change, if necessary
        }


        /// <summary>
        /// Gets the item's cover image.
        /// </summary>
        /// <param name="mcMplItem">The MediaCenter MPL item.</param>
        /// <returns>The item's cover image bitmap.</returns>
        private static Bitmap GetCoverImage(McMplItem mcMplItem)
        {
            Bitmap ret = null;

            // Get the item's cover image
            if ((mcMplItem.Image == null) && !mcMplItem.ImageFile.IsNullOrEmptyTrimmed())
            {
                var fn = WebUtility.HtmlDecode(mcMplItem.Filename);
                var ifn = WebUtility.HtmlDecode(mcMplItem.ImageFile);
                var ifi = new FileInfo(ifn);

                if (!ifi.Exists)
                {
                    if (!mcMplItem.Filename.IsNullOrEmptyTrimmed())
                    {
                        var dir = Path.GetDirectoryName(fn);

                        ifn = Path.Combine(dir, ifn);
                        ifi = new FileInfo(ifn);

                        if (!ifi.Exists)
                            ifn = string.Empty;
                    }
                }

                if (ifn.IsNullOrEmptyTrimmed())
                    ret = _emptyCoverImage;
                else
                {
                    // Get the item's bitmap
                    using (var fs = new FileStream(ifn, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var image = new Bitmap(fs);

                        ret = image;
                    }
                }
            }
            else if (mcMplItem.Image == null)
                ret = _emptyCoverImage;
            else
                ret = mcMplItem.Image;

            return ret;
        }


        /// <summary>
        /// Initializes the key down event for the UserControl and all its child controls recursively.
        /// </summary>
        /// <param name="control">The control.</param>
        private void InitKeyDownEvent(Control control = null)
        {
            if (control == null)
                InitKeyDownEvent(this);
            else
            {
                control.KeyDown += new KeyEventHandler(LyricsFinderCore_KeyDownAsync);

                // Iterate the child controls
                if (control.HasChildren)
                {
                    foreach (Control ctl in control.Controls)
                    {
                        InitKeyDownEvent(ctl);
                    }
                }
            }
        }


        /// <summary>
        /// Tests and initializes the data folder.
        /// </summary>
        public async Task InitLocalDataAsync() // "public" due to the unit tests
        {
            var dataFile = string.Empty;
            var tmpFile = string.Empty;

            try
            {
                await Logging.LogAsync(_progressPercentage, "Preparing load of local data...", true);
                dataFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(LyricsFinderCoreConfigurationSectionHandler.LocalAppDataFile));
                DataDirectory = Path.GetDirectoryName(dataFile);
                tmpFile = Path.Combine(DataDirectory, dataFile + ".tmp");

                // Try to create the data folder if necessary
                await Logging.LogAsync(_progressPercentage, $"Testing if local data directory \"{DataDirectory}\" is present, else creating it...", true);
                if (!Directory.Exists(DataDirectory))
                    Directory.CreateDirectory(DataDirectory);

                // Test if we may write files in the data folder
                await Logging.LogAsync(_progressPercentage, $"Testing if we may write to a file in the local data directory \"{tmpFile}\"...", true);
                using (var st = File.Create(tmpFile)) { }

                await Logging.LogAsync(_progressPercentage, $"Testing if we may delete the test file in the local data directory \"{tmpFile}\"...", true);
                File.Delete(tmpFile);
            }
            catch (Exception ex)
            {
                var msg = $"Failed writing to the data folder \"{DataDirectory}\": {ex.Message}";
                await ErrorHandling.ShowAndLogErrorHandlerAsync(msg, ex, _progressPercentage);
                await StatusMessageAsync("Warning");
            }

            try
            {
                await Logging.LogAsync(_progressPercentage, $"Initializing dynamic lyric services...", true);
                var services = await InitLyricServicesAsync();

                // Prepare the load
                await Logging.LogAsync(_progressPercentage, "Preparing list of known XML types...", true);
                LyricsFinderDataType.XmlKnownTypes.Add(typeof(LyricsFinderDataType));
                LyricsFinderDataType.XmlKnownTypes.Add(typeof(AbstractLyricService));
                LyricsFinderDataType.XmlKnownTypes.Add(typeof(CreditType));
                foreach (var service in services)
                {
                    LyricsFinderDataType.XmlKnownTypes.Add(service.GetType());
                }

                // Create LyricsFinderData with its list of lyrics services
                await Logging.LogAsync(_progressPercentage, "Loading local data from XML...", true);
                try
                {
                    // Load previously saved services
                    LyricsFinderData = LyricsFinderDataType.Load(dataFile);
                    LyricsFinderData.IsSaveOk = true;
                }
                catch (FileNotFoundException ex)
                {
                    await ErrorHandling.ErrorLogAsync($"LyricsFinder data file \"{dataFile}\" not found, initializing a new set of services.", ex);
                    LyricsFinderData = new LyricsFinderDataType(dataFile)
                    {
                        IsSaveOk = true
                    };
                }
                catch (Exception ex)
                {
                    await ErrorHandling.ErrorLogAsync("Failed to load the lyric services, ask if we should initialize a new set of services.", ex);
                    LyricsFinderData = new LyricsFinderDataType(dataFile);

                    var result = MessageBox.Show(this, $"LyricsFinder data file \n\"{dataFile}\" \nwas found but could not be loaded, error: \n"
                        + $"\"{ex.Message}\" \nWill you initialize and save a new set of services?",
                        "LyricsFinder datafile not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        LyricsFinderData.IsSaveOk = true;
                        LyricsFinderData.SaveAsync();
                    }
                    else
                        LyricsFinderData.IsSaveOk = false;
                }

                // Add any lyric services that were not loaded before
                await Logging.LogAsync(_progressPercentage, "Adding additional lyric services...", true);
                foreach (var service in services)
                {
                    if (!LyricsFinderData.LyricServices.Any(t => t.GetType() == service.GetType()))
                        LyricsFinderData.LyricServices.Add(service);
                }

                await Logging.LogAsync(_progressPercentage, "Refreshing lyric services from their old configurations...", true);
                foreach (var service in LyricsFinderData.LyricServices)
                {
                    service.LyricsFinderData = LyricsFinderData;
                    await service.RefreshServiceSettingsAsync();
                }

                LyricsFinderData.SaveAsync();
            }
            catch (Exception ex)
            {
                var msg = $"Failed initializing the lyric services.";
                await ErrorHandling.ShowAndLogErrorHandlerAsync(msg, ex, _progressPercentage);
                await StatusMessageAsync("Warning");
            }
        }


        /// <summary>
        /// Initializes the logging.
        /// </summary>
        /// <param name="initMessages">The initialize messages.</param>
        private async Task InitLoggingAsync(string[] initMessages = null)
        {
            var assy = Assembly.GetExecutingAssembly();
            var appDir = Path.GetDirectoryName(assy.Location);
            var xmlConfigFilePath = (_isStandAlone) ? Path.Combine(appDir, "Log4net.Standalone.xml") : Path.Combine(appDir, "Log4net.Plugin.xml");
            var loggerName = (_isStandAlone) ? "LyricsFinder.Standalone" : "LyricsFinder.Plugin";
            var fi = new FileInfo(xmlConfigFilePath);

            Logging.Init(loggerName, fi);

            if (!fi.Exists)
                throw new FileNotFoundException($"LyricsFinder configuration file is not found: \"{fi.FullName}\".");

            if (LogManager.GetRepository().Configured)
            {
                if (initMessages != null)
                {
                    foreach (var msg in initMessages)
                    {
                        await StatusLogAsync(msg);
                    }
                }
            }
            else
            {
                // log4net is not configured
                var msg = "Configuration is not found for log4net in LyricsFinder for JRiver Media Center";
                var sb = new StringBuilder(msg);

                sb.AppendLine();
                sb.AppendLine();

                foreach (LogLog logMsg in LogManager.GetRepository().ConfigurationMessages.Cast<LogLog>())
                {
                    // Collect configuration message
                    sb.AppendLine(logMsg.Message);
                }

                await ErrorHandling.ShowErrorHandlerAsync(sb.ToString(), _progressPercentage);
            }
        }


        /// <summary>
        /// Initializes the lyric services.
        /// </summary>
        /// <remarks>
        /// The lyric services are not referenced directly.
        /// Instead, they are loaded dynamically, thus making it easy to add or remove them from the application.
        /// </remarks>
        private static async Task<List<AbstractLyricService>> InitLyricServicesAsync()
        {
            // Load the lyric service assemblies
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            await Logging.LogAsync(_progressPercentage, $"Finding lyric service client assemblies in \"{dir}\"...", true);

            var files = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly);
            var ret = new List<AbstractLyricService>();

            // Load each assembly in the LyricServices folder
            await Logging.LogAsync(_progressPercentage, $"Loading dynamic lyric service client assemblies from \"{dir}\"...", true);
            foreach (var file in files)
            {
                await Logging.LogAsync(_progressPercentage, $"Looking at \"{file}\"...", true);

                var assy = Assembly.LoadFrom(file);

                // Get a list of the descendant lyrics service types
                await Logging.LogAsync(_progressPercentage, $"Trying to find service types in \"{file}\"...", true);

                IEnumerable<Type> assyLyricsServiceTypes;

                try
                {
                    assyLyricsServiceTypes = assy
                       .GetTypes()
                       .Where(t => t.IsSubclassOf(typeof(AbstractLyricService)));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting types from \"{file}\".", ex);
                }

                // Create service instance(s)
                await Logging.LogAsync(_progressPercentage,
                    ((assyLyricsServiceTypes != null) && (assyLyricsServiceTypes.Count<Type>() > 0))
                    ? $"Creating service instance(s) from \"{file}\"..."
                    : $"No lyric services in \"{file}\"."
                    , true);

                foreach (var assyServiceType in assyLyricsServiceTypes)
                {
                    if (!ret.Any(t => t.GetType() == assyServiceType))
                    {
                        if (!(Activator.CreateInstance(assyServiceType) is AbstractLyricService newService))
                            throw new Exception($"Could not create instance of type \"{assyServiceType}\" or it was not an AbstractLyricService descendant type.");

                        if (!newService.IsImplemented)
                            newService.IsActive = false;

                        ret.Add(newService);
                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// Loads the form settings.
        /// </summary>
        private void LoadFormSettings()
        {
            //Properties.Settings.Default.Reload();
            _lastUpdateCheck = LyricsFinderData.MainData.LastUpdateCheck;

            // Ensure the default of not overwriting (i.e. skipping) existing lyrics
            OverwriteMenuItem.Checked = false;
            MenuItem_ClickAsync(OverwriteMenuItem, new EventArgs());
        }


        /// <summary>
        /// Loads the playlist.
        /// </summary>
        /// <param name="menuItemName">Name of the menu item.</param>
        /// <returns>Playlist type <see cref="McMplResponse"/> object</returns>
        private async Task<McMplResponse> LoadPlaylistAsync(string menuItemName = null)
        {
            var id = 0;
            var name = string.Empty;
            McMplResponse ret;

            if (menuItemName.IsNullOrEmptyTrimmed())
            {
                // If reload, get the ID and name of the current playlist
                id = _currentLyricsFinderPlaylist?.Id ?? 0;
                name = _currentLyricsFinderPlaylist?.Name ?? "Playing Now";
            }
            else
            {
                // If called from a select playlist menu, get the ID and name of the playlist
                var idx = menuItemName.LastIndexOf(_menuNameDelim, StringComparison.InvariantCultureIgnoreCase);
                var idString = menuItemName.Substring(idx + 1);
                var tmp = menuItemName.Substring(0, idx);

                id = int.Parse(idString, NumberStyles.None, CultureInfo.InvariantCulture);
                idx = tmp.LastIndexOf(_menuNameDelim, StringComparison.InvariantCultureIgnoreCase);
                name = tmp.Substring(idx + 1);
            }

            await StatusMessageAsync($"Collecting the \"{name}\" playlist...");

            try
            {
                McStatusTimer.Stop();

                // Get the playlist
                if (id > 0)
                    ret = await McRestService.GetPlaylistFilesAsync(id, name);
                else
                    ret = await McRestService.GetPlayNowListAsync();

                await StatusMessageAsync($"Connected to MediaCenter, the current playlist \"{ret.Name}\" has {ret.Items.Count} items.");
            }
            finally
            {
                McStatusTimer.Start();
            }

            return ret;
        }


        /// <summary>
        /// Loads a play list menu item recursively.
        /// </summary>
        /// <param name="parentMenuItem">The parent menu item.</param>
        /// <param name="nodes">The path nodes.</param>
        /// <param name="itemId">The item identifier.</param>
        private void LoadPlaylistMenu(ToolStripMenuItem parentMenuItem, List<string> nodes, string itemId)
        {
            var firstNode = nodes.FirstOrDefault();
            var remainingNodes = nodes.Skip(1).ToList();

            if (firstNode.IsNullOrEmptyTrimmed())
                return;
            else if (remainingNodes.Count < 1)
            {
                // Leaf node
                var menuItem = new ToolStripMenuItem
                {
                    Name = string.Join(_menuNameDelim, parentMenuItem.Name, firstNode, itemId),
                    Text = firstNode
                };

                menuItem.Click += MenuItem_ClickAsync;
                parentMenuItem.DropDownItems.Add(menuItem);
            }
            else
            {
                // Branch node
                var menuName = string.Join(_menuNameDelim, parentMenuItem.Name, firstNode);
                var menuItems = parentMenuItem.DropDownItems.Find(menuName, false);

                // Existing sub-menu found?
                if (menuItems.Length > 0)
                    LoadPlaylistMenu(menuItems.First() as ToolStripMenuItem, remainingNodes, itemId);
                else
                {
                    // Create new sub-menu
                    var menuItem = new ToolStripMenuItem
                    {
                        Name = menuName,
                        Text = firstNode
                    };

                    parentMenuItem.DropDownItems.Add(menuItem);
                    LoadPlaylistMenu(menuItem, remainingNodes, itemId);
                }
            }
        }


        /// <summary>
        /// Loads all the play lists from the Media Center.
        /// </summary>
        private async Task LoadPlaylistMenusAsync()
        {
            var list = await McRestService.GetPlayListsAsync();

            if (!list.IsOk)
                throw new Exception("Unknown error finding Media Center playlists.");

            if (list.Items.Count == 0)
                throw new Exception("Empty list of playlists returned from Media Center.");

            if (_currentUnsortedMcPlaylistsResponse?.Equals(list) ?? false)
                return; // No changes since last fetch
            else
                _currentUnsortedMcPlaylistsResponse = list;

            // Sort the playlists by path
            _currentSortedMcPlaylists.Clear();

            foreach (var item in list.Items)
            {
                var playlist = item.Value;

                _currentSortedMcPlaylists.Add(playlist.SortKey, playlist);
            }

            // Populate the dropdown menus
            FileSelectPlaylistMenuItem.DropDownItems.Clear();

            // Create the "Playing Now" menu item
            var menuItem = new ToolStripMenuItem
            {
                Name = string.Join(_menuNameDelim, nameof(FileSelectPlaylistMenuItem), "Playing Now", 0),
                Text = "Playing Now"
            };

            menuItem.Click += MenuItem_ClickAsync;
            FileSelectPlaylistMenuItem.DropDownItems.Add(menuItem);

            for (int i = 0; i < _currentSortedMcPlaylists.Count; i++)
            {
                var currItem = _currentSortedMcPlaylists.ElementAt(i);
                var currId = currItem.Value.Id;
                var currPath = currItem.Value.Path;
                var currNodes = currPath.Split('\\', '/').ToList();

                LoadPlaylistMenu(FileSelectPlaylistMenuItem, currNodes, currId);
            }
        }


        /// <summary>
        /// Plays the item in the Playing Now list by the selected row index.
        /// </summary>
        private async Task PlayOrPauseAsync()
        {
            var rows = MainGridView.Rows;
            var selectedRows = MainGridView.SelectedRows;

            if (selectedRows.Count < 1)
                return;

            // Is the selected file in the Media Center's Playing Now list?
            var rowIdx = selectedRows[0].Index;
            var selectedIndexCell = rows[rowIdx].Cells[(int)GridColumnEnum.Index] as DataGridViewTextBoxCell;
            var selectedKeyCell = rows[rowIdx].Cells[(int)GridColumnEnum.Key] as DataGridViewTextBoxCell;
            var selectedIndex = (int)(selectedIndexCell?.Value ?? -1);
            var selectedKey = (int)(selectedKeyCell?.Value ?? -1);
            var isInPlayingNowList = _currentMcPlaylist.Items.ContainsKey(selectedKey);

            if (isInPlayingNowList)
            {
                if (selectedIndex == _playingIndex)
                    await McRestService.PlayPauseAsync();
                else
                    await McRestService.PlayByIndexAsync(selectedIndex);
            }
            else if ((_currentLyricsFinderPlaylist != null) && (_currentLyricsFinderPlaylist.Id > 0))
            {
                // Replace the MC Playing Now list with the current LyricsFinder playlist
                var rsp = await McRestService.PlayPlaylistAsync(_currentLyricsFinderPlaylist.Id);

                // Play the selected item
                if (rsp.IsOk)
                    await McRestService.PlayByIndexAsync(selectedIndex);
            }

            await SetPlayingImagesAndMenusAsync();
        }


        /// <summary>
        /// Stops playing any item.
        /// </summary>
        private async Task PlayStopAsync()
        {
            await McRestService.PlayStopAsync();

            await SetPlayingImagesAndMenusAsync();
        }


        /// <summary>
        /// Resets the items status.
        /// </summary>
        private void PrepareItemStatesBeforeSearch()
        {
            var rows = MainGridView.Rows;

            // Set the items' status
            for (int i = 0; i < rows.Count; i++)
            {
                var statusCell = rows[i].Cells[(int)GridColumnEnum.Status];
                var status = statusCell.Value.ToString().ToLyricResultEnum();

                if ((status & (LyricResultEnum.NotFound | LyricResultEnum.Error | LyricResultEnum.Canceled | LyricResultEnum.SkippedOldLyrics)) != 0) // If status is one of those enum values
                    statusCell.Value = LyricResultEnum.NotProcessedYet.ResultText();
            }
        }


        /// <summary>
        /// Saves the <c>LyricsFinderData</c> and the playlist items to the Media Center.
        /// </summary>
        private async Task SaveAllAsync()
        {
            if (!IsDataChanged) return;

            await StatusMessageAsync("Saving changed data...");

            try
            {
                LyricsFinderData.SaveAsync();

                // Iterate the displayed rows and save each row, if it is found or manually edited
                for (int i = 0; i < MainGridView.Rows.Count; i++)
                {
                    var row = MainGridView.Rows[i] as DataGridViewRow;

                    var keyTxt = row.Cells[(int)GridColumnEnum.Key].Value?.ToString();
                    var lyrics = row.Cells[(int)GridColumnEnum.Lyrics].Value?.ToString();
                    var statusTxt = row.Cells[(int)GridColumnEnum.Status].Value?.ToString();

                    if (!int.TryParse(keyTxt, out var key) || statusTxt.IsNullOrEmptyTrimmed())
                        throw new Exception($"Error parsing row {i} cell(s): keyTxt=\"{keyTxt}\", lyrics=\"{lyrics}\", status=\"{statusTxt}\".");

                    // We only save items if they are found or manually edited
                    if (((statusTxt.ToLyricResultEnum() & (LyricResultEnum.Found | LyricResultEnum.ManuallyEdited)) != 0) // If status is one of those enum values
                        && (!lyrics.IsNullOrEmptyTrimmed()))
                    {
                        var rsp = await McRestService.SetInfo(key, "Lyrics", lyrics);

                        if (!rsp.IsOk)
                            throw new Exception($"Saving item to Media Center failed for index {i} with key {key}.");
                    }
                }

                IsDataChanged = false;
                await StatusMessageAsync(string.Empty);
            }
            catch (Exception ex)
            {
                var msg = $"Saving data failed: {ex.Message}";

                await StatusMessageAsync(msg);
                throw new Exception(msg, ex);
            }
        }


        /// <summary>
        /// Sets the play images.
        /// </summary>
        /// <returns>Playing row index or -1 if nothing is playing.</returns>
        private async Task SetPlayingImagesAndMenusAsync()
        {
            McStatusTimer.Stop();

            var rows = MainGridView.Rows;
            var selectedRows = MainGridView.SelectedRows;

            if (selectedRows.Count < 1)
                return;

            var selectedKeyCell = selectedRows[0].Cells[(int)GridColumnEnum.Key] as DataGridViewTextBoxCell;
            var selectedKey = (int)(selectedKeyCell?.Value ?? -1);
            var mcInfo = await McRestService.InfoAsync();

            _playingIndex = -1;
            _playingKey = -1;

            if (mcInfo == null)
                return;

            // Try to find a song in the current list matching the one (if any) that Media Center is playing
            // and set the bitmap to play or blank accordingly.
            // The reason we don't just use the index is, that the current playlist in LyricsFinder 
            // may be another than the Media Center "Playing Now" list.
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var indexCell = row.Cells[(int)GridColumnEnum.Index] as DataGridViewTextBoxCell;
                var keyCell = row.Cells[(int)GridColumnEnum.Key] as DataGridViewTextBoxCell;
                var index = (int)(indexCell?.Value ?? -1);
                var key = keyCell?.Value?.ToString() ?? string.Empty;
                var imgCell = row.Cells[(int)GridColumnEnum.PlayImage] as DataGridViewImageCell;

                if (mcInfo.FileKey.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    _playingIndex = index;
                    _playingKey = int.Parse(mcInfo.FileKey, NumberStyles.None, CultureInfo.InvariantCulture);

                    await BlankPlayStatusBitmapsAsync(i); // Clear all other bitmaps than the one in playIdx row

                    if (mcInfo.Status?.StartsWith("Play", StringComparison.InvariantCultureIgnoreCase) ?? false)
                        imgCell.Value = Properties.Resources.Play;
                    else if (mcInfo.Status?.StartsWith("Pause", StringComparison.InvariantCultureIgnoreCase) ?? false)
                        imgCell.Value = Properties.Resources.Pause;
                    else
                        imgCell.Value = _emptyPlayPauseImage;

                    break;
                }
            }

            // If not found, blank all bitmaps
            if (_playingIndex < 0)
                await BlankPlayStatusBitmapsAsync();

            // Set the playing menus' states
            ContextPlayStopMenuItem.Text = "Stop play";

            if (mcInfo.Status?.StartsWith("Play", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                ContextPlayPauseMenuItem.Text = (_playingKey == selectedKey) ? "Pause play" : "Play";
                ContextPlayStopMenuItem.Visible = true;
                ToolsPlayStartStopButton.SetRunningState(true);
            }
            else if (mcInfo.Status?.StartsWith("Pause", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                ContextPlayPauseMenuItem.Text = (_playingKey == selectedKey) ? "Continue play" : "Play";
                ContextPlayStopMenuItem.Visible = false;
                ToolsPlayStartStopButton.SetRunningState(false);
            }
            else
            {
                ContextPlayPauseMenuItem.Text = "Play";
                ContextPlayStopMenuItem.Visible = false;
                ToolsPlayStartStopButton.SetRunningState(false);
            }

            McStatusTimer.Start();
        }


        /// <summary>
        /// Shows the bitmap form.
        /// </summary>
        /// <param name="colIdx">Index of the col.</param>
        /// <param name="rowIdx">Index of the row.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Column {colIdx}</exception>
        private BitmapForm ShowBitmap(int colIdx, int rowIdx)
        {
            var dgv = MainGridView;

            if ((colIdx < 0) || (colIdx > dgv.ColumnCount - 1)) return null;
            if ((rowIdx < 0) || (rowIdx > dgv.RowCount - 1)) return null;

            var row = dgv.Rows[rowIdx];
            var rect = dgv.RectangleToScreen(dgv.GetCellDisplayRectangle(colIdx, rowIdx, true));

            if (!(row.Cells[colIdx] is DataGridViewImageCell cell))
                throw new ArgumentException($"Column {colIdx} is not a DataGridViewImageCell.");

            if (!(cell.Value is Bitmap img)
                || (img.Equals(_emptyCoverImage)))
                return null;

            var pt = new Point(rect.Right, rect.Top);
            var location = new Point(pt.X, MousePosition.Y);
            var ret = new BitmapForm(img, location);

            ret.Show();

            return ret;
        }


        /// <summary>
        /// Shows the lyrics form.
        /// </summary>
        /// <param name="colIdx">Index of the col.</param>
        /// <param name="rowIdx">Index of the row.</param>
        /// <param name="isAutoOpen">if set to <c>true</c> the <see cref="LyricForm"/> is opened automatically when the mouse is moved over the lyrics column; else <c>false</c>.</param>
        /// <returns><see cref="LyricForm"/> object.</returns>
        /// <exception cref="ArgumentException">Column {colIdx}</exception>
        private LyricForm ShowLyrics(int colIdx, int rowIdx, bool isAutoOpen = false)
        {
            var dgv = MainGridView;

            if ((colIdx < 0) || (colIdx > dgv.ColumnCount - 1)) return null;
            if ((rowIdx < 0) || (rowIdx > dgv.RowCount - 1)) return null;

            var row = dgv.Rows[rowIdx];
            var rect = dgv.RectangleToScreen(dgv.GetCellDisplayRectangle(colIdx, rowIdx, true));

            if (!(row.Cells[colIdx] is DataGridViewTextBoxCell cell))
                throw new ArgumentException($"Column {colIdx} is not a DataGridViewTextBoxCell.");

            var location = new Point(MousePosition.X, MousePosition.Y);
            var size = LyricsFinderData.MainData.LyricFormSize;
            var ret = new LyricForm(cell, location, size, ShowLyricsCallbackAsync, LyricsFinderData)
            {
                StartPosition = (isAutoOpen) ? FormStartPosition.Manual : FormStartPosition.CenterParent
            };

            if (isAutoOpen)
                ret.Show(this);
            else
                ret.ShowDialog(this);

            return ret;
        }


        /// <summary>
        /// Shows the lyrics callback.
        /// </summary>
        /// <param name="lyricForm">The lyrics form.</param>
        private async void ShowLyricsCallbackAsync(LyricForm lyricForm)
        {
            try
            {
                if (lyricForm.Result == DialogResult.Yes)
                {
                    // Display the created/changed lyrics in the list
                    lyricForm.LyricCell.Value = lyricForm.Lyric;

                    var row = lyricForm.LyricCell.OwningRow;

                    row.Selected = true;
                    row.Cells[(int)GridColumnEnum.Status].Value = LyricResultEnum.ManuallyEdited.ResultText();

                    IsDataChanged = true;
                }
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Shows the services callback.
        /// </summary>
        /// <param name="lyricsServiceForm">The lyrics service form.</param>
        internal async void ShowServicesCallbackAsync(LyricServiceForm lyricsServiceForm)
        {
            try
            {
                LyricsFinderData.SaveAsync();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Shows or hides the menu shortcuts.
        /// </summary>
        /// <param name="isDisplayed">if set to <c>true</c> shortcuts are displayed; else they are hidden.</param>
        /// <param name="parentMenuItem">The parent menu item.</param>
        private void ShowShortcuts(bool isDisplayed, ToolStripMenuItem parentMenuItem = null)
        {
            if (parentMenuItem == null)
            {
                foreach (var item in TopMenu.Items)
                {
                    if (item is ToolStripMenuItem menuItem)
                    {
                        menuItem.ShowShortcutKeys = isDisplayed;
                        ShowShortcuts(isDisplayed, menuItem);
                    }
                }
            }
            else
            {
                foreach (var item in parentMenuItem.DropDownItems)
                {
                    if (item is ToolStripMenuItem menuItem)
                    {
                        menuItem.ShowShortcutKeys = isDisplayed;
                        ShowShortcuts(isDisplayed, menuItem);
                    }
                }
            }
        }


        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isDebug">if set to <c>true</c> [is debug].</param>
        private static async Task StatusLogAsync(string message, bool isDebug = false)
        {
            await Logging.LogAsync(_progressPercentage, message, isDebug);
        }


        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        private async Task StatusLogAsync(string message, Exception ex)
        {
            await Logging.LogAsync(_progressPercentage, message, ex);
        }


        /// <summary>
        /// Shows status message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="includeProgress">if set to <c>true</c> [include progress].</param>
        /// <param name="includeLogging">if set to <c>true</c> [include logging].</param>
        private async Task StatusMessageAsync(string message, bool includeProgress = false, bool includeLogging = false)
        {
            var msg1 = message?.Trim() ?? string.Empty;

            if (msg1.Length > 0)
            {
                msg1 = msg1.Substring(0, 1).ToUpperInvariant() + msg1.Remove(0, 1);
            }

            if (includeLogging)
                await StatusLogAsync(msg1);

            if (MainStatusLabel.Text != msg1)
            {
                MainStatusLabel.Text = msg1;
                MainStatusLabel.ToolTipText = msg1;
            }

            if (includeProgress && (_progressPercentage > 0))
                MainProgressBar.Value = _progressPercentage;

            MainStatusStrip.Update();
        }

    }

}