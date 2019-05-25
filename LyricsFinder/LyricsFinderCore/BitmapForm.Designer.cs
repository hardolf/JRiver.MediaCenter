namespace MediaCenter.LyricsFinder
{
    partial class BitmapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BitmapPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.BitmapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BitmapPictureBox
            // 
            this.BitmapPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BitmapPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BitmapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.BitmapPictureBox.Name = "BitmapPictureBox";
            this.BitmapPictureBox.Size = new System.Drawing.Size(250, 250);
            this.BitmapPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BitmapPictureBox.TabIndex = 0;
            this.BitmapPictureBox.TabStop = false;
            // 
            // BitmapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(250, 250);
            this.ControlBox = false;
            this.Controls.Add(this.BitmapPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "BitmapForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BitmapForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.BitmapPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox BitmapPictureBox;
    }
}