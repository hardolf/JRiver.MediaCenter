using MediaCenter.LyricsFinder.Model.Helpers;
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


        /// <summary>
        /// Initializes a new instance of the <see cref="OptionForm" /> class.
        /// </summary>
        public OptionForm()
        {
            InitializeComponent();

            AllowTransparency = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OptionForm"/> class.
        /// </summary>
        /// <param name="title">The title text.</param>
        public OptionForm(string title)
            : this()
        {
            _title = title;
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
        /// Handles the FormClosing event of the ConfigurationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void OptionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var newText = OptionLayoutPanel.GetAllTextBoxesText() + Environment.NewLine + UpdateCheckIntervalUpDown.Value.ToString(CultureInfo.InvariantCulture);
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
                        Properties.Settings.Default.UpdateCheckInterval = (int)UpdateCheckIntervalUpDown.Value;
                        Properties.Settings.Default.Save();
                        LyricsFinderCorePrivateConfigurationSectionHandler.Save(McAccessKeyTextBox.Text.Trim(), McWsUrlTextBox.Text.Trim(), McWsUsernameTextBox.Text.Trim(), McWsPasswordTextBox.Text.Trim());
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
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

                McAccessKeyTextBox.Text = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceAccessKey;
                McWsPasswordTextBox.Text = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServicePassword;
                McWsUrlTextBox.Text = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUrl;
                McWsUsernameTextBox.Text = LyricsFinderCorePrivateConfigurationSectionHandler.McWebServiceUserName;

                UpdateCheckIntervalUpDown.Value = Properties.Settings.Default.UpdateCheckInterval;

                _initialText = OptionLayoutPanel.GetAllTextBoxesText() + Environment.NewLine + UpdateCheckIntervalUpDown.Value.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }

    }

}
