using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Find/replace text form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FindReplaceForm : Form
    {

        private bool _isFind = true;
        private bool _isNext = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="FindReplaceForm"/> class.
        /// </summary>
        public FindReplaceForm()
        {
            InitializeComponent();
        }


        private LyricForm OwnerForm { get; set; }


        /// <summary>
        /// Handles the Click event of the CancelFormButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// The form does not close but merely hides, thus keeping it's text box values between activations.
        /// </remarks>
        private async void CancelFormButton_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Hide();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Activated event of the FindReplaceForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void FindReplaceForm_ActivatedAsync(object sender, EventArgs e)
        {
            try
            {
                FindTextBox.Select();
                // Console.Beep();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the FindReplaceForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private async void FindReplaceForm_KeyDownAsync(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = false;

                if ((e.KeyData == Keys.F3)
                    && !(e.Alt || e.Control || e.Shift))
                {
                    e.Handled = true;
                    OkButton.PerformClick();
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the MouseEnter event of the FindReplaceForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void FindReplaceForm_MouseEnterAsync(object sender, EventArgs e)
        {
            try
            {
                Focus();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the OkButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void OkButton_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                OwnerForm.FindReplaceAction(_isNext, FindTextBox.Text, (_isFind) ? null : ReplaceTextBox.Text);
                OwnerForm.Focus();

                Focus();

                _isNext = true;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Initializes the specified is find.
        /// </summary>
        /// <param name="ownerForm">The owner window.</param>
        /// <param name="isFind">if set to <c>true</c> find text; else replace text.</param>
        /// <param name="selectedText">The selected text.</param>
        /// <exception cref="ArgumentNullException">owner</exception>
        internal void Show(LyricForm ownerForm, bool? isFind = null, string selectedText = "")
        {
            if (ownerForm is null) throw new ArgumentNullException(nameof(ownerForm));

            _isNext = Visible;

            if (isFind.HasValue)
                _isFind = isFind.Value;

            OwnerForm = ownerForm;

            Text = (_isFind) ? "Find text" : "Find and replace text";

            ReplaceTextBox.Enabled = !_isFind;
            ReplaceTextLabel.Enabled = !_isFind;

            ReplaceTextBox.Visible = !_isFind;
            ReplaceTextLabel.Visible = !_isFind;

            if (_isNext)
            {
                OkButton.Text = "Next";

                FindReplaceToolTip.SetToolTip(OkButton, (_isFind) 
                    ? "Find next (F3)" 
                    : "Find and replace next (F3)");
            }
            else
            {
                OkButton.Text = "OK";

                if (!selectedText.IsNullOrEmptyTrimmed())
                    FindTextBox.Text = selectedText;

                FindReplaceToolTip.SetToolTip(OkButton, (_isFind)
                    ? "Find (F3)"
                    : "Find and replace (F3)");
            }

            Location = new Point(ownerForm.Left - Width, ownerForm.Top);
            BringToFront();
            FindTextBox.Focus();
            FindTextBox.SelectAll();

            if (!Visible)
                base.Show(ownerForm);
        }

    }

}
