using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model.LyricServices;


namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Error handing operations type.
    /// </summary>
    internal static class ErrorHandling
    {

        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <remarks>This routine does not write anything in the log, it is displaying a message on the screen only.</remarks>
        public static void ShowErrorHandler(string message, int progressPercentage = 0)
        {
            message = message.AppendProgressPercentage(progressPercentage);

            // MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ErrorForm.Show(message);
        }

        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="message">The message.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <remarks>This routine does not write anything in the log, it is displaying a message on the screen only.</remarks>
        public static void ShowErrorHandler(IWin32Window owner, string message, int progressPercentage = 0)
        {
            message = message.AppendProgressPercentage(progressPercentage);

            // MessageBox.Show(owner, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ErrorForm.Show(owner, message);
        }


        /// <summary>
        /// Error handler with detailed inner exception messages.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The primary exeption.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <param name="innerExceptionLevel">The inner exception level, i.e. the number of inner exception messages to list under the primary exception message. "0": show all inner exceptions.</param>
        public static void ShowAndLogDetailedErrorHandler(string message, Exception exception, int progressPercentage = 0, int innerExceptionLevel = 0)
        {
            const string indent = "    ";
            var sb = new StringBuilder(message.AppendProgressPercentage(progressPercentage));

            ErrorLog(sb.ToString(), exception);

            sb.AppendLine();
            sb.AppendLine($"{indent}{exception.GetType().Name}: {exception.Message.Trim(' ', '"')}");
            sb.AppendLine();
            sb.AppendLine($"The failure occurred in class object {exception.Source}");
            sb.AppendLine();
            sb.AppendLine($"Inner exceptions: ({((innerExceptionLevel == 0) ? "all" : $"only {innerExceptionLevel}")})");

            var cnt = 0;
            var innerEx = exception.InnerException;

            while ((innerEx != null)
                && ((innerExceptionLevel < 1) || (cnt < innerExceptionLevel)))
            {
                var msg = innerEx.Message.Trim(' ', '"');

                cnt++;

                sb.Append($"{indent}{innerEx.GetType().Name}: ");

                if (innerEx is LyricServiceCommunicationException ex1)
                {
                    sb.Append($"There was a communication error, i.e. a request failed during lyrics search ");
                    sb.Append($"for Artist \"{ex1.McItem.Artist}\",  Album \"{ex1.McItem.Album}\" and Song \"{ex1.McItem.Name}\", ");
                    sb.Append($"request: \"{ex1.RequestUri.AbsoluteUri}\", ");
                    sb.Append($"error: {msg}");
                }
                else if (innerEx is GeneralLyricServiceException ex2)
                {
                    sb.Append($"There was a lyric service error during lyrics search ");
                    sb.Append($"for Artist \"{ex2.McItem.Artist}\",  Album \"{ex2.McItem.Album}\" and Song \"{ex2.McItem.Name}\", ");
                    sb.Append($"error: {msg}");
                }
                else
                    sb.Append($"{indent}{msg}");

                sb.AppendLine();
                innerEx = innerEx.InnerException;
            }

            sb.AppendLine();
            sb.AppendLine($"Stack trace:");
            sb.AppendLine($"{exception.StackTrace}");

            ShowErrorHandler(sb.ToString());
        }


        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The primary exeption.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <param name="innerExceptionLevel">The inner exception level, i.e. the number of inner exception messages to list under the primary exception message. "0": show all inner exceptions.</param>
        public static void ShowAndLogErrorHandler(string message, Exception exception, int progressPercentage = 0, int innerExceptionLevel = 0)
        {
            const string indent = "    ";
            var sb = new StringBuilder(message.AppendProgressPercentage(progressPercentage));

            ErrorLog(sb.ToString(), exception);

            sb.AppendLine();
            sb.AppendLine($"{indent}{exception.Message.Trim(' ', '"')}");
            sb.AppendLine();
            sb.AppendLine($"The failure occurred in class object {exception.Source}");
            sb.AppendLine();
            sb.AppendLine($"Inner exception messages: ({((innerExceptionLevel == 0) ? "all" : $"only {innerExceptionLevel}")})");

            var cnt = 0;
            var ex = exception.InnerException;

            while ((ex != null)
                && ((innerExceptionLevel < 1) || (cnt < innerExceptionLevel)))
            {
                cnt++;
                sb.AppendLine($"{indent}{ex.Message.Trim(' ', '"')}");
                ex = ex.InnerException;
            }

            sb.AppendLine();
            sb.AppendLine($"Stack trace:");
            sb.AppendLine($"{exception.StackTrace}");

            ShowErrorHandler(sb.ToString());
        }


        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        internal static void ErrorLog(string message, Exception exception, int progressPercentage = 0)
        {
            Logging.Log(progressPercentage, message, exception);
        }


        /// <summary>
        /// Appends the progress percentage.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        /// <returns>The message string with progress percentage appended, if applicable.</returns>
        private static string AppendProgressPercentage(this string message, int progressPercentage)
        {
            var ret = message ?? string.Empty;

            if (!ret.EndsWith(".", StringComparison.InvariantCultureIgnoreCase))
                ret += ".";

            if (progressPercentage > 0)
                ret += $" Progress: {progressPercentage}%";

            return ret;
        }

    }

}
