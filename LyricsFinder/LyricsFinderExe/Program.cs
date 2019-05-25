/*
Description: Lyrics finder for JRiver Media Center

Author: Hardolf

Standalone program creation date:
2018.04.12

Version Number:
1.0.0

Modified: 2019.05.25 by Hardolf.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaCenter.LyricsFinder
{

    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

    }

}
