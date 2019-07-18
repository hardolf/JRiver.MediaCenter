namespace MediaCenter.LyricsFinder
{
    partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.AboutBoxPanel = new System.Windows.Forms.Panel();
            this.AboutBoxLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.ProductNameLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.CompanyNameLabel = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ReleaseNotesLinkLabel = new System.Windows.Forms.LinkLabel();
            this.BuildDateLabel = new System.Windows.Forms.Label();
            this.ProjectLinkLabel = new System.Windows.Forms.LinkLabel();
            this.UpdateCheckButton = new System.Windows.Forms.Button();
            this.AboutToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.AboutBoxPanel.SuspendLayout();
            this.AboutBoxLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AboutBoxPanel
            // 
            this.AboutBoxPanel.Controls.Add(this.AboutBoxLayoutPanel);
            this.AboutBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.AboutBoxPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AboutBoxPanel.Name = "AboutBoxPanel";
            this.AboutBoxPanel.Padding = new System.Windows.Forms.Padding(5);
            this.AboutBoxPanel.Size = new System.Drawing.Size(484, 326);
            this.AboutBoxPanel.TabIndex = 0;
            // 
            // AboutBoxLayoutPanel
            // 
            this.AboutBoxLayoutPanel.ColumnCount = 3;
            this.AboutBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.AboutBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.AboutBoxLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.AboutBoxLayoutPanel.Controls.Add(this.LogoPictureBox, 0, 0);
            this.AboutBoxLayoutPanel.Controls.Add(this.ProductNameLabel, 1, 0);
            this.AboutBoxLayoutPanel.Controls.Add(this.VersionLabel, 1, 1);
            this.AboutBoxLayoutPanel.Controls.Add(this.CopyrightLabel, 1, 4);
            this.AboutBoxLayoutPanel.Controls.Add(this.CompanyNameLabel, 1, 5);
            this.AboutBoxLayoutPanel.Controls.Add(this.DescriptionTextBox, 1, 6);
            this.AboutBoxLayoutPanel.Controls.Add(this.CloseButton, 2, 7);
            this.AboutBoxLayoutPanel.Controls.Add(this.ReleaseNotesLinkLabel, 2, 1);
            this.AboutBoxLayoutPanel.Controls.Add(this.BuildDateLabel, 1, 2);
            this.AboutBoxLayoutPanel.Controls.Add(this.ProjectLinkLabel, 1, 3);
            this.AboutBoxLayoutPanel.Controls.Add(this.UpdateCheckButton, 1, 7);
            this.AboutBoxLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutBoxLayoutPanel.Location = new System.Drawing.Point(5, 5);
            this.AboutBoxLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AboutBoxLayoutPanel.Name = "AboutBoxLayoutPanel";
            this.AboutBoxLayoutPanel.RowCount = 8;
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.AboutBoxLayoutPanel.Size = new System.Drawing.Size(474, 316);
            this.AboutBoxLayoutPanel.TabIndex = 0;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoPictureBox.Image")));
            this.LogoPictureBox.Location = new System.Drawing.Point(3, 3);
            this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.AboutBoxLayoutPanel.SetRowSpan(this.LogoPictureBox, 8);
            this.LogoPictureBox.Size = new System.Drawing.Size(155, 306);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogoPictureBox.TabIndex = 12;
            this.LogoPictureBox.TabStop = false;
            // 
            // ProductNameLabel
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.ProductNameLabel, 2);
            this.ProductNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProductNameLabel.Location = new System.Drawing.Point(167, 0);
            this.ProductNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.ProductNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.ProductNameLabel.Name = "ProductNameLabel";
            this.ProductNameLabel.Size = new System.Drawing.Size(304, 17);
            this.ProductNameLabel.TabIndex = 1;
            this.ProductNameLabel.Text = "Product Name";
            // 
            // VersionLabel
            // 
            this.VersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VersionLabel.Location = new System.Drawing.Point(167, 25);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.VersionLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(147, 17);
            this.VersionLabel.TabIndex = 2;
            this.VersionLabel.Text = "Version";
            // 
            // CopyrightLabel
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.CopyrightLabel, 2);
            this.CopyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CopyrightLabel.Location = new System.Drawing.Point(167, 100);
            this.CopyrightLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.CopyrightLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(304, 17);
            this.CopyrightLabel.TabIndex = 6;
            this.CopyrightLabel.Text = "Copyright";
            // 
            // CompanyNameLabel
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.CompanyNameLabel, 2);
            this.CompanyNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompanyNameLabel.Location = new System.Drawing.Point(167, 125);
            this.CompanyNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.CompanyNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.CompanyNameLabel.Name = "CompanyNameLabel";
            this.CompanyNameLabel.Size = new System.Drawing.Size(304, 5);
            this.CompanyNameLabel.TabIndex = 7;
            this.CompanyNameLabel.Text = "Company Name";
            this.CompanyNameLabel.Visible = false;
            // 
            // DescriptionTextBox
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.DescriptionTextBox, 2);
            this.DescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionTextBox.Location = new System.Drawing.Point(169, 130);
            this.DescriptionTextBox.Margin = new System.Windows.Forms.Padding(8, 0, 7, 0);
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.ReadOnly = true;
            this.DescriptionTextBox.Size = new System.Drawing.Size(298, 146);
            this.DescriptionTextBox.TabIndex = 8;
            this.DescriptionTextBox.TabStop = false;
            this.DescriptionTextBox.Text = "Description";
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(392, 286);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(7, 3, 7, 7);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_ClickAsync);
            // 
            // ReleaseNotesLinkLabel
            // 
            this.ReleaseNotesLinkLabel.AutoSize = true;
            this.ReleaseNotesLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReleaseNotesLinkLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ReleaseNotesLinkLabel.Location = new System.Drawing.Point(323, 25);
            this.ReleaseNotesLinkLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.ReleaseNotesLinkLabel.Name = "ReleaseNotesLinkLabel";
            this.ReleaseNotesLinkLabel.Size = new System.Drawing.Size(148, 25);
            this.ReleaseNotesLinkLabel.TabIndex = 3;
            this.ReleaseNotesLinkLabel.TabStop = true;
            this.ReleaseNotesLinkLabel.Text = "Release notes";
            this.ReleaseNotesLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClickedAsync);
            // 
            // BuildDateLabel
            // 
            this.BuildDateLabel.AutoSize = true;
            this.BuildDateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BuildDateLabel.Location = new System.Drawing.Point(167, 50);
            this.BuildDateLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.BuildDateLabel.Name = "BuildDateLabel";
            this.BuildDateLabel.Size = new System.Drawing.Size(147, 25);
            this.BuildDateLabel.TabIndex = 4;
            this.BuildDateLabel.Text = "Build Date";
            // 
            // ProjectLinkLabel
            // 
            this.ProjectLinkLabel.AutoSize = true;
            this.AboutBoxLayoutPanel.SetColumnSpan(this.ProjectLinkLabel, 2);
            this.ProjectLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectLinkLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ProjectLinkLabel.Location = new System.Drawing.Point(167, 75);
            this.ProjectLinkLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.ProjectLinkLabel.Name = "ProjectLinkLabel";
            this.ProjectLinkLabel.Size = new System.Drawing.Size(304, 25);
            this.ProjectLinkLabel.TabIndex = 5;
            this.ProjectLinkLabel.TabStop = true;
            this.ProjectLinkLabel.Text = "LyricsFinder project source on GitHub";
            this.AboutToolTip.SetToolTip(this.ProjectLinkLabel, "https://github.com/hardolf/JRiver.MediaCenter");
            this.ProjectLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClickedAsync);
            // 
            // UpdateCheckButton
            // 
            this.UpdateCheckButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UpdateCheckButton.Location = new System.Drawing.Point(168, 286);
            this.UpdateCheckButton.Margin = new System.Windows.Forms.Padding(7, 3, 7, 7);
            this.UpdateCheckButton.Name = "UpdateCheckButton";
            this.UpdateCheckButton.Size = new System.Drawing.Size(124, 23);
            this.UpdateCheckButton.TabIndex = 9;
            this.UpdateCheckButton.Text = "Check for Updates...";
            this.UpdateCheckButton.UseVisualStyleBackColor = true;
            this.UpdateCheckButton.Click += new System.EventHandler(this.UpdateCheckButton_ClickAsync);
            // 
            // AboutBox
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(484, 326);
            this.Controls.Add(this.AboutBoxPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.Load += new System.EventHandler(this.AboutBox_LoadAsync);
            this.AboutBoxPanel.ResumeLayout(false);
            this.AboutBoxLayoutPanel.ResumeLayout(false);
            this.AboutBoxLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel AboutBoxPanel;
        private System.Windows.Forms.TableLayoutPanel AboutBoxLayoutPanel;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Label ProductNameLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.Label CompanyNameLabel;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.LinkLabel ReleaseNotesLinkLabel;
        private System.Windows.Forms.Label BuildDateLabel;
        private System.Windows.Forms.LinkLabel ProjectLinkLabel;
        private System.Windows.Forms.ToolTip AboutToolTip;
        private System.Windows.Forms.Button UpdateCheckButton;
    }
}
