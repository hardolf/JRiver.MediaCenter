﻿namespace MediaCenter.LyricsFinder.Forms
{
    partial class ErrorForm
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
            this.ErrorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ErrorPanel = new System.Windows.Forms.Panel();
            this.ErrorLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CopyToClipboardButton = new System.Windows.Forms.Button();
            this.ErrorTextBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ErrorPanel.SuspendLayout();
            this.ErrorLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ErrorPanel
            // 
            this.ErrorPanel.AutoSize = true;
            this.ErrorPanel.Controls.Add(this.ErrorLayoutPanel);
            this.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorPanel.Location = new System.Drawing.Point(0, 0);
            this.ErrorPanel.Name = "ErrorPanel";
            this.ErrorPanel.Size = new System.Drawing.Size(384, 211);
            this.ErrorPanel.TabIndex = 0;
            // 
            // ErrorLayoutPanel
            // 
            this.ErrorLayoutPanel.AutoSize = true;
            this.ErrorLayoutPanel.ColumnCount = 2;
            this.ErrorLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ErrorLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ErrorLayoutPanel.Controls.Add(this.CopyToClipboardButton, 0, 1);
            this.ErrorLayoutPanel.Controls.Add(this.ErrorTextBox, 0, 0);
            this.ErrorLayoutPanel.Controls.Add(this.CloseButton, 1, 1);
            this.ErrorLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ErrorLayoutPanel.Name = "ErrorLayoutPanel";
            this.ErrorLayoutPanel.RowCount = 2;
            this.ErrorLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ErrorLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ErrorLayoutPanel.Size = new System.Drawing.Size(384, 211);
            this.ErrorLayoutPanel.TabIndex = 4;
            // 
            // CopyToClipboardButton
            // 
            this.CopyToClipboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CopyToClipboardButton.Location = new System.Drawing.Point(7, 181);
            this.CopyToClipboardButton.Margin = new System.Windows.Forms.Padding(7, 3, 7, 7);
            this.CopyToClipboardButton.Name = "CopyToClipboardButton";
            this.CopyToClipboardButton.Size = new System.Drawing.Size(103, 23);
            this.CopyToClipboardButton.TabIndex = 2;
            this.CopyToClipboardButton.Text = "C&opy to clipboard";
            this.CopyToClipboardButton.UseVisualStyleBackColor = true;
            this.CopyToClipboardButton.Click += new System.EventHandler(this.CopyToClipboardButton_ClickAsync);
            // 
            // ErrorTextBox
            // 
            this.ErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ErrorLayoutPanel.SetColumnSpan(this.ErrorTextBox, 2);
            this.ErrorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorTextBox.Location = new System.Drawing.Point(7, 10);
            this.ErrorTextBox.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            this.ErrorTextBox.Multiline = true;
            this.ErrorTextBox.Name = "ErrorTextBox";
            this.ErrorTextBox.ReadOnly = true;
            this.ErrorTextBox.Size = new System.Drawing.Size(370, 151);
            this.ErrorTextBox.TabIndex = 1;
            this.ErrorTextBox.WordWrap = false;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(302, 181);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(7, 3, 7, 7);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // ErrorForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.ErrorPanel);
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "ErrorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.ErrorPanel.ResumeLayout(false);
            this.ErrorPanel.PerformLayout();
            this.ErrorLayoutPanel.ResumeLayout(false);
            this.ErrorLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip ErrorToolTip;
        private System.Windows.Forms.Panel ErrorPanel;
        private System.Windows.Forms.TableLayoutPanel ErrorLayoutPanel;
        private System.Windows.Forms.Button CopyToClipboardButton;
        private System.Windows.Forms.TextBox ErrorTextBox;
        private System.Windows.Forms.Button CloseButton;
    }
}