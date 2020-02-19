using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaCenter.SharedComponents
{

    public partial class TransparentTrackBar : TrackBar
    {

        private const int WM_DWMCOMPOSITIONCHANGED = 0x031A;
        private const int WM_THEMECHANGED = 0x031E;


        public new Color BackColor 
        { 
            get { return base.BackColor; } 
            set { if (value != Color.Transparent)  base.BackColor = value; } 
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TransparentTrackBar"/> class.
        /// </summary>
        public TransparentTrackBar()
            : base()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.CreateControl" /> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            if (Parent != null)
                BackColor = Parent.BackColor;

            base.OnCreateControl();
        }


        /// <summary>
        /// Raises the <see cref="Paint" /> event.
        /// </summary>
        /// <param name="pe">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.VisibleChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
        //protected override void OnVisibleChanged(EventArgs e)
        //{
        //    Color color = this.BackColor;
        //    BackColor = Color.FromArgb(color.R, color.G, color.B);
        //}


        /// <summary>
        /// Overrides the <see cref="System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> method.
        /// </summary>
        /// <param name="m">A Windows Message object.</param>
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_DWMCOMPOSITIONCHANGED || m.Msg == WM_THEMECHANGED)
        //        OnVisibleChanged(new EventArgs());

        //    base.WndProc(ref m);
        //}

    }

}
