using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;
using log4net.Appender;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Logging routines.
    /// </summary>
    internal static class Logging
    {

        private static readonly ILog _log = LogManager.GetLogger(nameof(LyricsFinder));


        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <param name="message">The message.</param>
        /// <param name="isDebug">if set to <c>true</c> [is debug].</param>
        public static void Log(int progressPercentage, string message, bool isDebug = false)
        {
            if (progressPercentage > 0)
                message = $"{progressPercentage,3}% - {message}";

            if (isDebug && _log.IsDebugEnabled)
                _log.Debug(message);

            if (!isDebug && _log.IsInfoEnabled)
                _log.Info(message);
        }


        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Log(int progressPercentage, string message, Exception exception)
        {
            var msg = message ?? string.Empty;

            msg += (exception == null)
                ? string.Empty
                : " " + exception.Message;

            if (progressPercentage >= 0)
                msg = $"{progressPercentage,3}% - {msg}";

            if (_log.IsErrorEnabled)
                _log.Error(msg, exception);
        }


        /// <summary>
        /// Shows the log.
        /// </summary>
        public static void ShowLog()
        {
            var appenders = _log.Logger.Repository.GetAppenders();

            if (!(appenders.FirstOrDefault() is FileAppender appender))
                throw new Exception("No active log4net file appenders.");

            var file = appender.File;
            var dir = Path.GetDirectoryName(file);
            var ext = Path.GetExtension(file);
            var logFileDialog = new OpenFileDialog
            {
                CheckPathExists = true,
                DefaultExt = ext,
                InitialDirectory = dir
            };
            var result = logFileDialog.ShowDialog();

            if ((result == DialogResult.OK) && (!string.IsNullOrEmpty(logFileDialog.FileName)))
                Process.Start(logFileDialog.FileName);
        }

    }

}
