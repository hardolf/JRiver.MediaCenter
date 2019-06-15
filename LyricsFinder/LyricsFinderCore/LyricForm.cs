using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

    /// <summary>
    /// Form to show the lyrics.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class LyricForm : Form
    {

        /// <summary>
        /// The callback function.
        /// </summary>
        private Action<LyricForm> _callback;

        /// <summary>
        /// The initial lyric from as of the form load.
        /// </summary>
        private string _initLyric = string.Empty;

        /// <summary>
        /// Set to <c>true</c> if the form is a search form; otherwase set <c>false</c>.
        /// </summary>
        private bool _isSearch = false;

        /// <summary>
        /// The final lyric as of the form closing.
        /// </summary>
        private string _finalLyric = string.Empty;

        /// <summary>
        /// The search form.
        /// </summary>
        private LyricForm _searchForm = null;

        /// <summary>
        /// The found lyric list with credits.
        /// </summary>
        private List<FoundLyricType> _foundLyricList = new List<FoundLyricType>();

        /// <summary>
        /// The Media Center item from the caller.
        /// </summary>
        private McMplItem _McItem = null;

        /// <summary>
        /// The hit count for each of the services.
        /// </summary>
        private Dictionary<string, int> _serviceCounts = new Dictionary<string, int>();


        /// <summary>
        /// Gets the lyric cell.
        /// </summary>
        /// <value>
        /// The lyric cell.
        /// </value>
        public DataGridViewTextBoxCell LyricCell { get; private set; }


        /// <summary>
        /// Gets the lyric.
        /// </summary>
        /// <value>
        /// The lyric.
        /// </value>
        public string Lyric { get; private set; }

        /// <summary>
        /// Gets or sets the lyric finder data.
        /// </summary>
        /// <value>
        /// The lyric finder data.
        /// </value>
        private LyricsFinderDataType LyricsFinderData { get; set; }

        /// <summary>
        /// Gets the dialog result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public DialogResult Result { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricForm"/> class.
        /// </summary>
        protected LyricForm()
        {
            InitializeComponent();

            AllowTransparency = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricForm" /> class.
        /// </summary>
        /// <param name="lyricCell">The lyric cell.</param>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="lyricsFinderData">The lyrics finder data.</param>
        /// <param name="isSearch">if set to <c>true</c> [is search].</param>
        /// <param name="artist">The artist, only used for search.</param>
        /// <param name="album">The album, only used for search.</param>
        /// <param name="track">The track, only used for search.</param>
        /// <exception cref="ArgumentNullException">
        /// lyricCell
        /// or
        /// callback
        /// or
        /// lyricsFinderData
        /// </exception>
        internal LyricForm(DataGridViewTextBoxCell lyricCell, Point location, Size? size, Action<LyricForm> callback, LyricsFinderDataType lyricsFinderData, bool isSearch = false, string artist = null, string album = null, string track = null)
            : this()
        {
#pragma warning disable IDE0016 // Use 'throw' expression
            if (lyricCell == null) throw new ArgumentNullException(nameof(lyricCell));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
#pragma warning restore IDE0016 // Use 'throw' expression

            var lyric = (lyricCell.Value as string)?.LfToCrLf() ?? string.Empty;

            _callback = callback;
            _initLyric = lyric?.Trim() ?? string.Empty;
            _finalLyric = _initLyric;
            _isSearch = isSearch;
            LyricsFinderData = lyricsFinderData;

            if (size != null)
                Size = size.Value;

            // Create a pseudo MC playlist item for the search
            var row = lyricCell.OwningRow;

            if (isSearch)
            {
                // Cursor.Current = Cursors.WaitCursor;
                location.Offset(-Size.Width + 10, 0);
                ArtistTextBox.ReadOnly = true;
                AlbumTextBox.ReadOnly = true;
                TrackTextBox.ReadOnly = true;
                LyricTextBox.ReadOnly = true;
                LyricFormTrackBar.Enabled = true;
                LyricFormTrackBar.Visible = true;
                LyricFormTrackBar.Select();
                SearchButton.Enabled = false;
                SearchButton.Visible = false;
                UseWaitCursor = true;

                _McItem = new McMplItem
                {
                    Artist = artist,
                    Album = album,
                    Name = track
                };
            }
            else
            {
                location.Offset(-Size.Width - 10, -Convert.ToInt32(Size.Height / 2));
                ArtistTextBox.ReadOnly = false;
                AlbumTextBox.ReadOnly = false;
                TrackTextBox.ReadOnly = false;
                LyricTextBox.ReadOnly = false;
                LyricFormTrackBar.Enabled = false;
                LyricFormTrackBar.Visible = false;
                LyricTextBox.Text = _initLyric;
                SearchButton.Enabled = true;
                SearchButton.Visible = true;
                UseWaitCursor = false;

                _McItem = new McMplItem
                {
                    Artist = row.Cells[(int)GridColumnEnum.Artist].Value as string,
                    Album = row.Cells[(int)GridColumnEnum.Album].Value as string,
                    Name = row.Cells[(int)GridColumnEnum.Title].Value as string
                };
            }

            ArtistTextBox.Text = _McItem.Artist;
            AlbumTextBox.Text = _McItem.Album;
            TrackTextBox.Text = _McItem.Name;

            Location = location;
            LyricCell = lyricCell;
            LyricTextBox.SelectionStart = 0;
            LyricTextBox.SelectionLength = 0;
            Text = lyricCell.OwningRow.Cells[(int)GridColumnEnum.Title].Value as string;
        }


        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the FormClosing event of the LyricForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void LyricForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _finalLyric = LyricTextBox.Text.Trim();

                if (!_isSearch && (_searchForm != null))
                {
                    _searchForm.Close();
                    _searchForm = null;
                }

                var question = (_isSearch)
                    ? "Selected lyric is different from the old lyric\nDo you want to use the new selected lyric?"
                    : "Lyric is changed\nDo you want to use the new lyric?";

                if (_finalLyric == _initLyric)
                {
                    e.Cancel = false;
                    Result = DialogResult.No;
                }
                else if (_isSearch && _finalLyric.IsNullOrEmptyTrimmed())
                {
                    e.Cancel = false;
                    Result = DialogResult.No;
                }
                else
                {
                    Result = MessageBox.Show(this, question, "Lyric changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    switch (Result)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;

                        case DialogResult.No:
                            e.Cancel = false;
                            Lyric = _initLyric;
                            _callback(this);
                            break;

                        case DialogResult.Yes:
                            e.Cancel = false;
                            Lyric = _finalLyric;
                            _callback(this);
                            break;

                        default:
                            break;
                    }
                }

                Properties.Settings.Default.LyricsFormSize = Size;
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the LyricForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void LyricForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = false;

                if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Load event of the LyricForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (_isSearch)
                {
                    LyricFormStatusLabel.Text = "Searching...";

                    LyricFormTimer.Start();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Tick event of the LyricFormTimer control.
        /// Used when searching for lyrics.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricFormTimer_Tick(object sender, EventArgs e)
        {
            var msg = string.Empty;

            try
            {
                LyricFormTimer.Stop();

                // Clear list and search for all the lyrics in each lyric service
                _foundLyricList.Clear();

                foreach (var service in LyricsFinderData.Services)
                {
                    if (!service.IsImplemented || !service.IsActive || service.IsQuotaExceeded) continue;

                    msg = $" in \"{service?.Credit.ServiceName ?? "Unknown service"}\" service";

                    service.Process(_McItem, true);

                    if (service.LyricResult != LyricResultEnum.Found) continue;

                    foreach (var foundLyric in service.FoundLyricList)
                    {
                        _foundLyricList.Add(foundLyric);

                        var serviceName = foundLyric.Service.Credit.ServiceName;
                        var serviceCount = 0;

                        // Keep track of the number of hits for each lyric service
                        if (_serviceCounts.ContainsKey(serviceName))
                        {
                            serviceCount = _serviceCounts.First(s => s.Key == serviceName).Value;
                            _serviceCounts.Remove(serviceName);
                        }

                        _serviceCounts.Add(serviceName, serviceCount + 1);
                    }
                }

                // No more service-specific error details
                msg = string.Empty;

                // Set the trackbar and call the Scroll event handler to initialize the text box
                LyricFormTrackBar.Maximum = _foundLyricList.Count - 1;
                LyricFormTrackBar_Scroll(this, new EventArgs());

                LyricFormStatusLabel.Text = $"{_foundLyricList.Count} lyrics found";

                LyricsFinderData.Save();
                UseWaitCursor = false;
            }
            catch (LyricsQuotaExceededException ex)
            {
                // Cursor.Current = Cursors.Default;
                UseWaitCursor = false;
                ErrorHandling.ShowErrorHandler($"Error{msg}: {ex.Message}.");
                Close();
            }
            catch (Exception ex)
            {
                // Cursor.Current = Cursors.Default;
                UseWaitCursor = false;
                ErrorHandling.ShowAndLogErrorHandler($"Error{msg} in {MethodBase.GetCurrentMethod().Name} event.", ex);
                Close();
            }
        }


        /// <summary>
        /// Handles the Scroll event of the LyricTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricFormTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (_foundLyricList.Count == 0) return;

                var foundLyric = _foundLyricList[LyricFormTrackBar.Value];
                var serviceName = foundLyric.Service.Credit.ServiceName;

                LyricTextBox.Text = foundLyric.ToString();
                LyricTextBox.Select(0, 0);
                LyricTextBox.SelectionLength = 0;

                Lyric = LyricTextBox.Text;

                LyricFormFoundStatusLabel.Text = $"Source: {serviceName} {GetServiceCount(serviceName)}";
                LyricFormFoundStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Gets the service count.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Hit count of the specified service.</returns>
        private object GetServiceCount(string serviceName)
        {
            var count = _serviceCounts.First(s => s.Key == serviceName).Value;

            return (count > 1) ? $"({count})" : string.Empty;
        }


        /// <summary>
        /// Handles the Click event of the SearchButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a pseudo MC playlist item for the search
                var row = LyricCell.OwningRow;

                _searchForm = new LyricForm(LyricCell, Location, Size, SearchLyricCallback, LyricsFinderData, true, ArtistTextBox.Text, AlbumTextBox.Text, TrackTextBox.Text);
                _searchForm.Show(this);

            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        private void SearchLyricCallback(LyricForm lyricForm)
        {
            try
            {
                if (!_isSearch && (_searchForm != null))
                {
                    switch (lyricForm.Result)
                    {
                        case DialogResult.Cancel:
                        case DialogResult.No:
                            break;

                        case DialogResult.Yes:
                            LyricTextBox.Text = _searchForm.Lyric;
                            break;

                        default:
                            throw new Exception($"Unknown DialogResult: \"{lyricForm.Result}\".");
                    }

                    _searchForm = null;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }

    }

}
