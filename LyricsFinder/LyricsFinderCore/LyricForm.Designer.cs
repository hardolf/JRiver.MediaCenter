namespace MediaCenter.LyricsFinder
{
    partial class LyricForm
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

                if (_searchForm != null)
                    _searchForm.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.LyricsFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LyricsFormTrackBar = new System.Windows.Forms.TrackBar();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ArtistTextBox = new System.Windows.Forms.TextBox();
            this.AlbumTextBox = new System.Windows.Forms.TextBox();
            this.TrackTextBox = new System.Windows.Forms.TextBox();
            this.LyricsFormTimer = new System.Windows.Forms.Timer(this.components);
            this.LyricsFormPanel = new System.Windows.Forms.Panel();
            this.LyricsParmsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ArtistLabel = new System.Windows.Forms.Label();
            this.AlbumLabel = new System.Windows.Forms.Label();
            this.TrackLabel = new System.Windows.Forms.Label();
            this.LyricsFormStatusStrip = new System.Windows.Forms.StatusStrip();
            this.LyricsFormStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LyricsTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LyricsFormTrackBar)).BeginInit();
            this.LyricsFormPanel.SuspendLayout();
            this.LyricsParmsPanel.SuspendLayout();
            this.LyricsFormStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // LyricsFormTrackBar
            // 
            this.LyricsFormTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LyricsFormTrackBar.Location = new System.Drawing.Point(3, 391);
            this.LyricsFormTrackBar.Maximum = 0;
            this.LyricsFormTrackBar.Name = "LyricsFormTrackBar";
            this.LyricsFormTrackBar.Size = new System.Drawing.Size(283, 45);
            this.LyricsFormTrackBar.TabIndex = 6;
            this.LyricsFormToolTip.SetToolTip(this.LyricsFormTrackBar, "Switch between all the lyrics search results (Arrows)");
            this.LyricsFormTrackBar.Visible = false;
            this.LyricsFormTrackBar.Scroll += new System.EventHandler(this.LyricsFormTrackBar_Scroll);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(211, 401);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 7;
            this.SearchButton.Text = "&Search";
            this.LyricsFormToolTip.SetToolTip(this.SearchButton, "Search for more lyrics (Alt+S)");
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Visible = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(300, 401);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(74, 23);
            this.CloseButton.TabIndex = 8;
            this.CloseButton.Text = "&Close (Esc)";
            this.LyricsFormToolTip.SetToolTip(this.CloseButton, "Close the window (Esc)");
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ArtistTextBox
            // 
            this.ArtistTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArtistTextBox.Location = new System.Drawing.Point(3, 18);
            this.ArtistTextBox.Multiline = true;
            this.ArtistTextBox.Name = "ArtistTextBox";
            this.ArtistTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ArtistTextBox.Size = new System.Drawing.Size(121, 43);
            this.ArtistTextBox.TabIndex = 1;
            this.LyricsFormToolTip.SetToolTip(this.ArtistTextBox, "Artist name, change to refine search");
            // 
            // AlbumTextBox
            // 
            this.AlbumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlbumTextBox.Location = new System.Drawing.Point(130, 18);
            this.AlbumTextBox.Multiline = true;
            this.AlbumTextBox.Name = "AlbumTextBox";
            this.AlbumTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AlbumTextBox.Size = new System.Drawing.Size(122, 43);
            this.AlbumTextBox.TabIndex = 1;
            this.LyricsFormToolTip.SetToolTip(this.AlbumTextBox, "Album name, change to refine search");
            // 
            // TrackTextBox
            // 
            this.TrackTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackTextBox.Location = new System.Drawing.Point(258, 18);
            this.TrackTextBox.Multiline = true;
            this.TrackTextBox.Name = "TrackTextBox";
            this.TrackTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TrackTextBox.Size = new System.Drawing.Size(123, 43);
            this.TrackTextBox.TabIndex = 1;
            this.LyricsFormToolTip.SetToolTip(this.TrackTextBox, "Track title, change to refine search");
            // 
            // LyricsFormTimer
            // 
            this.LyricsFormTimer.Tick += new System.EventHandler(this.LyricsFormTimer_Tick);
            // 
            // LyricsFormPanel
            // 
            this.LyricsFormPanel.Controls.Add(this.LyricsParmsPanel);
            this.LyricsFormPanel.Controls.Add(this.LyricsFormStatusStrip);
            this.LyricsFormPanel.Controls.Add(this.LyricsFormTrackBar);
            this.LyricsFormPanel.Controls.Add(this.SearchButton);
            this.LyricsFormPanel.Controls.Add(this.LyricsTextBox);
            this.LyricsFormPanel.Controls.Add(this.CloseButton);
            this.LyricsFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricsFormPanel.Location = new System.Drawing.Point(0, 0);
            this.LyricsFormPanel.Name = "LyricsFormPanel";
            this.LyricsFormPanel.Size = new System.Drawing.Size(384, 461);
            this.LyricsFormPanel.TabIndex = 0;
            // 
            // LyricsParmsPanel
            // 
            this.LyricsParmsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricsParmsPanel.ColumnCount = 3;
            this.LyricsParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.LyricsParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.LyricsParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.LyricsParmsPanel.Controls.Add(this.ArtistLabel, 0, 0);
            this.LyricsParmsPanel.Controls.Add(this.AlbumLabel, 1, 0);
            this.LyricsParmsPanel.Controls.Add(this.TrackLabel, 2, 0);
            this.LyricsParmsPanel.Controls.Add(this.ArtistTextBox, 0, 1);
            this.LyricsParmsPanel.Controls.Add(this.AlbumTextBox, 1, 1);
            this.LyricsParmsPanel.Controls.Add(this.TrackTextBox, 2, 1);
            this.LyricsParmsPanel.Location = new System.Drawing.Point(0, 4);
            this.LyricsParmsPanel.Name = "LyricsParmsPanel";
            this.LyricsParmsPanel.RowCount = 2;
            this.LyricsParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.LyricsParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LyricsParmsPanel.Size = new System.Drawing.Size(384, 64);
            this.LyricsParmsPanel.TabIndex = 10;
            // 
            // ArtistLabel
            // 
            this.ArtistLabel.AutoSize = true;
            this.ArtistLabel.Location = new System.Drawing.Point(3, 0);
            this.ArtistLabel.Name = "ArtistLabel";
            this.ArtistLabel.Size = new System.Drawing.Size(30, 13);
            this.ArtistLabel.TabIndex = 0;
            this.ArtistLabel.Text = "Artist";
            // 
            // AlbumLabel
            // 
            this.AlbumLabel.AutoSize = true;
            this.AlbumLabel.Location = new System.Drawing.Point(130, 0);
            this.AlbumLabel.Name = "AlbumLabel";
            this.AlbumLabel.Size = new System.Drawing.Size(36, 13);
            this.AlbumLabel.TabIndex = 0;
            this.AlbumLabel.Text = "Album";
            // 
            // TrackLabel
            // 
            this.TrackLabel.AutoSize = true;
            this.TrackLabel.Location = new System.Drawing.Point(258, 0);
            this.TrackLabel.Name = "TrackLabel";
            this.TrackLabel.Size = new System.Drawing.Size(58, 13);
            this.TrackLabel.TabIndex = 0;
            this.TrackLabel.Text = "Track Title";
            // 
            // LyricsFormStatusStrip
            // 
            this.LyricsFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LyricsFormStatusLabel});
            this.LyricsFormStatusStrip.Location = new System.Drawing.Point(0, 439);
            this.LyricsFormStatusStrip.Name = "LyricsFormStatusStrip";
            this.LyricsFormStatusStrip.Size = new System.Drawing.Size(384, 22);
            this.LyricsFormStatusStrip.TabIndex = 9;
            this.LyricsFormStatusStrip.Text = "statusStrip1";
            // 
            // LyricsFormStatusLabel
            // 
            this.LyricsFormStatusLabel.Name = "LyricsFormStatusLabel";
            this.LyricsFormStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // LyricsTextBox
            // 
            this.LyricsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricsTextBox.Location = new System.Drawing.Point(3, 74);
            this.LyricsTextBox.Multiline = true;
            this.LyricsTextBox.Name = "LyricsTextBox";
            this.LyricsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LyricsTextBox.Size = new System.Drawing.Size(378, 311);
            this.LyricsTextBox.TabIndex = 5;
            // 
            // LyricForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.LyricsFormPanel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(800, 1000);
            this.MinimumSize = new System.Drawing.Size(100, 120);
            this.Name = "LyricForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LyricsForm";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LyricsForm_FormClosing);
            this.Load += new System.EventHandler(this.LyricsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricsForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.LyricsFormTrackBar)).EndInit();
            this.LyricsFormPanel.ResumeLayout(false);
            this.LyricsFormPanel.PerformLayout();
            this.LyricsParmsPanel.ResumeLayout(false);
            this.LyricsParmsPanel.PerformLayout();
            this.LyricsFormStatusStrip.ResumeLayout(false);
            this.LyricsFormStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip LyricsFormToolTip;
        private System.Windows.Forms.Timer LyricsFormTimer;
        private System.Windows.Forms.Panel LyricsFormPanel;
        private System.Windows.Forms.StatusStrip LyricsFormStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel LyricsFormStatusLabel;
        private System.Windows.Forms.TrackBar LyricsFormTrackBar;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox LyricsTextBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TableLayoutPanel LyricsParmsPanel;
        private System.Windows.Forms.Label ArtistLabel;
        private System.Windows.Forms.Label AlbumLabel;
        private System.Windows.Forms.Label TrackLabel;
        private System.Windows.Forms.TextBox ArtistTextBox;
        private System.Windows.Forms.TextBox AlbumTextBox;
        private System.Windows.Forms.TextBox TrackTextBox;
    }
}