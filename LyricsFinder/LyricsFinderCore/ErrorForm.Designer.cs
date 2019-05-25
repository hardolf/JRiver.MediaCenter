namespace MediaCenter.LyricsFinder
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
            this.ErrorTextBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ErrorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ErrorPanel
            // 
            this.ErrorPanel.Controls.Add(this.ErrorTextBox);
            this.ErrorPanel.Controls.Add(this.CloseButton);
            this.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorPanel.Location = new System.Drawing.Point(0, 0);
            this.ErrorPanel.Name = "ErrorPanel";
            this.ErrorPanel.Size = new System.Drawing.Size(484, 461);
            this.ErrorPanel.TabIndex = 0;
            // 
            // ErrorTextBox
            // 
            this.ErrorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ErrorTextBox.Location = new System.Drawing.Point(12, 12);
            this.ErrorTextBox.Multiline = true;
            this.ErrorTextBox.Name = "ErrorTextBox";
            this.ErrorTextBox.ReadOnly = true;
            this.ErrorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ErrorTextBox.Size = new System.Drawing.Size(460, 397);
            this.ErrorTextBox.TabIndex = 2;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(397, 426);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ErrorForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.ErrorPanel);
            this.Name = "ErrorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error";
            this.TransparencyKey = System.Drawing.Color.Teal;
            this.ErrorPanel.ResumeLayout(false);
            this.ErrorPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip ErrorToolTip;
        private System.Windows.Forms.Panel ErrorPanel;
        private System.Windows.Forms.TextBox ErrorTextBox;
        private System.Windows.Forms.Button CloseButton;
    }
}