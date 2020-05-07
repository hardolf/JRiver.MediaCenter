using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Main form for standalone LyricsFinder for JRiver Media Center.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainForm : Form
    {

        /***********************************/
        /******* Private class-wide   ******/
        /***** constants and variables *****/
        /***********************************/

        private readonly bool _isDesignTime = false;


        /************************/
        /***** Constructors *****/
        /************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // Upgrade User Settings from previous version the first time
            if (Properties.Settings.Default.UpgradeSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeSettings = false;
            }

            InitializeComponent();

            _isDesignTime = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }


        /*********************/
        /***** Delegates *****/
        /*********************/

        /// <summary>
        /// Handles the Click event of the ExitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }


        /// <summary>
        /// Handles the FormClosing event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDesignTime) return;

            try
            {
                if ((LyricsFinderCore != null) && LyricsFinderCore.IsDataChanged)
                {
                    var result = MessageBox.Show("Data is changed and will be lost if you close.\nDo you want to close anyway?"
                        , "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            e.Cancel = false;
                            break;

                        case DialogResult.No:
                            e.Cancel = true;
                            break;

                        default:
                            break;
                    }
                }

                SaveFormSettings();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }


        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_isDesignTime) return;

            try
            {
                LoadFormSettings();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }


        /// <summary>
        /// Handles the Shown event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void MainForm_ShownAsync(object sender, EventArgs e)
        {
            if (_isDesignTime) return;

            try
            {
                await LyricsFinderCore.InitCoreAsync();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the RotButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RotButton_Click(object sender, EventArgs e)
        {
            try
            {
                var rot = ExamineRot.GetRotText();

                using (var textForm = new TextForm("Running Object Table (ROT)", rot))
                {
                    textForm.ShowDialog(this); 
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }


        /**************************/
        /***** Misc. routines *****/
        /**************************/

        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="ex">The exeption.</param>
        private void ErrorHandler(Exception ex)
        {
            var msg = $"A Fatal error occurred:\r\n{ex}";

            MessageBox.Show(msg, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Loads the form settings.
        /// </summary>
        private void LoadFormSettings()
        {
            if (_isDesignTime) return;

            var assy = Assembly.GetExecutingAssembly();
            var ver = assy.GetName().Version;
            var activeWorkingArea = Screen.FromControl(this).WorkingArea;
            var rect1 = new Rectangle(Properties.Settings.Default.MainFormLocation, Properties.Settings.Default.MainFormSize);
            var rect2 = rect1;
            var offset = 5;

            rect2.Location.Offset(offset, offset);

            if (activeWorkingArea.Contains(rect1.Location)
                && activeWorkingArea.Contains(rect2.Location)
                && (rect1.Height >= MinimumSize.Height)
                && (rect1.Width >= MinimumSize.Width))
            {
                // Place the form like the last time
                Location = rect1.Location;
                Size = rect1.Size;
            }
            else
            {
                // The last time the form was placed (in part) outside the screen, so reset the size and center on screen
                // Size = MinimumSize;

                var horOffset = (int)((activeWorkingArea.Width / 2) - (0.5 * Width));
                var vertOffset = (int)((activeWorkingArea.Height / 2) - (0.5 * Height));

                Location = new Point(horOffset, vertOffset);

                SaveFormSettings();

                Properties.Settings.Default.Reload();
            }

            RotButton.Visible = Properties.Settings.Default.ShowRotButton;
            Text = $"{Text} v{ver}";
        }


        /// <summary>
        /// Saves the form settings.
        /// </summary>
        private void SaveFormSettings()
        {
            if (_isDesignTime) return;

            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MainFormSize = Size;
            Properties.Settings.Default.Save();
        }

    }

}
