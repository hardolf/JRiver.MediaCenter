namespace MediaCenter.LyricsFinder.Model
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LyricServiceForm));
            this.LyricServiceMainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LyricServiceListPanel = new System.Windows.Forms.Panel();
            this.LyricServiceListContainer = new System.Windows.Forms.ToolStripContainer();
            this.LyricServiceListDataGridView = new System.Windows.Forms.DataGridView();
            this.Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Service = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DailyCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DailyMaximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.LyricServiceMainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LyricServiceMainTableLayoutPanel.Name = "LyricServiceMainTableLayoutPanel";
            this.LyricServiceMainTableLayoutPanel.RowCount = 2;
            this.LyricServiceMainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.LyricServiceMainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LyricServiceMainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.LyricServiceMainTableLayoutPanel.Size = new System.Drawing.Size(483, 106);
            this.LyricServiceMainTableLayoutPanel.TabIndex = 1;
            // 
            // LyricServiceListPanel
            // 
            this.LyricServiceListPanel.Controls.Add(this.LyricServiceListContainer);
            this.LyricServiceListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceListPanel.Location = new System.Drawing.Point(3, 3);
            this.LyricServiceListPanel.Name = "LyricServiceListPanel";
            this.LyricServiceListPanel.Size = new System.Drawing.Size(477, 94);
            this.LyricServiceListPanel.TabIndex = 14;
            // 
            // LyricServiceListContainer
            // 
            this.LyricServiceListContainer.BottomToolStripPanelVisible = false;
            // 
            // LyricServiceListContainer.ContentPanel
            // 
            this.LyricServiceListContainer.ContentPanel.Controls.Add(this.LyricServiceListDataGridView);
            this.LyricServiceListContainer.ContentPanel.Size = new System.Drawing.Size(453, 94);
            this.LyricServiceListContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceListContainer.LeftToolStripPanelVisible = false;
            this.LyricServiceListContainer.Location = new System.Drawing.Point(0, 0);
            this.LyricServiceListContainer.Name = "LyricServiceListContainer";
            // 
            // LyricServiceListContainer.RightToolStripPanel
            // 
            this.LyricServiceListContainer.RightToolStripPanel.Controls.Add(this.LyricServiceListRightToolbar);
            this.LyricServiceListContainer.Size = new System.Drawing.Size(477, 94);
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
            this.DailyCount,
            this.DailyMaximum,
            this.QuotaResetTime});
            this.LyricServiceListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricServiceListDataGridView.Location = new System.Drawing.Point(0, 0);
            this.LyricServiceListDataGridView.MultiSelect = false;
            this.LyricServiceListDataGridView.Name = "LyricServiceListDataGridView";
            this.LyricServiceListDataGridView.RowHeadersVisible = false;
            this.LyricServiceListDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LyricServiceListDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LyricServiceListDataGridView.ShowEditingIcon = false;
            this.LyricServiceListDataGridView.Size = new System.Drawing.Size(453, 94);
            this.LyricServiceListDataGridView.TabIndex = 0;
            this.LyricServiceListDataGridView.TabStop = false;
            this.LyricServiceListDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LyricServiceListDataGridView_CellClick);
            this.LyricServiceListDataGridView.SelectionChanged += new System.EventHandler(this.LyricServiceListDataGridView_SelectionChanged);
            // 
            // Active
            // 
            this.Active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.ToolTipText = "Check if this lyrics service should be used during searhes";
            this.Active.Width = 40;
            // 
            // Service
            // 
            this.Service.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Service.DefaultCellStyle = dataGridViewCellStyle1;
            this.Service.HeaderText = "Service name";
            this.Service.Name = "Service";
            this.Service.ReadOnly = true;
            this.Service.ToolTipText = "Lyrics service name";
            // 
            // DailyCount
            // 
            this.DailyCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DailyCount.DefaultCellStyle = dataGridViewCellStyle2;
            this.DailyCount.FillWeight = 80F;
            this.DailyCount.HeaderText = "Today";
            this.DailyCount.MinimumWidth = 60;
            this.DailyCount.Name = "DailyCount";
            this.DailyCount.ReadOnly = true;
            this.DailyCount.ToolTipText = "Search count today to this service";
            this.DailyCount.Width = 60;
            // 
            // DailyMaximum
            // 
            this.DailyMaximum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DailyMaximum.DefaultCellStyle = dataGridViewCellStyle3;
            this.DailyMaximum.HeaderText = "Maximum";
            this.DailyMaximum.MinimumWidth = 60;
            this.DailyMaximum.Name = "DailyMaximum";
            this.DailyMaximum.ReadOnly = true;
            this.DailyMaximum.ToolTipText = "Maximum allowed searches today to this service";
            this.DailyMaximum.Width = 60;
            // 
            // QuotaResetTime
            // 
            this.QuotaResetTime.HeaderText = "Next reset time";
            this.QuotaResetTime.MinimumWidth = 120;
            this.QuotaResetTime.Name = "QuotaResetTime";
            this.QuotaResetTime.ReadOnly = true;
            this.QuotaResetTime.ToolTipText = "Local time of next quota reset";
            this.QuotaResetTime.Width = 120;
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
            this.MoveUpButton.Click += new System.EventHandler(this.LyricServicesContainer_RightPanelButton_Click);
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
            this.MoveDownButton.Click += new System.EventHandler(this.LyricServicesContainer_RightPanelButton_Click);
            // 
            // LyricServiceDetailsTableLayoutPanel
            // 
            this.LyricServiceDetailsTableLayoutPanel.AutoSize = true;
            this.LyricServiceDetailsTableLayoutPanel.ColumnCount = 2;
            this.LyricServiceDetailsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.LyricServiceDetailsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LyricServiceDetailsTableLayoutPanel.Location = new System.Drawing.Point(3, 103);
            this.LyricServiceDetailsTableLayoutPanel.Name = "LyricServiceDetailsTableLayoutPanel";
            this.LyricServiceDetailsTableLayoutPanel.RowCount = 1;
            this.LyricServiceDetailsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LyricServiceDetailsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.LyricServiceDetailsTableLayoutPanel.Size = new System.Drawing.Size(220, 0);
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
            // LyricServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 166);
            this.Controls.Add(this.LyricServiceMainTableLayoutPanel);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 205);
            this.Name = "LyricServiceForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Lyric Services";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LyricServiceForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LyricServiceForm_KeyDown);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn DailyCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DailyMaximum;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuotaResetTime;
    }

}