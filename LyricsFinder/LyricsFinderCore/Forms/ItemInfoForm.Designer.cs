namespace MediaCenter.LyricsFinder.Forms
{
    partial class ItemInfoForm
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
            this.CloseButton = new System.Windows.Forms.Button();
            this.MainDataGridView = new System.Windows.Forms.DataGridView();
            this.MainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.IncludeCalculatedCheckBox = new System.Windows.Forms.CheckBox();
            this.CalculatedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(297, 526);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_ClickAsync);
            // 
            // MainDataGridView
            // 
            this.MainDataGridView.AllowUserToAddRows = false;
            this.MainDataGridView.AllowUserToDeleteRows = false;
            this.MainDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CalculatedColumn,
            this.ItemNameColumn,
            this.ItemValueColumn});
            this.MainDataGridView.Location = new System.Drawing.Point(12, 12);
            this.MainDataGridView.MultiSelect = false;
            this.MainDataGridView.Name = "MainDataGridView";
            this.MainDataGridView.ReadOnly = true;
            this.MainDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.MainDataGridView.Size = new System.Drawing.Size(360, 508);
            this.MainDataGridView.TabIndex = 0;
            // 
            // MainBindingSource
            // 
            this.MainBindingSource.AllowNew = false;
            // 
            // IncludeCalculatedCheckBox
            // 
            this.IncludeCalculatedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IncludeCalculatedCheckBox.AutoSize = true;
            this.IncludeCalculatedCheckBox.Location = new System.Drawing.Point(12, 532);
            this.IncludeCalculatedCheckBox.Name = "IncludeCalculatedCheckBox";
            this.IncludeCalculatedCheckBox.Size = new System.Drawing.Size(140, 17);
            this.IncludeCalculatedCheckBox.TabIndex = 2;
            this.IncludeCalculatedCheckBox.Text = "Include calculated fields";
            this.IncludeCalculatedCheckBox.UseVisualStyleBackColor = true;
            this.IncludeCalculatedCheckBox.CheckedChanged += new System.EventHandler(this.IncludeCalculatedCheckBox_CheckedChangedAsync);
            // 
            // CalculatedColumn
            // 
            this.CalculatedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CalculatedColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.CalculatedColumn.HeaderText = "Calc.";
            this.CalculatedColumn.Name = "CalculatedColumn";
            this.CalculatedColumn.ReadOnly = true;
            this.CalculatedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CalculatedColumn.Width = 35;
            // 
            // ItemNameColumn
            // 
            this.ItemNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.ItemNameColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.ItemNameColumn.HeaderText = "Name";
            this.ItemNameColumn.Name = "TagNameColumn";
            this.ItemNameColumn.ReadOnly = true;
            this.ItemNameColumn.Width = 60;
            // 
            // ItemValueColumn
            // 
            this.ItemValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.ItemValueColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.ItemValueColumn.HeaderText = "Value";
            this.ItemValueColumn.Name = "TagValueColumn";
            this.ItemValueColumn.ReadOnly = true;
            // 
            // ItemInfoForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.IncludeCalculatedCheckBox);
            this.Controls.Add(this.MainDataGridView);
            this.Controls.Add(this.CloseButton);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "ItemInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ItemInfoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ItemInfoForm_FormClosingAsync);
            this.Load += new System.EventHandler(this.ItemInfoForm_LoadAsync);
            ((System.ComponentModel.ISupportInitialize)(this.MainDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.DataGridView MainDataGridView;
        private System.Windows.Forms.BindingSource MainBindingSource;
        private System.Windows.Forms.CheckBox IncludeCalculatedCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn CalculatedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemValueColumn;
    }
}