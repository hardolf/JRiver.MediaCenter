/*
Description: 

Author: 

Plugin Creation Date:
____.__.__

Version Number:
0.0.1

Template Created by Mr ChriZ
Source: http://accessories.jriver.com/mediacenter/mc_data/plugins/CSPlugin_Template.rar
Modified: 2018.04.10 by Hardolf.
*/


#region Libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#endregion


namespace MediaCenter.Plugin.Interface
{

    /// <summary>
    /// Main interface type. 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <remarks>
    /// The interop services allow the plugin to be registered with a CLSID so Media Center can find it.
    /// The Prog ID must match with that in the registry in order for MediaCenter to be able to pick up the plugin.
    /// </remarks>
    [ProgId("MediaCenter.Plugin.Interface.CsTemplate")]
    public partial class MainInterface : UserControl
    {

        #region Properties

        /// <summary>
        /// The Media Center reference. 
        /// </summary>
        /// <remarks>
        /// This is the Interface to Media Center and is set when Media Center calls the Init Method. 
        /// </remarks>
        private MCAutomation MediaCenterReference { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// This is the main constructor for the plugin.
        /// </summary>
        public MainInterface()
        {
            InitializeComponent();
        }

        #endregion


        #region Media Center Initialisation
        /// <summary>
        /// After the plugin has been created Media Center will call the following method, 
        /// giving us a reference to the Media Center interface.
        /// </summary>
        /// <param name="mediaCenterReference">Media Center Reference</param>
        public void Init(MCAutomation mediaCenterReference)
        {
            try
            {
                // ErrorTest();

                this.MediaCenterReference = mediaCenterReference;

                // This tells MC to also call our MJEvent method
                this.MediaCenterReference.FireMJEvent += new IMJAutomationEvents_FireMJEventEventHandler(MJEvent);

                this.Process();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }

            // Placing anything outside of this try catch may cause MC to fail to open.
            // Play safe and insert it in the try area! 
        }

        #endregion


        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="ex">The exeption.</param>
        protected virtual void ErrorHandler(Exception ex)
        {
            var msg = $"A Fatal error occurred while creating the MediaCenter plugin:\n"
                + $"  {ex.Message}\n\n"
                + $"The failure occurred in class object {ex.Source}.\n"
                + $"when calling Method {ex.TargetSite}.\n";

            if (ex.InnerException != null)
                msg += "\n"
                    + $"Inner exception:\n"
                    + $"  {ex.InnerException}\n";

            msg += "\n"
                + $"Stack trace:\n"
                + $"{ex.StackTrace}";

            MessageBox.Show(msg, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.Enabled = false;
        }


        #region ErrorTest

        /// <summary>
        /// Test routine for the error handling.
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="Exception">Test error</exception>
        private void ErrorTest()
        {
            try
            {
                throw new DivideByZeroException();
            }
            catch (Exception ex)
            {
                throw new Exception("Test error", ex);
            }
        }

        #endregion


        /// <summary>
        /// MJEvent event handler.
        /// </summary>
        /// <param name="s1">The s1 string.</param>
        /// <param name="s2">The s2 string.</param>
        /// <param name="s3">The s3 string.</param>
        protected virtual void MJEvent(string s1, string s2, string s3)
        {
            switch (s1)
            {
                case "MJEvent type: MCCommand":
                    switch (s2)
                    {
                        case "MCC: NOTIFY_TRACK_CHANGE":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYERSTATE_CHANGE":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_ADDED":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_INFO_CHANGED":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_FILES_CHANGED":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_REMOVED":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_COLLECTION_CHANGED":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_PROPERTIES_CHANGED":
                            // Your code here
                            break;

                        case "MCC: NOTIFY_SKIN_CHANGED":
                            // Your code here
                            break;

                        default:
                            // Unknown (new?) event
                            break;
                    }

                    break;

                default:
                    // Unknown (new?) type
                    break;
            }
        }


        /// <summary>
        /// Process code for this Media Center plugin instance.
        /// </summary>
        /// <remarks>
        /// This is where the actual plugin code is.
        /// </remarks>
        protected virtual void Process()
        {
            try
            {
                // ErrorTest();

            }
            catch (Exception ex)
            {
                var type = this.GetType();
                var typeNs = type?.Namespace ?? "?";
                var typeName = type?.Name ?? "?";
                var routineName = MethodBase.GetCurrentMethod()?.Name ?? "?";

                throw new Exception($"Error in MediaCenter plugin routine \"{typeNs}.{typeName}.{routineName}\".", ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click!", "Click", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }

}