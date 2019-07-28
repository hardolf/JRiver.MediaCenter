using MediaCenter.SharedComponents.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// MjpCreator program.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Main routine.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Execution errorlevel, 0: successfull, 1: failed.</returns>
        public static int Main(string[] args)
        {
            return MjpCreator.MjpCreatorExecute(args);
        }

    }

}