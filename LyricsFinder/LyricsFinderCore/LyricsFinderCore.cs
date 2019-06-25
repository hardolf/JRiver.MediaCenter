/*
Description: Lyrics finder for JRiver Media Center

Author: Hardolf

Core module creation date:
2018.04.12

Version Number:
1.2.0

Modified: 2019.05.25 by Hardolf.
Modified: 2019.07.01 by Hardolf.
*/


using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;

using MediaCenter;
using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Main interface type. 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [ProgId("MediaCenter.LyricsFinder.LyricsFinderCore")]
    [ComVisible(true)]
    public partial class LyricsFinderCore : UserControl
    {

        /**********************/
        /***** Properties *****/
        /**********************/

        /// <summary>
        /// Gets or sets the authentication.
        /// </summary>
        /// <value>
        /// The authentication.
        /// </value>
        private McAuthenticationResponse Authentication { get; set; }

        /// <summary>
        /// Gets or sets the data directory.
        /// </summary>
        /// <value>
        /// The data directory.
        /// </value>
        public string DataDirectory { get; set; }

        /// <summary>
        /// Gets or sets the entry assembly.
        /// </summary>
        /// <value>
        /// The entry assembly.
        /// </value>
        public Assembly EntryAssembly { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the playlist data is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the playlist data is changed; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Do not change _isDataChanged, always change this property instead.
        /// </remarks>
        [ComVisible(false)]
        public bool IsDataChanged
        {
            get
            {
                if (_isDesignTime) return false;

                // var ret = LyricsFinderData.IsChanged || _isGridDataChanged;
                var ret = _isGridDataChanged;

                FileSaveMenuItem.Enabled = ret;
                DataChangedTextBox.Visible = ret;

                return ret;
            }
            set
            {
                if (_isDesignTime) return;

                _isGridDataChanged = value;

                FileSaveMenuItem.Enabled = value;
                DataChangedTextBox.Visible = value;
            }
        }

        /// <summary>
        /// The lyrics finder data.
        /// </summary>
        internal LyricsFinderDataType LyricsFinderData { get; private set; }


        /************************/
        /***** Constructors *****/
        /************************/


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsFinderCore"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor for the core.
        /// </remarks>
        public LyricsFinderCore()
            : this(true)
        {
            if (EntryAssembly == null)
                EntryAssembly = Assembly.GetEntryAssembly();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsFinderCore" /> class.
        /// </summary>
        /// <param name="isStandAlone">if set to <c>true</c> <see cref="LyricsFinderCore" /> is called from stand alone EXE; else called from JRiver Media Center.</param>
        /// <param name="callingAssembly">The entry assembly.</param>
        /// <remarks>
        /// This is the main constructor for the core.
        /// </remarks>
        public LyricsFinderCore(bool isStandAlone, Assembly callingAssembly = null)
            : base()
        {
            if ((EntryAssembly == null) && (callingAssembly != null))
                EntryAssembly = callingAssembly;

            _isStandAlone = isStandAlone;
            _isDesignTime = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            InitializeComponent();
        }


        /// <summary>
        /// Creates the lyrics finder core object.
        /// </summary>
        /// <returns><see cref="LyricsFinderCore"/> object.</returns>
        [ComVisible(false)]
        public static LyricsFinderCore CreateLyricsFinderCore()
        {
            var ret = new LyricsFinderCore(true);

            return ret;
        }


        /*********************/
        /***** Delegates *****/
        /*********************/


        /// <summary>
        /// Handles the DropDownOpening event of the FileSelectPlaylistMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void FileSelectPlaylistMenuItem_DropDownOpeningAsync(object sender, EventArgs e)
        {
            try
            {
                await LoadPlaylistMenus();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the LyricsFinderCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private async void LyricsFinderCore_KeyDownAsync(object sender, KeyEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                e.Handled = false;

                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    ToolsPlayStartStopButton.PerformClick();
                }
                else if (e.Control && (e.KeyCode == Keys.S))
                {
                    e.Handled = true;
                    Save();
                }
                else if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;

                    await PlayOrPause();
                }
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the ItemClicked event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private async void MainContextMenu_ItemClickedAsync(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                var rows = MainDataGridView.SelectedRows;

                if (rows.Count < 1)
                    return;

                var colIdx = (int)GridColumnEnum.Lyrics;
                var rowIdx = rows[0].Index;

                if (e.ClickedItem == ContextEditMenuItem)
                    _lyricsForm = ShowLyrics(colIdx, rowIdx);
                else if (e.ClickedItem == ContextPlayPauseMenuItem)
                {
                    if (!ToolsPlayStartStopButton.IsRunning)
                        ToolsPlayStartStopButton.PerformClick();
                    else
                        await PlayOrPause();
                }
                else if (e.ClickedItem == ContextPlayStopMenuItem)
                    ToolsPlayStartStopButton.Stop();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Opening event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private async void MainContextMenu_OpeningAsync(object sender, CancelEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                await SetPlayingImagesAndMenus();

                if (MainDataGridView.SelectedRows.Count < 1)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellDoubleClick event of the MainDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void MainDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != (int)GridColumnEnum.Lyrics)
                    ToolsPlayStartStopButton.PerformClick();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellMouseClick event of the MainDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void MainDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if ((_lyricsForm != null) && _lyricsForm.Visible) return;

                var rows = MainDataGridView.Rows;

                if (rows.Count > 1)
                    rows[e.RowIndex].Selected = true;

                if (e.Clicks > 1) return;

                if (e.Button == MouseButtons.Right)
                    MainContextMenu.Show(Cursor.Position, ToolStripDropDownDirection.Default);
                else
                {
                    switch (e.ColumnIndex)
                    {
                        case (int)GridColumnEnum.Artist:
                        case (int)GridColumnEnum.Album:
                        case (int)GridColumnEnum.Title:
                        case (int)GridColumnEnum.Cover:
                            break;

                        case (int)GridColumnEnum.Lyrics:
                            _lyricsForm = ShowLyrics(e.ColumnIndex, e.RowIndex);
                            break;

                        default:
                            break;
                    }
                }

                _currentMouseColumnIndex = e.ColumnIndex;
                _currentMouseRowIndex = e.RowIndex;
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellMouseMove event of the MainDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private void MainDataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if ((_lyricsForm != null) && _lyricsForm.Visible && !LyricsFinderCoreConfigurationSectionHandler.MouseMoveOpenLyricsForm) return;
                if ((e.ColumnIndex == (int)GridColumnEnum.Lyrics) && !LyricsFinderCoreConfigurationSectionHandler.MouseMoveOpenLyricsForm) return;
                if ((e.ColumnIndex == _currentMouseColumnIndex) && (e.RowIndex == _currentMouseRowIndex)) return;

                _bitmapForm?.Close();
                _bitmapForm = null;
                _lyricsForm?.Close();
                _lyricsForm = null;

                switch (e.ColumnIndex)
                {
                    case (int)GridColumnEnum.Artist:
                    case (int)GridColumnEnum.Album:
                    case (int)GridColumnEnum.Title:
                        break;

                    case (int)GridColumnEnum.Cover:
                        _bitmapForm = ShowBitmap(e.ColumnIndex, e.RowIndex);
                        break;

                    case (int)GridColumnEnum.Lyrics:
                        _lyricsForm = ShowLyrics(e.ColumnIndex, e.RowIndex);
                        break;

                    default:
                        break;
                }

                _currentMouseColumnIndex = e.ColumnIndex;
                _currentMouseRowIndex = e.RowIndex;
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the MouseLeave event of the MainDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainDataGridView_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (!LyricsFinderCoreConfigurationSectionHandler.MouseMoveOpenLyricsForm) return;

                var pt = Cursor.Position;
                var rect = MainDataGridView.RectangleToScreen(MainDataGridView.ClientRectangle);

                if (!rect.Contains(pt))
                {
                    _bitmapForm?.Close();
                    _bitmapForm = null;
                    _lyricsForm?.Close();
                    _lyricsForm = null;
                }
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Resize event of the MainDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainDataGridView_Resize(object sender, EventArgs e)
        {
            const int fraction = 6;

            try
            {
                // Set the Artist, Album and Track columns' width to a 5th of the total width
                // The Lyrics column is set to "Fill", so it will adjust itself
                MainDataGridView.Columns[(int)GridColumnEnum.Artist].Width = (int)(MainDataGridView.Width / (1.5 * fraction));
                MainDataGridView.Columns[(int)GridColumnEnum.Album].Width = (int)(MainDataGridView.Width / (1 * fraction));
                MainDataGridView.Columns[(int)GridColumnEnum.Title].Width = (int)(MainDataGridView.Width / (1 * fraction));
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Tick event of the McStatusTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>The timer polls Media Center for the playing item, if any, and sets the PlayImage accordingly.</remarks>
        private async void McStatusTimer_TickAsync(object sender, EventArgs e)
        {
            try
            {
                McStatusTimer.Stop();

                await SetPlayingImagesAndMenus();

                McStatusTimer.Interval = _mcStatusIntervalNormal;
                McStatusTimer.Start();
            }
            catch (Exception ex)
            {
                // We don't bother the user with this error, since MC could just be shut down.
                // Instead we set up the timer interval and try again.
                // Also, we only log the first incident.

                if (McStatusTimer.Interval == _mcStatusIntervalNormal)
                    ErrorHandling.ErrorLog($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex, _progressPercentage);

                await BlankPlayStatusBitmaps();

                McStatusTimer.Interval = _mcStatusIntervalError;
                McStatusTimer.Start();
            }
        }


        /// <summary>
        /// Handles the Click event of the ExitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MenuItem_ClickAsync(object sender, EventArgs e)
        {
            var itemName = "Undefined item";
            var msg = string.Empty;

            try
            {
                if (_isDesignTime) return;
                // MessageBox.Show($"Click!\n{sender}", "Click", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!(sender is ToolStripMenuItem menuItem))
                    throw new Exception($"Unknown sender: \"{sender}\".");
                else
                    itemName = menuItem.Name;

                // Special handling of the select playlist menu
                if (itemName.StartsWith(nameof(FileSelectPlaylistMenuItem), StringComparison.InvariantCultureIgnoreCase))
                {
                    // Ignore any "Select playlist "branch" menu and only accept the "leaf"
                    if (menuItem.DropDownItems.Count > 0)
                        return;

                    if (IsDataChanged)
                    {
                        var result = MessageBox.Show("Data is changed and will be lost if you continue.\nDo you want to continue anyway?"
                            , "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.No)
                            return;
                    }

                    // Get the MC playlist and let LyricsFinder know about it
                    await ReloadPlaylist(false, itemName);
                }
                else
                {
                    switch (itemName)
                    {
                        case nameof(FileExitMenuItem):
                            // Close(); // Not done here, in standalone it is done in LyricsFinderExe, in plug-in it is done by Media Center
                            break;

                        case nameof(FileReloadMenuItem):
                            await ReloadPlaylist(true);
                            break;

                        case nameof(FileSaveMenuItem):
                            Save();
                            break;

                        case nameof(HelpAboutMenuItem):
                            using (var about = new AboutBox(EntryAssembly))
                            {
                                about.ShowDialog();
                            }
                            break;

                        case nameof(HelpContentsMenuItem):
                            var url = "https://github.com/hardolf/JRiver.MediaCenter/wiki/LyricsFinder-User-Manual";
                            System.Diagnostics.Process.Start(url);
                            break;

                        case nameof(HelpLookForUpdatesMenuItem):
                            Model.Helpers.Utility.UpdateCheckWithRetries(EntryAssembly.GetName().Version, true);
                            break;

                        case nameof(ToolsLyricServicesMenuItem):
                            using (var lyricsServiceForm = new LyricServiceForm(LyricsFinderData, ShowServicesCallback))
                            {
                                lyricsServiceForm.ShowDialog(this);
                            }
                            break;

                        case nameof(ToolsOptionsMenuItem):
                            using (var frm = new OptionForm("LyricsFinder connection setup"))
                            {
                                frm.ShowDialog(this);
                            }
                            break;

                        case nameof(ToolsShowLogMenuItem):
                            Logging.ShowLogDir();
                            break;

                        case nameof(ToolsTestMenuItem):
                            // MessageBox.Show($"{Properties.Settings.Default.McWebServiceUser}", "Menu item selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case nameof(OverwriteMenuItem):
                            OverwriteMenuItem.Text = (OverwriteMenuItem.Checked)
                                ? "Overwrite existing lyrics"
                                : "Skip existing lyrics";
                            break;

                        default:
                            throw new Exception($"Unknown menu item: \"{itemName}\".");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage($"Error {(msg.IsNullOrEmptyTrimmed() ? msg : msg + " ")}in menu item \"{itemName}\".", true, true);
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex, $"{(msg.IsNullOrEmptyTrimmed() ? msg : msg + " ")}in menu item: \"{itemName}\"");
            }
        }


        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.HandleDestroyed" /> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
        [ComVisible(false)]
        protected override void OnHandleDestroyed(EventArgs e)
        {
            try
            {
                if (!_isDesignTime && !_isOnHandleDestroyedDone)
                {
                    _isOnHandleDestroyedDone = true;
                    _progressPercentage = 0;
                    StatusLog("LyricsFinder for JRiver Media Center closed.");
                    StatusLog(_logHeader + Environment.NewLine);

                    Dispose(true);
                }

                base.OnHandleDestroyed(e);
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Starting event of the StartStopToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void SearchAllStartStopButton_Starting(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                if (!ToolsSearchAllStartStopButton.IsRunning)
                {
                    if (IsDataChanged && (DialogResult.No == MessageBox.Show("Data is changed and will be lost if you proceed\nDo you want to proceed.?", "Proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)))
                        return;

                    ToolsSearchAllStartStopButton.Start();
                }

                _progressPercentage = 0;

                // Start the automatic search process job
                await ProcessAsync(_cancellationTokenSource);
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the StartStopToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void SearchAllStartStopButton_Stopping(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                if (ToolsSearchAllStartStopButton.IsRunning)
                    ToolsSearchAllStartStopButton.Stop();

                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Starting event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void ToolsPlayStartStopButton_StartingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                var dvg = MainDataGridView;

                if (dvg.SelectedRows.Count > 0)
                    await PlayOrPause();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void ToolsPlayStartStopButton_StoppingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                await PlayStop();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Starting event of the ToolsSearchAllStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void ToolsSearchAllStartStopButton_Starting(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                if (!SearchAllStartStopButton.IsRunning)
                    SearchAllStartStopButton.Start();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the ToolsSearchAllStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void ToolsSearchAllStartStopButton_Stopping(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                if (SearchAllStartStopButton.IsRunning)
                    SearchAllStartStopButton.Stop();
            }
            catch (Exception ex)
            {
                ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Tick event of the UpdateCheckTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UpdateCheckTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateCheckTimer.Stop();

                // Is it about time for a check?
                var updInterval = LyricsFinderCorePrivateConfigurationSectionHandler.UpdateCheckIntervalDays;
                var daysSinceLast = (DateTime.Now - _lastUpdateCheck).TotalDays;

                if ((updInterval == 0)
                    || ((updInterval > 0) && (daysSinceLast >= updInterval)))
                {
                    var version = Assembly.GetExecutingAssembly().GetName().Version;
                    var isUpdated = Model.Helpers.Utility.UpdateCheckWithRetries(version);

                    if (!isUpdated)
                        Model.Helpers.Utility.UpdateCheckWithRetries(version, true);

                    _lastUpdateCheck = DateTime.Now;
                    LyricsFinderCorePrivateConfigurationSectionHandler.Save(lastUpdateCheck: _lastUpdateCheck);
                }

                // We only use this timer once in each session, when the check is successful, so no need to start it again
            }
#pragma warning disable CS0168 // Variable is declared but never used
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            catch (Exception ex)
#pragma warning restore IDE0059 // Value assigned to symbol is never used
#pragma warning restore CS0168 // Variable is declared but never used
            {
                // We ignore this exception for now
                // ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);

                UpdateCheckTimer.Interval *= 10;
                UpdateCheckTimer.Start();
            }
        }

    }

}
