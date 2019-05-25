//Description:
//
//Author:
//
//Plugin Creation Date:
//__/__/____
//Version Number:
//0.0.1
//Template Created by Mr ChriZ


#region Libraries
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
#endregion

namespace MC_CSPlugin
{
    #region Interop Program ID Registration
    //The interop services allow the plugin to be registered with a CLSID so Media Center
    //Can find it
    //The Prog ID must match with that in the registry in order for 
    //MC to be able to pick up the plugin
    [System.Runtime.InteropServices.ProgId ( "Template_MCPlugin.CSTemplate" )]
    #endregion
    public partial class MainInterface : UserControl
    {
        #region Attributes
        //This is the Interface to Media Center
        //This is set when Media Center calls the Init Method        
        private MediaJukebox.MJAutomation mediaCenterReference;
        #endregion

        #region Constructor
        /// <summary>
        /// This is the main constructor for the plugin
        /// </summary>
        public MainInterface ( )
        {
            InitializeComponent ( );
        }
        #endregion

        #region Media Center Initialisation
        /// <summary>
        /// After the plugin has been created Media Center 
        /// will call the following method, giving us a reference
        /// to the Media Center interface.
        /// </summary>
        /// <param name="mediaCenterReference">
        /// Media Center Reference
        /// </param>        
        public void Init ( MediaJukebox.MJAutomation mediaCenterReference )
        {   
            try
            {                  
                this.mediaCenterReference = mediaCenterReference;
            }
            catch (Exception e)
                    {
                        MessageBox.Show ( "A Fatal error has occured while creating plugin:-" + e.Message +
                                "\n The Failure Occured" +
                                "\n In Class Object " + e.Source +
                                "\n when calling Method " + e.TargetSite +
                                "\n \n The following Inner Exception was caused" + e.InnerException +
                                "\n \n The Stack Trace Follows: \n\n" + e.StackTrace );
                        this.Enabled = false;
            }
            //Placing anything outside of this
            //try catch may cause MC to fail to open.
            //Play safe and insert it try area! 
        }

        #endregion

    }
}