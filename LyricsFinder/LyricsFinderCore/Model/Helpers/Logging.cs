using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;
using log4net.Appender;

using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Logging routines.
    /// </summary>
    internal static class Logging
    {

        private static ILog _log;


        /// <summary>
        /// Initializes the logger with the specified parameters.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="xmlConfigfileInfo">The log4net XML configuration file information.</param>
        public static void Init(string loggerName, FileInfo xmlConfigfileInfo)
        {
            _log = LogManager.GetLogger(loggerName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(xmlConfigfileInfo);
        }


        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <param name="message">The message.</param>
        /// <param name="isDebug">if set to <c>true</c> [is debug].</param>
        public static void Log(int progressPercentage, string message = null, bool isDebug = false)
        {
            if (!message.IsNullOrEmptyTrimmed())
                message = message.Substring(0, 1).ToUpperInvariant() + message.Remove(0, 1);

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
        /// Shows the log folder.
        /// </summary>
        /// <exception cref="System.Exception">No active log4net file appenders.</exception>
        /// <exception cref="Exception">No active log4net file appenders.</exception>
        public static void ShowLogDir()
        {
            var appenders = _log.Logger.Repository.GetAppenders();

            if (!(appenders.FirstOrDefault() is FileAppender appender))
                throw new Exception("No active log4net file appenders.");

            var file = appender.File;
            var dir = Path.GetDirectoryName(file);
            var ext = Path.GetExtension(file);

            using (var logFileDialog = new OpenFileDialog
            {
                CheckPathExists = true,
                DefaultExt = ext,
                InitialDirectory = dir
            })
            {
                var result = logFileDialog.ShowDialog();

                if ((result == DialogResult.OK) && (!string.IsNullOrEmpty(logFileDialog.FileName)))
                    Process.Start(logFileDialog.FileName);
            }
        }

    }

}
