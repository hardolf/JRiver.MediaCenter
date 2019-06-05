namespace MediaCenter.LyricsFinder
{
    partial class ConfigurationForm
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
            this.ConfigurationPanel = new System.Windows.Forms.Panel();
            this.ConfigurationLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.McWsLabel = new System.Windows.Forms.Label();
            this.McWsUrlTextBox = new System.Windows.Forms.TextBox();
            this.McWsPasswordTextBox = new System.Windows.Forms.TextBox();
            this.McWsPassword = new System.Windows.Forms.Label();
            this.HeaderTextBox = new System.Windows.Forms.TextBox();
            this.McAccessKeyLabel = new System.Windows.Forms.Label();
            this.McAccessKeyTextBox = new System.Windows.Forms.TextBox();
            this.McWsUsernameLabel = new System.Windows.Forms.Label();
            this.McWsUsernameTextBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ConfigurationPanel.SuspendLayout();
            this.ConfigurationLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConfigurationPanel
            // 
            this.ConfigurationPanel.AutoSize = true;
            this.ConfigurationPanel.Controls.Add(this.ConfigurationLayoutPanel);
            this.ConfigurationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfigurationPanel.Location = new System.Drawing.Point(0, 0);
            this.ConfigurationPanel.Name = "ConfigurationPanel";
            this.ConfigurationPanel.Size = new System.Drawing.Size(451, 191);
            this.ConfigurationPanel.TabIndex = 0;
            // 
            // ConfigurationLayoutPanel
            // 
            this.ConfigurationLayoutPanel.AutoSize = true;
            this.ConfigurationLayoutPanel.ColumnCount = 2;
            this.ConfigurationLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ConfigurationLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ConfigurationLayoutPanel.Controls.Add(this.McWsLabel, 0, 4);
            this.ConfigurationLayoutPanel.Controls.Add(this.McWsUrlTextBox, 1, 4);
            this.ConfigurationLayoutPanel.Controls.Add(this.McWsPasswordTextBox, 1, 3);
            this.ConfigurationLayoutPanel.Controls.Add(this.McWsPassword, 0, 3);
            this.ConfigurationLayoutPanel.Controls.Add(this.HeaderTextBox, 0, 0);
            this.ConfigurationLayoutPanel.Controls.Add(this.McAccessKeyLabel, 0, 1);
            this.ConfigurationLayoutPanel.Controls.Add(this.McAccessKeyTextBox, 1, 1);
            this.ConfigurationLayoutPanel.Controls.Add(this.McWsUsernameLabel, 0, 2);
            this.ConfigurationLayoutPanel.Controls.Add(this.McWsUsernameTextBox, 1, 2);
            this.ConfigurationLayoutPanel.Controls.Add(this.CloseButton, 1, 5);
            this.ConfigurationLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfigurationLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ConfigurationLayoutPanel.Name = "ConfigurationLayoutPanel";
            this.ConfigurationLayoutPanel.RowCount = 6;
            this.ConfigurationLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ConfigurationLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ConfigurationLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ConfigurationLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ConfigurationLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ConfigurationLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ConfigurationLayoutPanel.Size = new System.Drawing.Size(451, 191);
            this.ConfigurationLayoutPanel.TabIndex = 0;
            // 
            // McWsLabel
            // 
            this.McWsLabel.AutoSize = true;
            this.McWsLabel.Location = new System.Drawing.Point(3, 135);
            this.McWsLabel.Name = "McWsLabel";
            this.McWsLabel.Size = new System.Drawing.Size(137, 13);
            this.McWsLabel.TabIndex = 7;
            this.McWsLabel.Text = "Web Service (MCWS) URL";
            // 
            // McWsUrlTextBox
            // 
            this.McWsUrlTextBox.Location = new System.Drawing.Point(146, 138);
            this.McWsUrlTextBox.Name = "McWsUrlTextBox";
            this.McWsUrlTextBox.Size = new System.Drawing.Size(300, 20);
            this.McWsUrlTextBox.TabIndex = 8;
            // 
            // McWsPasswordTextBox
            // 
            this.McWsPasswordTextBox.Location = new System.Drawing.Point(146, 113);
            this.McWsPasswordTextBox.Name = "McWsPasswordTextBox";
            this.McWsPasswordTextBox.Size = new System.Drawing.Size(150, 20);
            this.McWsPasswordTextBox.TabIndex = 6;
            // 
            // McWsPassword
            // 
            this.McWsPassword.AutoSize = true;
            this.McWsPassword.Location = new System.Drawing.Point(3, 110);
            this.McWsPassword.Name = "McWsPassword";
            this.McWsPassword.Size = new System.Drawing.Size(124, 13);
            this.McWsPassword.TabIndex = 5;
            this.McWsPassword.Text = "Authentication Password";
            // 
            // HeaderTextBox
            // 
            this.HeaderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConfigurationLayoutPanel.SetColumnSpan(this.HeaderTextBox, 2);
            this.HeaderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderTextBox.Location = new System.Drawing.Point(3, 3);
            this.HeaderTextBox.Multiline = true;
            this.HeaderTextBox.Name = "HeaderTextBox";
            this.HeaderTextBox.ReadOnly = true;
            this.HeaderTextBox.Size = new System.Drawing.Size(445, 54);
            this.HeaderTextBox.TabIndex = 0;
            this.HeaderTextBox.TabStop = false;
            this.HeaderTextBox.Text = "Configuration";
            // 
            // McAccessKeyLabel
            // 
            this.McAccessKeyLabel.AutoSize = true;
            this.McAccessKeyLabel.Location = new System.Drawing.Point(3, 60);
            this.McAccessKeyLabel.Name = "McAccessKeyLabel";
            this.McAccessKeyLabel.Size = new System.Drawing.Size(63, 13);
            this.McAccessKeyLabel.TabIndex = 1;
            this.McAccessKeyLabel.Text = "Access Key";
            // 
            // McAccessKeyTextBox
            // 
            this.McAccessKeyTextBox.Location = new System.Drawing.Point(146, 63);
            this.McAccessKeyTextBox.Name = "McAccessKeyTextBox";
            this.McAccessKeyTextBox.Size = new System.Drawing.Size(150, 20);
            this.McAccessKeyTextBox.TabIndex = 2;
            // 
            // McWsUsernameLabel
            // 
            this.McWsUsernameLabel.AutoSize = true;
            this.McWsUsernameLabel.Location = new System.Drawing.Point(3, 85);
            this.McWsUsernameLabel.Name = "McWsUsernameLabel";
            this.McWsUsernameLabel.Size = new System.Drawing.Size(126, 13);
            this.McWsUsernameLabel.TabIndex = 3;
            this.McWsUsernameLabel.Text = "Authentication Username";
            // 
            // McWsUsernameTextBox
            // 
            this.McWsUsernameTextBox.Location = new System.Drawing.Point(146, 88);
            this.McWsUsernameTextBox.Name = "McWsUsernameTextBox";
            this.McWsUsernameTextBox.Size = new System.Drawing.Size(150, 20);
            this.McWsUsernameTextBox.TabIndex = 4;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(373, 165);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 9;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(451, 191);
            this.Controls.Add(this.ConfigurationPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationForm_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.ConfigurationPanel.ResumeLayout(false);
            this.ConfigurationPanel.PerformLayout();
            this.ConfigurationLayoutPanel.ResumeLayout(false);
            this.ConfigurationLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ConfigurationPanel;
        private System.Windows.Forms.TableLayoutPanel ConfigurationLayoutPanel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TextBox HeaderTextBox;
        private System.Windows.Forms.TextBox McWsPasswordTextBox;
        private System.Windows.Forms.Label McWsPassword;
        private System.Windows.Forms.Label McAccessKeyLabel;
        private System.Windows.Forms.TextBox McAccessKeyTextBox;
        private System.Windows.Forms.Label McWsUsernameLabel;
        private System.Windows.Forms.TextBox McWsUsernameTextBox;
        private System.Windows.Forms.Label McWsLabel;
        private System.Windows.Forms.TextBox McWsUrlTextBox;
    }
}