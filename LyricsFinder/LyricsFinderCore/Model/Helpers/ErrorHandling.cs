using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;


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
        /// Error handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exeption.</param>
        /// <param name="progressPercentage">The progress percentage.</param>
        public static void ShowAndLogErrorHandler(string message, Exception exception, int progressPercentage = 0)
        {
            const string indent = "    ";

            message = message.AppendProgressPercentage(progressPercentage);

            ErrorLog(message, exception);

            message += $" \r\n"
                + $"{indent}{exception.Message} \r\n\r\n"
                + $"The failure occurred in class object {exception.Source} \r\n";

            if (exception.InnerException != null)
                message += " \r\n"
                    + $"Inner exception: \r\n"
                    + $"{indent}{exception.InnerException} \r\n";

            message += " \r\n"
                + $"Stack trace: \r\n"
                + $"{exception.StackTrace}";

            ShowErrorHandler(message);
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



    /// <summary>
    /// Exception is thrown when the lyrics quota is exceeded.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class LyricsQuotaExceededException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        public LyricsQuotaExceededException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricsQuotaExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LyricsQuotaExceededException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricsQuotaExceededException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }

}
