namespace MediaCenter.LyricsFinder
{

    partial class McPlayControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(McPlayControl));
            this.McControlToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.McPositionTrackBar = new System.Windows.Forms.TrackBar();
            this.TrackingLabel = new System.Windows.Forms.Label();
            this.McControlLeftToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolsPlayStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            this.McControlToolStripContainer.ContentPanel.SuspendLayout();
            this.McControlToolStripContainer.LeftToolStripPanel.SuspendLayout();
            this.McControlToolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.McPositionTrackBar)).BeginInit();
            this.McControlLeftToolStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.McControlToolStripContainer.ContentPanel.Controls.Add(this.McPositionTrackBar);
            this.McControlToolStripContainer.ContentPanel.Controls.Add(this.TrackingLabel);
            this.McControlToolStripContainer.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.McControlToolStripContainer.ContentPanel.Size = new System.Drawing.Size(369, 60);
            this.McControlToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // McControlToolStripContainer.LeftToolStripPanel
            // 
            this.McControlToolStripContainer.LeftToolStripPanel.Controls.Add(this.McControlLeftToolStrip);
            this.McControlToolStripContainer.LeftToolStripPanel.MouseEnter += new System.EventHandler(this.McControlLeftToolStrip_MouseEnterAsync);
            this.McControlToolStripContainer.LeftToolStripPanel.MouseLeave += new System.EventHandler(this.McControlLeftToolStrip_MouseLeaveAsync);
            this.McControlToolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.McControlToolStripContainer.Name = "McControlToolStripContainer";
            // 
            // McControlToolStripContainer.RightToolStripPanel
            // 
            this.McControlToolStripContainer.RightToolStripPanel.Enabled = false;
            this.McControlToolStripContainer.RightToolStripPanelVisible = false;
            this.McControlToolStripContainer.Size = new System.Drawing.Size(400, 60);
            this.McControlToolStripContainer.TabIndex = 0;
            this.McControlToolStripContainer.Text = "toolStripContainer1";
            // 
            // McControlToolStripContainer.TopToolStripPanel
            // 
            this.McControlToolStripContainer.TopToolStripPanel.Enabled = false;
            this.McControlToolStripContainer.TopToolStripPanelVisible = false;
            // 
            // McPositionTrackBar
            // 
            this.McPositionTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.McPositionTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.McPositionTrackBar.LargeChange = 10;
            this.McPositionTrackBar.Location = new System.Drawing.Point(0, 3);
            this.McPositionTrackBar.Maximum = 100;
            this.McPositionTrackBar.Name = "McPositionTrackBar";
            this.McPositionTrackBar.Size = new System.Drawing.Size(356, 45);
            this.McPositionTrackBar.SmallChange = 5;
            this.McPositionTrackBar.TabIndex = 2;
            this.McPositionTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.McPositionTrackBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.McPositionTrackBar_KeyDownAndUpAsync);
            this.McPositionTrackBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.McPositionTrackBar_KeyPressAsync);
            this.McPositionTrackBar.KeyUp += new System.Windows.Forms.KeyEventHandler(this.McPositionTrackBar_KeyDownAndUpAsync);
            this.McPositionTrackBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.McPositionTrackBar_MouseDownAsync);
            this.McPositionTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.McPositionTrackBar_MouseUpAsync);
            this.McPositionTrackBar.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.McPositionTrackBar_PreviewKeyDownAsync);
            // 
            // TrackingLabel
            // 
            this.TrackingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackingLabel.AutoSize = true;
            this.TrackingLabel.BackColor = System.Drawing.SystemColors.Control;
            this.TrackingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrackingLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TrackingLabel.Location = new System.Drawing.Point(167, 42);
            this.TrackingLabel.Name = "TrackingLabel";
            this.TrackingLabel.Size = new System.Drawing.Size(0, 13);
            this.TrackingLabel.TabIndex = 3;
            this.TrackingLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            this.McControlLeftToolStrip.Size = new System.Drawing.Size(31, 25);
            this.McControlLeftToolStrip.TabIndex = 1;
            this.McControlLeftToolStrip.TabStop = true;
            this.McControlLeftToolStrip.MouseEnter += new System.EventHandler(this.McPlayControl_MouseEnterAsync);
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
            this.ToolsPlayStartStopButton.ToolTipText = "Start / pause play of the current item (Ctrl+P)";
            // 
            // McPlayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.McControlToolStripContainer);
            this.Name = "McPlayControl";
            this.Size = new System.Drawing.Size(400, 60);
            this.Load += new System.EventHandler(this.McPlayControl_LoadAsync);
            this.MouseEnter += new System.EventHandler(this.McPlayControl_MouseEnterAsync);
            this.MouseLeave += new System.EventHandler(this.McPlayControl_MouseLeaveAsync);
            this.McControlToolStripContainer.ContentPanel.ResumeLayout(false);
            this.McControlToolStripContainer.ContentPanel.PerformLayout();
            this.McControlToolStripContainer.LeftToolStripPanel.ResumeLayout(false);
            this.McControlToolStripContainer.LeftToolStripPanel.PerformLayout();
            this.McControlToolStripContainer.ResumeLayout(false);
            this.McControlToolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.McPositionTrackBar)).EndInit();
            this.McControlLeftToolStrip.ResumeLayout(false);
            this.McControlLeftToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer McControlToolStripContainer;
        private System.Windows.Forms.ToolStrip McControlLeftToolStrip;
        private System.Windows.Forms.Label TrackingLabel;
        private System.Windows.Forms.TrackBar McPositionTrackBar;
        private StartStopToolStripButton ToolsPlayStartStopButton;
    }

}
