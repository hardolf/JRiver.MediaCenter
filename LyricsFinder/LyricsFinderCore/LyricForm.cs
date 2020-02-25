using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
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

        private readonly Action<LyricForm> _callback;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly string _initLyric = string.Empty;
        private readonly bool _isSearch = false;
        private string _finalLyric = string.Empty;
        private LyricForm _searchForm = null;
        private readonly List<FoundLyricType> _foundLyricList = new List<FoundLyricType>();
        private readonly McMplItem _McItem = null;
        private readonly Dictionary<string, int> _serviceCounts = new Dictionary<string, int>();
        private readonly CultureInfo _initialCulture = Thread.CurrentThread.CurrentCulture;


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
        /// Gets the dialog result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public DialogResult Result { get; private set; }

        private LyricsFinderCore LyricsFinderCore { get; set; }

        private LyricsFinderDataType LyricsFinderData { get; set; }


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
        /// <param name="lyricsFinderCore">The lyrics finder core.</param>
        /// <param name="isSearch">if set to <c>true</c> [is search].</param>
        /// <param name="artist">The artist, only used for search.</param>
        /// <param name="album">The album, only used for search.</param>
        /// <param name="track">The track, only used for search.</param>
        /// <exception cref="ArgumentNullException">lyricCell
        /// or
        /// callback
        /// or
        /// lyricsFinderData</exception>
        internal LyricForm(DataGridViewTextBoxCell lyricCell, Point location, Size? size, Action<LyricForm> callback, LyricsFinderDataType lyricsFinderData, LyricsFinderCore lyricsFinderCore, bool isSearch = false, string artist = null, string album = null, string track = null)
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
            LyricsFinderCore = lyricsFinderCore;

            if (size != null)
                Size = size.Value;

            SharedComponents.Utility.EnableOrDisableToolStripMenuItems(HelpMenuItem, true);

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
                LyricTextBox.BackColor = SystemColors.ControlDark;
                LyricFormTrackBar.Enabled = true;
                LyricFormTrackBar.Visible = true;
                LyricFormTrackBar.Select();
                SearchButton.Enabled = false;
                SearchButton.Visible = false;
                SharedComponents.Utility.EnableOrDisableToolStripMenuItems(EditMenuItem, false);
                SharedComponents.Utility.EnableOrDisableToolStripMenuItems(ToolsMenuItem, false);

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
                SharedComponents.Utility.EnableOrDisableToolStripMenuItems(EditMenuItem, true);
                SharedComponents.Utility.EnableOrDisableToolStripMenuItems(ToolsMenuItem, true);

                _McItem = new McMplItem
                {
                    Artist = row.Cells[(int)GridColumnEnum.Artist].Value as string,
                    Album = row.Cells[(int)GridColumnEnum.Album].Value as string,
                    Name = row.Cells[(int)GridColumnEnum.Title].Value as string
                };

                SetPlayingState(false);

                ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_StartingAsync;
                ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_StoppingAsync;

                var installedCultures = LyricTextBox.InstalledCultures;

                foreach (var culture in installedCultures)
                {
                    var name = $"{culture.EnglishName} ({culture.Name})";
                    var item = new ToolStripMenuItem(name, null, MenuItemLanguage_ClickAsync);

                    EditSpellCheckLanguageMenuItem.DropDownItems.Add(item);
                }
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
                            break;

                        default:
                            break;
                    }

                    Thread.CurrentThread.CurrentCulture = _initialCulture;
                    Thread.CurrentThread.CurrentUICulture = _initialCulture;
                    System.Windows.Input.InputLanguageManager.Current.CurrentInputLanguage = _initialCulture;
                }

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

                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        _cancellationTokenSource.Cancel();
                        e.Handled = true;
                        Close();
                        break;

                    case Keys.Left:
                        if (e.Control && !LyricTextBox.Focused)
                        {
                            await LyricsFinderCore.McPlayControl?.JumpAsync(true, true);
                            e.Handled = true;
                        }
                        break;

                    case Keys.Right:
                        if (e.Control && !LyricTextBox.Focused)
                        {
                            await LyricsFinderCore.McPlayControl?.JumpAsync(false, true);
                            e.Handled = true;
                        }
                        break;

                    case Keys.P:
                        if (e.Control)
                        {
                            ToolsPlayStartStopButton.PerformClick();
                            e.Handled = true;
                        }
                        break;

                    default:
                        e.Handled = false;
                        break;
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
                BringToFront();

                if (_isSearch)
                {
                    UseWaitCursor = true;
                    LyricTextBox.UseWaitCursor = true;
                    LyricFormStatusLabel.Text = "Searching...";

                    // LyricFormTimer.Start();
                    await SearchAsync();

                    if (LyricFormTrackBar.CanFocus)
                        LyricFormTrackBar.Focus();
                }
                else
                {
                    LyricsFinderCore.ShowMcPlayControl(this);

                    if (LyricTextBox.CanFocus)
                        LyricTextBox.Focus();
                }

                if (_cancellationTokenSource.IsCancellationRequested)
                    LyricFormStatusLabel.Text = "Search canceled.";

                SetMenuStates();
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
                LyricTextBox.UseWaitCursor = false;
            }
        }


        /// <summary>
        /// Handles the MouseEnterAsync event of some of the LyricForm controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricForm_MouseEnterAsync(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
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

                if (e.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.A:
                            EditSelectAllMenuItem.PerformClick();
                            e.Handled = true;
                            break;

                        case Keys.C:
                        case Keys.V:
                        case Keys.X:
                        case Keys.Y:
                        case Keys.Z:
                            e.Handled = true;
                            break;

                        default:
                            OnKeyDown(e);
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    //EditDeleteMenuItem.PerformClick();
                    e.Handled = true;
                }
                else
                    OnKeyDown(e);
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
            var menuName = "Undefined menu";

            try
            {
                if (sender is ToolStripMenuItem menuItem)
                    menuName = menuItem.Name;
                else
                    throw new Exception($"Unknown sender: \"{sender}\".");

                // Edit menu

                if (menuName.StartsWith("Edit", StringComparison.InvariantCultureIgnoreCase))
                {
                    var focusedControl = SharedComponents.Utility.GetFocusedControl(this);

                    if (focusedControl is TextBox)
                    {
                        var txt = focusedControl as TextBox;

                        switch (menuName)
                        {
                            case nameof(EditMenuItem):
                                break;

                            case nameof(EditSelectAllMenuItem):
                                txt.SelectAll();
                                break;

                            case nameof(EditCopyMenuItem):
                                txt.Copy();
                                break;

                            case nameof(EditCutMenuItem):
                                txt.Cut();
                                break;

                            case nameof(EditDeleteMenuItem):
                                if (txt.SelectionLength < 1)
                                    txt.SelectionLength = 1;
                                txt.Cut();
                                break;

                            case nameof(EditPasteMenuItem):
                                // Determine if there is any text in the Clipboard to paste into the text box.
                                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
                                    txt.Paste();
                                break;

                            case nameof(EditRedoMenuItem):
                                //txt.Redo();
                                //txt.ClearUndo(); // Clear the undo buffer to prevent last action from being redone 
                                break;

                            case nameof(EditTrimMenuItem):
                                txt.Text = txt.Text.Trim();
                                break;

                            case nameof(EditUndoMenuItem):
                                if (txt.CanUndo)
                                {
                                    txt.Undo();
                                    txt.ClearUndo(); // Clear the undo buffer to prevent last action from being redone
                                }
                                break;

                            default:
                                throw new Exception($"Unknown menu item: \"{menuName}\".");
                        }
                    }
                    else if (focusedControl is SpellBox)
                    {
                        var txt = LyricTextBox;

                        switch (menuName)
                        {
                            case nameof(EditMenuItem):
                                break;

                            case nameof(EditSelectAllMenuItem):
                                txt.SelectAll();
                                break;

                            case nameof(EditCopyMenuItem):
                                txt.Copy();
                                break;

                            case nameof(EditCutMenuItem):
                                txt.Cut();
                                break;

                            case nameof(EditDeleteMenuItem):
                                if (txt.SelectionLength < 1)
                                    txt.SelectionLength = 1;
                                txt.Cut();
                                break;

                            case nameof(EditLowerCaseMenuItem):
                                if (DoTextOperationQuestion(true))
                                    txt.SelectedText = txt.SelectedText.ToLower(ci);
                                break;

                            case nameof(EditPasteMenuItem):
                                // Determine if there is any text in the Clipboard to paste into the text box.
                                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
                                    txt.Paste();
                                break;

                            case nameof(EditProperCaseMenuItem):
                                if (DoTextOperationQuestion(true))
                                    txt.SelectedText = txt.SelectedText.ToProperCase();
                                break;

                            case nameof(EditSentenceCaseMenuItem):
                                if (DoTextOperationQuestion(true))
                                    txt.SelectedText = txt.SelectedText.ToSentenceCase();
                                break;

                            case nameof(EditToggleSpellCheckMenuItem):
                                SetOrToggleSpellCheck();
                                break;

                            case nameof(EditTitleCaseMenuItem):
                                if (DoTextOperationQuestion(true))
                                    txt.SelectedText = txt.SelectedText.ToTitleCase();
                                break;

                            case nameof(EditTrimMenuItem):
                                if (DoTextOperationQuestion(true))
                                {
                                    var sb = new StringBuilder();
                                    using (var sr = new StringReader(txt.SelectedText))
                                    {
                                        string line;
                                        while ((line = sr.ReadLine()) != null)
                                        {
                                            sb.AppendLine(line.Trim());
                                        }
                                    }
                                    txt.SelectedText = sb.ToString();
                                }
                                break;

                            case nameof(EditUpperCaseMenuItem):
                                if (DoTextOperationQuestion(true))
                                    txt.SelectedText = txt.SelectedText.ToUpper(ci);
                                break;

                            case nameof(EditUndoMenuItem):
                                if (txt.CanUndo)
                                    txt.Undo();
                                break;

                            case nameof(EditRedoMenuItem):
                                if (txt.CanRedo)
                                    txt.Redo();
                                break;

                            default:
                                throw new Exception($"Unknown menu item: \"{menuName}\".");
                        }
                    }
                }

                // Help and Tools menus

                else if (menuName.StartsWith("Help", StringComparison.InvariantCultureIgnoreCase)
                    || menuName.StartsWith("Tools", StringComparison.InvariantCultureIgnoreCase))
                {
                    switch (menuName)
                    {
                        case nameof(HelpMenuItem):
                        case nameof(ToolsMenuItem):
                            break;

                        // Help menu

                        case nameof(HelpHelpMenuItem):
                            System.Diagnostics.Process.Start("https://github.com/hardolf/JRiver.MediaCenter/wiki/Lyrics-Window");
                            break;

                        // Tools menu

                        case nameof(ToolsSearchMenuItem):
                            SearchButton.PerformClick();
                            break;

                        case nameof(ToolsPlayJumpAheadLargeMenuItem):
                            await LyricsFinderCore?.McPlayControl?.JumpAsync(false, true);
                            break;

                        case nameof(ToolsPlayJumpBackLargeMenuItem):
                            await LyricsFinderCore?.McPlayControl?.JumpAsync(true, true);
                            break;

                        default:
                            throw new Exception($"Unknown menu item: \"{menuName}\".");
                    }
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the ClickAsync event of the MenuItem (Language) control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MenuItemLanguage_ClickAsync(object sender, EventArgs e)
        {
            string itemText;

            try
            {
                if (!(sender is ToolStripMenuItem menuItem))
                    throw new Exception($"Unknown sender: \"{sender}\".");
                else
                    itemText = menuItem.Text;

                var idx1 = itemText.LastIndexOf('(');
                var idx2 = itemText.LastIndexOf(')');

                if ((idx1 < 0) || (idx2 <= idx1))
                    throw new Exception($"Improper format of language menu item \"{itemText}\".");

                var cultureName = itemText.Substring(idx1 + 1, idx2 - idx1 - 1);
                var culture = new CultureInfo(cultureName, true);

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                System.Windows.Input.InputLanguageManager.Current.CurrentInputLanguage = culture;

                SetOrToggleSpellCheck(true);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Enter event of the controls other than the SpellBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void NonSpellChecBox_EnterAsync(object sender, EventArgs e)
        {
            try
            {
                SetMenuStates();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Leave event of the controls other than the SpellBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void NonSpellChecBox_LeaveAsync(object sender, EventArgs e)
        {
            try
            {
                SetMenuStates();
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

                _searchForm = new LyricForm(LyricCell, Location, Size, SearchLyricCallbackAsync, LyricsFinderData, LyricsFinderCore, true, ArtistTextBox.Text, AlbumTextBox.Text, TrackTextBox.Text);
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


        /// <summary>
        /// Handles the Starting event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private async void ToolsPlayStartStopButton_StartingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                await LyricsFinderCore.PlayOrPauseAsync();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Stopping event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private async void ToolsPlayStartStopButton_StoppingAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                await LyricsFinderCore.PlayOrPauseAsync();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the EnterAsync event of one of the TopTextBox controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void TopTextBox_EnterAsync(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /**************************/
        /***** Misc. routines *****/
        /**************************/

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
        /// Resizes the Media Center play control.
        /// </summary>
        public void PositionAndResizeMcPlayControl()
        {
            if (LyricsFinderCore.McPlayControl == null) return;

            const int margin = 40;

            var refCtl = HelpMenuItem;
            // var refScreenLocation = refCtl.PointToScreen(refCtl.Location);
            var leftOffset = refCtl.Bounds.X + margin;
            //var left = refScreenLocation.X + leftOffset;
            //var top = refScreenLocation.Y;
            var left = leftOffset;
            var top = refCtl.Bounds.Y;
            var width = LyricFormMenuStrip.Width - leftOffset;
            var height = LyricFormMenuStrip.Height + (int)LyricParmsPanel.RowStyles[0].Height;

            // MessageBox.Show($"left={left} top={top} width={width} height={height}", "Test");

            LyricsFinderCore.McPlayControl.Left = left;
            LyricsFinderCore.McPlayControl.Top = top;
            LyricsFinderCore.McPlayControl.Width = width;
            LyricsFinderCore.McPlayControl.Height = height;
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
                if (service.LyricResult != LyricsResultEnum.Found) continue;

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


        /// <summary>
        /// Set or toggle the spell checking feature on or off.
        /// </summary>
        /// <param name="enable">if set to null, toggle the state on/off; else set it to the <c>enable</c> state.</param>
        private void SetOrToggleSpellCheck(bool? enable = null)
        {
            try
            {
                SuspendLayout();

                var txt = LyricTextBox.Text;

                LyricTextBox.Text = string.Empty;
                LyricTextBox.SpellCheckEnabled = enable ?? !LyricTextBox.SpellCheckEnabled;

                LyricTextBox.SpellCheckEnabled = !LyricTextBox.SpellCheckEnabled;
                LyricTextBox.SpellCheckEnabled = !LyricTextBox.SpellCheckEnabled;

                LyricTextBox.Text = txt;
            }
            finally
            {
                ResumeLayout();
            }
        }


        /// <summary>
        /// Sets the menu states.
        /// </summary>
        private void SetMenuStates()
        {
            var focusedControl = SharedComponents.Utility.GetFocusedControl(this);

            EditCopyMenuItem.Enabled = true;
            EditCutMenuItem.Enabled = true;
            EditDeleteMenuItem.Enabled = true;
            EditLowerCaseMenuItem.Enabled = (focusedControl is SpellBox);
            EditPasteMenuItem.Enabled = true;
            EditProperCaseMenuItem.Enabled = (focusedControl is SpellBox);
            EditSelectAllMenuItem.Enabled = true;
            EditSentenceCaseMenuItem.Enabled = (focusedControl is SpellBox);
            EditSpellCheckLanguageMenuItem.Enabled = (focusedControl is SpellBox);
            EditTitleCaseMenuItem.Enabled = (focusedControl is SpellBox);
            EditToggleSpellCheckMenuItem.Enabled = (focusedControl is SpellBox);
            EditTrimMenuItem.Enabled = (focusedControl is SpellBox);
            EditRedoMenuItem.Enabled = (focusedControl is SpellBox);
            EditUndoMenuItem.Enabled = true;
            EditUpperCaseMenuItem.Enabled = (focusedControl is SpellBox);

            ToolsPlayJumpAheadLargeMenuItem.Enabled = !_isSearch;
            ToolsPlayJumpBackLargeMenuItem.Enabled = !_isSearch;
            ToolsPlayJumpAheadLargeMenuItem.ShowShortcutKeys = !(focusedControl is SpellBox);
            ToolsPlayJumpBackLargeMenuItem.ShowShortcutKeys = !(focusedControl is SpellBox);
        }


        /// <summary>
        /// Sets the playing state.
        /// </summary>
        /// <param name="isPlaying">if set to <c>true</c> the Media Center is playing; else <c>false</c>.</param>
        internal void SetPlayingState(bool isPlaying)
        {
            ToolsPlayStartStopButton.SetRunningState(isPlaying);
        }

    }

}
