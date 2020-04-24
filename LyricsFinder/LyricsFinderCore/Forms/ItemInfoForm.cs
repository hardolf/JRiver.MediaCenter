using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Forms
{

    /// <summary>
    /// Item information form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class ItemInfoForm : Form
    {

        const string _emptyPlayListCollection = "<Not collected yet, try again later...>";

        private readonly int _mcItemKey = -1;

        private readonly Dictionary<string, string> _itemFields = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _itemFieldsFull = new Dictionary<string, string>();


        /**********************/
        /***** Properties *****/
        /**********************/

        private LyricsFinderCore LyricsFinderCore { get; set; }


        /************************/
        /***** Constructors *****/
        /************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfoForm"/> class.
        /// </summary>
        public ItemInfoForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfoForm" /> class.
        /// </summary>
        /// <param name="lyricsFinderCore">The lyrics finder core.</param>
        /// <param name="mcItemKey">The Media Center item key.</param>
        public ItemInfoForm(LyricsFinderCore lyricsFinderCore, int mcItemKey)
            : this()
        {
            LyricsFinderCore = lyricsFinderCore;
            _mcItemKey = mcItemKey;
        }


        /*********************/
        /***** Delegates *****/
        /*********************/

        /// <summary>
        /// Handles the ClickAsync event of the CloseButton control.
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
        /// Handles the CheckedChanged event of the IncludeCalculatedCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void IncludeCalculatedCheckBox_CheckedChangedAsync(object sender, EventArgs e)
        {
            try
            {
                FillData();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the FormClosing event of the ItemInfoForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private async void ItemInfoForm_FormClosingAsync(object sender, FormClosingEventArgs e)
        {
            try
            {
                LyricsFinderCore.LyricsFinderData.MainData.ItemInfoFormLocation = Location;
                LyricsFinderCore.LyricsFinderData.MainData.ItemInfoFormSize = Size;
                await LyricsFinderCore.LyricsFinderData.SaveAsync();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the LoadAsync event of the ItemInfoForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void ItemInfoForm_LoadAsync(object sender, EventArgs e)
        {
            try
            {
                Location = LyricsFinderCore.LyricsFinderData.MainData.ItemInfoFormLocation;
                Size = LyricsFinderCore.LyricsFinderData.MainData.ItemInfoFormSize;

                // Check if the form is visible on screen
                if (this.IsOnScreenTopLeft())
                    StartPosition = FormStartPosition.Manual;
                else
                {
                    StartPosition = FormStartPosition.CenterParent;
                    Size = MinimumSize;
                }

                Refresh();

                // Populate the 2 item field dictionaries
                for (int i = 0; i < 2; i++)
                {
                    var mcMplInfo = (i == 0)
                        ? await McRestService.GetInfoAsync(_mcItemKey.ToString(CultureInfo.InvariantCulture), false)
                        : await McRestService.GetInfoAsync(_mcItemKey.ToString(CultureInfo.InvariantCulture), true);

                    foreach (var item in mcMplInfo.Items)
                    {
                        foreach (var fld in item.Value.Fields)
                        {
                            var value = fld.Value;
                            var valueText = value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;

                            if (i == 0)
                                _itemFields.Add(fld.Key, valueText);
                            else
                                _itemFieldsFull.Add(fld.Key, valueText);
                        }
                    }
                }

                if (_itemFields.TryGetValue("Name", out var text))
                    Text = text;

                FillData();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the CellFormatting event of the MainDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> instance containing the event data.</param>
        private async void MainDataGridView_CellFormattingAsync(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex == MainDataGridView.Columns[nameof(ItemValueColumn)].Index)
                    && e.Value.ToString().Equals(_emptyPlayListCollection, StringComparison.InvariantCultureIgnoreCase))
                {
                    var cell = MainDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.ToolTipText = "The collection of the playlists for the current items in LyricsFinder may take a couple of minutes.";
                }
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
        /// Fills the data.
        /// </summary>
        private void FillData()
        {
            var dgv = MainDataGridView;
            var fields = (IncludeCalculatedCheckBox.Checked) ? _itemFieldsFull : _itemFields;
            var sortCol = dgv.SortedColumn;
            var sortOrder = dgv.SortOrder;
            var id = -1;

            if (fields.TryGetValue("Key", out var idTxt))
                id = int.Parse(idTxt, CultureInfo.InvariantCulture);
            else
                id = -1;

            dgv.Rows.Clear();

            // Populate the data grid
            // First, add the item's playlists text
            var playListsText = string.Empty;

            if ((LyricsFinderCore.ItemsPlayListIds != null)
                && (LyricsFinderCore.ItemsPlayListIds.TryGetValue(id, out var playListIds)))
            {
                // Create a new list of playlist names
                var playLists = new Dictionary<int, string>();

                foreach (var playListId in playListIds)
                {
                    var playList = LyricsFinderCore.CurrentSortedMcPlaylists.Values
                        .Where(p => p.Id == playListId.ToString(CultureInfo.InvariantCulture))
                        .FirstOrDefault();

                    playLists.Add(playListId, playList?.Name ?? "?");
                }

                // Join the item's playlist names
                foreach (var playList in playLists.OrderBy(p => p.Value))
                {
                    if (!playListsText.IsNullOrEmptyTrimmed())
                        playListsText += ", ";

                    playListsText += playList.Value;
                }
            }
            else
                playListsText = _emptyPlayListCollection;

            dgv.Rows.Add("*", "PlayLists", playListsText);

            // Now, add the remaining properties
            foreach (var fld in fields)
            {
                var calc = (_itemFields.ContainsKey(fld.Key)) ? string.Empty : "*";

                dgv.Rows.Add(calc, fld.Key, fld.Value);
            }

            dgv.Columns[0].Visible = true; // IncludeCalculatedCheckBox.Checked;

            // Sorting
            if ((sortCol is null) || (sortOrder == SortOrder.None))
            {
                // First sort after form opening
                // Special handling to get the playlists on top from start
                // and the remaining columns sorted by name

                // Sort by name first
                sortCol = dgv.Columns[1];
                sortOrder = SortOrder.Ascending;

                dgv.Sort(sortCol, (sortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);

                // Now sort by the Calc column
                sortCol = dgv.Columns[0];
                sortOrder = SortOrder.Descending;

                dgv.Sort(sortCol, (sortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }
            else
            {
                // Restore the previous sorting
                dgv.Sort(sortCol, (sortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }
        }

    }

}
