/*
Description: Lyrics finder for JRiver Media Center

Author: Hardolf

Core module creation date:
2018.04.12

Version Number:
1.0.0

Modified: 2019.05.25 by Hardolf.
*/


using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
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

            if (!_isDesignTime)
            {
                // Upgrade User Settings from previous version the first time
                if (Properties.Settings.Default.UpgradeSettings)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.UpgradeSettings = false;
                }
            }

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
        /// Handles the KeyDown event of the LyricsFinderCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void LyricsFinderCore_KeyDown(object sender, KeyEventArgs e)
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
                    PlayOrPause();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Load event of the LyricsFinderCore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricsFinderCore_Load(object sender, EventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (ProcessWorker.IsBusy) return;

                var msg = "LyricsFinder initializes...";

                Init();

                MainDataGridView.Select();
                Thread.Sleep(100);

                _progressPercentage = 0;
                _noLyricsSearchList.AddRange(Properties.Settings.Default.McNoLyricsSearchList.Split(',', ';'));

                StatusLog(msg);
                StatusMessage(msg);

                ProcessWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the ItemClicked event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void MainContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                        PlayOrPause();
                }
                else if (e.ClickedItem == ContextPlayStopMenuItem)
                    ToolsPlayStartStopButton.Stop();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Opening event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void MainContextMenu_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;
                if (MainDataGridView.SelectedRows.Count < 1)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                if ((_lyricsForm != null) && _lyricsForm.Visible && !Properties.Settings.Default.MouseMoveOpenLyricsForm) return;
                if ((e.ColumnIndex == (int)GridColumnEnum.Lyrics) && !Properties.Settings.Default.MouseMoveOpenLyricsForm) return;
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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                if (!Properties.Settings.Default.MouseMoveOpenLyricsForm) return;

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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Tick event of the McStatusTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>The timer polls Media Center for the playing item, if any, and sets the PlayImage accordingly.</remarks>
        private void McStatusTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                McStatusTimer.Stop();

                var rows = MainDataGridView.Rows;
                var mcInfo = McRestService.Info();

                if (mcInfo != null)
                {
                    var playIdx = int.Parse(mcInfo.PlayingNowPosition, NumberStyles.None, CultureInfo.InvariantCulture);

                    if ((playIdx >= 0) && (playIdx < rows.Count))
                    {
                        var row = rows[playIdx];
                        var cell = row.Cells[(int)GridColumnEnum.PlayImage] as DataGridViewImageCell;
                        var blank = new Bitmap(16, 16);

                        BlankPlayStatusBitmaps(playIdx); // Clear all other bitmaps than the one in playIdx row

                        if (mcInfo.Status?.StartsWith("Play", StringComparison.InvariantCultureIgnoreCase) ?? false)
                        {
                            cell.Value = Properties.Resources.Play;
                        }
                        else if (mcInfo.Status?.StartsWith("Pause", StringComparison.InvariantCultureIgnoreCase) ?? false)
                        {
                            cell.Value = Properties.Resources.Pause;
                        }
                        else
                        {
                            cell.Value = blank;
                        }
                    } 
                }

                McStatusTimer.Interval = _McStatusIntervalNormal;
                McStatusTimer.Start();
            }
            catch (Exception ex)
            {
                // We don't bother the user with this error, since MC could just be shut down.
                // Instead we set up the timer interval and try again.
                ErrorHandling.ErrorLog($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
                BlankPlayStatusBitmaps();
                McStatusTimer.Interval = _McStatusIntervalError;
                McStatusTimer.Start();
            }
        }


        /// <summary>
        /// Handles the Click event of the ExitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MenuItem_Click(object sender, EventArgs e)
        {
            var itemName = "Undefined item";

            try
            {
                if (_isDesignTime) return;
                // MessageBox.Show($"Click!\n{sender}", "Click", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!(sender is ToolStripMenuItem menuItem))
                    throw new Exception($"Unknown sender: \"{sender}\".");
                else
                    itemName = menuItem.Name;

                var position = menuItem.Owner.Location;

                position.Offset(menuItem.Bounds.Size.Width, 0);

                switch (itemName)
                {
                    case nameof(FileReloadMenuItem):
                        _isConnectedToMc = false;
                        LyricsFinderCore_Load(this, new EventArgs());
                        break;

                    case nameof(FileSaveMenuItem):
                        Save();
                        break;

                    case nameof(FileExitMenuItem):
                        // Close(); // Not done here, in stanalone it is done in LyricsFinderExe, in plug-in it is done by Media Center
                        break;

                    case nameof(HelpAboutMenuItem):
                        var about = new AboutBox(EntryAssembly);
                        about.ShowDialog();
                        break;

                    case nameof(HelpContentsMenuItem):
                        var url = "https://github.com/hardolf/JRiver.MediaCenter/wiki/LyricsFinder-User-Manual";
                        System.Diagnostics.Process.Start(url);
                        break;

                    case nameof(HelpLookForUpdatesMenuItem):
                        Model.Helpers.Utility.UpdateCheck(EntryAssembly.GetName().Version, true);
                        break;

                    case nameof(ToolsLyricsServicesMenuItem):
                        var lyricsServiceForm = new LyricServiceForm(LyricsFinderData, position, ShowServicesCallback);
                        lyricsServiceForm.ShowDialog(this);
                        break;

                    case nameof(ToolsMcWsConnectionpMenuItem):
                        var frm = new ConfigurationForm("LyricsFinder connection setup");
                        frm.ShowDialog(this);
                        break;

                    case nameof(ToolsShowLogMenuItem):
                        Logging.ShowLog();
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
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in menu item: \"{itemName}\" in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                if (!_isDesignTime)
                {
                    _progressPercentage = -1;

                    if (ProcessWorker.WorkerSupportsCancellation)
                        ProcessWorker.CancelAsync();

                    StatusLog("LyricsFinder for JRiver Media Center closed.");
                    StatusLog(_logHeader + Environment.NewLine);

                    SaveFormSettings();
                }

                base.OnHandleDestroyed(e);
            }
            catch (Exception ex)
            {
                if (ProcessWorker.WorkerSupportsCancellation)
                    ProcessWorker.CancelAsync();

                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the DoWork event of the ProcessWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs" /> instance containing the event data.</param>
        private void ProcessWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                var worker = sender as BackgroundWorker;

                if (_isConnectedToMc)
                    Process(worker, e);
                else
                    Connect(worker);
            }
            catch
            {
                if (ProcessWorker.WorkerSupportsCancellation)
                    ProcessWorker.CancelAsync();

                throw; // This exception is available in the RunWorkerCompletedEventArgs in the ProcessWorker_RunWorkerCompleted event
            }
        }


        /// <summary>
        /// Handles the ProgressChanged event of the ProcessWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs" /> instance containing the event data.</param>
        private void ProcessWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                _progressPercentage = e.ProgressPercentage;

                var userState = e.UserState as WorkerUserState;
                var currentDataGridViewItem = MainDataGridView.CurrentRow;

                // Write the log and the status label
                StatusLog(userState?.Message ?? "");
                StatusMessage(userState?.Message ?? "");

                var isInSync = ((userState != null) && (userState.CurrentItem != null) && (MainDataGridView.Rows.Count > 0) && (MainDataGridView.Rows.Count == userState.Items.Count));

                // Update the item list in GUI, if empty
                if (!isInSync && (userState.Items.Count > 0))
                    FillDataGrid(userState.Items);

                // Finish the item row, e.g. set the item status and found lyrics
                if (isInSync)
                    FinishDataGridRow(userState);
            }
            catch (Exception ex)
            {
                if (ProcessWorker.WorkerSupportsCancellation)
                    ProcessWorker.CancelAsync();

                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the RunWorkerCompleted event of the ProcessWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void ProcessWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                var msg = string.Empty;

                if (e.Cancelled == true)
                {
                    //msg = "Process canceled.";
                    //StatusLog(msg);
                    //StatusMessage(msg);
                }
                else if (e.Error != null)
                {
                    _statusWarning = $"{e.Error.Message}";
                    StatusMessage("Warning");

                    if (e.Error is LyricsQuotaExceededException)
                        ErrorHandling.ShowErrorHandler(this, e.Error.Message);
                    else
                        ErrorHandling.ShowAndLogErrorHandler("Error handling background worker event in LyricsFinder.", e.Error, _progressPercentage);
                }
                else
                {
                    _progressPercentage = 100;
                    //msg = "Process completed.";
                    //StatusLog(msg);
                    //StatusMessage(msg);
                }

                SearchAllStartStopButton.Stop();
                SearchAllStartStopButton.Checked = false;
            }
            catch (Exception ex)
            {
                if (ProcessWorker.WorkerSupportsCancellation)
                    ProcessWorker.CancelAsync();

                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Starting event of the StartStopToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void StartStopButton_Starting(object sender, StartStopButtonEventArgs e)
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

                // Start the bacground worker
                if (!ProcessWorker.IsBusy)
                    ProcessWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the StartStopToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void StartStopButton_Stopping(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                if (ToolsSearchAllStartStopButton.IsRunning)
                    ToolsSearchAllStartStopButton.Stop();

                if (ProcessWorker.WorkerSupportsCancellation)
                    ProcessWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Starting event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void ToolsPlayStartStopButton_Starting(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                var dvg = MainDataGridView;

                if (dvg.SelectedRows.Count > 0)
                    PlayOrPause();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private void ToolsPlayStartStopButton_Stopping(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                if (_isDesignTime) return;

                PlayStop();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
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
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex, _progressPercentage);
            }
        }

    }

}
