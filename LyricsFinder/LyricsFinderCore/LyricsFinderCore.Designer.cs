
using System;


namespace MediaCenter.LyricsFinder
{

    partial class LyricsFinderCore
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
                if (components != null) components.Dispose();
                if (_lyricsForm != null) _lyricsForm.Dispose();
                if (_bitmapForm != null) _bitmapForm.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LyricsFinderCore));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainContainer = new System.Windows.Forms.ToolStripContainer();
            this.BottomMenu = new System.Windows.Forms.MenuStrip();
            this.StartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            this.TestTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.OverwriteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatus = new System.Windows.Forms.StatusStrip();
            this.MainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.MainDataGridView = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cover = new System.Windows.Forms.DataGridViewImageColumn();
            this.Artist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Album = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lyrics = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileReloadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSepMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            this.ToolLyricsServicesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolShowLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolSepMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DataChangedTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TopSubMenu = new System.Windows.Forms.MenuStrip();
            this.TopSubMenuTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ProcessWorker = new System.ComponentModel.BackgroundWorker();
            this.InitTimer = new System.Windows.Forms.Timer(this.components);
            this.HelpSepMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpContentsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainContainer.BottomToolStripPanel.SuspendLayout();
            this.MainContainer.ContentPanel.SuspendLayout();
            this.MainContainer.TopToolStripPanel.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.BottomMenu.SuspendLayout();
            this.MainStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).BeginInit();
            this.TopMenu.SuspendLayout();
            this.TopSubMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            // 
            // MainContainer.BottomToolStripPanel
            // 
            this.MainContainer.BottomToolStripPanel.Controls.Add(this.BottomMenu);
            this.MainContainer.BottomToolStripPanel.Controls.Add(this.MainStatus);
            // 
            // MainContainer.ContentPanel
            // 
            this.MainContainer.ContentPanel.Controls.Add(this.MainDataGridView);
            this.MainContainer.ContentPanel.Size = new System.Drawing.Size(738, 412);
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.LeftToolStripPanelVisible = false;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.RightToolStripPanelVisible = false;
            this.MainContainer.Size = new System.Drawing.Size(738, 509);
            this.MainContainer.TabIndex = 0;
            this.MainContainer.Text = "toolStripContainer1";
            // 
            // MainContainer.TopToolStripPanel
            // 
            this.MainContainer.TopToolStripPanel.Controls.Add(this.TopMenu);
            this.MainContainer.TopToolStripPanel.Controls.Add(this.TopSubMenu);
            // 
            // BottomMenu
            // 
            this.BottomMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.BottomMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartStopButton,
            this.TestTextBox,
            this.OverwriteMenuItem});
            this.BottomMenu.Location = new System.Drawing.Point(0, 0);
            this.BottomMenu.Name = "BottomMenu";
            this.BottomMenu.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.BottomMenu.ShowItemToolTips = true;
            this.BottomMenu.Size = new System.Drawing.Size(738, 27);
            this.BottomMenu.TabIndex = 1;
            this.BottomMenu.Text = "menuStrip1";
            // 
            // StartStopButton
            // 
            this.StartStopButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.StartStopButton.CheckOnClick = true;
            this.StartStopButton.Clicked = false;
            this.StartStopButton.Image = ((System.Drawing.Image)(resources.GetObject("StartStopButton.Image")));
            this.StartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("StartStopButton.ImageStart")));
            this.StartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("StartStopButton.ImageStop")));
            this.StartStopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartStopButton.IsRunning = false;
            this.StartStopButton.Margin = new System.Windows.Forms.Padding(2, 1, 4, 2);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(103, 20);
            this.StartStopButton.Text = "&Start search all";
            this.StartStopButton.TextStart = "&Start search all";
            this.StartStopButton.TextStop = "&Stop search all";
            this.StartStopButton.ToolTipText = "Start / stop finding all the lyrics for the current playlist";
            // 
            // TestTextBox
            // 
            this.TestTextBox.Name = "TestTextBox";
            this.TestTextBox.Size = new System.Drawing.Size(100, 23);
            this.TestTextBox.Visible = false;
            // 
            // OverwriteMenuItem
            // 
            this.OverwriteMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.OverwriteMenuItem.CheckOnClick = true;
            this.OverwriteMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OverwriteMenuItem.Name = "OverwriteMenuItem";
            this.OverwriteMenuItem.ShortcutKeyDisplayString = "(Alt+E)";
            this.OverwriteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.OverwriteMenuItem.Size = new System.Drawing.Size(143, 23);
            this.OverwriteMenuItem.Text = "Overwrite &existing lyrics";
            this.OverwriteMenuItem.ToolTipText = "Should the operation overwrite existing lyrics?";
            this.OverwriteMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // MainStatus
            // 
            this.MainStatus.Dock = System.Windows.Forms.DockStyle.None;
            this.MainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainStatusLabel,
            this.MainProgressBar});
            this.MainStatus.Location = new System.Drawing.Point(0, 27);
            this.MainStatus.Name = "MainStatus";
            this.MainStatus.ShowItemToolTips = true;
            this.MainStatus.Size = new System.Drawing.Size(738, 22);
            this.MainStatus.SizingGrip = false;
            this.MainStatus.TabIndex = 0;
            // 
            // MainStatusLabel
            // 
            this.MainStatusLabel.AutoSize = false;
            this.MainStatusLabel.AutoToolTip = true;
            this.MainStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainStatusLabel.Name = "MainStatusLabel";
            this.MainStatusLabel.Size = new System.Drawing.Size(621, 17);
            this.MainStatusLabel.Spring = true;
            this.MainStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MainProgressBar.AutoToolTip = true;
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(100, 16);
            this.MainProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // MainDataGridView
            // 
            this.MainDataGridView.AllowUserToAddRows = false;
            this.MainDataGridView.AllowUserToDeleteRows = false;
            this.MainDataGridView.AllowUserToResizeColumns = false;
            this.MainDataGridView.AllowUserToResizeRows = false;
            this.MainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Cover,
            this.Artist,
            this.Album,
            this.Title,
            this.Lyrics,
            this.Status});
            this.MainDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainDataGridView.Location = new System.Drawing.Point(0, 0);
            this.MainDataGridView.MultiSelect = false;
            this.MainDataGridView.Name = "MainDataGridView";
            this.MainDataGridView.ReadOnly = true;
            this.MainDataGridView.RowHeadersVisible = false;
            this.MainDataGridView.RowTemplate.Height = 30;
            this.MainDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MainDataGridView.Size = new System.Drawing.Size(738, 412);
            this.MainDataGridView.TabIndex = 0;
            this.MainDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainDataGridView_CellDoubleClick);
            this.MainDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainDataGridView_CellMouseClick);
            this.MainDataGridView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainDataGridView_CellMouseMove);
            this.MainDataGridView.MouseLeave += new System.EventHandler(this.MainDataGridView_MouseLeave);
            this.MainDataGridView.Resize += new System.EventHandler(this.MainDataGridView_Resize);
            // 
            // Key
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.Key.DefaultCellStyle = dataGridViewCellStyle7;
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Visible = false;
            this.Key.Width = 5;
            // 
            // Cover
            // 
            this.Cover.HeaderText = "Cover";
            this.Cover.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Cover.Name = "Cover";
            this.Cover.ReadOnly = true;
            this.Cover.Width = 40;
            // 
            // Artist
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Artist.DefaultCellStyle = dataGridViewCellStyle8;
            this.Artist.HeaderText = "Artist";
            this.Artist.Name = "Artist";
            this.Artist.ReadOnly = true;
            this.Artist.Width = 150;
            // 
            // Album
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Album.DefaultCellStyle = dataGridViewCellStyle9;
            this.Album.HeaderText = "Album";
            this.Album.Name = "Album";
            this.Album.ReadOnly = true;
            this.Album.Width = 150;
            // 
            // Title
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Title.DefaultCellStyle = dataGridViewCellStyle10;
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 150;
            // 
            // Lyrics
            // 
            this.Lyrics.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Lyrics.DefaultCellStyle = dataGridViewCellStyle11;
            this.Lyrics.HeaderText = "Lyrics";
            this.Lyrics.Name = "Lyrics";
            this.Lyrics.ReadOnly = true;
            // 
            // Status
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.DefaultCellStyle = dataGridViewCellStyle12;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Status.Width = 150;
            // 
            // TopMenu
            // 
            this.TopMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem,
            this.DataChangedTextBox,
            this.helpToolStripMenuItem});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.ShowItemToolTips = true;
            this.TopMenu.Size = new System.Drawing.Size(738, 24);
            this.TopMenu.TabIndex = 0;
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileReloadMenuItem,
            this.FileSaveMenuItem,
            this.FileSepMenuItem1,
            this.FileExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "&File";
            // 
            // FileReloadMenuItem
            // 
            this.FileReloadMenuItem.Name = "FileReloadMenuItem";
            this.FileReloadMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.FileReloadMenuItem.Size = new System.Drawing.Size(255, 22);
            this.FileReloadMenuItem.Text = "&Reload current playlist";
            this.FileReloadMenuItem.ToolTipText = "Reload the Play Now list. \r\nCan also be used to reconnect to the running Media Ce" +
    "nter, \r\ne.g. if connect failed at startup of the standalone application.";
            this.FileReloadMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // FileSaveMenuItem
            // 
            this.FileSaveMenuItem.Name = "FileSaveMenuItem";
            this.FileSaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.FileSaveMenuItem.Size = new System.Drawing.Size(255, 22);
            this.FileSaveMenuItem.Text = "&Save items to MediaCenter";
            this.FileSaveMenuItem.ToolTipText = "Save the found/changed lyrics to the song tags";
            this.FileSaveMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // FileSepMenuItem1
            // 
            this.FileSepMenuItem1.Name = "FileSepMenuItem1";
            this.FileSepMenuItem1.Size = new System.Drawing.Size(252, 6);
            this.FileSepMenuItem1.Visible = false;
            // 
            // FileExitMenuItem
            // 
            this.FileExitMenuItem.Name = "FileExitMenuItem";
            this.FileExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.FileExitMenuItem.Size = new System.Drawing.Size(255, 22);
            this.FileExitMenuItem.Text = "E&xit";
            this.FileExitMenuItem.ToolTipText = "Close the Lyrics Finder standalone application";
            this.FileExitMenuItem.Visible = false;
            this.FileExitMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStartStopButton,
            this.ToolLyricsServicesMenuItem,
            this.ToolShowLogMenuItem,
            this.ToolSepMenuItem1,
            this.ToolTestMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(47, 20);
            this.ToolsMenuItem.Text = "&Tools";
            // 
            // MenuStartStopButton
            // 
            this.MenuStartStopButton.Clicked = false;
            this.MenuStartStopButton.Image = ((System.Drawing.Image)(resources.GetObject("MenuStartStopButton.Image")));
            this.MenuStartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("MenuStartStopButton.ImageStart")));
            this.MenuStartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("MenuStartStopButton.ImageStop")));
            this.MenuStartStopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuStartStopButton.IsRunning = true;
            this.MenuStartStopButton.Name = "MenuStartStopButton";
            this.MenuStartStopButton.Size = new System.Drawing.Size(103, 20);
            this.MenuStartStopButton.Text = "&Stop search all";
            this.MenuStartStopButton.TextStart = "&Start search all";
            this.MenuStartStopButton.TextStop = "&Stop search all";
            this.MenuStartStopButton.ToolTipText = "Start / stop finding all the lyrics for the current playlist";
            // 
            // ToolLyricsServicesMenuItem
            // 
            this.ToolLyricsServicesMenuItem.Name = "ToolLyricsServicesMenuItem";
            this.ToolLyricsServicesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Y)));
            this.ToolLyricsServicesMenuItem.Size = new System.Drawing.Size(198, 22);
            this.ToolLyricsServicesMenuItem.Text = "L&yric services";
            this.ToolLyricsServicesMenuItem.ToolTipText = "Select the services to be used when searching for lyrics. \r\nYou can also change t" +
    "he search service order.";
            this.ToolLyricsServicesMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // ToolShowLogMenuItem
            // 
            this.ToolShowLogMenuItem.Name = "ToolShowLogMenuItem";
            this.ToolShowLogMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.ToolShowLogMenuItem.Size = new System.Drawing.Size(198, 22);
            this.ToolShowLogMenuItem.Text = "Show &logs folder";
            this.ToolShowLogMenuItem.ToolTipText = "Open the folder where the logs are located";
            this.ToolShowLogMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // ToolSepMenuItem1
            // 
            this.ToolSepMenuItem1.Name = "ToolSepMenuItem1";
            this.ToolSepMenuItem1.Size = new System.Drawing.Size(195, 6);
            this.ToolSepMenuItem1.Visible = false;
            // 
            // ToolTestMenuItem
            // 
            this.ToolTestMenuItem.Name = "ToolTestMenuItem";
            this.ToolTestMenuItem.Size = new System.Drawing.Size(198, 22);
            this.ToolTestMenuItem.Text = "Test";
            this.ToolTestMenuItem.Visible = false;
            this.ToolTestMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // DataChangedTextBox
            // 
            this.DataChangedTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.DataChangedTextBox.AutoSize = false;
            this.DataChangedTextBox.AutoToolTip = true;
            this.DataChangedTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.DataChangedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataChangedTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.DataChangedTextBox.Name = "DataChangedTextBox";
            this.DataChangedTextBox.ReadOnly = true;
            this.DataChangedTextBox.Size = new System.Drawing.Size(130, 20);
            this.DataChangedTextBox.Text = "Playlist data changed";
            this.DataChangedTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataChangedTextBox.ToolTipText = "Playlist data is changed and needs to be saved to the MediaCenter (use the File m" +
    "enu)";
            this.DataChangedTextBox.Visible = false;
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpContentsMenuItem,
            this.HelpSepMenuItem1,
            this.HelpAboutMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // HelpAboutMenuItem
            // 
            this.HelpAboutMenuItem.Name = "HelpAboutMenuItem";
            this.HelpAboutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.HelpAboutMenuItem.Size = new System.Drawing.Size(210, 22);
            this.HelpAboutMenuItem.Text = "&About LyricsFinder";
            this.HelpAboutMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // TopSubMenu
            // 
            this.TopSubMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.TopSubMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopSubMenuTextBox});
            this.TopSubMenu.Location = new System.Drawing.Point(0, 24);
            this.TopSubMenu.Name = "TopSubMenu";
            this.TopSubMenu.Size = new System.Drawing.Size(738, 24);
            this.TopSubMenu.TabIndex = 1;
            this.TopSubMenu.Text = "menuStrip1";
            // 
            // TopSubMenuTextBox
            // 
            this.TopSubMenuTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TopSubMenuTextBox.Name = "TopSubMenuTextBox";
            this.TopSubMenuTextBox.ReadOnly = true;
            this.TopSubMenuTextBox.Size = new System.Drawing.Size(200, 20);
            this.TopSubMenuTextBox.Text = "Current playlist items:";
            this.TopSubMenuTextBox.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // MainToolTip
            // 
            this.MainToolTip.AutomaticDelay = 50;
            this.MainToolTip.AutoPopDelay = 10000;
            this.MainToolTip.InitialDelay = 50;
            this.MainToolTip.ReshowDelay = 10;
            // 
            // ProcessWorker
            // 
            this.ProcessWorker.WorkerReportsProgress = true;
            this.ProcessWorker.WorkerSupportsCancellation = true;
            this.ProcessWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ProcessWorker_DoWork);
            this.ProcessWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ProcessWorker_ProgressChanged);
            this.ProcessWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ProcessWorker_RunWorkerCompleted);
            // 
            // InitTimer
            // 
            this.InitTimer.Interval = 10;
            this.InitTimer.Tick += new System.EventHandler(this.InitTimer_Tick);
            // 
            // HelpSepMenuItem1
            // 
            this.HelpSepMenuItem1.Name = "HelpSepMenuItem1";
            this.HelpSepMenuItem1.Size = new System.Drawing.Size(207, 6);
            // 
            // HelpContentsMenuItem
            // 
            this.HelpContentsMenuItem.Name = "HelpContentsMenuItem";
            this.HelpContentsMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.HelpContentsMenuItem.Size = new System.Drawing.Size(210, 22);
            this.HelpContentsMenuItem.Text = "&Help contents";
            this.HelpContentsMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // LyricsFinderCore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainContainer);
            this.Name = "LyricsFinderCore";
            this.Size = new System.Drawing.Size(738, 509);
            this.Load += new System.EventHandler(this.LyricsFinderCore_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricsFinderCore_KeyDown);
            this.MainContainer.BottomToolStripPanel.ResumeLayout(false);
            this.MainContainer.BottomToolStripPanel.PerformLayout();
            this.MainContainer.ContentPanel.ResumeLayout(false);
            this.MainContainer.TopToolStripPanel.ResumeLayout(false);
            this.MainContainer.TopToolStripPanel.PerformLayout();
            this.MainContainer.ResumeLayout(false);
            this.MainContainer.PerformLayout();
            this.BottomMenu.ResumeLayout(false);
            this.BottomMenu.PerformLayout();
            this.MainStatus.ResumeLayout(false);
            this.MainStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).EndInit();
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.TopSubMenu.ResumeLayout(false);
            this.TopSubMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ToolStripContainer MainContainer;
        private System.Windows.Forms.StatusStrip MainStatus;
        private System.Windows.Forms.ToolStripStatusLabel MainStatusLabel;
        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileSepMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem FileExitMenuItem;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolTestMenuItem;
        private System.Windows.Forms.MenuStrip BottomMenu;
        private StartStopToolStripButton StartStopButton;
        private System.ComponentModel.BackgroundWorker ProcessWorker;
        private System.Windows.Forms.ToolStripProgressBar MainProgressBar;
        private System.Windows.Forms.DataGridView MainDataGridView;
        private System.Windows.Forms.ToolStripTextBox TestTextBox;
        private System.Windows.Forms.ToolStripMenuItem FileSaveMenuItem;
        private System.Windows.Forms.ToolStripTextBox DataChangedTextBox;
        private StartStopToolStripButton MenuStartStopButton;
        private System.Windows.Forms.ToolStripSeparator ToolSepMenuItem1;
        private System.Windows.Forms.MenuStrip TopSubMenu;
        private System.Windows.Forms.ToolStripTextBox TopSubMenuTextBox;
        private System.Windows.Forms.ToolStripMenuItem ToolLyricsServicesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpAboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolShowLogMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileReloadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OverwriteMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewImageColumn Cover;
        private System.Windows.Forms.DataGridViewTextBoxColumn Artist;
        private System.Windows.Forms.DataGridViewTextBoxColumn Album;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lyrics;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Timer InitTimer;
        private System.Windows.Forms.ToolStripMenuItem HelpContentsMenuItem;
        private System.Windows.Forms.ToolStripSeparator HelpSepMenuItem1;
    }

}
