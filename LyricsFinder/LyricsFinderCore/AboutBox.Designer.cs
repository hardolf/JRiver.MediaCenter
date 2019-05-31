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
            this.AboutBoxPanel.Size = new System.Drawing.Size(384, 211);
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
            this.AboutBoxLayoutPanel.Controls.Add(this.CopyrightLabel, 1, 2);
            this.AboutBoxLayoutPanel.Controls.Add(this.CompanyNameLabel, 1, 3);
            this.AboutBoxLayoutPanel.Controls.Add(this.DescriptionTextBox, 1, 4);
            this.AboutBoxLayoutPanel.Controls.Add(this.CloseButton, 2, 5);
            this.AboutBoxLayoutPanel.Controls.Add(this.ReleaseNotesLinkLabel, 2, 1);
            this.AboutBoxLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutBoxLayoutPanel.Location = new System.Drawing.Point(5, 5);
            this.AboutBoxLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AboutBoxLayoutPanel.Name = "AboutBoxLayoutPanel";
            this.AboutBoxLayoutPanel.RowCount = 6;
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AboutBoxLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.AboutBoxLayoutPanel.Size = new System.Drawing.Size(374, 201);
            this.AboutBoxLayoutPanel.TabIndex = 1;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoPictureBox.Image")));
            this.LogoPictureBox.Location = new System.Drawing.Point(3, 3);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.AboutBoxLayoutPanel.SetRowSpan(this.LogoPictureBox, 6);
            this.LogoPictureBox.Size = new System.Drawing.Size(121, 195);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogoPictureBox.TabIndex = 12;
            this.LogoPictureBox.TabStop = false;
            // 
            // ProductNameLabel
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.ProductNameLabel, 2);
            this.ProductNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProductNameLabel.Location = new System.Drawing.Point(133, 0);
            this.ProductNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.ProductNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.ProductNameLabel.Name = "ProductNameLabel";
            this.ProductNameLabel.Size = new System.Drawing.Size(238, 17);
            this.ProductNameLabel.TabIndex = 19;
            this.ProductNameLabel.Text = "Product Name";
            this.ProductNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VersionLabel
            // 
            this.VersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VersionLabel.Location = new System.Drawing.Point(133, 25);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.VersionLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(114, 17);
            this.VersionLabel.TabIndex = 0;
            this.VersionLabel.Text = "Version";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CopyrightLabel
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.CopyrightLabel, 2);
            this.CopyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CopyrightLabel.Location = new System.Drawing.Point(133, 50);
            this.CopyrightLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.CopyrightLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(238, 17);
            this.CopyrightLabel.TabIndex = 21;
            this.CopyrightLabel.Text = "Copyright";
            this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CompanyNameLabel
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.CompanyNameLabel, 2);
            this.CompanyNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompanyNameLabel.Location = new System.Drawing.Point(133, 75);
            this.CompanyNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.CompanyNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.CompanyNameLabel.Name = "CompanyNameLabel";
            this.CompanyNameLabel.Size = new System.Drawing.Size(238, 17);
            this.CompanyNameLabel.TabIndex = 22;
            this.CompanyNameLabel.Text = "Company Name";
            this.CompanyNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CompanyNameLabel.Visible = false;
            // 
            // DescriptionTextBox
            // 
            this.AboutBoxLayoutPanel.SetColumnSpan(this.DescriptionTextBox, 2);
            this.DescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionTextBox.Location = new System.Drawing.Point(133, 103);
            this.DescriptionTextBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.ReadOnly = true;
            this.DescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DescriptionTextBox.Size = new System.Drawing.Size(238, 65);
            this.DescriptionTextBox.TabIndex = 23;
            this.DescriptionTextBox.TabStop = false;
            this.DescriptionTextBox.Text = "Description";
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(296, 175);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 24;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ReleaseNotesLinkLabel
            // 
            this.ReleaseNotesLinkLabel.AutoSize = true;
            this.ReleaseNotesLinkLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ReleaseNotesLinkLabel.Location = new System.Drawing.Point(253, 25);
            this.ReleaseNotesLinkLabel.Name = "ReleaseNotesLinkLabel";
            this.ReleaseNotesLinkLabel.Size = new System.Drawing.Size(75, 13);
            this.ReleaseNotesLinkLabel.TabIndex = 25;
            this.ReleaseNotesLinkLabel.TabStop = true;
            this.ReleaseNotesLinkLabel.Text = "Release notes";
            this.ReleaseNotesLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ReleaseNotesLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ReleaseNotesLinkLabel_LinkClicked);
            // 
            // AboutBox
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(384, 211);
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
    }
}
