using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Close();
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Show(string message)
        {
            Show(null, message);
        }


        /// <summary>
        /// Shows the specified message.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="message">The message.</param>
        public static void Show(IWin32Window owner, string message)
        {
            var frm = new ErrorForm();

            frm.ErrorTextBox.Text = message;
            frm.ErrorTextBox.Select(0, 0);

            frm.ShowDialog(owner);
        }

    }

}
