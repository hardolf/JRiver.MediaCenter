
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
        /// <param name="disposing"><c>true</c> if managed resources should be disposed; else <c>false</c>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { _currentLyricsFinderPlaylist = null; } catch { /* Ignore */ }
                try { _currentMcPlaylist = null; } catch { /* Ignore */ }
                try { _currentUnsortedMcPlaylistsResponse = null; } catch { /* Ignore */ }
                try { if (_bitmapForm != null) _bitmapForm.Close(); } catch { /* Ignore */ }
                try { if (_lyricsForm != null) _lyricsForm.Close(); } catch { /* Ignore */ }
                try { if (_noLyricsSearchList != null) _noLyricsSearchList.Clear(); _noLyricsSearchList = null; } catch { /* Ignore */ }
                try { if (_currentSortedMcPlaylists != null) _currentSortedMcPlaylists.Clear(); _currentSortedMcPlaylists = null; } catch { /* Ignore */ }
                try { if (_cancellationTokenSource != null) _cancellationTokenSource.Dispose(); } catch { /* Ignore */ }
                try { if (_emptyCoverImage != null) _emptyCoverImage.Dispose(); } catch { /* Ignore */ }
                try { if (_emptyPlayPauseImage != null) _emptyPlayPauseImage.Dispose(); } catch { /* Ignore */ }
                try { if (components != null) components.Dispose(); } catch { /* Ignore */ }
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainContainer = new System.Windows.Forms.ToolStripContainer();
            this.BottomMenu = new System.Windows.Forms.MenuStrip();
            this.SearchAllStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            this.TestTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.OverwriteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.MainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.MainGridView = new System.Windows.Forms.DataGridView();
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileReloadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSelectPlaylistMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dummyItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSepMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSepMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.FileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsSearchAllStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            this.ToolsPlayStartStopButton = new MediaCenter.LyricsFinder.StartStopToolStripButton();
            this.ToolSepMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolsLyricServicesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsOptionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolSepMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolsShowLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolSepMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolsTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DataChangedTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpContentsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpSepMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpLookForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpSepMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TopSubMenu = new System.Windows.Forms.MenuStrip();
            this.TopSubMenuTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.MainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextEditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextPlayPauseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextPlayStopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.McStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.UpdateCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.Cover = new System.Windows.Forms.DataGridViewImageColumn();
            this.Artist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Album = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lyrics = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainContainer.BottomToolStripPanel.SuspendLayout();
            this.MainContainer.ContentPanel.SuspendLayout();
            this.MainContainer.TopToolStripPanel.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.BottomMenu.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).BeginInit();
            this.TopMenu.SuspendLayout();
            this.TopSubMenu.SuspendLayout();
            this.MainContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            // 
            // MainContainer.BottomToolStripPanel
            // 
            this.MainContainer.BottomToolStripPanel.Controls.Add(this.BottomMenu);
            this.MainContainer.BottomToolStripPanel.Controls.Add(this.MainStatusStrip);
            // 
            // MainContainer.ContentPanel
            // 
            this.MainContainer.ContentPanel.Controls.Add(this.MainGridView);
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
            this.SearchAllStartStopButton,
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
            // SearchAllStartStopButton
            // 
            this.SearchAllStartStopButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SearchAllStartStopButton.CheckOnClick = true;
            this.SearchAllStartStopButton.Clicked = false;
            this.SearchAllStartStopButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchAllStartStopButton.Image")));
            this.SearchAllStartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("SearchAllStartStopButton.ImageStart")));
            this.SearchAllStartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("SearchAllStartStopButton.ImageStop")));
            this.SearchAllStartStopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SearchAllStartStopButton.IsRunning = false;
            this.SearchAllStartStopButton.Margin = new System.Windows.Forms.Padding(2, 1, 4, 2);
            this.SearchAllStartStopButton.Name = "SearchAllStartStopButton";
            this.SearchAllStartStopButton.Size = new System.Drawing.Size(103, 20);
            this.SearchAllStartStopButton.Text = "&Start search all";
            this.SearchAllStartStopButton.TextStart = "&Start search all";
            this.SearchAllStartStopButton.TextStop = "&Stop search all";
            this.SearchAllStartStopButton.ToolTipText = "Start / stop finding all the lyrics for the current playlist";
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
            this.OverwriteMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainStatusLabel,
            this.MainProgressBar});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 27);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.ShowItemToolTips = true;
            this.MainStatusStrip.Size = new System.Drawing.Size(738, 22);
            this.MainStatusStrip.SizingGrip = false;
            this.MainStatusStrip.TabIndex = 0;
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
            // MainGridView
            // 
            this.MainGridView.AllowUserToAddRows = false;
            this.MainGridView.AllowUserToDeleteRows = false;
            this.MainGridView.AllowUserToResizeColumns = false;
            this.MainGridView.AllowUserToResizeRows = false;
            this.MainGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.Key,
            this.PlayImage,
            this.Cover,
            this.Artist,
            this.Album,
            this.Title,
            this.Lyrics,
            this.Status});
            this.MainGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGridView.Location = new System.Drawing.Point(0, 0);
            this.MainGridView.MultiSelect = false;
            this.MainGridView.Name = "MainGridView";
            this.MainGridView.ReadOnly = true;
            this.MainGridView.RowHeadersVisible = false;
            this.MainGridView.RowTemplate.Height = 30;
            this.MainGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MainGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MainGridView.Size = new System.Drawing.Size(738, 412);
            this.MainGridView.TabIndex = 0;
            this.MainGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainGridView_CellDoubleClickAsync);
            this.MainGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_CellMouseClickAsync);
            this.MainGridView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_CellMouseMoveAsync);
            this.MainGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_ColumnHeaderMouseClickAsync);
            this.MainGridView.SelectionChanged += new System.EventHandler(this.MainGridView_SelectionChangedAsync);
            this.MainGridView.MouseLeave += new System.EventHandler(this.MainGridView_MouseLeaveAsync);
            this.MainGridView.Resize += new System.EventHandler(this.MainGridView_ResizeAsync);
            // 
            // TopMenu
            // 
            this.TopMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem,
            this.DataChangedTextBox,
            this.HelpMenuItem});
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
            this.FileSelectPlaylistMenuItem,
            this.FileSepMenuItem1,
            this.FileSaveMenuItem,
            this.FileSepMenuItem2,
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
            this.FileReloadMenuItem.ToolTipText = "Reload the Play Now list. \r\nAlso reconnects to the running Media Center, \r\nuseful" +
    " if the connect failed at startup of the standalone application.";
            this.FileReloadMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // FileSelectPlaylistMenuItem
            // 
            this.FileSelectPlaylistMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyItemToolStripMenuItem});
            this.FileSelectPlaylistMenuItem.Name = "FileSelectPlaylistMenuItem";
            this.FileSelectPlaylistMenuItem.Size = new System.Drawing.Size(255, 22);
            this.FileSelectPlaylistMenuItem.Text = "Select playlist";
            this.FileSelectPlaylistMenuItem.DropDownOpening += new System.EventHandler(this.FileSelectPlaylistMenuItem_DropDownOpeningAsync);
            this.FileSelectPlaylistMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // dummyItemToolStripMenuItem
            // 
            this.dummyItemToolStripMenuItem.Name = "dummyItemToolStripMenuItem";
            this.dummyItemToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.dummyItemToolStripMenuItem.Text = "Dummy item";
            // 
            // FileSepMenuItem1
            // 
            this.FileSepMenuItem1.Name = "FileSepMenuItem1";
            this.FileSepMenuItem1.Size = new System.Drawing.Size(252, 6);
            // 
            // FileSaveMenuItem
            // 
            this.FileSaveMenuItem.Name = "FileSaveMenuItem";
            this.FileSaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.FileSaveMenuItem.Size = new System.Drawing.Size(255, 22);
            this.FileSaveMenuItem.Text = "&Save items to MediaCenter";
            this.FileSaveMenuItem.ToolTipText = "Save the found/changed lyrics to the song tags";
            this.FileSaveMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // FileSepMenuItem2
            // 
            this.FileSepMenuItem2.Name = "FileSepMenuItem2";
            this.FileSepMenuItem2.Size = new System.Drawing.Size(252, 6);
            this.FileSepMenuItem2.Visible = false;
            // 
            // FileExitMenuItem
            // 
            this.FileExitMenuItem.Name = "FileExitMenuItem";
            this.FileExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.FileExitMenuItem.Size = new System.Drawing.Size(255, 22);
            this.FileExitMenuItem.Text = "E&xit";
            this.FileExitMenuItem.ToolTipText = "Close the Lyrics Finder standalone application";
            this.FileExitMenuItem.Visible = false;
            this.FileExitMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsSearchAllStartStopButton,
            this.ToolsPlayStartStopButton,
            this.ToolSepMenuItem1,
            this.ToolsLyricServicesMenuItem,
            this.ToolsOptionsMenuItem,
            this.ToolSepMenuItem2,
            this.ToolsShowLogMenuItem,
            this.ToolSepMenuItem3,
            this.ToolsTestMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(47, 20);
            this.ToolsMenuItem.Text = "&Tools";
            // 
            // ToolsSearchAllStartStopButton
            // 
            this.ToolsSearchAllStartStopButton.Clicked = false;
            this.ToolsSearchAllStartStopButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolsSearchAllStartStopButton.Image")));
            this.ToolsSearchAllStartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("ToolsSearchAllStartStopButton.ImageStart")));
            this.ToolsSearchAllStartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("ToolsSearchAllStartStopButton.ImageStop")));
            this.ToolsSearchAllStartStopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolsSearchAllStartStopButton.IsRunning = false;
            this.ToolsSearchAllStartStopButton.Name = "ToolsSearchAllStartStopButton";
            this.ToolsSearchAllStartStopButton.Size = new System.Drawing.Size(103, 20);
            this.ToolsSearchAllStartStopButton.Text = "&Stop search all";
            this.ToolsSearchAllStartStopButton.TextStart = "&Start search all";
            this.ToolsSearchAllStartStopButton.TextStop = "&Stop search all";
            this.ToolsSearchAllStartStopButton.ToolTipText = "Start / stop finding all the lyrics for the current playlist";
            // 
            // ToolsPlayStartStopButton
            // 
            this.ToolsPlayStartStopButton.Clicked = false;
            this.ToolsPlayStartStopButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolsPlayStartStopButton.Image")));
            this.ToolsPlayStartStopButton.ImageStart = ((System.Drawing.Bitmap)(resources.GetObject("ToolsPlayStartStopButton.ImageStart")));
            this.ToolsPlayStartStopButton.ImageStop = ((System.Drawing.Bitmap)(resources.GetObject("ToolsPlayStartStopButton.ImageStop")));
            this.ToolsPlayStartStopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolsPlayStartStopButton.IsRunning = false;
            this.ToolsPlayStartStopButton.Name = "ToolsPlayStartStopButton";
            this.ToolsPlayStartStopButton.Size = new System.Drawing.Size(76, 20);
            this.ToolsPlayStartStopButton.Text = "Stop &play";
            this.ToolsPlayStartStopButton.TextStart = "Start &play";
            this.ToolsPlayStartStopButton.TextStop = "Stop &play";
            this.ToolsPlayStartStopButton.ToolTipText = "Start / stop play of the current item in the current playlist";
            // 
            // ToolSepMenuItem1
            // 
            this.ToolSepMenuItem1.Name = "ToolSepMenuItem1";
            this.ToolSepMenuItem1.Size = new System.Drawing.Size(204, 6);
            // 
            // ToolsLyricServicesMenuItem
            // 
            this.ToolsLyricServicesMenuItem.Name = "ToolsLyricServicesMenuItem";
            this.ToolsLyricServicesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Y)));
            this.ToolsLyricServicesMenuItem.Size = new System.Drawing.Size(207, 22);
            this.ToolsLyricServicesMenuItem.Text = "L&yric services...";
            this.ToolsLyricServicesMenuItem.ToolTipText = "Select the services to be used when searching for lyrics. \r\nYou can also change t" +
    "he search service order.";
            this.ToolsLyricServicesMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolsOptionsMenuItem
            // 
            this.ToolsOptionsMenuItem.Name = "ToolsOptionsMenuItem";
            this.ToolsOptionsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.ToolsOptionsMenuItem.Size = new System.Drawing.Size(207, 22);
            this.ToolsOptionsMenuItem.Text = "&Options...";
            this.ToolsOptionsMenuItem.ToolTipText = "Configure connection to Media Center Network Service";
            this.ToolsOptionsMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolSepMenuItem2
            // 
            this.ToolSepMenuItem2.Name = "ToolSepMenuItem2";
            this.ToolSepMenuItem2.Size = new System.Drawing.Size(204, 6);
            // 
            // ToolsShowLogMenuItem
            // 
            this.ToolsShowLogMenuItem.Name = "ToolsShowLogMenuItem";
            this.ToolsShowLogMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.ToolsShowLogMenuItem.Size = new System.Drawing.Size(207, 22);
            this.ToolsShowLogMenuItem.Text = "Show &logs folder...";
            this.ToolsShowLogMenuItem.ToolTipText = "Open the folder where the logs are located";
            this.ToolsShowLogMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // ToolSepMenuItem3
            // 
            this.ToolSepMenuItem3.Name = "ToolSepMenuItem3";
            this.ToolSepMenuItem3.Size = new System.Drawing.Size(204, 6);
            this.ToolSepMenuItem3.Visible = false;
            // 
            // ToolsTestMenuItem
            // 
            this.ToolsTestMenuItem.Name = "ToolsTestMenuItem";
            this.ToolsTestMenuItem.Size = new System.Drawing.Size(207, 22);
            this.ToolsTestMenuItem.Text = "Test";
            this.ToolsTestMenuItem.Visible = false;
            this.ToolsTestMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
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
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpContentsMenuItem,
            this.HelpSepMenuItem1,
            this.HelpLookForUpdatesMenuItem,
            this.HelpSepMenuItem2,
            this.HelpAboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "&Help";
            // 
            // HelpContentsMenuItem
            // 
            this.HelpContentsMenuItem.Name = "HelpContentsMenuItem";
            this.HelpContentsMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.HelpContentsMenuItem.Size = new System.Drawing.Size(219, 22);
            this.HelpContentsMenuItem.Text = "&Help contents...";
            this.HelpContentsMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // HelpSepMenuItem1
            // 
            this.HelpSepMenuItem1.Name = "HelpSepMenuItem1";
            this.HelpSepMenuItem1.Size = new System.Drawing.Size(216, 6);
            // 
            // HelpLookForUpdatesMenuItem
            // 
            this.HelpLookForUpdatesMenuItem.Name = "HelpLookForUpdatesMenuItem";
            this.HelpLookForUpdatesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.U)));
            this.HelpLookForUpdatesMenuItem.Size = new System.Drawing.Size(219, 22);
            this.HelpLookForUpdatesMenuItem.Text = "Look for updates...";
            this.HelpLookForUpdatesMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // HelpSepMenuItem2
            // 
            this.HelpSepMenuItem2.Name = "HelpSepMenuItem2";
            this.HelpSepMenuItem2.Size = new System.Drawing.Size(216, 6);
            // 
            // HelpAboutMenuItem
            // 
            this.HelpAboutMenuItem.Name = "HelpAboutMenuItem";
            this.HelpAboutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.HelpAboutMenuItem.Size = new System.Drawing.Size(219, 22);
            this.HelpAboutMenuItem.Text = "&About LyricsFinder...";
            this.HelpAboutMenuItem.Click += new System.EventHandler(this.MenuItem_ClickAsync);
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
            this.TopSubMenuTextBox.Click += new System.EventHandler(this.MenuItem_ClickAsync);
            // 
            // MainToolTip
            // 
            this.MainToolTip.AutomaticDelay = 50;
            this.MainToolTip.AutoPopDelay = 10000;
            this.MainToolTip.InitialDelay = 50;
            this.MainToolTip.ReshowDelay = 10;
            // 
            // MainContextMenu
            // 
            this.MainContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextEditMenuItem,
            this.ContextPlayPauseMenuItem,
            this.ContextPlayStopMenuItem});
            this.MainContextMenu.Name = "ContextMenu";
            this.MainContextMenu.Size = new System.Drawing.Size(170, 70);
            this.MainContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MainContextMenu_OpeningAsync);
            this.MainContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainContextMenu_ItemClickedAsync);
            // 
            // ContextEditMenuItem
            // 
            this.ContextEditMenuItem.Name = "ContextEditMenuItem";
            this.ContextEditMenuItem.Size = new System.Drawing.Size(169, 22);
            this.ContextEditMenuItem.Text = "Edit / search lyrics";
            // 
            // ContextPlayPauseMenuItem
            // 
            this.ContextPlayPauseMenuItem.Name = "ContextPlayPauseMenuItem";
            this.ContextPlayPauseMenuItem.Size = new System.Drawing.Size(169, 22);
            this.ContextPlayPauseMenuItem.Text = "Play / pause";
            // 
            // ContextPlayStopMenuItem
            // 
            this.ContextPlayStopMenuItem.Name = "ContextPlayStopMenuItem";
            this.ContextPlayStopMenuItem.Size = new System.Drawing.Size(169, 22);
            this.ContextPlayStopMenuItem.Text = "Stop play";
            // 
            // McStatusTimer
            // 
            this.McStatusTimer.Interval = 500;
            this.McStatusTimer.Tick += new System.EventHandler(this.McStatusTimer_TickAsync);
            // 
            // UpdateCheckTimer
            // 
            this.UpdateCheckTimer.Interval = 5000;
            this.UpdateCheckTimer.Tick += new System.EventHandler(this.UpdateCheckTimer_TickAsync);
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Index.Visible = false;
            // 
            // Key
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.Key.DefaultCellStyle = dataGridViewCellStyle1;
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Key.Visible = false;
            this.Key.Width = 5;
            // 
            // PlayImage
            // 
            this.PlayImage.HeaderText = "";
            this.PlayImage.MinimumWidth = 18;
            this.PlayImage.Name = "PlayImage";
            this.PlayImage.ReadOnly = true;
            this.PlayImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PlayImage.Width = 18;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Artist.DefaultCellStyle = dataGridViewCellStyle2;
            this.Artist.HeaderText = "Artist";
            this.Artist.Name = "Artist";
            this.Artist.ReadOnly = true;
            this.Artist.Width = 150;
            // 
            // Album
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Album.DefaultCellStyle = dataGridViewCellStyle3;
            this.Album.HeaderText = "Album";
            this.Album.Name = "Album";
            this.Album.ReadOnly = true;
            this.Album.Width = 150;
            // 
            // Title
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Title.DefaultCellStyle = dataGridViewCellStyle4;
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 150;
            // 
            // Lyrics
            // 
            this.Lyrics.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Lyrics.DefaultCellStyle = dataGridViewCellStyle5;
            this.Lyrics.HeaderText = "Lyrics";
            this.Lyrics.Name = "Lyrics";
            this.Lyrics.ReadOnly = true;
            // 
            // Status
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.DefaultCellStyle = dataGridViewCellStyle6;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Status.Width = 150;
            // 
            // LyricsFinderCore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainContainer);
            this.Name = "LyricsFinderCore";
            this.Size = new System.Drawing.Size(738, 509);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricsFinderCore_KeyDownAsync);
            this.Resize += new System.EventHandler(this.LyricsFinderCore_ResizeAsync);
            this.MainContainer.BottomToolStripPanel.ResumeLayout(false);
            this.MainContainer.BottomToolStripPanel.PerformLayout();
            this.MainContainer.ContentPanel.ResumeLayout(false);
            this.MainContainer.TopToolStripPanel.ResumeLayout(false);
            this.MainContainer.TopToolStripPanel.PerformLayout();
            this.MainContainer.ResumeLayout(false);
            this.MainContainer.PerformLayout();
            this.BottomMenu.ResumeLayout(false);
            this.BottomMenu.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).EndInit();
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.TopSubMenu.ResumeLayout(false);
            this.TopSubMenu.PerformLayout();
            this.MainContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ToolStripContainer MainContainer;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MainStatusLabel;
        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileSepMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem FileExitMenuItem;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsTestMenuItem;
        private System.Windows.Forms.MenuStrip BottomMenu;
        private StartStopToolStripButton SearchAllStartStopButton;
        private System.Windows.Forms.ToolStripProgressBar MainProgressBar;
        private System.Windows.Forms.DataGridView MainGridView;
        private System.Windows.Forms.ToolStripTextBox TestTextBox;
        private System.Windows.Forms.ToolStripMenuItem FileSaveMenuItem;
        private System.Windows.Forms.ToolStripTextBox DataChangedTextBox;
        private StartStopToolStripButton ToolsSearchAllStartStopButton;
        private System.Windows.Forms.ToolStripSeparator ToolSepMenuItem3;
        private System.Windows.Forms.MenuStrip TopSubMenu;
        private System.Windows.Forms.ToolStripTextBox TopSubMenuTextBox;
        private System.Windows.Forms.ToolStripMenuItem ToolsLyricServicesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpAboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsShowLogMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileReloadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OverwriteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpContentsMenuItem;
        private System.Windows.Forms.ToolStripSeparator HelpSepMenuItem1;
        private StartStopToolStripButton ToolsPlayStartStopButton;
        private System.Windows.Forms.ContextMenuStrip MainContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ContextEditMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ContextPlayPauseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ContextPlayStopMenuItem;
        private System.Windows.Forms.Timer McStatusTimer;
        private System.Windows.Forms.ToolStripMenuItem ToolsOptionsMenuItem;
        private System.Windows.Forms.ToolStripSeparator ToolSepMenuItem1;
        private System.Windows.Forms.ToolStripSeparator ToolSepMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem HelpLookForUpdatesMenuItem;
        private System.Windows.Forms.ToolStripSeparator HelpSepMenuItem2;
        private System.Windows.Forms.Timer UpdateCheckTimer;
        private System.Windows.Forms.ToolStripSeparator FileSepMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem FileSelectPlaylistMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dummyItemToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewImageColumn PlayImage;
        private System.Windows.Forms.DataGridViewImageColumn Cover;
        private System.Windows.Forms.DataGridViewTextBoxColumn Artist;
        private System.Windows.Forms.DataGridViewTextBoxColumn Album;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lyrics;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }

}
