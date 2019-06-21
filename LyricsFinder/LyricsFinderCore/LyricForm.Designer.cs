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
            this.LyricFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LyricFormTrackBar = new System.Windows.Forms.TrackBar();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ArtistTextBox = new System.Windows.Forms.TextBox();
            this.AlbumTextBox = new System.Windows.Forms.TextBox();
            this.TrackTextBox = new System.Windows.Forms.TextBox();
            this.LyricFormPanel = new System.Windows.Forms.Panel();
            this.LyricParmsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ArtistLabel = new System.Windows.Forms.Label();
            this.AlbumLabel = new System.Windows.Forms.Label();
            this.TrackLabel = new System.Windows.Forms.Label();
            this.LyricFormStatusStrip = new System.Windows.Forms.StatusStrip();
            this.LyricFormStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LyricFormFoundStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LyricTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LyricFormTrackBar)).BeginInit();
            this.LyricFormPanel.SuspendLayout();
            this.LyricParmsPanel.SuspendLayout();
            this.LyricFormStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // LyricFormTrackBar
            // 
            this.LyricFormTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LyricFormTrackBar.Location = new System.Drawing.Point(3, 491);
            this.LyricFormTrackBar.Maximum = 0;
            this.LyricFormTrackBar.Name = "LyricFormTrackBar";
            this.LyricFormTrackBar.Size = new System.Drawing.Size(283, 45);
            this.LyricFormTrackBar.TabIndex = 6;
            this.LyricFormToolTip.SetToolTip(this.LyricFormTrackBar, "Switch between all the lyrics search results (Arrows)");
            this.LyricFormTrackBar.Visible = false;
            this.LyricFormTrackBar.Scroll += new System.EventHandler(this.LyricFormTrackBar_Scroll);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(211, 501);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 7;
            this.SearchButton.Text = "&Search...";
            this.LyricFormToolTip.SetToolTip(this.SearchButton, "Search for more lyrics");
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Visible = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(300, 501);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(74, 23);
            this.CloseButton.TabIndex = 8;
            this.CloseButton.Text = "&Close (Esc)";
            this.LyricFormToolTip.SetToolTip(this.CloseButton, "Close the window (Esc)");
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
            this.LyricFormToolTip.SetToolTip(this.ArtistTextBox, "Artist name, change to refine search");
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
            this.LyricFormToolTip.SetToolTip(this.AlbumTextBox, "Album name, change to refine search");
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
            this.LyricFormToolTip.SetToolTip(this.TrackTextBox, "Track title, change to refine search");
            // 
            // LyricFormPanel
            // 
            this.LyricFormPanel.Controls.Add(this.LyricParmsPanel);
            this.LyricFormPanel.Controls.Add(this.LyricFormStatusStrip);
            this.LyricFormPanel.Controls.Add(this.LyricFormTrackBar);
            this.LyricFormPanel.Controls.Add(this.SearchButton);
            this.LyricFormPanel.Controls.Add(this.LyricTextBox);
            this.LyricFormPanel.Controls.Add(this.CloseButton);
            this.LyricFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricFormPanel.Location = new System.Drawing.Point(0, 0);
            this.LyricFormPanel.Name = "LyricFormPanel";
            this.LyricFormPanel.Size = new System.Drawing.Size(384, 561);
            this.LyricFormPanel.TabIndex = 0;
            // 
            // LyricParmsPanel
            // 
            this.LyricParmsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricParmsPanel.ColumnCount = 3;
            this.LyricParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.LyricParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.LyricParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.LyricParmsPanel.Controls.Add(this.ArtistLabel, 0, 0);
            this.LyricParmsPanel.Controls.Add(this.AlbumLabel, 1, 0);
            this.LyricParmsPanel.Controls.Add(this.TrackLabel, 2, 0);
            this.LyricParmsPanel.Controls.Add(this.ArtistTextBox, 0, 1);
            this.LyricParmsPanel.Controls.Add(this.AlbumTextBox, 1, 1);
            this.LyricParmsPanel.Controls.Add(this.TrackTextBox, 2, 1);
            this.LyricParmsPanel.Location = new System.Drawing.Point(0, 4);
            this.LyricParmsPanel.Name = "LyricParmsPanel";
            this.LyricParmsPanel.RowCount = 2;
            this.LyricParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.LyricParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LyricParmsPanel.Size = new System.Drawing.Size(384, 64);
            this.LyricParmsPanel.TabIndex = 10;
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
            // LyricFormStatusStrip
            // 
            this.LyricFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LyricFormStatusLabel,
            this.LyricFormFoundStatusLabel});
            this.LyricFormStatusStrip.Location = new System.Drawing.Point(0, 539);
            this.LyricFormStatusStrip.Name = "LyricFormStatusStrip";
            this.LyricFormStatusStrip.Size = new System.Drawing.Size(384, 22);
            this.LyricFormStatusStrip.TabIndex = 9;
            this.LyricFormStatusStrip.Text = "statusStrip1";
            // 
            // LyricFormStatusLabel
            // 
            this.LyricFormStatusLabel.Name = "LyricFormStatusLabel";
            this.LyricFormStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // LyricFormFoundStatusLabel
            // 
            this.LyricFormFoundStatusLabel.Name = "LyricFormFoundStatusLabel";
            this.LyricFormFoundStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // LyricTextBox
            // 
            this.LyricTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricTextBox.Location = new System.Drawing.Point(3, 74);
            this.LyricTextBox.Multiline = true;
            this.LyricTextBox.Name = "LyricTextBox";
            this.LyricTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LyricTextBox.Size = new System.Drawing.Size(378, 411);
            this.LyricTextBox.TabIndex = 5;
            // 
            // LyricForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.LyricFormPanel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(800, 1000);
            this.MinimumSize = new System.Drawing.Size(250, 200);
            this.Name = "LyricForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LyricsForm";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LyricForm_FormClosing);
            this.Load += new System.EventHandler(this.LyricForm_LoadAsync);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.LyricFormTrackBar)).EndInit();
            this.LyricFormPanel.ResumeLayout(false);
            this.LyricFormPanel.PerformLayout();
            this.LyricParmsPanel.ResumeLayout(false);
            this.LyricParmsPanel.PerformLayout();
            this.LyricFormStatusStrip.ResumeLayout(false);
            this.LyricFormStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip LyricFormToolTip;
        private System.Windows.Forms.Panel LyricFormPanel;
        private System.Windows.Forms.StatusStrip LyricFormStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel LyricFormStatusLabel;
        private System.Windows.Forms.TrackBar LyricFormTrackBar;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox LyricTextBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TableLayoutPanel LyricParmsPanel;
        private System.Windows.Forms.Label ArtistLabel;
        private System.Windows.Forms.Label AlbumLabel;
        private System.Windows.Forms.Label TrackLabel;
        private System.Windows.Forms.TextBox ArtistTextBox;
        private System.Windows.Forms.TextBox AlbumTextBox;
        private System.Windows.Forms.TextBox TrackTextBox;
        private System.Windows.Forms.ToolStripStatusLabel LyricFormFoundStatusLabel;
    }
}