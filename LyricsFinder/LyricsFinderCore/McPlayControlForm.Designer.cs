namespace MediaCenter.LyricsFinder
{
    partial class McPlayControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(McPlayControlForm));
            this.McCurrentPositionTrackBar = new System.Windows.Forms.TrackBar();
            this.TrackingLabel = new System.Windows.Forms.Label();
            this.McControlToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.McControlLeftToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolsPlayStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.McCurrentPositionTrackBar)).BeginInit();
            this.McControlToolStripContainer.ContentPanel.SuspendLayout();
            this.McControlToolStripContainer.LeftToolStripPanel.SuspendLayout();
            this.McControlToolStripContainer.SuspendLayout();
            this.McControlLeftToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // McCurrentPositionTrackBar
            // 
            this.McCurrentPositionTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.McCurrentPositionTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.McCurrentPositionTrackBar.LargeChange = 10;
            this.McCurrentPositionTrackBar.Location = new System.Drawing.Point(0, 0);
            this.McCurrentPositionTrackBar.Maximum = 100;
            this.McCurrentPositionTrackBar.Name = "McCurrentPositionTrackBar";
            this.McCurrentPositionTrackBar.Size = new System.Drawing.Size(322, 45);
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
            this.TrackingLabel.BackColor = System.Drawing.SystemColors.Control;
            this.TrackingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrackingLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TrackingLabel.Location = new System.Drawing.Point(90, 82);
            this.TrackingLabel.Name = "TrackingLabel";
            this.TrackingLabel.Size = new System.Drawing.Size(0, 13);
            this.TrackingLabel.TabIndex = 1;
            this.TrackingLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // McControlToolStripContainer
            // 
            // 
            // McControlToolStripContainer.BottomToolStripPanel
            // 
            this.McControlToolStripContainer.BottomToolStripPanel.Enabled = false;
            this.McControlToolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // McControlToolStripContainer.ContentPanel
            // 
            this.McControlToolStripContainer.ContentPanel.BackColor = System.Drawing.Color.Transparent;
            this.McControlToolStripContainer.ContentPanel.Controls.Add(this.McCurrentPositionTrackBar);
            this.McControlToolStripContainer.ContentPanel.Controls.Add(this.TrackingLabel);
            this.McControlToolStripContainer.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 0, 50, 0);
            this.McControlToolStripContainer.ContentPanel.Size = new System.Drawing.Size(368, 100);
            this.McControlToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // McControlToolStripContainer.LeftToolStripPanel
            // 
            this.McControlToolStripContainer.LeftToolStripPanel.BackColor = System.Drawing.Color.Transparent;
            this.McControlToolStripContainer.LeftToolStripPanel.Controls.Add(this.McControlLeftToolStrip);
            this.McControlToolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.McControlToolStripContainer.Name = "McControlToolStripContainer";
            // 
            // McControlToolStripContainer.RightToolStripPanel
            // 
            this.McControlToolStripContainer.RightToolStripPanel.Enabled = false;
            this.McControlToolStripContainer.RightToolStripPanelVisible = false;
            this.McControlToolStripContainer.Size = new System.Drawing.Size(400, 100);
            this.McControlToolStripContainer.TabIndex = 2;
            // 
            // McControlToolStripContainer.TopToolStripPanel
            // 
            this.McControlToolStripContainer.TopToolStripPanel.Enabled = false;
            this.McControlToolStripContainer.TopToolStripPanelVisible = false;
            // 
            // McControlLeftToolStrip
            // 
            this.McControlLeftToolStrip.AllowMerge = false;
            this.McControlLeftToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.McControlLeftToolStrip.CanOverflow = false;
            this.McControlLeftToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.McControlLeftToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.McControlLeftToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsPlayStartStopButton});
            this.McControlLeftToolStrip.Location = new System.Drawing.Point(0, 3);
            this.McControlLeftToolStrip.Name = "McControlLeftToolStrip";
            this.McControlLeftToolStrip.Size = new System.Drawing.Size(32, 44);
            this.McControlLeftToolStrip.TabIndex = 2;
            this.McControlLeftToolStrip.MouseEnter += new System.EventHandler(this.McControlLeftToolStrip_MouseEnterAsync);
            // 
            // ToolsPlayStartStopButton
            // 
            this.ToolsPlayStartStopButton.AutoSize = false;
            this.ToolsPlayStartStopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ToolsPlayStartStopButton.Clicked = false;
            this.ToolsPlayStartStopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolsPlayStartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("ToolsPlayStartStopButton.ImageStart")));
            this.ToolsPlayStartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("ToolsPlayStartStopButton.ImageStop")));
            this.ToolsPlayStartStopButton.ImageTransparentColor = System.Drawing.SystemColors.Control;
            this.ToolsPlayStartStopButton.IsRunning = false;
            this.ToolsPlayStartStopButton.Name = "ToolsPlayStartStopButton";
            this.ToolsPlayStartStopButton.Size = new System.Drawing.Size(30, 20);
            this.ToolsPlayStartStopButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.ToolsPlayStartStopButton.TextStart = "";
            this.ToolsPlayStartStopButton.TextStop = "";
            this.ToolsPlayStartStopButton.ToolTipText = "Start / stop play of the current item";
            // 
            // McPlayControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(400, 100);
            this.ControlBox = false;
            this.Controls.Add(this.McControlToolStripContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "McPlayControlForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.Load += new System.EventHandler(this.McControlForm_LoadAsync);
            this.MouseEnter += new System.EventHandler(this.McControlForm_MouseEnterAsync);
            ((System.ComponentModel.ISupportInitialize)(this.McCurrentPositionTrackBar)).EndInit();
            this.McControlToolStripContainer.ContentPanel.ResumeLayout(false);
            this.McControlToolStripContainer.ContentPanel.PerformLayout();
            this.McControlToolStripContainer.LeftToolStripPanel.ResumeLayout(false);
            this.McControlToolStripContainer.LeftToolStripPanel.PerformLayout();
            this.McControlToolStripContainer.ResumeLayout(false);
            this.McControlToolStripContainer.PerformLayout();
            this.McControlLeftToolStrip.ResumeLayout(false);
            this.McControlLeftToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar McCurrentPositionTrackBar;
        private System.Windows.Forms.Label TrackingLabel;
        private System.Windows.Forms.ToolStripContainer McControlToolStripContainer;
        private System.Windows.Forms.ToolStrip McControlLeftToolStrip;
        private StartStopToolStripButton ToolsPlayStartStopButton;
    }
}