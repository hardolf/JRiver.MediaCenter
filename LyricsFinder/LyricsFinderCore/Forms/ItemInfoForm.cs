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

            dgv.Rows.Clear();

            // Populate the data grid
            foreach (var fld in fields)
            {
                var calc = (_itemFields.ContainsKey(fld.Key)) ? string.Empty : "*";

                dgv.Rows.Add(calc, fld.Key, fld.Value);

                if (fld.Key.Equals("Key", StringComparison.InvariantCultureIgnoreCase))
                    id = int.Parse(fld.Value, CultureInfo.InvariantCulture);
            }

            // Add the item's playlist
            var playListsText = "<Not collected yet, try again later...>";

            if (LyricsFinderCore.ItemsPlayLists != null)
            {
                if (LyricsFinderCore.ItemsPlayLists.TryGetValue(id, out var playLists))
                {
                    playListsText = string.Empty;

                    foreach (var playList in playLists.OrderBy(p => p.Value))
                    {
                        if (!playListsText.IsNullOrEmptyTrimmed())
                            playListsText += ", ";

                        playListsText += playList.Value;
                    }
                }
            }

            dgv.Rows.Add("*", "PlayLists", playListsText);

            dgv.Columns[0].Visible = IncludeCalculatedCheckBox.Checked;

            // Restore the previous sorting, if necessary
            if ((sortCol != null) && (sortOrder != SortOrder.None))
            {
                dgv.Sort(sortCol, (sortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }
        }

    }

}
