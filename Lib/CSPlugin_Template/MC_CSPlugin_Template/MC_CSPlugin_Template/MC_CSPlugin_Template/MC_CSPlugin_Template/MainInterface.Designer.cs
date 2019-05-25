namespace MC_CSPlugin
{    
    partial class MainInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            this.label_removable = new System.Windows.Forms.Label ( );
            this.SuspendLayout ( );
            // 
            // label_removable
            // 
            this.label_removable.AutoSize = true;
            this.label_removable.Location = new System.Drawing.Point ( 179, 227 );
            this.label_removable.Name = "label_removable";
            this.label_removable.Size = new System.Drawing.Size ( 201, 13 );
            this.label_removable.TabIndex = 0;
            this.label_removable.Text = "I am the main user interface for the plugin";
            this.label_removable.Visible = false;
            // 
            // MainInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.label_removable );
            this.Name = "MainInterface";
            this.Size = new System.Drawing.Size ( 610, 563 );
            this.ResumeLayout ( false );
            this.PerformLayout ( );

        }

        #endregion

        private System.Windows.Forms.Label label_removable;
    }
}
