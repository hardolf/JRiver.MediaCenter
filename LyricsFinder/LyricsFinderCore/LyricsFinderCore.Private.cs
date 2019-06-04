using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
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

        private bool _isConnectedToMc = false;
        private bool _isDesignTime = false;
        private bool _isGridDataChanged = false; // Do not change this in code, always change the <c>IsDataChanged</c> property instead.
        private readonly bool _isStandAlone = true;

        private int _currentMouseColumnIndex = -1;
        private int _currentMouseRowIndex = -1;
        private int _playingIndex = -1;
        private static int _progressPercentage = -1;

        private readonly string _logHeader = "".PadRight(80, '-');
        private string _statusWarning = string.Empty;

        private BitmapForm _bitmapForm = null;
        private LyricForm _lyricsForm = null;
        private List<string> _noLyricsSearchList = new List<string>();
        //private ConfigurationFileChanger _configurationFileChanger;


        #region ErrorTest

        /// <summary>
        /// Test routine for the error handling.
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="Exception">Test error</exception>
        private void ErrorTest()
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
        /// Fills the data grid.
        /// </summary>
        /// <param name="items">The items.</param>
        private void FillDataGrid(Dictionary<int, McMplItem> items)
        {
            // ErrorTest();

            var dgv = MainDataGridView;

            dgv.Rows.Clear();
            IsDataChanged = false;

            foreach (var item in items)
            {
                var initStatus = LyricResultEnum.NotProcessedYet.ResultText();
                var row = new DataGridViewRow();
                var value = item.Value;
                Bitmap img = null;

                if ((value.Image == null) && !value.ImageFile.IsNullOrEmptyTrimmed())
                {
                    var fn = WebUtility.HtmlDecode(value.Filename);
                    var ifn = WebUtility.HtmlDecode(value.ImageFile);
                    var ifi = new FileInfo(ifn);

                    if (!ifi.Exists)
                    {
                        if (!value.Filename.IsNullOrEmptyTrimmed())
                        {
                            var dir = Path.GetDirectoryName(fn);

                            ifn = Path.Combine(dir, ifn);
                            ifi = new FileInfo(ifn);

                            if (!ifi.Exists)
                                ifn = string.Empty;
                        }
                    }

                    if (!ifn.IsNullOrEmptyTrimmed())
                    {
                        using (var tmp = new FileStream(ifn, FileMode.Open))
                        {
                            img = new Bitmap(tmp);
                        }
                    }
                }
                else
                    img = value.Image;

                var bmp = new Bitmap(16, 16);

                bmp.MakeTransparent();

                row.CreateCells(dgv, value.Key, bmp, img, WebUtility.HtmlDecode(value.Artist), WebUtility.HtmlDecode(value.Album), WebUtility.HtmlDecode(value.Name), value.Lyrics, initStatus);
                row.Height = dgv.Columns[(int)GridColumnEnum.Cover].Width;

                dgv.Rows.Add(row);
            }

            if (_playingIndex >= 0)
            {
                if (dgv.Rows.Count >= _playingIndex + 1)
                {
                    dgv.Rows[_playingIndex].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = (_playingIndex > 2) ? _playingIndex - 3 : 0;

                    ToolsPlayStartStopButton.Start();
                }
            }
            else
                ToolsPlayStartStopButton.Stop();

            if (ToolsPlayStartStopButton.GetStartingEventSubscribers().Length == 0)
                ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_Starting;

            if (ToolsPlayStartStopButton.GetStoppingEventSubscribers().Length == 0)
                ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_Stopping;

            McStatusTimer.Start();

            var x = IsDataChanged; // Force display of data change, if necessary
        }


        /// <summary>
        /// Finishes the data grid row.
        /// </summary>
        /// <param name="userState">State of the user.</param>
        /// <exception cref="Exception"></exception>
        private void FinishDataGridRow(WorkerUserState userState)
        {
            var dgv = MainDataGridView;

            if (dgv.Rows.Count - 1 < userState.CurrentItemIndex)
                return;

            var isOk = false;

            // Get the displayed rows index from the item key - and update it from the userState
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                var row = dgv.Rows[i];
                var key = (int)row.Cells[(int)GridColumnEnum.Key].Value;

                if (key == userState.CurrentItem.Key)
                {
                    var lyricsCell = row.Cells[(int)GridColumnEnum.Lyrics];
                    var statusCell = row.Cells[(int)GridColumnEnum.Status];
                    var newLyrics = userState.LyricsTextList.FirstOrDefault() ?? string.Empty;
                    var isNewLyricsDifferent = (newLyrics != (lyricsCell.Value?.ToString() ?? string.Empty));

                    statusCell.Value = userState.LyricsStatus;

                    if (isNewLyricsDifferent && (userState.LyricsStatus == LyricResultEnum.Found))
                    {
                        IsDataChanged = true;
                        lyricsCell.Value = newLyrics;
                    }

                    isOk = true;
                    // row.Selected = true;
                    dgv.CurrentCell = dgv.Rows[i].Cells[(int)GridColumnEnum.Artist];
                    break;
                }
            }

            var processIdx = userState.CurrentItemIndex;
            var processKey = userState.CurrentItem.Key;

            StatusLog($"FinishDataGridRow: currently processed item index: {userState.CurrentItemIndex}, key: {userState.CurrentItem.Key}.", true);

            if (!isOk)
                throw new Exception($"The currently processed item key ({userState.CurrentItem.Key}) is not found in the display list.");
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
                control.KeyDown += new KeyEventHandler(LyricsFinderCore_KeyDown);

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
        private void InitLocalData()
        {
            var dataFile = string.Empty;
            var dataDir = string.Empty;
            var tmpFile = string.Empty;

            try
            {
                Logging.Log(_progressPercentage, "Preparing load of local data...", true);
                dataFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.LocalAppDataFile));
                dataDir = Path.GetDirectoryName(dataFile);
                tmpFile = Path.Combine(dataDir, dataFile + ".tmp");

                // Try to create the data folder if necessary
                Logging.Log(_progressPercentage, $"Testing if local data directory \"{dataDir}\" is present, else creating it...", true);
                if (!Directory.Exists(dataDir))
                    Directory.CreateDirectory(dataDir);

                // Test if we may write files in the data folder
                Logging.Log(_progressPercentage, $"Testing if we may write to a file in the local data directory \"{tmpFile}\"...", true);
                using (var st = File.Create(tmpFile))

                    Logging.Log(_progressPercentage, $"Testing if we may delete the test file in the local data directory \"{tmpFile}\"...", true);
                File.Delete(tmpFile);
            }
            catch (Exception ex)
            {
                _statusWarning = $"Failed writing to the data folder \"{dataDir}\": {ex.Message}";
                ErrorHandling.ShowAndLogErrorHandler(_statusWarning, ex, _progressPercentage);
                StatusMessage("Warning");
            }

            try
            {
                Logging.Log(_progressPercentage, $"Initializing dynamic lyric services...", true);

                var services = InitLyricServices();

                // Prepare the load
                Logging.Log(_progressPercentage, "Preparing list of known XML types...", true);
                foreach (var service in services)
                {
                    LyricsFinderDataType.XmlKnownTypes.Add(service.GetType());
                }

                // Create LyricsFinderData with its list of lyrics services
                Logging.Log(_progressPercentage, "Loading local data from XML...", true);
                try
                {
                    // Load previously saved services
                    LyricsFinderData = LyricsFinderDataType.Load(dataFile);
                }
                catch (Exception ex)
                {
                    ErrorHandling.ErrorLog("Failed to load the lyric services, initializing a new set of services.", ex);

                    LyricsFinderData = new LyricsFinderDataType(dataFile);
                }

                // Add any lyric services that were not loaded before
                Logging.Log(_progressPercentage, "Adding additional lyric services...", true);
                foreach (var service in services)
                {
                    if (!LyricsFinderData.Services.Any(t => t.GetType() == service.GetType()))
                        LyricsFinderData.Services.Add(service);
                }

                Logging.Log(_progressPercentage, "Refreshing lyric services from their configurations...", true);
                foreach (var service in LyricsFinderData.Services)
                {
                    service.RefreshServiceSettings();
                }

                LyricsFinderData.Save();
            }
            catch (Exception ex)
            {
                _statusWarning = $"Failed initializing the lyric services.";
                ErrorHandling.ShowAndLogErrorHandler(_statusWarning, ex, _progressPercentage);
                StatusMessage("Warning");
            }
        }


        /// <summary>
        /// Initializes the logging.
        /// </summary>
        /// <param name="initMessages">The initialize messages.</param>
        private static void InitLogging(string[] initMessages = null)
        {
            var assy = Assembly.GetExecutingAssembly();
            var configFile = $"{assy.Location}.config";
            var fi = new FileInfo(configFile);

            log4net.Config.XmlConfigurator.ConfigureAndWatch(fi);

            if (LogManager.GetRepository().Configured)
            {
                if (initMessages != null)
                {
                    foreach (var msg in initMessages)
                    {
                        StatusLog(msg);
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

                ErrorHandling.ShowErrorHandler(sb.ToString(), _progressPercentage);
            }
        }


        /// <summary>
        /// Initializes the lyric services.
        /// </summary>
        /// <remarks>
        /// The lyric services are not referenced directly.
        /// Instead, they are loaded dynamically, thus making it easy to add or remove them from the application.
        /// </remarks>
        private static List<AbstractLyricService> InitLyricServices()
        {
            // Load the lyric service assemblies
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Logging.Log(_progressPercentage, $"Finding lyric service client assemblies in \"{dir}\"...", true);

            var files = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly);
            var ret = new List<AbstractLyricService>();

            // Load each assembly in the LyricServices folder
            Logging.Log(_progressPercentage, $"Loading dynamic lyric service client assemblies from \"{dir}\"...", true);
            foreach (var file in files)
            {
                Logging.Log(_progressPercentage, $"Looking at \"{file}\"...", true);

                var assy = Assembly.LoadFrom(file);

                // Get a list of the descendant lyrics service types
                Logging.Log(_progressPercentage, $"Trying to find service types in \"{file}\"...", true);

                var assyLyricsServiceTypes = assy
                   .GetTypes()
                   .Where(t => t.IsSubclassOf(typeof(AbstractLyricService)));

                // Create service instance(s)
                Logging.Log(_progressPercentage,
                    ((assyLyricsServiceTypes != null) && (assyLyricsServiceTypes.Count<Type>() > 0))
                    ? $"Creating service instance(s) from \"{file}\"..."
                    : $"No lyric services in \"{file}\"."
                    , true);

                foreach (var assyServiceType in assyLyricsServiceTypes)
                {
                    if (!ret.Any(t => t.GetType() == assyServiceType))
                    {
                        if (!(Activator.CreateInstance(assyServiceType) is AbstractLyricService newService))
                            throw new Exception($"Could not create instance of type \"{assyServiceType}\" or it was not an AbstractLyricService descendent type.");

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

            // Ensure the default of not overwriting (i.e. skipping) existing lyrics
            OverwriteMenuItem.Checked = false;
            MenuItem_Click(OverwriteMenuItem, new EventArgs());
        }


        /// <summary>
        /// Get the Media Center information and take som action with it.
        /// </summary>
        private void McInfo()
        {
            var mcInfo = McRestService.Info();

            _playingIndex = (mcInfo.Status.StartsWith("Play", StringComparison.InvariantCultureIgnoreCase))
                ? int.Parse(mcInfo.PlayingNowPosition, NumberStyles.None, CultureInfo.InvariantCulture)
                : -1;
        }


        /// <summary>
        /// Plays the item in the Playing Now list by the selected row index.
        /// </summary>
        private void PlayOrPause()
        {
            var rows = MainDataGridView.SelectedRows;

            if (rows.Count < 1)
                return;

            var rowIdx = rows[0].Index;

            if (rowIdx == _playingIndex)
                McRestService.PlayPause();
            else
                McRestService.PlayByIndex(rowIdx);

            _playingIndex = rowIdx;
        }


        /// <summary>
        /// Stops playing any item.
        /// </summary>
        private void PlayStop()
        {
            McRestService.PlayStop();
        }


        /// <summary>
        /// Saves the <c>LyricsFinderData</c> and the playlist items to the Media Center.
        /// </summary>
        private void Save()
        {
            if (!IsDataChanged) return;

            if (ProcessWorker.IsBusy)
                throw new Exception("You cannot save while the search process is running.");

            LyricsFinderData.Save();

            var lyricsResultTest = $"{LyricResultEnum.Found.ResultText()}|{LyricResultEnum.ManuallyEdited.ResultText()}".ToUpperInvariant();

            // Iterate the displayed rows and save each row, if it is found or manually edited
            for (int i = 0; i < MainDataGridView.Rows.Count; i++)
            {
                var row = MainDataGridView.Rows[i] as DataGridViewRow;

                var keyTxt = row.Cells[(int)GridColumnEnum.Key].Value?.ToString();
                var lyrics = row.Cells[(int)GridColumnEnum.Lyrics].Value?.ToString();
                var status = row.Cells[(int)GridColumnEnum.Status].Value?.ToString();

                if ((!lyrics.IsNullOrEmptyTrimmed()) && lyricsResultTest.Contains(status.ToUpperInvariant()))
                {
                    if (!int.TryParse(keyTxt, out var key) || lyrics.IsNullOrEmptyTrimmed() || status.IsNullOrEmptyTrimmed())
                        throw new Exception($"Error parsing row {i} cell(s): keyTxt=\"{keyTxt}\", lyrics=\"{lyrics}\", status=\"{status}\".");

                    var rsp = McRestService.SetInfo(key, "Lyrics", lyrics);
                }
            }

            // Force a list refresh
            _isConnectedToMc = false;
            IsDataChanged = false;
            MainDataGridView.Rows.Clear();
            ProcessWorker.RunWorkerAsync();

            SaveFormSettings();
        }


        /// <summary>
        /// Saves the form settings.
        /// </summary>
        private static void SaveFormSettings()
        {
            Properties.Settings.Default.Save();
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
            var dgv = MainDataGridView;

            if ((colIdx < 0) || (colIdx > dgv.ColumnCount - 1)) return null;
            if ((rowIdx < 0) || (rowIdx > dgv.RowCount - 1)) return null;

            var row = dgv.Rows[rowIdx];
            var rect = dgv.RectangleToScreen(dgv.GetCellDisplayRectangle(colIdx, rowIdx, true));

            if (!(row.Cells[colIdx] is DataGridViewImageCell cell))
                throw new ArgumentException($"Column {colIdx} is not a DataGridViewImageCell.");

            if (!(cell.Value is Bitmap img))
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
        /// <returns></returns>
        /// <exception cref="ArgumentException">Column {colIdx}</exception>
        private LyricForm ShowLyrics(int colIdx, int rowIdx)
        {
            var dgv = MainDataGridView;

            if ((colIdx < 0) || (colIdx > dgv.ColumnCount - 1)) return null;
            if ((rowIdx < 0) || (rowIdx > dgv.RowCount - 1)) return null;

            var row = dgv.Rows[rowIdx];
            var rect = dgv.RectangleToScreen(dgv.GetCellDisplayRectangle(colIdx, rowIdx, true));

            if (!(row.Cells[colIdx] is DataGridViewTextBoxCell cell))
                throw new ArgumentException($"Column {colIdx} is not a DataGridViewTextBoxCell.");

            var location = new Point(MousePosition.X, MousePosition.Y);
            var size = Properties.Settings.Default.LyricsFormSize;
            var ret = new LyricForm(cell, location, size, ShowLyricsCallback, LyricsFinderData);

            ret.ShowDialog();

            return ret;
        }


        /// <summary>
        /// Shows the lyrics callback.
        /// </summary>
        /// <param name="lyricsForm">The lyrics form.</param>
        private void ShowLyricsCallback(LyricForm lyricsForm)
        {
            try
            {
                if (lyricsForm.Result == DialogResult.Yes)
                {
                    // Display the created/changed lyrics in the list
                    lyricsForm.LyricCell.Value = lyricsForm.Lyric;

                    var row = lyricsForm.LyricCell.OwningRow;

                    row.Selected = true;
                    row.Cells[(int)GridColumnEnum.Status].Value = LyricResultEnum.ManuallyEdited.ResultText();

                    IsDataChanged = true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod()} event.", ex, _progressPercentage);
            }
        }


        /// <summary>
        /// Shows the services callback.
        /// </summary>
        /// <param name="lyricsServiceForm">The lyrics service form.</param>
        internal void ShowServicesCallback(LyricServiceForm lyricsServiceForm)
        {
            try
            {
                LyricsFinderData.Save();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod()} event.", ex, _progressPercentage);
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
        /// <param name="msg">The message.</param>
        /// <param name="isDebug">if set to <c>true</c> [is debug].</param>
        private static void StatusLog(string msg, bool isDebug = false)
        {
            Logging.Log(_progressPercentage, msg, isDebug);
        }


        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="ex">The exception.</param>
        private static void StatusLog(string msg, Exception ex)
        {
            Logging.Log(_progressPercentage, msg, ex);
        }


        /// <summary>
        /// Shows status message.
        /// </summary>
        /// <param name="msg">The message.</param>
        private void StatusMessage(string msg)
        {
            if (!_statusWarning.IsNullOrEmptyTrimmed())
                msg = $"{msg} - {_statusWarning}";

            if (MainStatusLabel.Text != msg)
            {
                MainStatusLabel.Text = msg;
                MainStatusLabel.ToolTipText = msg;
            }

            if (_progressPercentage >= 0)
                MainProgressBar.Value = _progressPercentage;
        }

    }

}