﻿/*
Description: Lyrics finder for JRiver MediaCenter

Author: Hardolf

Plugin creation date:
2018.04.12

Version Number:
1.2.0

Inspired by template created by Mr ChriZ
Source: http://accessories.jriver.com/mediacenter/mc_data/plugins/CSPlugin_Template.rar

Modified: 2019.05.25 by Hardolf.
Modified: 2019.07.01 by Hardolf.
*/

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Main interface type. 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <remarks>
    /// The interop services allow the plugin to be registered with a CLSID so Media Center can find it.
    /// The Prog ID must match with that in the registry in order for MediaCenter to be able to pick up the plugin.
    /// </remarks>
    [ProgId("MediaCenter.LyricsFinder.Plugin")]
    [ComVisible(true)]
    public class LyricsFinderPlugin : LyricsFinderCore
    {

        /// <summary>
        /// The Media Center reference. 
        /// </summary>
        /// <remarks>
        /// This is the Interface to Media Center and is set when Media Center calls the Init Method. 
        /// </remarks>
        private MCAutomation MediaCenterReference { get; set; }


        /// <summary>
        /// This is the main constructor for the plugin.
        /// </summary>
        public LyricsFinderPlugin()
            : base(false, Assembly.GetExecutingAssembly())
        {
        }


        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exeption.</param>
        private static void ErrorHandler(string message, Exception exception)
        {
            const string indent = "    ";

            try
            {
                // StatusLog(ex);

                message += " " + Constants.NewLine
                    + $"{indent}{exception.Message} " + Constants.DoubleNewLine
                    + $"The failure occurred in class object {exception.Source} " + Constants.NewLine
                    + $"when calling Method {exception.TargetSite}." + Constants.NewLine;

                if (exception.InnerException != null)
                    message += " " + Constants.NewLine
                        + $"Inner exception: " + Constants.NewLine
                        + $"{indent}{exception.InnerException} " + Constants.NewLine;

                message += " " + Constants.NewLine
                    + $"Stack trace: " + Constants.NewLine
                    + $"{exception.StackTrace}";

                MessageBox.Show(message, "Fatal plugin error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying error message: " + ex.Message, "Fatal plugin error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// After the plugin has been created Media Center will call the following method,
        /// giving us a reference to the Media Center interface.
        /// </summary>
        /// <param name="mediaCenterReference">Media Center Reference.</param>
        /// <exception cref="ArgumentNullException">mediaCenterReference</exception>
        /// <remarks>
        /// <para>The "async Task" construct seems to work instead of just "void".</para>
        /// <para>The routine name must be "Init", otherwise Media Center won't find it.</para>
        /// </remarks>
        [ComVisible(true)]
        public async void Init(MCAutomation mediaCenterReference)
        {
            try
            {
                MediaCenterReference = mediaCenterReference ?? throw new ArgumentNullException(nameof(mediaCenterReference));

                // This tells MC to also call our MJEvent method
                MediaCenterReference.FireMJEvent += new IMJAutomationEvents_FireMJEventEventHandler(MJEvent);

                await InitCoreAsync();
            }
            catch (Exception ex)
            {
                ErrorHandler("A Fatal error occurred while initializing the MediaCenter LyricsFinder plugin", ex);
            }

            // Placing anything outside of this try catch may cause MC to fail to open.
            // Play safe and insert it in the try area! 
        }


        /// <summary>
        /// MJEvent event handler.
        /// </summary>
        /// <param name="s1">The s1 string.</param>
        /// <param name="s2">The s2 string.</param>
        /// <param name="s3">The s3 string.</param>
        private void MJEvent(string s1, string s2, string s3)
        {
            try
            {
                // Test
                // MessageBox.Show($"MJEvent(\"{s1}\", \"{s2}\", \"{s3}\") fired", "MJEvent fired", MessageBoxButtons.OK, MessageBoxIcon.Information);

                switch (s1)
                {
                    case "MJEvent type: MCCommand":
                        switch (s2)
                        {
                            case "MCC: NOTIFY_TRACK_CHANGE":
                                break;

                            case "MCC: NOTIFY_PLAYERSTATE_CHANGE":
                                break;

                            case "MCC: NOTIFY_PLAYLIST_ADDED":
                                break;

                            case "MCC: NOTIFY_PLAYLIST_INFO_CHANGED":
                                break;

                            case "MCC: NOTIFY_PLAYLIST_FILES_CHANGED":
                                break;

                            case "MCC: NOTIFY_PLAYLIST_REMOVED":
                                break;

                            case "MCC: NOTIFY_PLAYLIST_COLLECTION_CHANGED":
                                break;

                            case "MCC: NOTIFY_PLAYLIST_PROPERTIES_CHANGED":
                                break;

                            case "MCC: NOTIFY_SKIN_CHANGED":
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
            catch (Exception ex)
            {
                ErrorHandler("Error handling MediaCenter event in LyricsFinder plugin.", ex);
            }
        }

    }

}
