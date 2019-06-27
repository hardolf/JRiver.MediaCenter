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

using MediaCenter.SharedComponents;
using MediaCenter.LyricsFinder.Model;
using MediaCenter.LyricsFinder.Model.Helpers;

namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Configuration form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class OptionForm : Form
    {

        private string _title = "Options";

        private string _headerText = " Set the options here.\r\n"
            + " Set the parameters in order to enable the LyricsFinder to connect with the Media Center.\r\n"
            + " You can find the values in the Media Center (Tools menu > Options > Media Network).\r\n"
            + " Also, ensure that the Media Network service is enabled.";

        private string _initialText = string.Empty;

        private LyricsFinderDataType _lyricsFinderData = null;


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
        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the FormClosing event of the ConfigurationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void OptionForm_FormClosing(object sender, FormClosingEventArgs e)
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
                            // _lyricsFinderData.MainData.LastUpdateCheck = DateTime.Parse(LastUpdateCheckTextBox.Text, CultureInfo.InvariantCulture); // Readonly!
                            _lyricsFinderData.MainData.MaxQueueLength = (int)MaxQueueLengthUpDown.Value;
                            _lyricsFinderData.MainData.McAccessKey = McAccessKeyTextBox.Text;
                            _lyricsFinderData.MainData.McWsConnectAttempts = (int)McWsConnectAttemptsUpDown.Value;
                            _lyricsFinderData.MainData.McWsPassword = McWsPasswordTextBox.Text;
                            _lyricsFinderData.MainData.McWsUrl = McWsUrlTextBox.Text;
                            _lyricsFinderData.MainData.McWsUsername = McWsUsernameTextBox.Text;
                            _lyricsFinderData.MainData.MouseMoveOpenLyricsForm = MouseMoveOpenLyricsFormCheckBox.Checked;
                            _lyricsFinderData.MainData.NoLyricsSearchFilter = NoLyricsSearchFilterTextBox.Text;
                            _lyricsFinderData.MainData.UpdateCheckIntervalDays = (int)UpdateCheckIntervalDaysUpDown.Value;

                            _lyricsFinderData.Save();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandling.ShowErrorHandler(this, $"Options failed to save, look at the error details and try again: \r\n{ex}");
                            e.Cancel = true;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Load event of the ConfigurationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OptionForm_Load(object sender, EventArgs e)
        {
            try
            {
                Text = _title;
                HeaderTextBox.Text = _headerText;

                LastUpdateCheckTextBox.Text = _lyricsFinderData.MainData.LastUpdateCheck.ToString(CultureInfo.CurrentCulture);
                MaxQueueLengthUpDown.Value = _lyricsFinderData.MainData.MaxQueueLength;
                McAccessKeyTextBox.Text = _lyricsFinderData.MainData.McAccessKey;
                McWsConnectAttemptsUpDown.Value = _lyricsFinderData.MainData.McWsConnectAttempts;
                McWsPasswordTextBox.Text = _lyricsFinderData.MainData.McWsPassword;
                McWsUrlTextBox.Text = _lyricsFinderData.MainData.McWsUrl;
                McWsUsernameTextBox.Text = _lyricsFinderData.MainData.McWsUsername;
                MouseMoveOpenLyricsFormCheckBox.Checked = _lyricsFinderData.MainData.MouseMoveOpenLyricsForm;
                NoLyricsSearchFilterTextBox.Text = _lyricsFinderData.MainData.NoLyricsSearchFilter;
                UpdateCheckIntervalDaysUpDown.Value = _lyricsFinderData.MainData.UpdateCheckIntervalDays;

                McAccessKeyTextBox.Select();
                McAccessKeyTextBox.SelectionLength = 0;
                McAccessKeyTextBox.SelectionStart = 0;

                _initialText = OptionLayoutPanel.GetAllControlText();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Validating event of all the TextBox controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!(sender is Control ctl)) return;

            var ctlName = ctl?.Name;

            switch (ctlName)
            {
                case nameof(LastUpdateCheckTextBox):
                case nameof(MouseMoveOpenLyricsFormCheckBox):
                case nameof(NoLyricsSearchFilterTextBox):
                    e.Cancel = false;
                    break;

                case nameof(MaxQueueLengthUpDown):
                case nameof(McWsConnectAttemptsUpDown):
                case nameof(UpdateCheckIntervalDaysUpDown):
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

    }

}
