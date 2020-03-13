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
        /// Handles the Click event of the CopyToClipboardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void CopyToClipboardButton_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(ErrorTextBox.Text);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="maxWindowSize">Maximum size of the window.</param>
        public static async Task ShowAsync(string message, Size maxWindowSize)
        {
            try
            {
                await ShowAsync(null, message, maxWindowSize);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="message">The message.</param>
        /// <param name="maxWindowSize">Maximum size of the window.</param>
        public static async Task ShowAsync(IWin32Window owner, string message, Size maxWindowSize)
        {
            try
            {
                await ShowAsync(owner, null, message, maxWindowSize);
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="maxWindowSize">Maximum size of the window.</param>
        public static async Task ShowAsync(IWin32Window owner, string title, string message, Size maxWindowSize)
        {
            try
            {
                var isAutoSize = false;
                Size textSize;

                // Get the size of a temporary text box
                using (var txt = new TextBox())
                {
                    txt.Text = message;
                    textSize = txt.GetControlTextSize();
                };

                using (var frm = new ErrorForm())
                {
                    frm.MaximumSize = maxWindowSize;

                    if ((textSize.Height > maxWindowSize.Height)
                        && (textSize.Width > maxWindowSize.Width))
                    {
                        frm.Size = maxWindowSize;
                        frm.ErrorTextBox.ScrollBars = ScrollBars.Both;
                    }
                    else if (textSize.Height > maxWindowSize.Height)
                    {
                        frm.ErrorTextBox.ScrollBars = ScrollBars.Vertical;
                        frm.ErrorTextBox.Height = maxWindowSize.Height - (2 * 20);
                        frm.ErrorTextBox.Width = textSize.Width;
                    }
                    else if (textSize.Width > maxWindowSize.Width)
                    {
                        frm.ErrorTextBox.ScrollBars = ScrollBars.Horizontal;
                        frm.ErrorTextBox.Height = textSize.Height;
                        frm.ErrorTextBox.Width = maxWindowSize.Width - (2 * 15);
                    }
                    else
                        isAutoSize = true;

                    if (!title.IsNullOrEmptyTrimmed())
                        frm.Text = title;

                    frm.ErrorTextBox.Text = message;
                    frm.ErrorTextBox.Select(0, 0);

                    if (isAutoSize)
                        frm.ErrorTextBox.AutoSizeTextBox();

                    frm.ShowDialog(owner);
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event..", ex);
            }
        }

    }

}
