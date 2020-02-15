namespace MediaCenter.LyricsFinder
{
    partial class McControlForm
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
            if (disposing)
            {
                if (components != null)
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
            this.McCurrentPositionTrackBar = new System.Windows.Forms.TrackBar();
            this.TrackingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.McCurrentPositionTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // McCurrentPositionTrackBar
            // 
            this.McCurrentPositionTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.McCurrentPositionTrackBar.LargeChange = 10;
            this.McCurrentPositionTrackBar.Location = new System.Drawing.Point(0, 0);
            this.McCurrentPositionTrackBar.Maximum = 100;
            this.McCurrentPositionTrackBar.Name = "McCurrentPositionTrackBar";
            this.McCurrentPositionTrackBar.Size = new System.Drawing.Size(176, 45);
            this.McCurrentPositionTrackBar.SmallChange = 5;
            this.McCurrentPositionTrackBar.TabIndex = 0;
            this.McCurrentPositionTrackBar.TabStop = false;
            this.McCurrentPositionTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.McCurrentPositionTrackBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.McCurrentPositionTrackBar_MouseDownAsync);
            this.McCurrentPositionTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.McCurrentPositionTrackBar_MouseUpAsync);
            // 
            // TrackingLabel
            // 
            this.TrackingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackingLabel.AutoSize = true;
            this.TrackingLabel.BackColor = System.Drawing.Color.White;
            this.TrackingLabel.Location = new System.Drawing.Point(90, 32);
            this.TrackingLabel.Name = "TrackingLabel";
            this.TrackingLabel.Size = new System.Drawing.Size(0, 13);
            this.TrackingLabel.TabIndex = 1;
            this.TrackingLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // McControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(180, 50);
            this.ControlBox = false;
            this.Controls.Add(this.McCurrentPositionTrackBar);
            this.Controls.Add(this.TrackingLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "McControlForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.McCurrentPositionTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar McCurrentPositionTrackBar;
        private System.Windows.Forms.Label TrackingLabel;
    }
}