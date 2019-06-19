
namespace MediaCenter.LyricsFinder
{

    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LyricsFinderCore = new MediaCenter.LyricsFinder.LyricsFinderCore();
            this.RotButton = new System.Windows.Forms.Button();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.InitTimer = new System.Windows.Forms.Timer(this.components);
            this.TablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TablePanel
            // 
            this.TablePanel.ColumnCount = 2;
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.TablePanel.Controls.Add(this.ExitButton, 1, 1);
            this.TablePanel.Controls.Add(this.LyricsFinderCore, 0, 0);
            this.TablePanel.Controls.Add(this.RotButton, 0, 1);
            this.TablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TablePanel.Location = new System.Drawing.Point(0, 0);
            this.TablePanel.Name = "TablePanel";
            this.TablePanel.RowCount = 2;
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.TablePanel.Size = new System.Drawing.Size(984, 761);
            this.TablePanel.TabIndex = 0;
            // 
            // ExitButton
            // 
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(900, 728);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(5);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "&Exit";
            this.MainToolTip.SetToolTip(this.ExitButton, "Exit the stand-alone LyricsFinder");
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // LyricsFinderCore
            // 
            this.TablePanel.SetColumnSpan(this.LyricsFinderCore, 2);
            this.LyricsFinderCore.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::MediaCenter.LyricsFinder.Properties.Settings.Default, "MainFormLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LyricsFinderCore.DataDirectory = null;
            this.LyricsFinderCore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LyricsFinderCore.IsDataChanged = false;
            this.LyricsFinderCore.Location = global::MediaCenter.LyricsFinder.Properties.Settings.Default.MainFormLocation;
            this.LyricsFinderCore.Name = "LyricsFinderCore";
            this.LyricsFinderCore.Size = new System.Drawing.Size(978, 717);
            this.LyricsFinderCore.TabIndex = 2;
            this.MainToolTip.SetToolTip(this.LyricsFinderCore, "Start lyrics search for all playlist items");
            // 
            // RotButton
            // 
            this.RotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RotButton.Location = new System.Drawing.Point(790, 728);
            this.RotButton.Margin = new System.Windows.Forms.Padding(5);
            this.RotButton.Name = "RotButton";
            this.RotButton.Size = new System.Drawing.Size(100, 24);
            this.RotButton.TabIndex = 3;
            this.RotButton.Text = "&Examine the ROT";
            this.MainToolTip.SetToolTip(this.RotButton, resources.GetString("RotButton.ToolTip"));
            this.RotButton.UseVisualStyleBackColor = true;
            this.RotButton.Click += new System.EventHandler(this.RotButton_Click);
            // 
            // MainToolTip
            // 
            this.MainToolTip.AutomaticDelay = 200;
            this.MainToolTip.AutoPopDelay = 10000;
            this.MainToolTip.InitialDelay = 200;
            this.MainToolTip.ReshowDelay = 40;
            // 
            // InitTimer
            // 
            this.InitTimer.Tick += new System.EventHandler(this.InitTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.TablePanel);
            this.MinimumSize = new System.Drawing.Size(750, 300);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Lyrics finder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.TablePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TablePanel;
        private System.Windows.Forms.Button ExitButton;
        private MediaCenter.LyricsFinder.LyricsFinderCore LyricsFinderCore;
        private System.Windows.Forms.Button RotButton;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.Timer InitTimer;
    }

}

