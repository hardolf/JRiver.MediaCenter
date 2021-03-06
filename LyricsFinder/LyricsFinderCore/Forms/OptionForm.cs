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

using MediaCenter.SharedComponents;
using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;

namespace MediaCenter.LyricsFinder.Forms
{

    /// <summary>
    /// Configuration form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class OptionForm : Form
    {

        private readonly string _title = "Options";

        private readonly string _headerText = " Set the LyricsFinder options here." + Constants.NewLine
            + " Enable the LyricsFinder to connect with the Media Center by setting the connection options." + Constants.NewLine
            + " You can find the values in the Media Center (Tools menu > Options > Media Network)." + Constants.NewLine
            + " Also, ensure that the Media Network service is enabled.";

        private string _initialText = string.Empty;

        private readonly LyricsFinderDataType _lyricsFinderData = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="OptionForm" /> class.
        /// </summary>
        private OptionForm()
        {
            InitializeComponent();

            AllowTransparency = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OptionForm" /> class.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="lyricsFinderData">The lyrics finder data.</param>
        public OptionForm(string title, LyricsFinderDataType lyricsFinderData)
            : this()
        {
            _title = title ?? _title;
            _lyricsFinderData = lyricsFinderData ?? throw new ArgumentNullException(nameof(lyricsFinderData));
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
        /// Handles the FormClosing event of the ConfigurationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private async void OptionForm_FormClosingAsync(object sender, FormClosingEventArgs e)
        {
            try
            {
                var newText = OptionLayoutPanel.GetAllControlText();
                var question = "Do you want to use the new values?";
                var result = DialogResult.No;

                if (newText != _initialText)
                    result = MessageBox.Show(this, question, "Values are changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;

                    case DialogResult.No:
                        e.Cancel = false;
                        break;

                    case DialogResult.Yes:
                        e.Cancel = false;
                        try
                        {
                            // Save the data contents
                            _lyricsFinderData.MainData.CollectPlaylistInfoOnMcReconnectPlugin = CollectPlaylistInfoOnMcReconnectPlugingCheckBox.Checked;
                            _lyricsFinderData.MainData.CollectPlaylistInfoOnMcReconnectStandalone = CollectPlaylistInfoOnMcReconnectStandaloneCheckBox.Checked;
                            _lyricsFinderData.MainData.DelayMilliSecondsBetweenSearches = (int)DelayMilliSecondsBetweenSearchesUpDown.Value;
                            // _lyricsFinderData.MainData.LastUpdateCheck = DateTime.Parse(LastUpdateCheckTextBox.Text, CultureInfo.InvariantCulture); // Readonly!
                            _lyricsFinderData.MainData.MaxQueueLength = (int)MaxQueueLengthUpDown.Value;
                            _lyricsFinderData.MainData.McAccessKey = McAccessKeyTextBox.Text;
                            _lyricsFinderData.MainData.MaxMcWsConnectAttempts = (int)MaxMcWsConnectAttemptsUpDown.Value;
                            _lyricsFinderData.MainData.McWsPassword = McWsPasswordTextBox.Text;
                            _lyricsFinderData.MainData.McWsUrl = McWsUrlTextBox.Text;
                            _lyricsFinderData.MainData.McWsUsername = McWsUsernameTextBox.Text;
                            _lyricsFinderData.MainData.MouseMoveOpenLyricsForm = MouseMoveOpenLyricsFormCheckBox.Checked;
                            _lyricsFinderData.MainData.NoLyricsSearchFilter = NoLyricsSearchFilterTextBox.Text;
                            _lyricsFinderData.MainData.SerialServiceRequestsDuringAutomaticSearch = SerialServiceRequestsDuringAutomaticSearchCheckBox.Checked;
                            _lyricsFinderData.MainData.StrictSearchOnly = StrictSearchOnlyCheckBox.Checked;
                            _lyricsFinderData.MainData.UpdateCheckIntervalDays = (int)UpdateCheckIntervalDaysUpDown.Value;

                            await _lyricsFinderData.SaveAsync();
                        }
                        catch (Exception ex)
                        {
                            await ErrorHandling.ShowErrorHandlerAsync(this, $"Options failed to save, look at the error details and try again: {Constants.NewLine}{ex}");
                            e.Cancel = true;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Load event of the ConfigurationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void OptionForm_LoadAsync(object sender, EventArgs e)
        {
            try
            {
                Text = _title;
                HeaderTextBox.Text = _headerText;

                // Set the data contents
                CollectPlaylistInfoOnMcReconnectPlugingCheckBox.Checked = _lyricsFinderData.MainData.CollectPlaylistInfoOnMcReconnectPlugin;
                CollectPlaylistInfoOnMcReconnectStandaloneCheckBox.Checked = _lyricsFinderData.MainData.CollectPlaylistInfoOnMcReconnectStandalone;
                DelayMilliSecondsBetweenSearchesUpDown.Value = _lyricsFinderData.MainData.DelayMilliSecondsBetweenSearches;
                LastUpdateCheckTextBox.Text = _lyricsFinderData.MainData.LastUpdateCheck.ToString(CultureInfo.CurrentCulture);
                MaxQueueLengthUpDown.Value = _lyricsFinderData.MainData.MaxQueueLength;
                McAccessKeyTextBox.Text = _lyricsFinderData.MainData.McAccessKey;
                MaxMcWsConnectAttemptsUpDown.Value = _lyricsFinderData.MainData.MaxMcWsConnectAttempts;
                McWsPasswordTextBox.Text = _lyricsFinderData.MainData.McWsPassword;
                McWsUrlTextBox.Text = _lyricsFinderData.MainData.McWsUrl;
                McWsUsernameTextBox.Text = _lyricsFinderData.MainData.McWsUsername;
                MouseMoveOpenLyricsFormCheckBox.Checked = _lyricsFinderData.MainData.MouseMoveOpenLyricsForm;
                NoLyricsSearchFilterTextBox.Text = _lyricsFinderData.MainData.NoLyricsSearchFilter;
                SerialServiceRequestsDuringAutomaticSearchCheckBox.Checked = _lyricsFinderData.MainData.SerialServiceRequestsDuringAutomaticSearch;
                StrictSearchOnlyCheckBox.Checked = _lyricsFinderData.MainData.StrictSearchOnly;
                UpdateCheckIntervalDaysUpDown.Value = _lyricsFinderData.MainData.UpdateCheckIntervalDays;

                // Set the labels' tooltips the same as the tooltips of the controls to the right of the labels.
                // This is of secondary importance, so we ignore errors here.
                try
                {
                    System.Collections.IList list = OptionLayoutPanel.Controls;

                    for (int i = 0; i < list.Count; i++)
                    {
                        var ctl = list[i];

                        if (ctl is Label lbl)
                        {
                            var nextCtl = list[i + 1];

                            if (nextCtl is Control valueCtl)
                                OptionToolTip.SetToolTip(lbl, OptionToolTip.GetToolTip(valueCtl));
                        }
                    }
                }
                catch
                {
                    // I don't care
                }

                // Select the top TextBox and minimize the selected text
                McAccessKeyTextBox.Select();
                McAccessKeyTextBox.SelectionLength = 0;
                McAccessKeyTextBox.SelectionStart = 0;

                // Save the initial sum of control values for later change detection
                _initialText = OptionLayoutPanel.GetAllControlText();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Validating event of all the TextBox controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private async void TextBox_ValidatingAsync(object sender, CancelEventArgs e)
        {
            if (!(sender is Control ctl)) return;

            var ctlName = ctl?.Name;

            try
            {
                switch (ctlName)
                {
                    case nameof(CollectPlaylistInfoOnMcReconnectPlugingCheckBox):
                    case nameof(LastUpdateCheckTextBox):
                    case nameof(MouseMoveOpenLyricsFormCheckBox):
                    case nameof(NoLyricsSearchFilterTextBox):
                    case nameof(SerialServiceRequestsDuringAutomaticSearchCheckBox):
                    case nameof(StrictSearchOnlyCheckBox):
                        e.Cancel = false;
                        break;

                    case nameof(MaxQueueLengthUpDown):
                    case nameof(MaxMcWsConnectAttemptsUpDown):
                    case nameof(UpdateCheckIntervalDaysUpDown):
                    case nameof(DelayMilliSecondsBetweenSearchesUpDown):
                        e.Cancel = !int.TryParse(ctl.Text, out _);
                        break;

                    case nameof(McAccessKeyTextBox):
                    case nameof(McWsPasswordTextBox):
                    case nameof(McWsUrlTextBox):
                    case nameof(McWsUsernameTextBox):
                        e.Cancel = ctl.Text.IsNullOrEmptyTrimmed();
                        break;

                    default:
                        throw new Exception($"Unknown control: \"{ctlName}\".");
                }

                if (e.Cancel)
                {
                    Console.Beep();
                    ctl.ForeColor = Color.DarkRed;
                }
                else
                    ctl.ForeColor = SystemColors.WindowText;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }

    }

}
