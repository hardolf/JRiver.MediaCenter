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
                OwnerForm.Focus();
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
                do
                {
                    // Do the find or replace operation in the parent form
                    OwnerForm.FindReplaceAction(_isNext, FindTextBox.Text, (_isFind) ? null : ReplaceTextBox.Text);

                    // Ensure that the found text is highlighted in the parent form - and that this FindReplaceForm regains focus
                    OwnerForm.Focus();
                    Focus();

                    // From now on, we do repeat searches
                    _isNext = true;

                    OkButton.Text = "Next";

                    FindReplaceToolTip.SetToolTip(OkButton, (_isFind)
                        ? "Find next (F3)"
                        : "Find and replace next (F3)");
                } while (Visible && (sender == ReplaceAllButton));
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Initializes the specified find or replace and opens this form.
        /// </summary>
        /// <param name="ownerForm">The owner form.</param>
        /// <param name="isFind">if set to <c>true</c> find text; else find and replace text.</param>
        /// <param name="selectedText">The selected text in the parent form.</param>
        /// <exception cref="ArgumentNullException">owner</exception>
        internal void Show(LyricForm ownerForm, bool? isFind = null, string selectedText = "")
        {
            if (ownerForm is null) throw new ArgumentNullException(nameof(ownerForm));

            // If this form is already visible, this call must be a repeat search
            _isNext = Visible;

            // Null means that we should do the same operation as the last time we did a find/replace.
            // In that case, _isFind stays the same as the last time.
            if (isFind.HasValue)
                _isFind = isFind.Value;

            OwnerForm = ownerForm;

            Text = (_isFind) ? "Find text" : "Find and replace text";

            // Set the replace buttons' text and tooltips
            ReplaceAllButton.Enabled = !_isFind;
            ReplaceAllButton.Visible = !_isFind;

            ReplaceTextLabel.Enabled = !_isFind;
            ReplaceTextLabel.Visible = !_isFind;

            ReplaceTextBox.Enabled = !_isFind;
            ReplaceTextBox.Visible = !_isFind;

            // Set the OK (Find/replace) buttons' text and tooltips,
            // depending on whether this call is the first search or a repeat search
            if (_isNext)
            {
                OkButton.Text = "Next";

                FindReplaceToolTip.SetToolTip(OkButton, (_isFind)
                    ? "Find next (F3)"
                    : "Find and replace next (F3)");
            }
            else
            {
                OkButton.Text = (_isFind) ? "Find" : "Find/replace";

                if (!selectedText.IsNullOrEmptyTrimmed())
                    FindTextBox.Text = selectedText;

                FindReplaceToolTip.SetToolTip(OkButton, (_isFind)
                    ? "Find (F3)"
                    : "Find and replace (F3)");
            }

            // Place this form to the left of the parent's top-left corner
            Location = new Point(ownerForm.Left - Width, ownerForm.Top);
            BringToFront();

            if (!Visible)
                base.Show(ownerForm);

            // Ensure that the find text is highlighted
            OkButton.Select();
            FindTextBox.Select();

            if (_isNext)
                OkButton.PerformClick();

            // Prevent getting the selected text as the first match
            if (!selectedText.IsNullOrEmptyTrimmed())
                _isNext = true;
        }

    }

}
