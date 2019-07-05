namespace MediaCenter.LyricsFinder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionForm));
            this.OptionPanel = new System.Windows.Forms.Panel();
            this.OptionLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.NoLyricsSearchFilterLabel = new System.Windows.Forms.Label();
            this.HeaderTextBox = new System.Windows.Forms.TextBox();
            this.McAccessKeyLabel = new System.Windows.Forms.Label();
            this.McWsUsernameLabel = new System.Windows.Forms.Label();
            this.McWsPassword = new System.Windows.Forms.Label();
            this.McWsLabel = new System.Windows.Forms.Label();
            this.McAccessKeyTextBox = new System.Windows.Forms.TextBox();
            this.McWsUsernameTextBox = new System.Windows.Forms.TextBox();
            this.McWsPasswordTextBox = new System.Windows.Forms.TextBox();
            this.McWsUrlTextBox = new System.Windows.Forms.TextBox();
            this.UpdateCheckIntervalDaysUpDown = new System.Windows.Forms.NumericUpDown();
            this.LastUpdateCheckTextBox = new System.Windows.Forms.TextBox();
            this.NoLyricsSearchFilterTextBox = new System.Windows.Forms.TextBox();
            this.MouseMoveOpenLyricsFormCheckBox = new System.Windows.Forms.CheckBox();
            this.MaxMcWsConnectAttemptsUpDown = new System.Windows.Forms.NumericUpDown();
            this.CloseButton = new System.Windows.Forms.Button();
            this.MaxQueueLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxQueueLengthLabel = new System.Windows.Forms.Label();
            this.MouseMoveOpenLyricsFormLabel = new System.Windows.Forms.Label();
            this.LastUpdateCheckLabel = new System.Windows.Forms.Label();
            this.UpdateCheckIntervalDaysLabel = new System.Windows.Forms.Label();
            this.MaxMcWsConnectAttemptsLabel = new System.Windows.Forms.Label();
            this.DelayMilliSecondsBetweenSearchesLabel = new System.Windows.Forms.Label();
            this.DelayMilliSecondsBetweenSearchesUpDown = new System.Windows.Forms.NumericUpDown();
            this.OptionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.OptionPanel.SuspendLayout();
            this.OptionLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateCheckIntervalDaysUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxMcWsConnectAttemptsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxQueueLengthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayMilliSecondsBetweenSearchesUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OptionPanel
            // 
            this.OptionPanel.AutoSize = true;
            this.OptionPanel.Controls.Add(this.OptionLayoutPanel);
            this.OptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionPanel.Location = new System.Drawing.Point(0, 0);
            this.OptionPanel.Name = "OptionPanel";
            this.OptionPanel.Size = new System.Drawing.Size(451, 364);
            this.OptionPanel.TabIndex = 0;
            // 
            // OptionLayoutPanel
            // 
            this.OptionLayoutPanel.AutoSize = true;
            this.OptionLayoutPanel.ColumnCount = 2;
            this.OptionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.OptionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.OptionLayoutPanel.Controls.Add(this.NoLyricsSearchFilterLabel, 0, 10);
            this.OptionLayoutPanel.Controls.Add(this.HeaderTextBox, 0, 0);
            this.OptionLayoutPanel.Controls.Add(this.McAccessKeyLabel, 0, 1);
            this.OptionLayoutPanel.Controls.Add(this.McWsUsernameLabel, 0, 2);
            this.OptionLayoutPanel.Controls.Add(this.McWsPassword, 0, 3);
            this.OptionLayoutPanel.Controls.Add(this.McWsLabel, 0, 4);
            this.OptionLayoutPanel.Controls.Add(this.McAccessKeyTextBox, 1, 1);
            this.OptionLayoutPanel.Controls.Add(this.McWsUsernameTextBox, 1, 2);
            this.OptionLayoutPanel.Controls.Add(this.McWsPasswordTextBox, 1, 3);
            this.OptionLayoutPanel.Controls.Add(this.McWsUrlTextBox, 1, 4);
            this.OptionLayoutPanel.Controls.Add(this.UpdateCheckIntervalDaysUpDown, 1, 7);
            this.OptionLayoutPanel.Controls.Add(this.LastUpdateCheckTextBox, 1, 9);
            this.OptionLayoutPanel.Controls.Add(this.NoLyricsSearchFilterTextBox, 1, 10);
            this.OptionLayoutPanel.Controls.Add(this.MouseMoveOpenLyricsFormCheckBox, 1, 11);
            this.OptionLayoutPanel.Controls.Add(this.MaxMcWsConnectAttemptsUpDown, 1, 5);
            this.OptionLayoutPanel.Controls.Add(this.CloseButton, 1, 12);
            this.OptionLayoutPanel.Controls.Add(this.MaxQueueLengthUpDown, 1, 6);
            this.OptionLayoutPanel.Controls.Add(this.MaxQueueLengthLabel, 0, 6);
            this.OptionLayoutPanel.Controls.Add(this.MouseMoveOpenLyricsFormLabel, 0, 11);
            this.OptionLayoutPanel.Controls.Add(this.LastUpdateCheckLabel, 0, 9);
            this.OptionLayoutPanel.Controls.Add(this.UpdateCheckIntervalDaysLabel, 0, 7);
            this.OptionLayoutPanel.Controls.Add(this.MaxMcWsConnectAttemptsLabel, 0, 5);
            this.OptionLayoutPanel.Controls.Add(this.DelayMilliSecondsBetweenSearchesLabel, 0, 8);
            this.OptionLayoutPanel.Controls.Add(this.DelayMilliSecondsBetweenSearchesUpDown, 1, 8);
            this.OptionLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.OptionLayoutPanel.Name = "OptionLayoutPanel";
            this.OptionLayoutPanel.RowCount = 13;
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.OptionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.OptionLayoutPanel.Size = new System.Drawing.Size(451, 364);
            this.OptionLayoutPanel.TabIndex = 0;
            // 
            // NoLyricsSearchFilterLabel
            // 
            this.NoLyricsSearchFilterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.NoLyricsSearchFilterLabel.AutoSize = true;
            this.NoLyricsSearchFilterLabel.Location = new System.Drawing.Point(3, 285);
            this.NoLyricsSearchFilterLabel.Name = "NoLyricsSearchFilterLabel";
            this.NoLyricsSearchFilterLabel.Size = new System.Drawing.Size(104, 25);
            this.NoLyricsSearchFilterLabel.TabIndex = 19;
            this.NoLyricsSearchFilterLabel.Text = "No lyrics search filter";
            this.NoLyricsSearchFilterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.McAccessKeyLabel.Location = new System.Drawing.Point(3, 60);
            this.McAccessKeyLabel.Name = "McAccessKeyLabel";
            this.McAccessKeyLabel.Size = new System.Drawing.Size(63, 25);
            this.McAccessKeyLabel.TabIndex = 1;
            this.McAccessKeyLabel.Text = "Access Key";
            this.McAccessKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McWsUsernameLabel
            // 
            this.McWsUsernameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McWsUsernameLabel.AutoSize = true;
            this.McWsUsernameLabel.Location = new System.Drawing.Point(3, 85);
            this.McWsUsernameLabel.Name = "McWsUsernameLabel";
            this.McWsUsernameLabel.Size = new System.Drawing.Size(126, 25);
            this.McWsUsernameLabel.TabIndex = 3;
            this.McWsUsernameLabel.Text = "Authentication Username";
            this.McWsUsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McWsPassword
            // 
            this.McWsPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McWsPassword.AutoSize = true;
            this.McWsPassword.Location = new System.Drawing.Point(3, 110);
            this.McWsPassword.Name = "McWsPassword";
            this.McWsPassword.Size = new System.Drawing.Size(124, 25);
            this.McWsPassword.TabIndex = 5;
            this.McWsPassword.Text = "Authentication Password";
            this.McWsPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McWsLabel
            // 
            this.McWsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.McWsLabel.AutoSize = true;
            this.McWsLabel.Location = new System.Drawing.Point(3, 135);
            this.McWsLabel.Name = "McWsLabel";
            this.McWsLabel.Size = new System.Drawing.Size(137, 25);
            this.McWsLabel.TabIndex = 7;
            this.McWsLabel.Text = "Web Service (MCWS) URL";
            this.McWsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // McAccessKeyTextBox
            // 
            this.McAccessKeyTextBox.Location = new System.Drawing.Point(159, 63);
            this.McAccessKeyTextBox.Name = "McAccessKeyTextBox";
            this.McAccessKeyTextBox.Size = new System.Drawing.Size(150, 20);
            this.McAccessKeyTextBox.TabIndex = 2;
            this.OptionToolTip.SetToolTip(this.McAccessKeyTextBox, "Media Network Access Key from Media Center Options window > Media Network tab");
            this.McAccessKeyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_ValidatingAsync);
            // 
            // McWsUsernameTextBox
            // 
            this.McWsUsernameTextBox.Location = new System.Drawing.Point(159, 88);
            this.McWsUsernameTextBox.Name = "McWsUsernameTextBox";
            this.McWsUsernameTextBox.Size = new System.Drawing.Size(150, 20);
            this.McWsUsernameTextBox.TabIndex = 4;
            this.OptionToolTip.SetToolTip(this.McWsUsernameTextBox, "Username from Media Center Options window > Media Network tab");
            this.McWsUsernameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_ValidatingAsync);
            // 
            // McWsPasswordTextBox
            // 
            this.McWsPasswordTextBox.Location = new System.Drawing.Point(159, 113);
            this.McWsPasswordTextBox.Name = "McWsPasswordTextBox";
            this.McWsPasswordTextBox.Size = new System.Drawing.Size(150, 20);
            this.McWsPasswordTextBox.TabIndex = 6;
            this.OptionToolTip.SetToolTip(this.McWsPasswordTextBox, "Password from Media Center Options window > Media Network tab");
            this.McWsPasswordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_ValidatingAsync);
            // 
            // McWsUrlTextBox
            // 
            this.McWsUrlTextBox.Location = new System.Drawing.Point(159, 138);
            this.McWsUrlTextBox.Name = "McWsUrlTextBox";
            this.McWsUrlTextBox.Size = new System.Drawing.Size(289, 20);
            this.McWsUrlTextBox.TabIndex = 8;
            this.OptionToolTip.SetToolTip(this.McWsUrlTextBox, "MCWS web service URL from Media Center Options window > Media Network tab");
            this.McWsUrlTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_ValidatingAsync);
            // 
            // UpdateCheckIntervalDaysUpDown
            // 
            this.UpdateCheckIntervalDaysUpDown.Location = new System.Drawing.Point(159, 213);
            this.UpdateCheckIntervalDaysUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.UpdateCheckIntervalDaysUpDown.Name = "UpdateCheckIntervalDaysUpDown";
            this.UpdateCheckIntervalDaysUpDown.Size = new System.Drawing.Size(80, 20);
            this.UpdateCheckIntervalDaysUpDown.TabIndex = 14;
            this.OptionToolTip.SetToolTip(this.UpdateCheckIntervalDaysUpDown, "> 0: days between check for updates\r\n0: Check for updates each start\r\n< 0: disabl" +
        "e check for updates\r\n");
            // 
            // LastUpdateCheckTextBox
            // 
            this.LastUpdateCheckTextBox.Location = new System.Drawing.Point(159, 263);
            this.LastUpdateCheckTextBox.Name = "LastUpdateCheckTextBox";
            this.LastUpdateCheckTextBox.ReadOnly = true;
            this.LastUpdateCheckTextBox.Size = new System.Drawing.Size(150, 20);
            this.LastUpdateCheckTextBox.TabIndex = 18;
            this.LastUpdateCheckTextBox.TabStop = false;
            this.OptionToolTip.SetToolTip(this.LastUpdateCheckTextBox, "Date of the last check for new updates");
            // 
            // NoLyricsSearchFilterTextBox
            // 
            this.NoLyricsSearchFilterTextBox.Location = new System.Drawing.Point(159, 288);
            this.NoLyricsSearchFilterTextBox.Name = "NoLyricsSearchFilterTextBox";
            this.NoLyricsSearchFilterTextBox.Size = new System.Drawing.Size(289, 20);
            this.NoLyricsSearchFilterTextBox.TabIndex = 20;
            this.OptionToolTip.SetToolTip(this.NoLyricsSearchFilterTextBox, resources.GetString("NoLyricsSearchFilterTextBox.ToolTip"));
            this.NoLyricsSearchFilterTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_ValidatingAsync);
            // 
            // MouseMoveOpenLyricsFormCheckBox
            // 
            this.MouseMoveOpenLyricsFormCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MouseMoveOpenLyricsFormCheckBox.AutoSize = true;
            this.MouseMoveOpenLyricsFormCheckBox.Location = new System.Drawing.Point(159, 313);
            this.MouseMoveOpenLyricsFormCheckBox.Name = "MouseMoveOpenLyricsFormCheckBox";
            this.MouseMoveOpenLyricsFormCheckBox.Size = new System.Drawing.Size(15, 19);
            this.MouseMoveOpenLyricsFormCheckBox.TabIndex = 22;
            this.OptionToolTip.SetToolTip(this.MouseMoveOpenLyricsFormCheckBox, "Check if a mouse move in the lyrics column \r\nshould open the lyrics form automati" +
        "cally");
            this.MouseMoveOpenLyricsFormCheckBox.UseVisualStyleBackColor = true;
            // 
            // MaxMcWsConnectAttemptsUpDown
            // 
            this.MaxMcWsConnectAttemptsUpDown.Location = new System.Drawing.Point(159, 163);
            this.MaxMcWsConnectAttemptsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxMcWsConnectAttemptsUpDown.Name = "MaxMcWsConnectAttemptsUpDown";
            this.MaxMcWsConnectAttemptsUpDown.Size = new System.Drawing.Size(80, 20);
            this.MaxMcWsConnectAttemptsUpDown.TabIndex = 10;
            this.OptionToolTip.SetToolTip(this.MaxMcWsConnectAttemptsUpDown, "The maximum attempts to connect with the MCWS");
            this.MaxMcWsConnectAttemptsUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(373, 339);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 23;
            this.CloseButton.Text = "&Close (Esc)";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_ClickAsync);
            // 
            // MaxQueueLengthUpDown
            // 
            this.MaxQueueLengthUpDown.Location = new System.Drawing.Point(159, 188);
            this.MaxQueueLengthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxQueueLengthUpDown.Name = "MaxQueueLengthUpDown";
            this.MaxQueueLengthUpDown.Size = new System.Drawing.Size(80, 20);
            this.MaxQueueLengthUpDown.TabIndex = 12;
            this.OptionToolTip.SetToolTip(this.MaxQueueLengthUpDown, "Maximum number of items/songs to be queued \r\nand processed concurrently during th" +
        "e automatic search.\r\nAdjust this number to the number of virtual CPU cores\r\n(thr" +
        "eads) you wish to be used concurrently.");
            this.MaxQueueLengthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MaxQueueLengthLabel
            // 
            this.MaxQueueLengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaxQueueLengthLabel.AutoSize = true;
            this.MaxQueueLengthLabel.Location = new System.Drawing.Point(3, 185);
            this.MaxQueueLengthLabel.Name = "MaxQueueLengthLabel";
            this.MaxQueueLengthLabel.Size = new System.Drawing.Size(116, 25);
            this.MaxQueueLengthLabel.TabIndex = 11;
            this.MaxQueueLengthLabel.Text = "Maximum queue length";
            this.MaxQueueLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MouseMoveOpenLyricsFormLabel
            // 
            this.MouseMoveOpenLyricsFormLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MouseMoveOpenLyricsFormLabel.AutoSize = true;
            this.MouseMoveOpenLyricsFormLabel.Location = new System.Drawing.Point(3, 310);
            this.MouseMoveOpenLyricsFormLabel.Name = "MouseMoveOpenLyricsFormLabel";
            this.MouseMoveOpenLyricsFormLabel.Size = new System.Drawing.Size(147, 25);
            this.MouseMoveOpenLyricsFormLabel.TabIndex = 21;
            this.MouseMoveOpenLyricsFormLabel.Text = "Mouse move: open lyrics form";
            this.MouseMoveOpenLyricsFormLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LastUpdateCheckLabel
            // 
            this.LastUpdateCheckLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LastUpdateCheckLabel.AutoSize = true;
            this.LastUpdateCheckLabel.Location = new System.Drawing.Point(3, 260);
            this.LastUpdateCheckLabel.Name = "LastUpdateCheckLabel";
            this.LastUpdateCheckLabel.Size = new System.Drawing.Size(96, 25);
            this.LastUpdateCheckLabel.TabIndex = 17;
            this.LastUpdateCheckLabel.Text = "Last update check";
            this.LastUpdateCheckLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UpdateCheckIntervalDaysLabel
            // 
            this.UpdateCheckIntervalDaysLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.UpdateCheckIntervalDaysLabel.AutoSize = true;
            this.UpdateCheckIntervalDaysLabel.Location = new System.Drawing.Point(3, 210);
            this.UpdateCheckIntervalDaysLabel.Name = "UpdateCheckIntervalDaysLabel";
            this.UpdateCheckIntervalDaysLabel.Size = new System.Drawing.Size(148, 25);
            this.UpdateCheckIntervalDaysLabel.TabIndex = 13;
            this.UpdateCheckIntervalDaysLabel.Text = "Update check interval in days";
            this.UpdateCheckIntervalDaysLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaxMcWsConnectAttemptsLabel
            // 
            this.MaxMcWsConnectAttemptsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaxMcWsConnectAttemptsLabel.AutoSize = true;
            this.MaxMcWsConnectAttemptsLabel.Location = new System.Drawing.Point(3, 160);
            this.MaxMcWsConnectAttemptsLabel.Name = "MaxMcWsConnectAttemptsLabel";
            this.MaxMcWsConnectAttemptsLabel.Size = new System.Drawing.Size(150, 25);
            this.MaxMcWsConnectAttemptsLabel.TabIndex = 9;
            this.MaxMcWsConnectAttemptsLabel.Text = "Maximum connection attempts";
            this.MaxMcWsConnectAttemptsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DelayMilliSecondsBetweenSearchesLabel
            // 
            this.DelayMilliSecondsBetweenSearchesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DelayMilliSecondsBetweenSearchesLabel.AutoSize = true;
            this.DelayMilliSecondsBetweenSearchesLabel.Location = new System.Drawing.Point(3, 235);
            this.DelayMilliSecondsBetweenSearchesLabel.Name = "DelayMilliSecondsBetweenSearchesLabel";
            this.DelayMilliSecondsBetweenSearchesLabel.Size = new System.Drawing.Size(124, 25);
            this.DelayMilliSecondsBetweenSearchesLabel.TabIndex = 15;
            this.DelayMilliSecondsBetweenSearchesLabel.Text = "Delay between searches";
            this.DelayMilliSecondsBetweenSearchesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DelayMilliSecondsBetweenSearchesUpDown
            // 
            this.DelayMilliSecondsBetweenSearchesUpDown.Location = new System.Drawing.Point(159, 238);
            this.DelayMilliSecondsBetweenSearchesUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.DelayMilliSecondsBetweenSearchesUpDown.Name = "DelayMilliSecondsBetweenSearchesUpDown";
            this.DelayMilliSecondsBetweenSearchesUpDown.Size = new System.Drawing.Size(80, 20);
            this.DelayMilliSecondsBetweenSearchesUpDown.TabIndex = 16;
            this.OptionToolTip.SetToolTip(this.DelayMilliSecondsBetweenSearchesUpDown, "Delay milliseconds between searches.\r\nCan be used as a countermeasure \r\nagainst s" +
        "ervice banning of your IP address.");
            // 
            // OptionToolTip
            // 
            this.OptionToolTip.AutoPopDelay = 25000;
            this.OptionToolTip.InitialDelay = 500;
            this.OptionToolTip.ReshowDelay = 100;
            // 
            // OptionForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(451, 364);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionForm_FormClosingAsync);
            this.Load += new System.EventHandler(this.OptionForm_LoadAsync);
            this.OptionPanel.ResumeLayout(false);
            this.OptionPanel.PerformLayout();
            this.OptionLayoutPanel.ResumeLayout(false);
            this.OptionLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateCheckIntervalDaysUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxMcWsConnectAttemptsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxQueueLengthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayMilliSecondsBetweenSearchesUpDown)).EndInit();
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
        private System.Windows.Forms.Label UpdateCheckIntervalDaysLabel;
        private System.Windows.Forms.NumericUpDown UpdateCheckIntervalDaysUpDown;
        private System.Windows.Forms.Label LastUpdateCheckLabel;
        private System.Windows.Forms.TextBox LastUpdateCheckTextBox;
        private System.Windows.Forms.TextBox NoLyricsSearchFilterTextBox;
        private System.Windows.Forms.Label MouseMoveOpenLyricsFormLabel;
        private System.Windows.Forms.Label MaxMcWsConnectAttemptsLabel;
        private System.Windows.Forms.NumericUpDown MaxMcWsConnectAttemptsUpDown;
        private System.Windows.Forms.CheckBox MouseMoveOpenLyricsFormCheckBox;
        private System.Windows.Forms.NumericUpDown MaxQueueLengthUpDown;
        private System.Windows.Forms.Label MaxQueueLengthLabel;
        private System.Windows.Forms.Label NoLyricsSearchFilterLabel;
        private System.Windows.Forms.Label DelayMilliSecondsBetweenSearchesLabel;
        private System.Windows.Forms.NumericUpDown DelayMilliSecondsBetweenSearchesUpDown;
    }
}