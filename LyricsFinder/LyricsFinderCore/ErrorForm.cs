using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{
    /// <summary>
    /// Error form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    /// <remarks>
    /// Used instead of the <see cref="MessageBox"/> which does not show properly when used as a JRiver Media Center plug-in.
    /// </remarks>
    public partial class ErrorForm : Form
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorForm" /> class.
        /// </summary>
        private ErrorForm()
        {
            InitializeComponent();

            AllowTransparency = false;
        }


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
        /// Handles the Click event of the CopyToClipboardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CopyToClipboardButton_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(ErrorTextBox.Text);
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Show(string message)
        {
            try
            {
                Show(null, message);
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="message">The message.</param>
        public static void Show(IWin32Window owner, string message)
        {
            try
            {
                using (var frm = new ErrorForm())
                {
                    frm.ErrorTextBox.Text = message;
                    frm.ErrorTextBox.Select(0, 0);
                    frm.ErrorTextBox.AutoSizeTextBox();

                    frm.ShowDialog(owner); 
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public static void Show(IWin32Window owner, string title, string message)
        {
            try
            {
                using (var frm = new ErrorForm())
                {
                    frm.ErrorTextBox.Text = message;
                    frm.ErrorTextBox.Select(0, 0);
                    frm.Text = title;
                    frm.ErrorTextBox.AutoSizeTextBox();

                    frm.ShowDialog(owner);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }

    }

}
