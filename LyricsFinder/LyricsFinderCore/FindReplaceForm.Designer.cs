namespace MediaCenter.LyricsFinder
{
    partial class FindReplaceForm
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
            this.FindReplaceTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ReplaceTextBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelFormButton = new System.Windows.Forms.Button();
            this.FindTextLabel = new System.Windows.Forms.Label();
            this.ReplaceTextLabel = new System.Windows.Forms.Label();
            this.FindTextBox = new System.Windows.Forms.TextBox();
            this.FindReplaceToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FindReplaceTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FindReplaceTableLayoutPanel
            // 
            this.FindReplaceTableLayoutPanel.ColumnCount = 4;
            this.FindReplaceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.FindReplaceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.FindReplaceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.FindReplaceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.FindReplaceTableLayoutPanel.Controls.Add(this.ReplaceTextBox, 1, 1);
            this.FindReplaceTableLayoutPanel.Controls.Add(this.OkButton, 2, 2);
            this.FindReplaceTableLayoutPanel.Controls.Add(this.CancelFormButton, 3, 2);
            this.FindReplaceTableLayoutPanel.Controls.Add(this.FindTextLabel, 0, 0);
            this.FindReplaceTableLayoutPanel.Controls.Add(this.ReplaceTextLabel, 0, 1);
            this.FindReplaceTableLayoutPanel.Controls.Add(this.FindTextBox, 1, 0);
            this.FindReplaceTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FindReplaceTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.FindReplaceTableLayoutPanel.Name = "FindReplaceTableLayoutPanel";
            this.FindReplaceTableLayoutPanel.RowCount = 3;
            this.FindReplaceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.FindReplaceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.FindReplaceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FindReplaceTableLayoutPanel.Size = new System.Drawing.Size(334, 81);
            this.FindReplaceTableLayoutPanel.TabIndex = 0;
            // 
            // ReplaceTextBox
            // 
            this.ReplaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindReplaceTableLayoutPanel.SetColumnSpan(this.ReplaceTextBox, 3);
            this.ReplaceTextBox.Location = new System.Drawing.Point(83, 28);
            this.ReplaceTextBox.Name = "ReplaceTextBox";
            this.ReplaceTextBox.Size = new System.Drawing.Size(248, 20);
            this.ReplaceTextBox.TabIndex = 5;
            this.FindReplaceToolTip.SetToolTip(this.ReplaceTextBox, "Text to replace the found text");
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(170, 55);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.FindReplaceToolTip.SetToolTip(this.OkButton, "Start the search/replace process (Enter)");
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_ClickAsync);
            // 
            // CancelFormButton
            // 
            this.CancelFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelFormButton.Location = new System.Drawing.Point(256, 55);
            this.CancelFormButton.Name = "CancelFormButton";
            this.CancelFormButton.Size = new System.Drawing.Size(75, 23);
            this.CancelFormButton.TabIndex = 1;
            this.CancelFormButton.Text = "Cancel";
            this.FindReplaceToolTip.SetToolTip(this.CancelFormButton, "Stop the find/replace (Escape)");
            this.CancelFormButton.UseVisualStyleBackColor = true;
            this.CancelFormButton.Click += new System.EventHandler(this.CancelFormButton_ClickAsync);
            // 
            // FindTextLabel
            // 
            this.FindTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FindTextLabel.AutoSize = true;
            this.FindTextLabel.Location = new System.Drawing.Point(3, 0);
            this.FindTextLabel.Name = "FindTextLabel";
            this.FindTextLabel.Size = new System.Drawing.Size(47, 25);
            this.FindTextLabel.TabIndex = 2;
            this.FindTextLabel.Text = "Find text";
            this.FindTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReplaceTextLabel
            // 
            this.ReplaceTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ReplaceTextLabel.AutoSize = true;
            this.ReplaceTextLabel.Location = new System.Drawing.Point(3, 25);
            this.ReplaceTextLabel.Name = "ReplaceTextLabel";
            this.ReplaceTextLabel.Size = new System.Drawing.Size(67, 25);
            this.ReplaceTextLabel.TabIndex = 3;
            this.ReplaceTextLabel.Text = "Replace text";
            this.ReplaceTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FindTextBox
            // 
            this.FindTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindReplaceTableLayoutPanel.SetColumnSpan(this.FindTextBox, 3);
            this.FindTextBox.Location = new System.Drawing.Point(83, 3);
            this.FindTextBox.Name = "FindTextBox";
            this.FindTextBox.Size = new System.Drawing.Size(248, 20);
            this.FindTextBox.TabIndex = 4;
            this.FindReplaceToolTip.SetToolTip(this.FindTextBox, "Text to search for");
            // 
            // FindReplaceForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelFormButton;
            this.ClientSize = new System.Drawing.Size(334, 81);
            this.Controls.Add(this.FindReplaceTableLayoutPanel);
            this.KeyPreview = true;
            this.Name = "FindReplaceForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Find/Replace Form";
            this.Activated += new System.EventHandler(this.FindReplaceForm_ActivatedAsync);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDownAsync);
            this.MouseEnter += new System.EventHandler(this.FindReplaceForm_MouseEnterAsync);
            this.FindReplaceTableLayoutPanel.ResumeLayout(false);
            this.FindReplaceTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button CancelFormButton;
        private System.Windows.Forms.Label FindTextLabel;
        internal System.Windows.Forms.TableLayoutPanel FindReplaceTableLayoutPanel;
        private System.Windows.Forms.ToolTip FindReplaceToolTip;
        private System.Windows.Forms.TextBox ReplaceTextBox;
        private System.Windows.Forms.TextBox FindTextBox;
        private System.Windows.Forms.Label ReplaceTextLabel;
        private System.Windows.Forms.Button OkButton;
    }
}