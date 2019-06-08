﻿namespace MediaCenter.LyricsFinder
{
    partial class OptionForm
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
            this.OptionPanel = new System.Windows.Forms.Panel();
            this.OptionLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
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
            this.UpdateCheckLabel = new System.Windows.Forms.Label();
            this.UpdateCheckIntervalUpDown = new System.Windows.Forms.NumericUpDown();
            this.OptionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.OptionPanel.SuspendLayout();
            this.OptionLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateCheckIntervalUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OptionPanel
            // 
            this.OptionPanel.AutoSize = true;
            this.OptionPanel.Controls.Add(this.OptionLayoutPanel);
            this.OptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionPanel.Location = new System.Drawing.Point(0, 0);
            this.OptionPanel.Name = "OptionPanel";
            this.OptionPanel.Size = new System.Drawing.Size(451, 221);
            this.OptionPanel.TabIndex = 0;
            // 
            // OptionLayoutPanel
            // 
            this.OptionLayoutPanel.AutoSize = true;
            this.OptionLayoutPanel.ColumnCount = 2;
            this.OptionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.OptionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.OptionLayoutPanel.Controls.Add(this.McWsLabel, 0, 5);
            this.OptionLayoutPanel.Controls.Add(this.McWsUrlTextBox, 1, 5);
            this.OptionLayoutPanel.Controls.Add(this.McWsPasswordTextBox, 1, 4);
            this.OptionLayoutPanel.Controls.Add(this.McWsPassword, 0, 4);
            this.OptionLayoutPanel.Controls.Add(this.HeaderTextBox, 0, 0);
            this.OptionLayoutPanel.Controls.Add(this.McAccessKeyLabel, 0, 2);
            this.OptionLayoutPanel.Controls.Add(this.McAccessKeyTextBox, 1, 2);
            this.OptionLayoutPanel.Controls.Add(this.McWsUsernameLabel, 0, 3);
            this.OptionLayoutPanel.Controls.Add(this.McWsUsernameTextBox, 1, 3);
            this.OptionLayoutPanel.Controls.Add(this.CloseButton, 1, 6);
            this.OptionLayoutPanel.Controls.Add(this.UpdateCheckLabel, 0, 1);
            this.OptionLayoutPanel.Controls.Add(this.UpdateCheckIntervalUpDown, 1, 1);
            this.OptionLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.OptionLayoutPanel.Name = "OptionLayoutPanel";
            this.OptionLayoutPanel.RowCount = 7;
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.OptionLayoutPanel.Size = new System.Drawing.Size(451, 221);
            this.OptionLayoutPanel.TabIndex = 0;
            // 
            // McWsLabel
            // 
            this.McWsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McWsLabel.AutoSize = true;
            this.McWsLabel.Location = new System.Drawing.Point(3, 160);
            this.McWsLabel.Name = "McWsLabel";
            this.McWsLabel.Size = new System.Drawing.Size(137, 25);
            this.McWsLabel.TabIndex = 9;
            this.McWsLabel.Text = "Web Service (MCWS) URL";
            this.McWsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McWsUrlTextBox
            // 
            this.McWsUrlTextBox.Location = new System.Drawing.Point(157, 163);
            this.McWsUrlTextBox.Name = "McWsUrlTextBox";
            this.McWsUrlTextBox.Size = new System.Drawing.Size(291, 20);
            this.McWsUrlTextBox.TabIndex = 10;
            this.OptionToolTip.SetToolTip(this.McWsUrlTextBox, "MCWS web service URL from Media Center Options window > Media Network tab");
            // 
            // McWsPasswordTextBox
            // 
            this.McWsPasswordTextBox.Location = new System.Drawing.Point(157, 138);
            this.McWsPasswordTextBox.Name = "McWsPasswordTextBox";
            this.McWsPasswordTextBox.Size = new System.Drawing.Size(150, 20);
            this.McWsPasswordTextBox.TabIndex = 8;
            this.OptionToolTip.SetToolTip(this.McWsPasswordTextBox, "Password from Media Center Options window > Media Network tab");
            // 
            // McWsPassword
            // 
            this.McWsPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McWsPassword.AutoSize = true;
            this.McWsPassword.Location = new System.Drawing.Point(3, 135);
            this.McWsPassword.Name = "McWsPassword";
            this.McWsPassword.Size = new System.Drawing.Size(124, 25);
            this.McWsPassword.TabIndex = 7;
            this.McWsPassword.Text = "Authentication Password";
            this.McWsPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HeaderTextBox
            // 
            this.HeaderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OptionLayoutPanel.SetColumnSpan(this.HeaderTextBox, 2);
            this.HeaderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderTextBox.Location = new System.Drawing.Point(3, 3);
            this.HeaderTextBox.Multiline = true;
            this.HeaderTextBox.Name = "HeaderTextBox";
            this.HeaderTextBox.ReadOnly = true;
            this.HeaderTextBox.Size = new System.Drawing.Size(445, 54);
            this.HeaderTextBox.TabIndex = 0;
            this.HeaderTextBox.TabStop = false;
            this.HeaderTextBox.Text = "Options";
            this.HeaderTextBox.WordWrap = false;
            // 
            // McAccessKeyLabel
            // 
            this.McAccessKeyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McAccessKeyLabel.AutoSize = true;
            this.McAccessKeyLabel.Location = new System.Drawing.Point(3, 85);
            this.McAccessKeyLabel.Name = "McAccessKeyLabel";
            this.McAccessKeyLabel.Size = new System.Drawing.Size(63, 25);
            this.McAccessKeyLabel.TabIndex = 3;
            this.McAccessKeyLabel.Text = "Access Key";
            this.McAccessKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McAccessKeyTextBox
            // 
            this.McAccessKeyTextBox.Location = new System.Drawing.Point(157, 88);
            this.McAccessKeyTextBox.Name = "McAccessKeyTextBox";
            this.McAccessKeyTextBox.Size = new System.Drawing.Size(150, 20);
            this.McAccessKeyTextBox.TabIndex = 4;
            this.OptionToolTip.SetToolTip(this.McAccessKeyTextBox, "Media Network Access Key from Media Center Options window > Media Network tab");
            // 
            // McWsUsernameLabel
            // 
            this.McWsUsernameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McWsUsernameLabel.AutoSize = true;
            this.McWsUsernameLabel.Location = new System.Drawing.Point(3, 110);
            this.McWsUsernameLabel.Name = "McWsUsernameLabel";
            this.McWsUsernameLabel.Size = new System.Drawing.Size(126, 25);
            this.McWsUsernameLabel.TabIndex = 5;
            this.McWsUsernameLabel.Text = "Authentication Username";
            this.McWsUsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McWsUsernameTextBox
            // 
            this.McWsUsernameTextBox.Location = new System.Drawing.Point(157, 113);
            this.McWsUsernameTextBox.Name = "McWsUsernameTextBox";
            this.McWsUsernameTextBox.Size = new System.Drawing.Size(150, 20);
            this.McWsUsernameTextBox.TabIndex = 6;
            this.OptionToolTip.SetToolTip(this.McWsUsernameTextBox, "Username from Media Center Options window > Media Network tab");
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(373, 195);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 11;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // UpdateCheckLabel
            // 
            this.UpdateCheckLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.UpdateCheckLabel.AutoSize = true;
            this.UpdateCheckLabel.Location = new System.Drawing.Point(3, 60);
            this.UpdateCheckLabel.Name = "UpdateCheckLabel";
            this.UpdateCheckLabel.Size = new System.Drawing.Size(148, 25);
            this.UpdateCheckLabel.TabIndex = 1;
            this.UpdateCheckLabel.Text = "Update check interval in days";
            this.UpdateCheckLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UpdateCheckIntervalUpDown
            // 
            this.UpdateCheckIntervalUpDown.Location = new System.Drawing.Point(157, 63);
            this.UpdateCheckIntervalUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.UpdateCheckIntervalUpDown.Name = "UpdateCheckIntervalUpDown";
            this.UpdateCheckIntervalUpDown.Size = new System.Drawing.Size(80, 20);
            this.UpdateCheckIntervalUpDown.TabIndex = 2;
            this.OptionToolTip.SetToolTip(this.UpdateCheckIntervalUpDown, "> 0: days between check for updates\r\n0: Check for updates each start\r\n< 0: disabl" +
        "e check for updates\r\n");
            // 
            // OptionForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(451, 221);
            this.Controls.Add(this.OptionPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionForm_Load);
            this.OptionPanel.ResumeLayout(false);
            this.OptionPanel.PerformLayout();
            this.OptionLayoutPanel.ResumeLayout(false);
            this.OptionLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateCheckIntervalUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel OptionPanel;
        private System.Windows.Forms.TableLayoutPanel OptionLayoutPanel;
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
        private System.Windows.Forms.ToolTip OptionToolTip;
        private System.Windows.Forms.Label UpdateCheckLabel;
        private System.Windows.Forms.NumericUpDown UpdateCheckIntervalUpDown;
    }
}