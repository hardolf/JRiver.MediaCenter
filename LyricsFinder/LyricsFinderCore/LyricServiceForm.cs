﻿using System;
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

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Form to show the lyric services.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class LyricServiceForm : Form
    {

        private bool _isListReady = false;

        private string _initialText = string.Empty;

        private LyricsFinderDataType _lyricsFinderData = null;

        /// <summary>
        /// The callback function.
        /// </summary>
        private Action<LyricServiceForm> _callback;


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceForm"/> class.
        /// </summary>
        public LyricServiceForm()
        {
            InitializeComponent();

            AllowTransparency = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceForm" /> class.
        /// </summary>
        /// <param name="lyricsFinderData">The lyrics finder data.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="ArgumentNullException">callback
        /// or
        /// lyricsFinderData</exception>
        public LyricServiceForm(LyricsFinderDataType lyricsFinderData, Action<LyricServiceForm> callback)
            : this()
        {
            var dgv = LyricServiceListDataGridView;

            _callback = callback ?? throw new ArgumentNullException(nameof(callback));

            // Fill the datagrid
            _isListReady = false;
            _lyricsFinderData = lyricsFinderData ?? throw new ArgumentNullException(nameof(lyricsFinderData));

            foreach (var service in _lyricsFinderData.LyricServices)
            {
                var value = service.DisplayProperties.GetPropertyValue("DailyQuota", true);
                var dailyQuota = (value == null) ? "-" : value.ToString();

                value = service.DisplayProperties.GetPropertyValue("QuotaResetTime", true);
                var quotaResetTimeString = (value is ServiceDateTimeWithZone quotaResetTime)
                    ? quotaResetTime.ClientLocalTime.ToString(CultureInfo.InvariantCulture)
                    : "-";

                if (service.IsImplemented)
                {
                    dgv.Rows.Add(service.IsActive, service.Credit.ServiceName, service.RequestCountToday, dailyQuota, quotaResetTimeString);
                    dgv.Rows[dgv.Rows.Count - 1].Cells[1].ToolTipText = service.Comment;
                }
            }

            _isListReady = true;

            // Adjust the form height
            var height = 65;

            foreach (DataGridViewRow dr in dgv.Rows)
            {
                height += dr.Height;
            }

            Height = height;
        }


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
        /// Fills the row.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="value">The value.</param>
        /// <param name="isEditAllowed">if set to <c>true</c> value edit is allowed; else it is read-only.</param>
        /// <param name="toolTip">The tooltip.</param>
        private void FillRow(string propertyName, string caption, string value, bool isEditAllowed = false, string toolTip = "")
        {
            var tlp = LyricServiceDetailsTableLayoutPanel;

            tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlp.RowCount++;

            var rowIdx = tlp.RowCount - 1;
            var lblPropertyName = new Label();
            var lblCaption = new Label();
            var txtValue = new TextBox();

            lblPropertyName.AutoSize = true;
            lblPropertyName.Name = $"PropertyNameLabel{rowIdx}";
            lblPropertyName.Text = propertyName ?? string.Empty;
            lblPropertyName.Visible = false;

            lblCaption.AutoSize = true;
            lblCaption.Name = $"CaptionLabel{rowIdx}";
            lblCaption.Text = caption;

            txtValue.AutoSize = true;
            txtValue.BorderStyle = (isEditAllowed) ? BorderStyle.FixedSingle : BorderStyle.None;
            txtValue.MinimumSize = new Size(200, 20);
            txtValue.Multiline = true;
            txtValue.Name = $"ValueTextbox{rowIdx}";
            txtValue.ReadOnly = !isEditAllowed;
            txtValue.ScrollBars = ScrollBars.None;
            txtValue.TabIndex = rowIdx;
            txtValue.TabStop = true;
            txtValue.WordWrap = true;
            txtValue.Text = (value.IsNullOrEmptyTrimmed()) ? " " : value; // Set to space if empty value, so that the text box resize will work

            toolTip = (toolTip ?? string.Empty).Trim().TrimEnd('.');

            if (!toolTip.IsNullOrEmptyTrimmed())
                LyricServiceFormToolTip.SetToolTip(txtValue, toolTip);

            //if (isEditAllowed)
            //    LyricServiceFormToolTip.SetToolTip(txtValue, $"{toolTip}. You can edit the value".TrimStart('.', ' '));

            txtValue.AutoSizeTextBox();
            txtValue.Text = value; // Do this after the text box resize to remove the space from above

            tlp.Controls.Add(lblPropertyName, 0, rowIdx);
            tlp.Controls.Add(lblCaption, 1, rowIdx);
            tlp.Controls.Add(txtValue, 2, rowIdx);

            tlp.RowStyles.Clear();

            foreach (RowStyle rs in tlp.RowStyles)
            {
                tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            if (tlp.Controls.Count > 0)
                tlp.Controls[0].Select();
        }


        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns>The selected service object.</returns>
        private AbstractLyricService GetSelectedService(out int selectedIndex)
        {
            AbstractLyricService ret = null;
            var dgv = LyricServiceListDataGridView;

            selectedIndex = -1;

            if ((dgv.SelectedRows == null) || (dgv.SelectedRows.Count == 0)) return ret;

            var row = dgv.SelectedRows[0];
            var tmp = row.Cells[1].Value;

            if (tmp == null)
                return ret;

            var serviceName = tmp.ToString();

            for (int i = 0; i < _lyricsFinderData.LyricServices.Count; i++)
            {
                var service = _lyricsFinderData.LyricServices[i];

                if (service.Credit.ServiceName.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret = service;
                    selectedIndex = i;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// Validates the details panel and determines whether data is changed.
        /// If they are, the user is asked whether to save, and if so, save the data.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the row change should be canceled; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidateCancel()
        {
            if (!_isListReady) return false;

            var newText = LyricServiceDetailsTableLayoutPanel.GetAllControlText();
            var question = "Do you want to use the new values?";
            var result = DialogResult.No;
            var ret = false;

            if (newText != _initialText)
                result = MessageBox.Show(this, question, "Values are changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.Cancel:
                    ret = true;
                    break;

                case DialogResult.No:
                    ret = false;
                    break;

                case DialogResult.Yes:
                    ret = false;
                    // Save the changes
                    SaveSelectedService();
                    break;

                default:
                    break;
            }

            return ret;
        }


        /// <summary>
        /// Handles the Click event of the LyricServicesContainer_RightToolStripPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricServicesContainer_RightPanelButton_ClickAsync(object sender, EventArgs e)
        {
            var dgv = LyricServiceListDataGridView;
            DataGridViewRow row = dgv.SelectedRows?[0] ?? null;

            try
            {
                if (dgv.SelectedRows.Count == 0) return;

                var rowIdxBefore = row.Index;
                var serviceIdx = -1;
                var service = GetSelectedService(out serviceIdx);

                if (service == null)
                    return;

                if (serviceIdx < 0)
                    throw new Exception($"No service matching \"{service.Credit.ServiceName}\" found in service list.");

                dgv.SuspendLayout();

                if (sender == MoveDownButton)
                {
                    if (row.Index == dgv.Rows.Count - 1) return;

                    _isListReady = false;
                    dgv.Rows.RemoveAt(rowIdxBefore);
                    _isListReady = true;
                    dgv.Rows.Insert(rowIdxBefore + 1, row);

                    _lyricsFinderData.LyricServices.RemoveAt(serviceIdx);
                    _lyricsFinderData.LyricServices.Insert(serviceIdx + 1, service);
                }
                else if (sender == MoveUpButton)
                {
                    if (row.Index == 0) return;

                    _isListReady = false;
                    dgv.Rows.RemoveAt(rowIdxBefore);
                    _isListReady = true;
                    dgv.Rows.Insert(rowIdxBefore - 1, row);

                    _lyricsFinderData.LyricServices.RemoveAt(serviceIdx);
                    _lyricsFinderData.LyricServices.Insert(serviceIdx - 1, service);
                }

                row.Selected = true;

                dgv.ResumeLayout();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the FormClosing event of the LyricServiceForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private async void LyricServiceForm_FormClosingAsync(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Validate the service details
                e.Cancel = IsValidateCancel();

                if (!e.Cancel)
                {
                    // Store the visual rows with the data, i.e. the IsActive service property
                    foreach (DataGridViewRow row in LyricServiceListDataGridView.Rows)
                    {
                        var cellChk = row.Cells[0];
                        var cellName = row.Cells[1];
                        var isChecked = (cellChk.Value == null) || (bool)cellChk.Value;
                        var serviceName = (cellName.Value == null) ? string.Empty : (string)cellName.Value;

                        _lyricsFinderData.LyricServices.First(service => service.Credit.ServiceName
                            .Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                            .IsActive = isChecked;
                    }

                    _callback(this);
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the LyricServiceListForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private async void LyricServiceForm_KeyDownAsync(object sender, KeyEventArgs e)
        {
            try
            {
                if (ActiveControl is TextBox)
                {
                    e.Handled = false;
                    return;
                }

                e.Handled = true;

                var dgv = LyricServiceListDataGridView;
                var row = dgv.SelectedRows[0];
                var serviceIdx = -1;
                var service = GetSelectedService(out serviceIdx);

                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (row.Index >= dgv.RowCount - 1) return;
                    if (IsValidateCancel()) return;
                    dgv.Rows[row.Index + 1].Selected = true;
                }
                else if ((e.KeyCode == Keys.End) || (e.KeyCode == Keys.PageDown))
                {
                    if (row.Index >= dgv.RowCount - 1)
                        return;
                    else
                        dgv.Rows[dgv.RowCount - 1].Selected = true;
                }
                else if ((e.KeyCode == Keys.Home) || (e.KeyCode == Keys.PageUp))
                {
                    if (row.Index <= 0)
                        return;
                    else
                        dgv.Rows[0].Selected = true;
                }
                else if (e.KeyCode == Keys.Space)
                {
                    if (row.Index < 0)
                        return;
                    else
                    {
                        var isChecked = (row.Cells[0].Value == null) || !((bool)row.Cells[0].Value);

                        row.Cells[0].Value = isChecked;
                        ShowDetails(isChecked);
                    }
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    var tlp = LyricServiceDetailsTableLayoutPanel;

                    foreach (var ctl in tlp.Controls)
                    {
                        if (ctl is TextBox txt)
                        {
                            txt.Select();
                            break;
                        }
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (row.Index <= 0) return;
                    if (IsValidateCancel()) return;
                    dgv.Rows[row.Index - 1].Selected = true;
                }
                else
                    e.Handled = false;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the CellClick event of the LyricServiceListDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private async void LyricServiceListDataGridView_CellClickAsync(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!_isListReady) return;
                if (e.ColumnIndex != 0) return; // We only look at the checkbox column here

                var row = LyricServiceListDataGridView.Rows[e.RowIndex];
                var isChecked = (row.Cells[0].Value == null) || !((bool)row.Cells[0].Value);

                ShowDetails(isChecked);

                row.Cells[0].Value = isChecked;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the RowValidating event of the LyricServiceListDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellCancelEventArgs"/> instance containing the event data.</param>
        private async void LyricServiceListDataGridView_RowValidatingAsync(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                e.Cancel = IsValidateCancel();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the SelectionChanged event of the LyricServiceFormDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void LyricServiceListDataGridView_SelectionChangedAsync(object sender, EventArgs e)
        {
            try
            {
                if (!_isListReady) return;

                var row = LyricServiceListDataGridView.CurrentRow;

                ShowDetails((bool?)row.Cells[0].Value);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Saves the selected service.
        /// </summary>
        private void SaveSelectedService()
        {
            var tlp = LyricServiceDetailsTableLayoutPanel;
            AbstractLyricService service = GetSelectedService(out _);

            if (service == null) return;

            for (int i = 0; i < tlp.Controls.Count; i++)
            {
                var ctl = tlp.Controls[i];

                if (ctl is TextBox txt)
                {
                    if (txt.ReadOnly) continue;

                    var lbl = tlp.Controls[i - 2]; // There is always a hidden label before the caption label and the value text box

                    if (service.DisplayProperties.TryGetValue(lbl.Text, out var dp))
                    {
                        if (!dp.IsEditAllowed) continue;

                        dp.Value = txt.Text;
                        dp.SetPropertyValue(service, true);
                    }

                    if (service.Credit.DisplayProperties.TryGetValue(lbl.Text, out var dpc))
                    {
                        if (!dpc.IsEditAllowed) continue;

                        dpc.Value = txt.Text;
                        dpc.SetPropertyValue(service.Credit, true);
                    }
                }
            }
        }


        /// <summary>
        /// Shows the details.
        /// </summary>
        private void ShowDetails(bool? isChecked = null)
        {
            if (!_isListReady) return;

            AbstractLyricService service = null;
            var dgv = LyricServiceListDataGridView;
            var row = dgv.SelectedRows[0];
            var serviceIdx = -1;
            var tlp = LyricServiceDetailsTableLayoutPanel;

            // Start the layout
            _isListReady = false;
            tlp.SuspendLayout();

            service = GetSelectedService(out serviceIdx);

            if (service == null)
                return;

            // Clear the detail rows except the first
            tlp.Controls.Clear();
            tlp.RowStyles.Clear();
            tlp.RowCount = 0;

            if (isChecked == null)
                isChecked = service.IsActive;

            foreach (var dp in service.Credit.DisplayProperties)
            {
                FillRow(dp.Key, dp.Value.Caption, dp.Value.Value?.ToString() ?? string.Empty, dp.Value.IsEditAllowed, dp.Value.ToolTips);
            }

            foreach (var dp in service.DisplayProperties)
            {
                FillRow(dp.Key, dp.Value.Caption, dp.Value.Value?.ToString() ?? string.Empty, dp.Value.IsEditAllowed, dp.Value.ToolTips);
            }

            tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlp.RowCount++;

            var btnClose = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Margin = new Padding(3, 15, 3, 3),
                Name = "CloseButton",
                TabStop = true,
                Text = "&Close (Esc)",
            };

            btnClose.Click += CloseButton_ClickAsync;
            LyricServiceFormToolTip.SetToolTip(btnClose, "Close the window (Esc)");

            tlp.Controls.Add(btnClose, 2, tlp.RowCount - 1);

            AcceptButton = btnClose;
            CancelButton = btnClose;

            // Enable or disable the move-buttons and do auto-scroll
            MoveDownButton.Enabled = false;
            MoveUpButton.Enabled = false;

            if (dgv.RowCount > 0)
            {
                if (row.Index < dgv.RowCount - 1)
                    MoveDownButton.Enabled = true;

                if (row.Index > 0)
                    MoveUpButton.Enabled = true;

                if (row.Index < dgv.FirstDisplayedScrollingRowIndex)
                    dgv.FirstDisplayedScrollingRowIndex--;

                if (row.Index > dgv.FirstDisplayedScrollingRowIndex + dgv.DisplayedRowCount(false) - 1)
                    dgv.FirstDisplayedScrollingRowIndex++;
            }

            // Finished the layout
            tlp.Select();
            Refresh();
            tlp.ResumeLayout(true);

            _initialText = tlp.GetAllControlText();
            _isListReady = true;
        }

    }

}
