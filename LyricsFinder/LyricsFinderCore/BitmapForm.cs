using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Form to show the item bitmap.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class BitmapForm : Form
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapForm"/> class.
        /// </summary>
        protected BitmapForm()
        {
            InitializeComponent();

            AllowTransparency = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapForm" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="location">The location.</param>
        public BitmapForm(Bitmap image, Point location)
            : this()
        {
            location.Offset(20, -Convert.ToInt32(Size.Height / 2));

            BitmapPictureBox.Image = image;
            Location = location;
            Size = image.Size;
        }


        /// <summary>
        /// Handles the KeyDown event of the BitmapForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void BitmapForm_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            Close();
        }

    }

}
