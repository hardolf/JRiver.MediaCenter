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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter;
using MediaCenter.LyricsFinder.Forms;
using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.McWs;
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
        /// Gets the current sorted Media Center playlists.
        /// </summary>
        /// <value>
        /// The current sorted Media Center playlists.
        /// </value>
        internal SortedDictionary<string, McPlayListType> CurrentSortedMcPlaylists { get; private set; } = new SortedDictionary<string, McPlayListType>();

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
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        internal bool IsPlaying
        {
            get => _playingIndex >= 0;
        }

        /// <summary>
        /// Gets the current items' playlists.
        /// </summary>
        /// <value>
        /// The current items' playlists.
        /// </value>
        internal Dictionary<int, List<int>> ItemsPlayListIds { get; private set; } = null;

        /// <summary>
        /// Gets the Media Center play control.
        /// </summary>
        /// <value>
        /// The Media Center play control.
        /// </value>
        internal McPlayControl McPlayControl { get; private set; }

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
                if (_isDesignTime) return;

                await LoadPlaylistMenusAsync();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
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

                if (e.Alt && (e.KeyCode == Keys.F4))
                {
                    e.Handled = false;
                    return;
                }
                else
                    e.Handled = true;

                var dgv = MainGridView;

                if (dgv.SelectedRows.Count < 1) return;

                var selectedRowIndex = (dgv.SelectedRows.Count > 0) ? dgv.SelectedRows[0].Index : -1;
                var colIdx = (int)GridColumnEnum.Lyrics;
                var rowIdx = dgv.SelectedRows[0].Index;

                if (e.KeyCode == Keys.Down)
                {
                    if (selectedRowIndex < dgv.RowCount - 1)
                    {
                        dgv.ClearSelection();
                        dgv.Rows[selectedRowIndex + 1].Selected = true;
                        ScrollDataGrid(dgv.SelectedRows[0].Index);
                    }
                }
                else if (e.KeyCode == Keys.End)
                {
                    dgv.ClearSelection();
                    dgv.Rows[dgv.Rows.Count - 1].Selected = true;
                    ScrollDataGrid(dgv.SelectedRows[0].Index);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    ToolsPlayStartStopButton.PerformClick();
                }
                else if (e.KeyCode == Keys.Home)
                {
                    dgv.ClearSelection();
                    dgv.Rows[0].Selected = true;
                    ScrollDataGrid(dgv.SelectedRows[0].Index);
                }
                else if (e.Alt && (e.KeyCode == Keys.L))
                {
                    ShowLyrics(colIdx, rowIdx);
                }
                else if (e.KeyCode == Keys.Left)
                {
                    if (McPlayControl != null)
                        await McPlayControl.JumpAsync(true, e.Control);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (McPlayControl != null)
                        await McPlayControl.JumpAsync(false, e.Control);
                }
                else if (e.Control && (e.KeyCode == Keys.S))
                {
                    await SaveAllAsync();
                }
                else if ((e.KeyCode == Keys.Space)
                    || (e.Control && (e.KeyCode == Keys.P)))
                {
                    await PlayOrPauseAsync();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (selectedRowIndex > 0)
                    {
                        dgv.ClearSelection();
                        dgv.Rows[selectedRowIndex - 1].Selected = true;
                        ScrollDataGrid(dgv.SelectedRows[0].Index);
                    }
                }
                else
                    e.Handled = false;
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Load event of the LyricsFinderCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricsFinderCore_LoadAsync(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                ShowMcPlayControl(this);
                this.Focus();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Move event of the LyricsFinderCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricsFinderCore_LocationChangedAsync(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                // PositionAndResizeMcPlayControl();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Resize event of the LyricsFinderCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricsFinderCore_ResizeAsync(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                ErrorHandling.InitMaxWindowSize(this.Size);

                // PositionAndResizeMcPlayControl();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
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

                var rows = MainGridView.SelectedRows;

                if (rows.Count < 1)
                    return;

                var colIdx = (int)GridColumnEnum.Lyrics;
                var rowIdx = rows[0].Index;

                switch (e.ClickedItem.Name)
                {
                    case nameof(ContextEditMenuItem):
                        ShowLyrics(colIdx, rowIdx);
                        break;

                    case nameof(ContextPlayPauseMenuItem):
                        await PlayOrPauseAsync();
                        ToolsPlayStartStopButton.SetRunningState(!ToolsPlayStartStopButton.IsRunning);
                        break;

                    case nameof(ContextPlayStopMenuItem):
                        ToolsPlayStartStopButton.Stop();
                        break;

                    case nameof(ContextItemInfoMenuItem):
                        ShowItemInfo();
                        break;

                    default:
                        throw new Exception($"Unknown context menu item \"{e.ClickedItem.Text}\".");
                }
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
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

                await SetPlayingImagesAndMenusAsync();

                if (MainGridView.SelectedRows.Count < 1)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellDoubleClick event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private async void MainGridView_CellDoubleClickAsync(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                if (e.ColumnIndex != (int)GridColumnEnum.Lyrics)
                    ToolsPlayStartStopButton.PerformClick();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellFormatting event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> instance containing the event data.</param>
        private async void MainGridView_CellFormattingAsync(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (e.ColumnIndex != (int)GridColumnEnum.Lyrics) return;

                var cell = MainGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var txt = e.Value?.ToString();

                if (txt.IsNullOrEmptyTrimmed()) return;

                if (txt.StringLineCount() > _maxLyricToolTipLines)
                    txt = txt.TruncateStringLines(_maxLyricToolTipLines).Trim() + Constants.NewLine + "...";

                cell.ToolTipText = txt;
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellMouseClick event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private async void MainGridView_CellMouseClickAsync(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (LyricsFinderData?.MainData == null) return;
                if ((_lyricForm != null) && _lyricForm.Visible) return;
                if (e.RowIndex < 0) return;

                var rows = MainGridView.Rows;

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
                            ShowLyrics(e.ColumnIndex, e.RowIndex);
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
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the CellMouseMove event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private async void MainGridView_CellMouseMoveAsync(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (LyricsFinderData?.MainData == null) return;
                if ((_lyricForm != null) && _lyricForm.Visible && !LyricsFinderData.MainData.MouseMoveOpenLyricsForm) return;
                if ((e.ColumnIndex == (int)GridColumnEnum.Lyrics) && !LyricsFinderData.MainData.MouseMoveOpenLyricsForm) return;
                if ((e.ColumnIndex == _currentMouseColumnIndex) && (e.RowIndex == _currentMouseRowIndex)) return;

                _bitmapForm?.Close();
                _bitmapForm = null;
                _lyricForm?.Close();
                _lyricForm = null;

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
                        ShowLyrics(e.ColumnIndex, e.RowIndex, LyricsFinderData.MainData.MouseMoveOpenLyricsForm);
                        break;

                    default:
                        break;
                }

                _currentMouseColumnIndex = e.ColumnIndex;
                _currentMouseRowIndex = e.RowIndex;
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the ColumnHeaderMouseClick event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs"/> instance containing the event data.</param>
        private async void MainGridView_ColumnHeaderMouseClickAsync(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (LyricsFinderData?.MainData == null) return;

                var rows = MainGridView.Rows;

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var keyCell = row.Cells[(int)GridColumnEnum.Key] as DataGridViewTextBoxCell;
                    var key = (int)(keyCell?.Value ?? -1);

                    if (key == _selectedKey)
                    {
                        row.Selected = true;
                        ScrollDataGrid(row.Index);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the MouseEnter event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MainGridView_MouseEnterAsync(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the MouseLeave event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MainGridView_MouseLeaveAsync(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (LyricsFinderData?.MainData == null) return;
                if ((_lyricForm != null) && _lyricForm.Visible && !LyricsFinderData.MainData.MouseMoveOpenLyricsForm) return;

                var pt = Cursor.Position;
                var rect = MainGridView.RectangleToScreen(MainGridView.ClientRectangle);

                if (!rect.Contains(pt))
                {
                    _bitmapForm?.Close();
                    _bitmapForm = null;
                    _lyricForm?.Close();
                    _lyricForm = null;
                }
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Resize event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MainGridView_ResizeAsync(object sender, EventArgs e)
        {
            const int fraction = 6;

            try
            {
                if (_isDesignTime) return;

                // Set the Artist, Album and Track columns' width to a 5th of the total width
                // The Lyrics column is set to "Fill", so it will adjust itself
                MainGridView.Columns[(int)GridColumnEnum.Artist].Width = (int)(MainGridView.Width / (1.5 * fraction));
                MainGridView.Columns[(int)GridColumnEnum.Album].Width = (int)(MainGridView.Width / (1 * fraction));
                MainGridView.Columns[(int)GridColumnEnum.Title].Width = (int)(MainGridView.Width / (1 * fraction));
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the SelectionChanged event of the MainGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MainGridView_SelectionChangedAsync(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if ((MainGridView.SelectedRows == null) || (MainGridView.SelectedRows.Count == 0)) return;

                var row = MainGridView.SelectedRows[0];
                var selectedKeyCell = row.Cells[(int)GridColumnEnum.Key] as DataGridViewTextBoxCell;

                _selectedKey = (int)(selectedKeyCell?.Value ?? -1);
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
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

                if (_isDesignTime) return;

                await SetPlayingImagesAndMenusAsync();

                // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                await _semaphoreSlim.WaitAsync();

                try
                {
                    var last = LyricsFinderData.MainData.LastMcStatusCheck;
                    var now = DateTime.Now;

                    // If this a new day (local time), we reset the services' daily counters
                    if (now.Date > last.Date)
                    {
                        foreach (var service in LyricsFinderData.LyricServices)
                        {
                            service.HitCountToday = 0;
                            service.RequestCountToday = 0;
                        }
                    }

                    LyricsFinderData.MainData.LastMcStatusCheck = now;
                }
                finally
                {
                    _semaphoreSlim.Release();
                }

                McStatusTimer.Interval = _mcStatusIntervalNormal;
            }
            catch (Exception ex)
            {
                // We don't bother the user with this error, since MC could just be shut down.
                // Instead we set up the timer interval and try again.
                // Also, we only log the first incident.

                if (McStatusTimer.Interval == _mcStatusIntervalNormal)
                    await ErrorHandling.ErrorLogAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex, _progressPercentage);

                await BlankPlayStatusBitmapsAsync();

                McStatusTimer.Interval = _mcStatusIntervalError;
            }
            finally
            {
                McStatusTimer.Start();

                GC.Collect();
            }
        }


        /// <summary>
        /// Handles the Click event of the ExitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MenuItem_ClickAsync(object sender, EventArgs e)
        {
            var menuName = "Undefined menu";
            var msg = string.Empty;

            try
            {
                if (_isDesignTime) return;

                if (!(sender is ToolStripMenuItem menuItem))
                    throw new Exception($"Unknown sender: \"{sender}\".");
                else
                    menuName = menuItem.Name;

                // Special handling of the select playlist menu
                if (menuName.StartsWith(nameof(FileSelectPlaylistMenuItem), StringComparison.InvariantCultureIgnoreCase))
                {
                    // Ignore any "Select playlist "branch" menu and only accept the "leaf"
                    if (menuItem.DropDownItems.Count > 0)
                        return;

                    // Get the MC playlist and let LyricsFinder know about it
                    await ReloadPlaylistAsync(false, menuName);
                }
                else
                {
                    switch (menuName)
                    {
                        case nameof(FileExitMenuItem):
                            // Close(); // Not done here, in standalone it is done in LyricsFinderExe, in plug-in it is done by Media Center
                            break;

                        case nameof(FileReloadMenuItem):
                            await ReloadPlaylistAsync(true);
                            break;

                        case nameof(FileSaveMenuItem):
                            await SaveAllAsync();
                            break;

                        case nameof(HelpAboutMenuItem):
                            using (var about = new AboutBox(this))
                                about.ShowDialog();
                            break;

                        case nameof(HelpBugReportsMenuItem):
                            System.Diagnostics.Process.Start("https://github.com/hardolf/JRiver.MediaCenter/projects/3");
                            break;

                        case nameof(HelpContentsMenuItem):
                            System.Diagnostics.Process.Start("https://github.com/hardolf/JRiver.MediaCenter/wiki/LyricsFinder-User-Manual");
                            break;

                        case nameof(HelpDevelopmentIssuesMenuItem):
                            System.Diagnostics.Process.Start("https://github.com/hardolf/JRiver.MediaCenter/projects/2");
                            break;

                        case nameof(HelpLookForUpdatesMenuItem):
                            await Model.Helpers.Utility.UpdateCheckWithRetriesAsync(EntryAssembly.GetName().Version, this.Size, true);
                            break;

                        case nameof(ToolsLyricServicesMenuItem):
                            using (var lyricsServiceForm = new LyricServiceForm(this, ShowServicesCallbackAsync))
                                lyricsServiceForm.ShowDialog(this);
                            break;

                        case nameof(ToolsOptionsMenuItem):
                            using (var frm = new OptionForm("LyricsFinder setup", LyricsFinderData))
                                frm.ShowDialog(this);
                            break;

                        case nameof(ToolsPlayJumpAheadLargeMenuItem):
                            await McPlayControl?.JumpAsync(false, true);
                            break;

                        case nameof(ToolsPlayJumpAheadSmallMenuItem):
                            await McPlayControl?.JumpAsync(false, false);
                            break;

                        case nameof(ToolsPlayJumpBackLargeMenuItem):
                            await McPlayControl?.JumpAsync(true, true);
                            break;

                        case nameof(ToolsPlayJumpBackSmallMenuItem):
                            await McPlayControl?.JumpAsync(true, false);
                            break;

                        case nameof(ToolsShowLogMenuItem):
                            Logging.ShowLogDir();
                            break;

                        case nameof(ToolsItemInfoMenuItem):
                            ShowItemInfo();
                            break;

                        case nameof(ToolsTestMenuItem):
                            // MessageBox.Show($"{Properties.Settings.Default.McWebServiceUser}", "Menu item selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case nameof(OverwriteExistingLyricsMenuItem):
                            OverwriteExistingLyricsMenuItem.Text = (OverwriteExistingLyricsMenuItem.Checked)
                                ? "Overwrite &existing lyrics"
                                : "Skip &existing lyrics";
                            break;

                        default:
                            throw new Exception($"Unknown menu item: \"{menuName}\".");
                    }
                }
            }
            catch (Exception ex)
            {
                await StatusMessageAsync($"Error {(msg.IsNullOrEmptyTrimmed() ? msg : msg + " ")}in menu item \"{menuName}\".", true, true);
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex, $"{(msg.IsNullOrEmptyTrimmed() ? msg : msg + " ")}in menu item: \"{menuName}\"");
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    StatusLogAsync("LyricsFinder for JRiver Media Center closed.");
                    StatusLogAsync(_logHeader + Constants.NewLine);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                    Dispose(true);
                }

                base.OnHandleDestroyed(e);
            }
            catch // (Exception ex)
            {
                // Let's ignore this!
                // ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Plays the item in the Playing Now list by the selected row index.
        /// </summary>
        public async Task PlayOrPauseAsync()
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
        /// Handles the Tick event of the ReadyTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void ReadyTimer_TickAsync(object sender, EventArgs e)
        {
            var begin = DateTime.Now;

            try
            {
                ReadyTimer.Stop();

                if (_isDesignTime) return;

                // Preload the Media Center playlists.
                await LoadPlaylistMenusAsync();

                var currentPlayListItemIds = new List<int>(_currentLyricsFinderPlaylist.Items.Keys);

                // Load the items' playlist IDs.
                // This is a long-running operation!
                ItemsPlayListIds?.Clear();
                ItemsPlayListIds = null;

                if (LyricsFinderData.MainData.CollectPlaylistInfoOnMcReconnect)
                    ItemsPlayListIds = await _currentUnsortedMcPlaylistsResponse.GetItemsPlaylistsAsync(currentPlayListItemIds, new[] { "Playlist" }, new[] { "Recent Playing Now" });

                // We only use this timer once in each session, when the check is successful, so no need to start it again
                // ReadyTimer.Start();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
            finally
            {
                var duration = DateTime.Now - begin;

                if (LyricsFinderData.MainData.CollectPlaylistInfoOnMcReconnect)
                    await StatusMessageAsync($"Collected playlists for the current items in {duration:m\\:ss}.", true, true, 10);
            }
        }


        /// <summary>
        /// Handles the Starting event of the StartStopToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void SearchAllStartStopButton_StartingAsync(object sender, StartStopButtonEventArgs e)
        {
            string msg;

            try
            {
                if (_isDesignTime) return;

                ToolsSearchAllStartStopButton.Start();

                // Start the automatic search process job
                _cancellationTokenSource = new CancellationTokenSource();
                await SearchAllProcessAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                msg = $"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event. ";
                await ErrorHandling.ShowAndLogDetailedErrorHandlerAsync(msg, ex);
            }
            finally
            {
                // UseWaitCursor = false;
            }
        }


        /// <summary>
        /// Handles the Stopping event of the StartStopToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void SearchAllStartStopButton_StoppingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                ToolsSearchAllStartStopButton.Stop();

                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
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

                await PlayOrPauseAsync();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
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

                var isRestart = (_playingKey != _selectedKey);

                await PlayStopAsync();

                if (isRestart)
                    ToolsPlayStartStopButton.PerformClick();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Starting event of the ToolsSearchAllStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void ToolsSearchAllStartStopButton_StartingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                SearchAllStartStopButton.Start();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the ToolsSearchAllStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void ToolsSearchAllStartStopButton_StoppingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                SearchAllStartStopButton.Stop();
            }
            catch (Exception ex)
            {
                await ErrorReportAsync(SharedComponents.Utility.GetActualAsyncMethodName(), ex);
            }
        }


        /// <summary>
        /// Handles the Tick event of the UpdateCheckTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void UpdateCheckTimer_TickAsync(object sender, EventArgs e)
        {
            try
            {
                UpdateCheckTimer.Stop();

                if (_isDesignTime) return;

                // Is it about time for a check?
                var updIntervalDays = LyricsFinderData.MainData.UpdateCheckIntervalDays;
                var daysSinceLast = (DateTime.Now - _lastUpdateCheck).TotalDays;

                if ((updIntervalDays == 0)
                    || ((updIntervalDays > 0) && (daysSinceLast >= updIntervalDays)))
                {
                    var version = Assembly.GetExecutingAssembly().GetName().Version;
                    var isUpdated = await Model.Helpers.Utility.UpdateCheckWithRetriesAsync(version, this.Size);

                    if (!isUpdated)
                        await Model.Helpers.Utility.UpdateCheckWithRetriesAsync(version, this.Size, true);

                    _lastUpdateCheck = DateTime.Now;
                    LyricsFinderData.MainData.LastUpdateCheck = _lastUpdateCheck;
                    await LyricsFinderData.SaveAsync();
                }

                // We only use this timer once in each session, when the check is successful, so no need to start it again
                // UpdateCheckTimer.Start();
            }
            catch // (Exception ex)
            {
                // We ignore this exception for now
                // ErrorReport(SharedComponents.Utility.GetActualAsyncMethodName(), ex);

                UpdateCheckTimer.Interval *= 10;
                UpdateCheckTimer.Start();
            }
        }

    }

}
