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

                if (_cancellationTokenSource != null)
                    _cancellationTokenSource.Dispose();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LyricForm));
            this.LyricFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ArtistTextBox = new System.Windows.Forms.TextBox();
            this.AlbumTextBox = new System.Windows.Forms.TextBox();
            this.TrackTextBox = new System.Windows.Forms.TextBox();
            this.LyricFormTrackBar = new System.Windows.Forms.TrackBar();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.LyricFormToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.LyricFormFoundStatusStrip = new System.Windows.Forms.StatusStrip();
            this.LyricFormFoundStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LyricFormStatusStrip = new System.Windows.Forms.StatusStrip();
            this.LyricFormStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LyricFormPanel = new System.Windows.Forms.Panel();
            this.LyricParmsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ArtistLabel = new System.Windows.Forms.Label();
            this.AlbumLabel = new System.Windows.Forms.Label();
            this.TrackLabel = new System.Windows.Forms.Label();
            this.LyricElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.LyricFormMenuStrip = new System.Windows.Forms.MenuStrip();
            this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditUndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditRedoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.EditFindMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditReplaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditFindReplaceNextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.EditSelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditCutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditPasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditDeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.EditProperCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditTitleCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditLowerCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditUpperCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSentenceCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.EditTrimMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.EditToggleSpellCheckMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSpellCheckLanguageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolsPlayJumpAheadLargeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsPlayJumpBackLargeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LyricTextBox = new MediaCenter.LyricsFinder.SpellBox();
            this.ToolsPlayStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.LyricFormTrackBar)).BeginInit();
            this.LyricFormToolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.LyricFormToolStripContainer.ContentPanel.SuspendLayout();
            this.LyricFormToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.LyricFormToolStripContainer.SuspendLayout();
            this.LyricFormFoundStatusStrip.SuspendLayout();
            this.LyricFormStatusStrip.SuspendLayout();
            this.LyricFormPanel.SuspendLayout();
            this.LyricParmsPanel.SuspendLayout();
            this.LyricFormMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ArtistTextBox
            // 
            this.ArtistTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArtistTextBox.Location = new System.Drawing.Point(3, 48);
            this.ArtistTextBox.Multiline = true;
            this.ArtistTextBox.Name = "ArtistTextBox";
            this.ArtistTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ArtistTextBox.Size = new System.Drawing.Size(121, 52);
            this.ArtistTextBox.TabIndex = 1;
            this.LyricFormToolTip.SetToolTip(this.ArtistTextBox, "Artist name, change to refine search");
            this.ArtistTextBox.Enter += new System.EventHandler(this.NonSpellChecBox_EnterAsync);
            this.ArtistTextBox.Leave += new System.EventHandler(this.NonSpellChecBox_LeaveAsync);
            // 
            // AlbumTextBox
            // 
            this.AlbumTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlbumTextBox.Location = new System.Drawing.Point(130, 48);
            this.AlbumTextBox.Multiline = true;
            this.AlbumTextBox.Name = "AlbumTextBox";
            this.AlbumTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AlbumTextBox.Size = new System.Drawing.Size(122, 52);
            this.AlbumTextBox.TabIndex = 3;
            this.LyricFormToolTip.SetToolTip(this.AlbumTextBox, "Album name, change to refine search");
            this.AlbumTextBox.Enter += new System.EventHandler(this.NonSpellChecBox_EnterAsync);
            this.AlbumTextBox.Leave += new System.EventHandler(this.NonSpellChecBox_LeaveAsync);
            // 
            // TrackTextBox
            // 
            this.TrackTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackTextBox.Location = new System.Drawing.Point(258, 48);
            this.TrackTextBox.Multiline = true;
            this.TrackTextBox.Name = "TrackTextBox";
            this.TrackTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TrackTextBox.Size = new System.Drawing.Size(123, 52);
            this.TrackTextBox.TabIndex = 5;
            this.LyricFormToolTip.SetToolTip(this.TrackTextBox, "Track title, change to refine search");
            this.TrackTextBox.Enter += new System.EventHandler(this.NonSpellChecBox_EnterAsync);
            this.TrackTextBox.Leave += new System.EventHandler(this.NonSpellChecBox_LeaveAsync);
            // 
            // LyricFormTrackBar
            // 
            this.LyricFormTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricFormTrackBar.Location = new System.Drawing.Point(3, 444);
            this.LyricFormTrackBar.Maximum = 0;
            this.LyricFormTrackBar.Name = "LyricFormTrackBar";
            this.LyricFormTrackBar.Size = new System.Drawing.Size(289, 45);
            this.LyricFormTrackBar.TabIndex = 2;
            this.LyricFormToolTip.SetToolTip(this.LyricFormTrackBar, "Switch between all the lyrics search results (Arrows)");
            this.LyricFormTrackBar.Visible = false;
            this.LyricFormTrackBar.Scroll += new System.EventHandler(this.LyricFormTrackBar_ScrollAsync);
            this.LyricFormTrackBar.Enter += new System.EventHandler(this.NonSpellChecBox_EnterAsync);
            this.LyricFormTrackBar.Leave += new System.EventHandler(this.NonSpellChecBox_LeaveAsync);
            this.LyricFormTrackBar.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LyricFormTrackBar_PreviewKeyDownAsync);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(200, 458);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 3;
            this.SearchButton.Text = "&Search...";
            this.LyricFormToolTip.SetToolTip(this.SearchButton, "Search for more lyrics");
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Visible = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_ClickAsync);
            this.SearchButton.Leave += new System.EventHandler(this.NonSpellChecBox_LeaveAsync);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(298, 458);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(74, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "&Close (Esc)";
            this.LyricFormToolTip.SetToolTip(this.CloseButton, "Close the window (Esc)");
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_ClickAsync);
            // 
            // LyricFormToolStripContainer
            // 
            // 
            // LyricFormToolStripContainer.BottomToolStripPanel
            // 
            this.LyricFormToolStripContainer.BottomToolStripPanel.Controls.Add(this.LyricFormFoundStatusStrip);
            this.LyricFormToolStripContainer.BottomToolStripPanel.Controls.Add(this.LyricFormStatusStrip);
            this.LyricFormToolStripContainer.BottomToolStripPanel.Enabled = false;
            // 
            // LyricFormToolStripContainer.ContentPanel
            // 
            this.LyricFormToolStripContainer.ContentPanel.Controls.Add(this.LyricFormPanel);
            this.LyricFormToolStripContainer.ContentPanel.Size = new System.Drawing.Size(384, 493);
            this.LyricFormToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // LyricFormToolStripContainer.LeftToolStripPanel
            // 
            this.LyricFormToolStripContainer.LeftToolStripPanel.Enabled = false;
            this.LyricFormToolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.LyricFormToolStripContainer.Name = "LyricFormToolStripContainer";
            // 
            // LyricFormToolStripContainer.RightToolStripPanel
            // 
            this.LyricFormToolStripContainer.RightToolStripPanel.Enabled = false;
            this.LyricFormToolStripContainer.Size = new System.Drawing.Size(384, 561);
            this.LyricFormToolStripContainer.TabIndex = 0;
            this.LyricFormToolStripContainer.TabStop = false;
            // 
            // LyricFormToolStripContainer.TopToolStripPanel
            // 
            this.LyricFormToolStripContainer.TopToolStripPanel.Controls.Add(this.LyricFormMenuStrip);
            // 
            // LyricFormFoundStatusStrip
            // 
            this.LyricFormFoundStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.LyricFormFoundStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LyricFormFoundStatusLabel});
            this.LyricFormFoundStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.LyricFormFoundStatusStrip.Name = "LyricFormFoundStatusStrip";
            this.LyricFormFoundStatusStrip.Size = new System.Drawing.Size(384, 22);
            this.LyricFormFoundStatusStrip.SizingGrip = false;
            this.LyricFormFoundStatusStrip.TabIndex = 6;
            // 
            // LyricFormFoundStatusLabel
            // 
            this.LyricFormFoundStatusLabel.Name = "LyricFormFoundStatusLabel";
            this.LyricFormFoundStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.LyricFormFoundStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LyricFormStatusStrip
            // 
            this.LyricFormStatusStrip.AutoSize = false;
            this.LyricFormStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.LyricFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LyricFormStatusLabel});
            this.LyricFormStatusStrip.Location = new System.Drawing.Point(0, 22);
            this.LyricFormStatusStrip.Name = "LyricFormStatusStrip";
            this.LyricFormStatusStrip.Size = new System.Drawing.Size(384, 22);
            this.LyricFormStatusStrip.TabIndex = 5;
            // 
            // LyricFormStatusLabel
            // 
            this.LyricFormStatusLabel.Name = "LyricFormStatusLabel";
            this.LyricFormStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.LyricFormStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LyricFormPanel
            // 
            this.LyricFormPanel.Controls.Add(this.LyricParmsPanel);
            this.LyricFormPanel.Controls.Add(this.LyricTextBox);
            this.LyricFormPanel.Controls.Add(this.LyricElementHost);
            this.LyricFormPanel.Controls.Add(this.LyricFormTrackBar);
            this.LyricFormPanel.Controls.Add(this.SearchButton);
            this.LyricFormPanel.Controls.Add(this.CloseButton);
            this.LyricFormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricFormPanel.Location = new System.Drawing.Point(0, 0);
            this.LyricFormPanel.Name = "LyricFormPanel";
            this.LyricFormPanel.Size = new System.Drawing.Size(384, 493);
            this.LyricFormPanel.TabIndex = 1;
            this.LyricFormPanel.MouseEnter += new System.EventHandler(this.LyricForm_MouseEnterAsync);
            // 
            // LyricParmsPanel
            // 
            this.LyricParmsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricParmsPanel.ColumnCount = 3;
            this.LyricParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.LyricParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.LyricParmsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.LyricParmsPanel.Controls.Add(this.ArtistLabel, 0, 1);
            this.LyricParmsPanel.Controls.Add(this.AlbumLabel, 1, 1);
            this.LyricParmsPanel.Controls.Add(this.TrackLabel, 2, 1);
            this.LyricParmsPanel.Controls.Add(this.ArtistTextBox, 0, 2);
            this.LyricParmsPanel.Controls.Add(this.AlbumTextBox, 1, 2);
            this.LyricParmsPanel.Controls.Add(this.TrackTextBox, 2, 2);
            this.LyricParmsPanel.Location = new System.Drawing.Point(0, 3);
            this.LyricParmsPanel.Name = "LyricParmsPanel";
            this.LyricParmsPanel.RowCount = 3;
            this.LyricParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.LyricParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.LyricParmsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LyricParmsPanel.Size = new System.Drawing.Size(384, 103);
            this.LyricParmsPanel.TabIndex = 0;
            // 
            // ArtistLabel
            // 
            this.ArtistLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ArtistLabel.AutoSize = true;
            this.ArtistLabel.Location = new System.Drawing.Point(3, 32);
            this.ArtistLabel.Name = "ArtistLabel";
            this.ArtistLabel.Size = new System.Drawing.Size(30, 13);
            this.ArtistLabel.TabIndex = 0;
            this.ArtistLabel.Text = "Artist";
            // 
            // AlbumLabel
            // 
            this.AlbumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AlbumLabel.AutoSize = true;
            this.AlbumLabel.Location = new System.Drawing.Point(130, 32);
            this.AlbumLabel.Name = "AlbumLabel";
            this.AlbumLabel.Size = new System.Drawing.Size(36, 13);
            this.AlbumLabel.TabIndex = 2;
            this.AlbumLabel.Text = "Album";
            // 
            // TrackLabel
            // 
            this.TrackLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TrackLabel.AutoSize = true;
            this.TrackLabel.Location = new System.Drawing.Point(258, 32);
            this.TrackLabel.Name = "TrackLabel";
            this.TrackLabel.Size = new System.Drawing.Size(58, 13);
            this.TrackLabel.TabIndex = 4;
            this.TrackLabel.Text = "Track Title";
            // 
            // LyricElementHost
            // 
            this.LyricElementHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricElementHost.Location = new System.Drawing.Point(4, 112);
            this.LyricElementHost.Name = "LyricElementHost";
            this.LyricElementHost.Size = new System.Drawing.Size(377, 326);
            this.LyricElementHost.TabIndex = 1;
            this.LyricElementHost.TabStop = false;
            this.LyricElementHost.Child = null;
            // 
            // LyricFormMenuStrip
            // 
            this.LyricFormMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.LyricFormMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditMenuItem,
            this.ToolsMenuItem,
            this.HelpMenuItem});
            this.LyricFormMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.LyricFormMenuStrip.Name = "LyricFormMenuStrip";
            this.LyricFormMenuStrip.Size = new System.Drawing.Size(384, 24);
            this.LyricFormMenuStrip.TabIndex = 0;
            this.LyricFormMenuStrip.TabStop = true;
            // 
            // EditMenuItem
            // 
            this.EditMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditUndoMenuItem,
            this.EditRedoMenuItem,
            this.EditSeparator0,
            this.EditFindMenuItem,
            this.EditReplaceMenuItem,
            this.EditFindReplaceNextMenuItem,
            this.EditSeparator1,
            this.EditSelectAllMenuItem,
            this.EditCutMenuItem,
            this.EditCopyMenuItem,
            this.EditPasteMenuItem,
            this.EditDeleteMenuItem,
            this.EditSeparator2,
            this.EditProperCaseMenuItem,
            this.EditTitleCaseMenuItem,
            this.EditLowerCaseMenuItem,
            this.EditUpperCaseMenuItem,
            this.EditSentenceCaseMenuItem,
            this.EditSeparator3,
            this.EditTrimMenuItem,
            this.EditSeparator4,
            this.EditToggleSpellCheckMenuItem,
            this.EditSpellCheckLanguageMenuItem});
            this.EditMenuItem.Name = "EditMenuItem";
            this.EditMenuItem.Size = new System.Drawing.Size(39, 20);
            this.EditMenuItem.Text = "&Edit";
            this.EditMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditUndoMenuItem
            // 
            this.EditUndoMenuItem.Name = "EditUndoMenuItem";
            this.EditUndoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.EditUndoMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditUndoMenuItem.Text = "Undo";
            this.EditUndoMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditRedoMenuItem
            // 
            this.EditRedoMenuItem.Name = "EditRedoMenuItem";
            this.EditRedoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.EditRedoMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditRedoMenuItem.Text = "Redo";
            this.EditRedoMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSeparator0
            // 
            this.EditSeparator0.Name = "EditSeparator0";
            this.EditSeparator0.Size = new System.Drawing.Size(232, 6);
            // 
            // EditFindMenuItem
            // 
            this.EditFindMenuItem.Name = "EditFindMenuItem";
            this.EditFindMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.EditFindMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditFindMenuItem.Text = "Find...";
            this.EditFindMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditReplaceMenuItem
            // 
            this.EditReplaceMenuItem.Name = "EditReplaceMenuItem";
            this.EditReplaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.EditReplaceMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditReplaceMenuItem.Text = "Replace...";
            this.EditReplaceMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditFindReplaceNextMenuItem
            // 
            this.EditFindReplaceNextMenuItem.Name = "EditFindReplaceNextMenuItem";
            this.EditFindReplaceNextMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.EditFindReplaceNextMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditFindReplaceNextMenuItem.Text = "Find / Replace next...";
            this.EditFindReplaceNextMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSeparator1
            // 
            this.EditSeparator1.Name = "EditSeparator1";
            this.EditSeparator1.Size = new System.Drawing.Size(232, 6);
            // 
            // EditSelectAllMenuItem
            // 
            this.EditSelectAllMenuItem.Name = "EditSelectAllMenuItem";
            this.EditSelectAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.EditSelectAllMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditSelectAllMenuItem.Text = "Select all";
            this.EditSelectAllMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditCutMenuItem
            // 
            this.EditCutMenuItem.Name = "EditCutMenuItem";
            this.EditCutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.EditCutMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditCutMenuItem.Text = "Cut";
            this.EditCutMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditCopyMenuItem
            // 
            this.EditCopyMenuItem.Name = "EditCopyMenuItem";
            this.EditCopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.EditCopyMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditCopyMenuItem.Text = "Copy";
            this.EditCopyMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditPasteMenuItem
            // 
            this.EditPasteMenuItem.Name = "EditPasteMenuItem";
            this.EditPasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.EditPasteMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditPasteMenuItem.Text = "Paste";
            this.EditPasteMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditDeleteMenuItem
            // 
            this.EditDeleteMenuItem.Name = "EditDeleteMenuItem";
            this.EditDeleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.EditDeleteMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditDeleteMenuItem.Text = "Delete";
            this.EditDeleteMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSeparator2
            // 
            this.EditSeparator2.Name = "EditSeparator2";
            this.EditSeparator2.Size = new System.Drawing.Size(232, 6);
            // 
            // EditProperCaseMenuItem
            // 
            this.EditProperCaseMenuItem.Name = "EditProperCaseMenuItem";
            this.EditProperCaseMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditProperCaseMenuItem.Text = "&Proper case selection";
            this.EditProperCaseMenuItem.ToolTipText = "In every word the first letter is capitalized";
            this.EditProperCaseMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditTitleCaseMenuItem
            // 
            this.EditTitleCaseMenuItem.Name = "EditTitleCaseMenuItem";
            this.EditTitleCaseMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditTitleCaseMenuItem.Text = "&Title case selection";
            this.EditTitleCaseMenuItem.ToolTipText = "In every word the first letter is capitalized,\r\nexcept where word length is less " +
    "than 4";
            this.EditTitleCaseMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditLowerCaseMenuItem
            // 
            this.EditLowerCaseMenuItem.Name = "EditLowerCaseMenuItem";
            this.EditLowerCaseMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditLowerCaseMenuItem.Text = "&Lower case selection";
            this.EditLowerCaseMenuItem.ToolTipText = "All text is in small letters";
            this.EditLowerCaseMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditUpperCaseMenuItem
            // 
            this.EditUpperCaseMenuItem.Name = "EditUpperCaseMenuItem";
            this.EditUpperCaseMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditUpperCaseMenuItem.Text = "&Upper case selection";
            this.EditUpperCaseMenuItem.ToolTipText = "All text is in capital letters";
            this.EditUpperCaseMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSentenceCaseMenuItem
            // 
            this.EditSentenceCaseMenuItem.Name = "EditSentenceCaseMenuItem";
            this.EditSentenceCaseMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditSentenceCaseMenuItem.Text = "&Sentence case selection";
            this.EditSentenceCaseMenuItem.ToolTipText = "In the first word after a newline or period, \r\nthe first letter is capitalized";
            this.EditSentenceCaseMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSeparator3
            // 
            this.EditSeparator3.Name = "EditSeparator3";
            this.EditSeparator3.Size = new System.Drawing.Size(232, 6);
            // 
            // EditTrimMenuItem
            // 
            this.EditTrimMenuItem.Name = "EditTrimMenuItem";
            this.EditTrimMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditTrimMenuItem.Text = "T&rim leading and trailing space";
            this.EditTrimMenuItem.ToolTipText = "Trim leading and trailing space in each line of the selected (or all) text";
            this.EditTrimMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSeparator4
            // 
            this.EditSeparator4.Name = "EditSeparator4";
            this.EditSeparator4.Size = new System.Drawing.Size(232, 6);
            // 
            // EditToggleSpellCheckMenuItem
            // 
            this.EditToggleSpellCheckMenuItem.Name = "EditToggleSpellCheckMenuItem";
            this.EditToggleSpellCheckMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.EditToggleSpellCheckMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditToggleSpellCheckMenuItem.Text = "Toggle spell check";
            this.EditToggleSpellCheckMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // EditSpellCheckLanguageMenuItem
            // 
            this.EditSpellCheckLanguageMenuItem.Name = "EditSpellCheckLanguageMenuItem";
            this.EditSpellCheckLanguageMenuItem.Size = new System.Drawing.Size(235, 22);
            this.EditSpellCheckLanguageMenuItem.Text = "Spell check language";
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsSearchMenuItem,
            this.ToolsSeparator0,
            this.ToolsPlayStartStopButton,
            this.ToolsPlayJumpAheadLargeMenuItem,
            this.ToolsPlayJumpBackLargeMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsMenuItem.Text = "&Tools";
            this.ToolsMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolsSearchMenuItem
            // 
            this.ToolsSearchMenuItem.Name = "ToolsSearchMenuItem";
            this.ToolsSearchMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.ToolsSearchMenuItem.Size = new System.Drawing.Size(240, 22);
            this.ToolsSearchMenuItem.Text = "&Search";
            this.ToolsSearchMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolsSeparator0
            // 
            this.ToolsSeparator0.Name = "ToolsSeparator0";
            this.ToolsSeparator0.Size = new System.Drawing.Size(237, 6);
            // 
            // ToolsPlayJumpAheadLargeMenuItem
            // 
            this.ToolsPlayJumpAheadLargeMenuItem.Name = "ToolsPlayJumpAheadLargeMenuItem";
            this.ToolsPlayJumpAheadLargeMenuItem.ShortcutKeyDisplayString = "Ctrl+Right";
            this.ToolsPlayJumpAheadLargeMenuItem.Size = new System.Drawing.Size(240, 22);
            this.ToolsPlayJumpAheadLargeMenuItem.Text = "Forward 10 seconds";
            this.ToolsPlayJumpAheadLargeMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolsPlayJumpBackLargeMenuItem
            // 
            this.ToolsPlayJumpBackLargeMenuItem.Name = "ToolsPlayJumpBackLargeMenuItem";
            this.ToolsPlayJumpBackLargeMenuItem.ShortcutKeyDisplayString = "Ctrl+Left";
            this.ToolsPlayJumpBackLargeMenuItem.Size = new System.Drawing.Size(240, 22);
            this.ToolsPlayJumpBackLargeMenuItem.Text = "Rewind 10 seconds";
            this.ToolsPlayJumpBackLargeMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpHelpMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "&Help";
            this.HelpMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // HelpHelpMenuItem
            // 
            this.HelpHelpMenuItem.Name = "HelpHelpMenuItem";
            this.HelpHelpMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.HelpHelpMenuItem.Size = new System.Drawing.Size(118, 22);
            this.HelpHelpMenuItem.Text = "&Help";
            this.HelpHelpMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // LyricTextBox
            // 
            this.LyricTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LyricTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.LyricTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.LyricTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LyricTextBox.Location = new System.Drawing.Point(4, 112);
            this.LyricTextBox.Multiline = true;
            this.LyricTextBox.Name = "LyricTextBox";
            this.LyricTextBox.ParentForm = this;
            this.LyricTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LyricTextBox.SelectedText = "";
            this.LyricTextBox.SelectionStart = 0;
            this.LyricTextBox.Size = new System.Drawing.Size(377, 326);
            this.LyricTextBox.SpellCheckEnabled = false;
            this.LyricTextBox.TabIndex = 1;
            this.LyricTextBox.KeyDown += new System.EventHandler<System.Windows.Forms.KeyEventArgs>(this.LyricTextBox_KeyDownAsync);
            this.LyricTextBox.Child = new System.Windows.Controls.TextBox();
            // 
            // ToolsPlayStartStopButton
            // 
            this.ToolsPlayStartStopButton.AutoSize = false;
            this.ToolsPlayStartStopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ToolsPlayStartStopButton.Clicked = false;
            this.ToolsPlayStartStopButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ToolsPlayStartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("ToolsPlayStartStopButton.ImageStart")));
            this.ToolsPlayStartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("ToolsPlayStartStopButton.ImageStop")));
            this.ToolsPlayStartStopButton.ImageTransparentColor = System.Drawing.SystemColors.Control;
            this.ToolsPlayStartStopButton.IsRunning = false;
            this.ToolsPlayStartStopButton.Name = "ToolsPlayStartStopButton";
            this.ToolsPlayStartStopButton.Size = new System.Drawing.Size(76, 20);
            this.ToolsPlayStartStopButton.Text = "Start play";
            this.ToolsPlayStartStopButton.TextStart = "Start play";
            this.ToolsPlayStartStopButton.TextStop = "Stop play";
            this.ToolsPlayStartStopButton.ToolTipText = "Start / stop play of the current item (Ctrl + P)";
            // 
            // LyricForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.LyricFormToolStripContainer);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.KeyPreview = true;
            this.MainMenuStrip = this.LyricFormMenuStrip;
            this.MaximumSize = new System.Drawing.Size(800, 1000);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "LyricForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LyricsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LyricForm_FormClosingAsync);
            this.Load += new System.EventHandler(this.LyricForm_LoadAsync);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricForm_KeyDownAsync);
            this.MouseEnter += new System.EventHandler(this.LyricForm_MouseEnterAsync);
            ((System.ComponentModel.ISupportInitialize)(this.LyricFormTrackBar)).EndInit();
            this.LyricFormToolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.LyricFormToolStripContainer.BottomToolStripPanel.PerformLayout();
            this.LyricFormToolStripContainer.ContentPanel.ResumeLayout(false);
            this.LyricFormToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.LyricFormToolStripContainer.TopToolStripPanel.PerformLayout();
            this.LyricFormToolStripContainer.ResumeLayout(false);
            this.LyricFormToolStripContainer.PerformLayout();
            this.LyricFormFoundStatusStrip.ResumeLayout(false);
            this.LyricFormFoundStatusStrip.PerformLayout();
            this.LyricFormStatusStrip.ResumeLayout(false);
            this.LyricFormStatusStrip.PerformLayout();
            this.LyricFormPanel.ResumeLayout(false);
            this.LyricFormPanel.PerformLayout();
            this.LyricParmsPanel.ResumeLayout(false);
            this.LyricParmsPanel.PerformLayout();
            this.LyricFormMenuStrip.ResumeLayout(false);
            this.LyricFormMenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip LyricFormToolTip;
        private System.Windows.Forms.ToolStripContainer LyricFormToolStripContainer;
        private System.Windows.Forms.Panel LyricFormPanel;
        private System.Windows.Forms.TableLayoutPanel LyricParmsPanel;
        private System.Windows.Forms.Label ArtistLabel;
        private System.Windows.Forms.Label AlbumLabel;
        private System.Windows.Forms.Label TrackLabel;
        private System.Windows.Forms.TextBox ArtistTextBox;
        private System.Windows.Forms.TextBox AlbumTextBox;
        private System.Windows.Forms.TextBox TrackTextBox;
        private System.Windows.Forms.TrackBar LyricFormTrackBar;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.StatusStrip LyricFormStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel LyricFormStatusLabel;
        private System.Windows.Forms.MenuStrip LyricFormMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem EditMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpHelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsSearchMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditUndoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditSelectAllMenuItem;
        private System.Windows.Forms.ToolStripSeparator EditSeparator2;
        private System.Windows.Forms.ToolStripMenuItem EditProperCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditTitleCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditLowerCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditUpperCaseMenuItem;
        private System.Windows.Forms.ToolStripSeparator EditSeparator3;
        private System.Windows.Forms.ToolStripMenuItem EditToggleSpellCheckMenuItem;
        private System.Windows.Forms.ToolStripSeparator EditSeparator1;
        private System.Windows.Forms.ToolStripMenuItem EditCutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditCopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditPasteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditSentenceCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditDeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditTrimMenuItem;
        private System.Windows.Forms.ToolStripSeparator EditSeparator4;
        private System.Windows.Forms.Integration.ElementHost LyricElementHost;
        private SpellBox LyricTextBox;
        private System.Windows.Forms.ToolStripMenuItem EditSpellCheckLanguageMenuItem;
        private System.Windows.Forms.ToolStripSeparator ToolsSeparator0;
        private StartStopToolStripButton ToolsPlayStartStopButton;
        private System.Windows.Forms.ToolStripMenuItem EditRedoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsPlayJumpAheadLargeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsPlayJumpBackLargeMenuItem;
        private System.Windows.Forms.ToolStripSeparator EditSeparator0;
        private System.Windows.Forms.ToolStripMenuItem EditFindMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditReplaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditFindReplaceNextMenuItem;
        private System.Windows.Forms.StatusStrip LyricFormFoundStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel LyricFormFoundStatusLabel;
    }
}