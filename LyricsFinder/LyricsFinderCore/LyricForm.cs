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
        /// The initial lyrics from as of the form load.
        /// </summary>
        private string _initLyrics = string.Empty;

        /// <summary>
        /// Set to <c>true</c> if the form is a search form; otherwase set <c>false</c>.
        /// </summary>
        private bool _isSearch = false;

        /// <summary>
        /// The final lyrics as of the form closing.
        /// </summary>
        private string _finalLyrics = string.Empty;

        /// <summary>
        /// The search form.
        /// </summary>
        private LyricForm _searchForm = null;

        /// <summary>
        /// The found lyrics list with credits.
        /// </summary>
        private List<string> _foundLyricsListWithCredits = new List<string>();

        /// <summary>
        /// The Media Center item from the caller.
        /// </summary>
        private McMplItem _McItem = null;


        /// <summary>
        /// Gets the lyrics cell.
        /// </summary>
        /// <value>
        /// The lyrics cell.
        /// </value>
        public DataGridViewTextBoxCell LyricsCell { get; private set; }


        /// <summary>
        /// Gets the lyrics.
        /// </summary>
        /// <value>
        /// The lyrics.
        /// </value>
        public string Lyrics { get; private set; }

        /// <summary>
        /// Gets or sets the lyrics finder data.
        /// </summary>
        /// <value>
        /// The lyrics finder data.
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
        /// <param name="lyricsCell">The lyrics cell.</param>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="lyricsFinderData">The lyrics finder data.</param>
        /// <param name="isSearch">if set to <c>true</c> [is search].</param>
        /// <param name="artist">The artist, only used for search.</param>
        /// <param name="album">The album, only used for search.</param>
        /// <param name="track">The track, only used for search.</param>
        internal LyricForm(DataGridViewTextBoxCell lyricsCell, Point location, Size? size, Action<LyricForm> callback, LyricsFinderDataType lyricsFinderData, bool isSearch = false, string artist = null, string album = null, string track = null)
            : this()
        {
#pragma warning disable IDE0016 // Use 'throw' expression
            if (lyricsCell == null) throw new ArgumentNullException(nameof(lyricsCell));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (lyricsFinderData == null) throw new ArgumentNullException(nameof(lyricsFinderData));
#pragma warning restore IDE0016 // Use 'throw' expression

            var lyrics = (lyricsCell.Value as string) ?? string.Empty;

            _callback = callback;
            _initLyrics = lyrics?.Trim() ?? string.Empty;
            _finalLyrics = _initLyrics;
            _isSearch = isSearch;
            LyricsFinderData = lyricsFinderData;

            if (size != null)
                Size = size.Value;

            // Create a pseudo MC playlist item for the search
            var row = lyricsCell.OwningRow;

            if (isSearch)
            {
                // Cursor.Current = Cursors.WaitCursor;
                location.Offset(-Size.Width + 10, 0);
                ArtistTextBox.ReadOnly = true;
                AlbumTextBox.ReadOnly = true;
                TrackTextBox.ReadOnly = true;
                LyricsTextBox.ReadOnly = true;
                LyricsFormTrackBar.Enabled = true;
                LyricsFormTrackBar.Visible = true;
                LyricsFormTrackBar.Select();
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
                LyricsTextBox.ReadOnly = false;
                LyricsFormTrackBar.Enabled = false;
                LyricsFormTrackBar.Visible = false;
                LyricsTextBox.Text = _initLyrics;
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
            LyricsCell = lyricsCell;
            LyricsTextBox.SelectionStart = 0;
            LyricsTextBox.SelectionLength = 0;
            Text = lyricsCell.OwningRow.Cells[(int)GridColumnEnum.Title].Value as string;
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
        /// Handles the FormClosing event of the LyricsForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void LyricsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _finalLyrics = LyricsTextBox.Text.Trim();

                if (!_isSearch && (_searchForm != null))
                {
                    _searchForm.Close();
                    _searchForm = null;
                }

                var question = (_isSearch)
                    ? "Selected lyrics are different from the old lyrics\nDo you want to use the new selected lyrics?"
                    : "Lyrics are changed\nDo you want to use the new lyrics?";

                if (_finalLyrics == _initLyrics)
                {
                    e.Cancel = false;
                    Result = DialogResult.No;
                }
                else if (_isSearch && _finalLyrics.IsNullOrEmptyTrimmed())
                {
                    e.Cancel = false;
                    Result = DialogResult.No;
                }
                else
                {
                    Result = MessageBox.Show(this, question, "Lyrics changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    switch (Result)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;

                        case DialogResult.No:
                            e.Cancel = false;
                            Lyrics = _initLyrics;
                            _callback(this);
                            break;

                        case DialogResult.Yes:
                            e.Cancel = false;
                            Lyrics = _finalLyrics;
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
        /// Handles the KeyDown event of the LyricsForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void LyricsForm_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;

            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
            }
        }


        /// <summary>
        /// Handles the Load event of the LyricsForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricsForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (_isSearch)
                {
                    LyricsFormStatusLabel.Text = "Searching...";

                    LyricsFormTimer.Start();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Tick event of the LyricsFormTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricsFormTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                LyricsFormTimer.Stop();

                // Search for all the lyrics in each service
                _foundLyricsListWithCredits.Clear();

                foreach (var service in LyricsFinderData.Services)
                {
                    if (!service.IsActive || service.IsQuotaExceeded) continue;

                    service.Process(_McItem, true);

                    if (service.LyricsResult != LyricsResultEnum.Found) continue;

                    foreach (var foundLyrics in service.FoundLyricsList)
                    {
                        var newLyrics = foundLyrics.ToString();

                        _foundLyricsListWithCredits.Add(newLyrics);
                    }
                }

                // Set the trackbar and call the Scroll event handler to initialize the text box
                LyricsFormTrackBar.Maximum = _foundLyricsListWithCredits.Count - 1;
                LyricsFormTrackBar_Scroll(this, new EventArgs());

                LyricsFormStatusLabel.Text = $"{_foundLyricsListWithCredits.Count} lyrics found";

                LyricsFinderData.Save();
                UseWaitCursor = false;
            }
            catch (LyricsQuotaExceededException ex)
            {
                // Cursor.Current = Cursors.Default;
                UseWaitCursor = false;
                ErrorHandling.ShowErrorHandler(ex.Message);
                Close();
            }
            catch (Exception ex)
            {
                // Cursor.Current = Cursors.Default;
                UseWaitCursor = false;
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
                Close();
            }
        }


        /// <summary>
        /// Handles the Scroll event of the LyricsTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LyricsFormTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (_foundLyricsListWithCredits.Count == 0) return;

                Lyrics = LyricsTextBox.Text;
                LyricsTextBox.Text = _foundLyricsListWithCredits[LyricsFormTrackBar.Value];
                LyricsTextBox.Select(0, 0);
                LyricsTextBox.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
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
                var row = LyricsCell.OwningRow;

                _searchForm = new LyricForm(LyricsCell, Location, Size, SearchLyricsCallback, LyricsFinderData, true, ArtistTextBox.Text, AlbumTextBox.Text, TrackTextBox.Text);
                _searchForm.Show(this);

            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        private void SearchLyricsCallback(LyricForm lyricsForm)
        {
            try
            {
                if (!_isSearch && (_searchForm != null))
                {
                    switch (lyricsForm.Result)
                    {
                        case DialogResult.Cancel:
                        case DialogResult.No:
                            break;

                        case DialogResult.Yes:
                            LyricsTextBox.Text = _searchForm.Lyrics;
                            break;

                        default:
                            throw new Exception($"Unknown DialogResult: \"{lyricsForm.Result}\".");
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
