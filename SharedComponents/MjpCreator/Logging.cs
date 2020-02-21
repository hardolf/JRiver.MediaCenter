using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;


namespace MediaCenter.SharedComponents
{

    /// <summary>
    ///     Logging class for projects.
    /// </summary>
    public static class Logging
    {

        private static string _logHeader1 = string.Empty;
        private static string _logHeader2 = string.Empty;

        internal static List<string> Errors { get; private set; }


        /// <summary>
        ///     Initializes the <see cref="Logging" /> class.
        /// </summary>
        static Logging()
        {
            Errors = new List<string>();
        }


        /// <summary>
        ///     Begins the log.
        /// </summary>
        public static void BeginLog()
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine(_logHeader1);
        }


        /// <summary>
        ///     Divides the log by writing a secondary log header.
        /// </summary>
        public static void DivideLog()
        {
            Console.WriteLine(_logHeader2);
        }


        /// <summary>
        ///     Ends the log.
        /// </summary>
        public static void EndLog()
        {
            Console.WriteLine(_logHeader1);
            Console.WriteLine(string.Empty);
        }


        /// <summary>
        /// Format the process begin message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="processText">The process text.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <returns>
        /// Formatted begin message.
        /// </returns>
        public static string FormatProcessBeginMessage(
            this string messageFormat,
            string processText,
            string timeFormat)
        {
            var ret = string.Format(CultureInfo.InvariantCulture, messageFormat, 
                processText, DateTime.Now.ToString(timeFormat, CultureInfo.InvariantCulture));

            return ret;
        }


        /// <summary>
        /// Format the process end message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="processText">The process text.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <param name="result">The result text.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="durationFormat">The duration format.</param>
        /// <param name="durationSecondsFormat">The duration seconds format.</param>
        /// <returns>
        /// Formatted end message.
        /// </returns>
        public static string FormatProcessEndMessage(
            this string messageFormat,
            string processText,
            string timeFormat,
            string result,
            DateTime startTime,
            string durationFormat,
            string durationSecondsFormat)
        {
            var endTime = DateTime.Now;
            var duration = endTime - startTime;
            var culture = CultureInfo.InvariantCulture;

            var ret = string.Format(CultureInfo.InvariantCulture, messageFormat, 
                processText, startTime.ToString(timeFormat, culture), result, endTime.ToString(timeFormat, culture),
                duration.ToString(durationFormat, culture), duration.TotalSeconds.ToString(durationSecondsFormat, culture));

            return ret;
        }


        /// <summary>
        ///     Initializes the specified log header1.
        /// </summary>
        /// <param name="logHeader1">The log header1.</param>
        /// <param name="logHeader2">The log header2.</param>
        public static void Init(
            string logHeader1,
            string logHeader2)
        {
            _logHeader1 = logHeader1;
            _logHeader2 = logHeader2;
        }


        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="optionalMessage">The optional message.</param>
        public static void LogError(
            MethodBase method,
            Exception ex,
            string optionalMessage = null)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            if (ex is null) throw new ArgumentNullException(nameof(ex));

            var msg = $"Error in {method.Name}: {optionalMessage}" + Environment.NewLine + ex;

            Console.WriteLine(msg);
        }


        /// <summary>
        ///     Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogInfo(
            string message = "")
        {
            Console.WriteLine(message);
        }

    }

}