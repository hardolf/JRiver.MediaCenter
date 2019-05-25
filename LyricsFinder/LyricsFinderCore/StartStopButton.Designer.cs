namespace MediaCenter.LyricsFinder
{

	partial class StartStopToolStripButton
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
			if (disposing)
			{
				if (components != null) components.Dispose();
                if (_imageStart != null) _imageStart.Dispose();
                if (_imageStop != null) _imageStop.Dispose();
            }

            base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			//
			// StartStopButton
			//
			this.Click += new System.EventHandler(this.StartStopButton_Click);
		}

		#endregion

	}

}
