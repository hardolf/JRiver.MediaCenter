using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Form to show the lyrics.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class LyricForm : Form
    {

        /***********************************/
        /***** Private assembly-wide   *****/
        /***** constants and variables *****/
        /***********************************/

        private Action<LyricForm> _callback;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private string _initLyric = string.Empty;
        private bool _isSearch = false;
        private string _finalLyric = string.Empty;
        private LyricForm _searchForm = null;
        private List<FoundLyricType> _foundLyricList = new List<FoundLyricType>();
        private McMplItem _McItem = null;
        private Dictionary<string, int> _serviceCounts = new Dictionary<string, int>();


        /**********************/
        /***** Properties *****/
        /**********************/

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


        /************************/
        /***** Constructors *****/
        /************************/

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
                LyricTextBox.Text = _initLyric;
                LyricTextBox.Select();
                LyricFormTrackBar.Enabled = false;
                LyricFormTrackBar.Visible = false;
                SearchButton.Enabled = true;
                SearchButton.Visible = true;

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


        /*********************/
        /***** Delegates *****/
        /*********************/

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void CloseButton_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the FormClosing event of the LyricForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private async void LyricForm_FormClosingAsync(object sender, FormClosingEventArgs e)
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

                if (_isSearch)
                    _callback(this);

                LyricsFinderData.MainData.LyricFormSize = Size;
                LyricsFinderData.SaveAsync();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the LyricForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private async void LyricForm_KeyDownAsync(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = false;

                if (e.KeyCode == Keys.Escape)
                {
                    _cancellationTokenSource.Cancel();
                    e.Handled = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Load event of the LyricForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricForm_LoadAsync(object sender, EventArgs e)
        {
            string msg;

            try
            {
                if (_isSearch)
                {
                    UseWaitCursor = true;
                    LyricFormStatusLabel.Text = "Searching...";

                    // LyricFormTimer.Start();
                    await SearchAsync();
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                    LyricFormStatusLabel.Text = "Search canceled.";
            }
            catch (LyricsQuotaExceededException ex)
            {
                msg = $"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event. ";
                await ErrorHandling.ShowAndLogDetailedErrorHandlerAsync(msg, ex);
                LyricFormStatusLabel.Text = "Service quota exceeded.";
            }
            catch (Exception ex)
            {
                msg = $"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event. ";
                await ErrorHandling.ShowAndLogDetailedErrorHandlerAsync(msg, ex);
                Close();
            }
            finally
            {
                UseWaitCursor = false;
            }
        }


        /// <summary>
        /// Handles the Scroll event of the LyricTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricFormTrackBar_ScrollAsync(object sender, EventArgs e)
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

                LyricFormFoundStatusLabel.Text = $"Source: {serviceName} {GetServiceCountText(serviceName)}";
                LyricFormFoundStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        private async void LyricTextBox_EnterAsync(object sender, EventArgs e)
        {
            try
            {
                SharedComponents.Utility.EnableOrDisableToolStripMenuItems(EditMenuItem, true);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the LyricTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private async void LyricTextBox_KeyDownAsync(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = false;

                if (e.Control && (e.KeyCode == Keys.A))
                {
                    LyricTextBox.SelectAll();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyPress event of the LyricTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private async void LyricTextBox_KeyPressAsync(object sender, KeyPressEventArgs e)
        {
            try
            {
                //LyricTextBox.sp
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the LeaveAsync event of the LyricTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricTextBox_LeaveAsync(object sender, EventArgs e)
        {
            try
            {
                SharedComponents.Utility.EnableOrDisableToolStripMenuItems(EditMenuItem, false);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the TextChanged event of the LyricTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricTextBox_TextChangedAsync(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MenuItem_ClickAsync(object sender, EventArgs e)
        {
            var ci = CultureInfo.CurrentCulture;
            var itemName = "Undefined item";

            try
            {
                if (!(sender is ToolStripMenuItem menuItem))
                    throw new Exception($"Unknown sender: \"{sender}\".");
                else
                    itemName = menuItem.Name;

                switch (itemName)
                {
                    case nameof(EditMenuItem):
                    case nameof(HelpMenuItem):
                    case nameof(ToolsMenuItem):
                        break;

                    case nameof(EditSelectAllMenuItem):
                        LyricTextBox.SelectAll();
                        break;

                    case nameof(EditCopyMenuItem):
                        if (DoTextOperationQuestion())
                        {
                            LyricTextBox.ClearUndo();
                            LyricTextBox.Copy();
                        }
                        break;

                    case nameof(EditCutMenuItem):
                        if (DoTextOperationQuestion())
                        {
                            LyricTextBox.ClearUndo();
                            LyricTextBox.Cut();
                        }
                        break;

                    case nameof(EditDeleteMenuItem):
                        if (EditMenuItem.Enabled)
                            DeleteText();
                        break;

                    case nameof(EditLowerCaseMenuItem):
                        if (DoTextOperationQuestion(true))
                            LyricTextBox.SelectedText = LyricTextBox.SelectedText.ToLower(ci);
                        break;

                    case nameof(EditPasteMenuItem):
                        // Determine if there is any text in the Clipboard to paste into the text box.
                        if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
                        {
                            // Determine if any text is selected in the text box.
                            if (LyricTextBox.SelectionLength > 0)
                            {
                                // Ask user if they want to paste over currently selected text.
                                if (DialogResult.No == MessageBox.Show("Do you want to paste over current selection?"
                                    , "Cut Example", MessageBoxButtons.YesNo))
                                    // Move selection to the point after the current selection and paste.
                                    LyricTextBox.SelectionStart += LyricTextBox.SelectionLength;
                            }
                            // Paste current text in Clipboard into text box.
                            LyricTextBox.Paste();
                        }
                        break;

                    case nameof(EditProperCaseMenuItem):
                        if (DoTextOperationQuestion(true))
                            LyricTextBox.SelectedText = LyricTextBox.SelectedText.ToProperCase();
                        break;

                    case nameof(EditSentenceCaseMenuItem):
                        if (DoTextOperationQuestion(true))
                            LyricTextBox.SelectedText = LyricTextBox.SelectedText.ToSentenceCase();
                        break;

                    case nameof(EditSpellCheckMenuItem):
                        if (DoTextOperationQuestion(true))
                            SpellCheck();
                        break;

                    case nameof(EditTitleCaseMenuItem):
                        if (DoTextOperationQuestion(true))
                            LyricTextBox.SelectedText = LyricTextBox.SelectedText.ToTitleCase();
                        break;

                    case nameof(EditUpperCaseMenuItem):
                        if (DoTextOperationQuestion(true))
                            LyricTextBox.SelectedText = LyricTextBox.SelectedText.ToUpper(ci);
                        break;

                    case nameof(EditUndoMenuItem):
                        if (LyricTextBox.CanUndo == true)
                        {
                            if (LyricTextBox.CanUndo)
                            {
                                LyricTextBox.Undo();
                                LyricTextBox.ClearUndo(); // Clear the undo buffer to prevent last action from being redone 
                            }
                        }
                        break;

                    case nameof(HelpHelpMenuItem):
                        System.Diagnostics.Process.Start("https://github.com/hardolf/JRiver.MediaCenter/wiki/Lyrics-Window");
                        break;

                    case nameof(ToolsSearchMenuItem):
                        SearchButton.PerformClick();
                        break;

                    default:
                        throw new Exception($"Unknown menu item: \"{itemName}\".");
                }

            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the SearchButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void SearchButton_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                // Create a pseudo MC playlist item for the search
                var row = LyricCell.OwningRow;

                if (_searchForm != null)
                    _searchForm.Close();

                _searchForm = new LyricForm(LyricCell, Location, Size, SearchLyricCallbackAsync, LyricsFinderData, true, ArtistTextBox.Text, AlbumTextBox.Text, TrackTextBox.Text);
                _searchForm.Show(this);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Lyric search callback routine.
        /// </summary>
        /// <param name="lyricForm">The lyric form.</param>
        /// <exception cref="Exception">Unknown DialogResult: \"{lyricForm.Result}\".</exception>
        private async void SearchLyricCallbackAsync(LyricForm lyricForm)
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
                    LyricTextBox.Select();
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /**********************************/
        /***** Private misc. routines *****/
        /**********************************/

        /// <summary>
        /// Deletes the text.
        /// </summary>
        private void DeleteText()
        {
            var idx = LyricTextBox.SelectionStart;

            if (idx > LyricTextBox.Text.Length - 1) return;

            if (DoTextOperationQuestion())
                LyricTextBox.Text = LyricTextBox.Text.Remove(idx, LyricTextBox.SelectionLength);
            else
                LyricTextBox.Text = LyricTextBox.Text.Remove(idx, 1);

            LyricTextBox.DeselectAll();
            LyricTextBox.SelectionStart = (idx > LyricTextBox.Text.Length - 1)
                ? LyricTextBox.Text.Length
                : idx;
            LyricTextBox.ScrollToCaret();
        }


        /// <summary>
        /// If the text box selection is empty, asks the user if the operation should be done on all text.
        /// </summary>
        /// <param name="isAllTextAsked">if set to <c>true</c> and the selected text is empty, all-text question should be asked; else it is not asked.</param>
        /// <returns>
        ///   <c>true</c>, if the operation should be done; else <c>false</c>.
        /// </returns>
        private bool DoTextOperationQuestion(bool isAllTextAsked = false)
        {
            var ret = (LyricTextBox.SelectedText.Length > 0);

            if (!ret && isAllTextAsked)
            {
                ret = (DialogResult.Yes == MessageBox.Show(this, "No text is selected.\r\n"
                    + "Do you want to do this on all text?"
                    , "No text is selected", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));

                if (ret)
                    LyricTextBox.SelectAll();
            }

            return ret;
        }


        /// <summary>
        /// Gets the service count.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Hit count of the specified service.</returns>
        private string GetServiceCountText(string serviceName)
        {
            var count = _serviceCounts.First(s => s.Key == serviceName).Value;

            return (count > 1) ? $"({count})" : string.Empty;
        }


        /// <summary>
        /// Spell check the selection.
        /// </summary>
        private void SpellCheck()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Searches this instance.
        /// </summary>
        /// <returns></returns>
        private async Task SearchAsync()
        {
            // Clear list and search for all the lyrics in each lyric service
            _foundLyricList.Clear();

            _cancellationTokenSource = new CancellationTokenSource();
            var resultServices = await LyricSearch.SearchAsync(LyricsFinderData, _McItem, _cancellationTokenSource.Token, true).ConfigureAwait(true);

            // Process the results
            foreach (var service in resultServices)
            {
                if (service.LyricResult != LyricResultEnum.Found) continue;

                _serviceCounts.Add(service.Credit.ServiceName, service.FoundLyricList.Count);

                foreach (var foundLyric in service.FoundLyricList)
                {
                    _foundLyricList.Add(foundLyric);
                }
            }

            // Set the trackbar and call the Scroll event handler to initialize the text box
            LyricFormTrackBar.Maximum = _foundLyricList.Count - 1;
            LyricFormTrackBar_ScrollAsync(this, new EventArgs());

            LyricFormStatusLabel.Text = $"{_foundLyricList.Count} lyrics found";
        }

    }

}
