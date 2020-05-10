namespace MediaCenter.LyricsFinder.Forms
{

    partial class LyricServiceForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LyricServiceForm));
            this.LyricServiceMainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LyricServiceListPanel = new System.Windows.Forms.Panel();
            this.LyricServiceListContainer = new System.Windows.Forms.ToolStripContainer();
            this.LyricServiceListDataGridView = new System.Windows.Forms.DataGridView();
            this.Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Service = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestsMaximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestsToday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestsTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HitsToday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HitsTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuotaResetTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LyricServiceListRightToolbar = new System.Windows.Forms.ToolStrip();
            this.MoveUpButton = new System.Windows.Forms.ToolStripButton();
            this.MoveDownButton = new System.Windows.Forms.ToolStripButton();
            this.LyricServiceDetailsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.LyricServiceFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LyricServiceMainTableLayoutPanel.SuspendLayout();
            this.LyricServiceListPanel.SuspendLayout();
            this.LyricServiceListContainer.ContentPanel.SuspendLayout();
            this.LyricServiceListContainer.RightToolStripPanel.SuspendLayout();
            this.LyricServiceListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LyricServiceListDataGridView)).BeginInit();
            this.LyricServiceListRightToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // LyricServiceMainTableLayoutPanel
            // 
            this.LyricServiceMainTableLayoutPanel.AutoSize = true;
            this.LyricServiceMainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LyricServiceMainTableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.LyricServiceMainTableLayoutPanel.ColumnCount = 1;
            this.LyricServiceMainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LyricServiceMainTableLayoutPanel.Controls.Add(this.LyricServiceListPanel, 0, 0);
            this.LyricServiceMainTableLayoutPanel.Controls.Add(this.LyricServiceDetailsTableLayoutPanel, 0, 1);
            this.LyricServiceMainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceMainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LyricServiceMainTableLayoutPanel.Name = "LyricServiceMainTableLayoutPanel";
            this.LyricServiceMainTableLayoutPanel.RowCount = 2;
            this.LyricServiceMainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.LyricServiceMainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LyricServiceMainTableLayoutPanel.Size = new System.Drawing.Size(684, 235);
            this.LyricServiceMainTableLayoutPanel.TabIndex = 1;
            // 
            // LyricServiceListPanel
            // 
            this.LyricServiceListPanel.Controls.Add(this.LyricServiceListContainer);
            this.LyricServiceListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceListPanel.Location = new System.Drawing.Point(3, 3);
            this.LyricServiceListPanel.Name = "LyricServiceListPanel";
            this.LyricServiceListPanel.Size = new System.Drawing.Size(678, 174);
            this.LyricServiceListPanel.TabIndex = 14;
            // 
            // LyricServiceListContainer
            // 
            this.LyricServiceListContainer.BottomToolStripPanelVisible = false;
            // 
            // LyricServiceListContainer.ContentPanel
            // 
            this.LyricServiceListContainer.ContentPanel.Controls.Add(this.LyricServiceListDataGridView);
            this.LyricServiceListContainer.ContentPanel.Size = new System.Drawing.Size(634, 154);
            this.LyricServiceListContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceListContainer.LeftToolStripPanelVisible = false;
            this.LyricServiceListContainer.Location = new System.Drawing.Point(0, 0);
            this.LyricServiceListContainer.Name = "LyricServiceListContainer";
            this.LyricServiceListContainer.Padding = new System.Windows.Forms.Padding(10);
            // 
            // LyricServiceListContainer.RightToolStripPanel
            // 
            this.LyricServiceListContainer.RightToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this.LyricServiceListContainer.RightToolStripPanel.Controls.Add(this.LyricServiceListRightToolbar);
            this.LyricServiceListContainer.Size = new System.Drawing.Size(678, 174);
            this.LyricServiceListContainer.TabIndex = 5;
            this.LyricServiceListContainer.Text = "Lyrics services";
            this.LyricServiceListContainer.TopToolStripPanelVisible = false;
            // 
            // LyricServiceListDataGridView
            // 
            this.LyricServiceListDataGridView.AllowUserToAddRows = false;
            this.LyricServiceListDataGridView.AllowUserToDeleteRows = false;
            this.LyricServiceListDataGridView.AllowUserToResizeColumns = false;
            this.LyricServiceListDataGridView.AllowUserToResizeRows = false;
            this.LyricServiceListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LyricServiceListDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Active,
            this.Service,
            this.RequestsMaximum,
            this.RequestsToday,
            this.RequestsTotal,
            this.HitsToday,
            this.HitsTotal,
            this.QuotaResetTime});
            this.LyricServiceListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceListDataGridView.Location = new System.Drawing.Point(0, 0);
            this.LyricServiceListDataGridView.MultiSelect = false;
            this.LyricServiceListDataGridView.Name = "LyricServiceListDataGridView";
            this.LyricServiceListDataGridView.RowHeadersVisible = false;
            this.LyricServiceListDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LyricServiceListDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LyricServiceListDataGridView.ShowEditingIcon = false;
            this.LyricServiceListDataGridView.Size = new System.Drawing.Size(634, 154);
            this.LyricServiceListDataGridView.TabIndex = 0;
            this.LyricServiceListDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LyricServiceListDataGridView_CellClickAsync);
            this.LyricServiceListDataGridView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.LyricServiceListDataGridView_RowValidatingAsync);
            this.LyricServiceListDataGridView.SelectionChanged += new System.EventHandler(this.LyricServiceListDataGridView_SelectionChangedAsync);
            // 
            // Active
            // 
            this.Active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            this.Active.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Active.ToolTipText = "Check if this lyrics service should be used during searhes";
            this.Active.Width = 40;
            // 
            // Service
            // 
            this.Service.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Service.DefaultCellStyle = dataGridViewCellStyle1;
            this.Service.HeaderText = "Service name";
            this.Service.MinimumWidth = 150;
            this.Service.Name = "Service";
            this.Service.ReadOnly = true;
            this.Service.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Service.ToolTipText = "Lyrics service name";
            // 
            // RequestsMaximum
            // 
            this.RequestsMaximum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            this.RequestsMaximum.DefaultCellStyle = dataGridViewCellStyle2;
            this.RequestsMaximum.HeaderText = "Quota";
            this.RequestsMaximum.MinimumWidth = 50;
            this.RequestsMaximum.Name = "RequestsMaximum";
            this.RequestsMaximum.ReadOnly = true;
            this.RequestsMaximum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RequestsMaximum.ToolTipText = "Maximum allowed quota for this service";
            this.RequestsMaximum.Width = 50;
            // 
            // RequestsToday
            // 
            this.RequestsToday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.RequestsToday.DefaultCellStyle = dataGridViewCellStyle3;
            this.RequestsToday.HeaderText = "Req. today";
            this.RequestsToday.MinimumWidth = 65;
            this.RequestsToday.Name = "RequestsToday";
            this.RequestsToday.ReadOnly = true;
            this.RequestsToday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RequestsToday.ToolTipText = "Search count today to this service";
            this.RequestsToday.Width = 65;
            // 
            // RequestsTotal
            // 
            this.RequestsTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.RequestsTotal.DefaultCellStyle = dataGridViewCellStyle4;
            this.RequestsTotal.HeaderText = "Req. total";
            this.RequestsTotal.MinimumWidth = 65;
            this.RequestsTotal.Name = "RequestsTotal";
            this.RequestsTotal.ReadOnly = true;
            this.RequestsTotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RequestsTotal.ToolTipText = "Total number of requests";
            this.RequestsTotal.Width = 65;
            // 
            // HitsToday
            // 
            this.HitsToday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            this.HitsToday.DefaultCellStyle = dataGridViewCellStyle5;
            this.HitsToday.HeaderText = "Hits today";
            this.HitsToday.MinimumWidth = 60;
            this.HitsToday.Name = "HitsToday";
            this.HitsToday.ReadOnly = true;
            this.HitsToday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HitsToday.ToolTipText = "Number of successful search results after midnight and until now";
            this.HitsToday.Width = 60;
            // 
            // HitsTotal
            // 
            this.HitsTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.HitsTotal.DefaultCellStyle = dataGridViewCellStyle6;
            this.HitsTotal.HeaderText = "Hits total";
            this.HitsTotal.MinimumWidth = 60;
            this.HitsTotal.Name = "HitsTotal";
            this.HitsTotal.ReadOnly = true;
            this.HitsTotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HitsTotal.ToolTipText = "Total number of successful search results ";
            this.HitsTotal.Width = 60;
            // 
            // QuotaResetTime
            // 
            this.QuotaResetTime.HeaderText = "Next quota reset time";
            this.QuotaResetTime.MinimumWidth = 125;
            this.QuotaResetTime.Name = "QuotaResetTime";
            this.QuotaResetTime.ReadOnly = true;
            this.QuotaResetTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.QuotaResetTime.ToolTipText = "Local time (client) of next quota reset";
            this.QuotaResetTime.Width = 125;
            // 
            // LyricServiceListRightToolbar
            // 
            this.LyricServiceListRightToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.LyricServiceListRightToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.LyricServiceListRightToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MoveUpButton,
            this.MoveDownButton});
            this.LyricServiceListRightToolbar.Location = new System.Drawing.Point(0, 3);
            this.LyricServiceListRightToolbar.Name = "LyricServiceListRightToolbar";
            this.LyricServiceListRightToolbar.Size = new System.Drawing.Size(24, 48);
            this.LyricServiceListRightToolbar.TabIndex = 0;
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveUpButton.Image")));
            this.MoveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(22, 20);
            this.MoveUpButton.Text = "Up";
            this.MoveUpButton.ToolTipText = "Move the selected service up";
            this.MoveUpButton.Click += new System.EventHandler(this.LyricServicesContainer_RightPanelButton_ClickAsync);
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveDownButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveDownButton.Image")));
            this.MoveDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(22, 20);
            this.MoveDownButton.Text = "Down";
            this.MoveDownButton.ToolTipText = "Move the selected service down";
            this.MoveDownButton.Click += new System.EventHandler(this.LyricServicesContainer_RightPanelButton_ClickAsync);
            // 
            // LyricServiceDetailsTableLayoutPanel
            // 
            this.LyricServiceDetailsTableLayoutPanel.AutoSize = true;
            this.LyricServiceDetailsTableLayoutPanel.ColumnCount = 3;
            this.LyricServiceDetailsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.LyricServiceDetailsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.LyricServiceDetailsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LyricServiceDetailsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceDetailsTableLayoutPanel.Location = new System.Drawing.Point(3, 183);
            this.LyricServiceDetailsTableLayoutPanel.Name = "LyricServiceDetailsTableLayoutPanel";
            this.LyricServiceDetailsTableLayoutPanel.Padding = new System.Windows.Forms.Padding(7);
            this.LyricServiceDetailsTableLayoutPanel.RowCount = 1;
            this.LyricServiceDetailsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LyricServiceDetailsTableLayoutPanel.Size = new System.Drawing.Size(678, 49);
            this.LyricServiceDetailsTableLayoutPanel.TabIndex = 0;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(417, 124);
            // 
            // LyricServiceFormToolTip
            // 
            this.LyricServiceFormToolTip.AutoPopDelay = 15000;
            this.LyricServiceFormToolTip.InitialDelay = 500;
            this.LyricServiceFormToolTip.ReshowDelay = 100;
            // 
            // LyricServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(684, 235);
            this.Controls.Add(this.LyricServiceMainTableLayoutPanel);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 250);
            this.Name = "LyricServiceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Lyric Services";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LyricServiceForm_FormClosingAsync);
            this.Load += new System.EventHandler(this.LyricServiceForm_LoadAsync);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricServiceForm_KeyDownAsync);
            this.LyricServiceMainTableLayoutPanel.ResumeLayout(false);
            this.LyricServiceMainTableLayoutPanel.PerformLayout();
            this.LyricServiceListPanel.ResumeLayout(false);
            this.LyricServiceListContainer.ContentPanel.ResumeLayout(false);
            this.LyricServiceListContainer.RightToolStripPanel.ResumeLayout(false);
            this.LyricServiceListContainer.RightToolStripPanel.PerformLayout();
            this.LyricServiceListContainer.ResumeLayout(false);
            this.LyricServiceListContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LyricServiceListDataGridView)).EndInit();
            this.LyricServiceListRightToolbar.ResumeLayout(false);
            this.LyricServiceListRightToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LyricServiceMainTableLayoutPanel;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.TableLayoutPanel LyricServiceDetailsTableLayoutPanel;
        private System.Windows.Forms.Panel LyricServiceListPanel;
        private System.Windows.Forms.ToolStripContainer LyricServiceListContainer;
        private System.Windows.Forms.ToolStrip LyricServiceListRightToolbar;
        private System.Windows.Forms.ToolStripButton MoveUpButton;
        private System.Windows.Forms.ToolStripButton MoveDownButton;
        private System.Windows.Forms.DataGridView LyricServiceListDataGridView;
        private System.Windows.Forms.ToolTip LyricServiceFormToolTip;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn Service;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestsMaximum;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestsToday;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestsTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn HitsToday;
        private System.Windows.Forms.DataGridViewTextBoxColumn HitsTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuotaResetTime;
    }

}