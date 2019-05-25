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
    /// General text display form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class TextForm : Form
    {

        /// <summary>
        /// Gets or sets the caption (title) of the form.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the text content of the form.
        /// </summary>
        /// <value>
        /// The content of the text.
        /// </value>
        public string TextContent { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TextForm"/> class.
        /// </summary>
        public TextForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TextForm" /> class.
        /// </summary>
        /// <param name="caption">The caption (title) of the form.</param>
        /// <param name="textContent">The text displayed by the control.</param>
        public TextForm(string caption, string textContent)
            : this()
        {
            Caption = caption;
            TextContent = textContent;

            Text = Caption;
            MainTextBox.Text = TextContent;
        }


        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }

}
