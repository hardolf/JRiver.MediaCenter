using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text.RegularExpressions;
using System.Threading;

// **********************************************************************************************************************************************
// ***** This MessageInspection assembly must be copied to the JRiver MediaCenter program folder when the LyricsFinder is used as a plug-in *****
// **********************************************************************************************************************************************

namespace MediaCenter.SharedComponents
{

    /// <summary>
    ///     Configuration behavior extension element.
    /// </summary>
    public class MessageInspectorBehaviorExtensionElement : BehaviorExtensionElement
    {

        private const string _requestConsoleOutputFormatDefault = "Writing service request to \"{0}\"...";
        private const string _responseConsoleOutputFormatDefault = "Writing service response to \"{0}\"...";

        private const string _enabledPropertyName = "enabled";
        private const string _requestCountBetweenCleanupsPropertyName = "requestCountBetweenCleanups";
        private const string _requestConsoleOutputFormatPropertyName = "requestConsoleOutputFormat";
        private const string _responseConsoleOutputFormatPropertyName = "responseConsoleOutputFormat";
        private const string _fileRetentionHoursPropertyName = "fileRetentionHours";
        private const string _requestMessageFilePropertyName = "requestMessageFile";
        private const string _responseMessageFilePropertyName = "responseMessageFile";


        /// <summary>
        /// Gets or sets the enabled.
        /// </summary>
        /// <value>
        /// The enabled.
        /// </value>
        [ConfigurationProperty(_enabledPropertyName, IsRequired = false)]
        public bool? Enabled
        {
            get { return (bool?)base[_enabledPropertyName]; }
            set { base[_enabledPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the file retention hours.
        /// </summary>
        /// <value>
        /// The file retention hours.
        /// </value>
        [ConfigurationProperty(_fileRetentionHoursPropertyName, IsRequired = false)]
        public double? FileRetentionHours
        {
            get { return (double?)base[_fileRetentionHoursPropertyName]; }
            set { base[_fileRetentionHoursPropertyName] = value; }
        }

        /// <summary>
        ///     Gets or sets the name of the request console output format property.
        /// </summary>
        /// <value>
        ///     The name of the request console output format property.
        /// </value>
        [ConfigurationProperty(_requestConsoleOutputFormatPropertyName, IsRequired = false)]
        public string RequestConsoleOutputFormat
        {
            get { return (string)base[_requestConsoleOutputFormatPropertyName]; }
            set { base[_requestConsoleOutputFormatPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the request count between cleanups.
        /// </summary>
        /// <value>
        /// The request count between cleanups.
        /// </value>
        [ConfigurationProperty(_requestCountBetweenCleanupsPropertyName, IsRequired = false)]
        public int? RequestCountBetweenCleanups
        {
            get { return (int?)base[_requestCountBetweenCleanupsPropertyName]; }
            set { base[_requestCountBetweenCleanupsPropertyName] = value; }
        }

        /// <summary>
        ///     Gets or sets the name of the response console output format property.
        /// </summary>
        /// <value>
        ///     The name of the response console output format property.
        /// </value>
        [ConfigurationProperty(_responseConsoleOutputFormatPropertyName, IsRequired = false)]
        public string ResponseConsoleOutputFormat
        {
            get { return (string)base[_responseConsoleOutputFormatPropertyName]; }
            set { base[_responseConsoleOutputFormatPropertyName] = value; }
        }

        /// <summary>
        ///     Gets or sets the request message file.
        /// </summary>
        /// <value>
        ///     The request message file.
        /// </value>
        [ConfigurationProperty(_requestMessageFilePropertyName, IsRequired = true)]
        public string RequestMessageFile
        {
            get { return (string)base[_requestMessageFilePropertyName]; }
            set { base[_requestMessageFilePropertyName] = value; }
        }

        /// <summary>
        ///     Gets or sets the response message file.
        /// </summary>
        /// <value>
        ///     The response message file.
        /// </value>
        [ConfigurationProperty(_responseMessageFilePropertyName, IsRequired = true)]
        public string ResponseMessageFile
        {
            get { return (string)base[_responseMessageFilePropertyName]; }
            set { base[_responseMessageFilePropertyName] = value; }
        }

        /// <summary>
        ///     Gets the type of behavior.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(MessageInspectorEndpointBehavior); }
        }


        /// <summary>
        ///     Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        ///     The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            // Create the  endpoint behavior that will insert the message
            // inspector into the client runtime

            var enabled = false; // Use false as default if property is not defined in configuration
            var requestCountBetweenCleanups = 1; // Use 1 as default if property is not defined in configuration
            TimeSpan? fileRetentionTimeSpan = null;

            if (Enabled.HasValue)
                enabled = (bool)Enabled;

            if (RequestCountBetweenCleanups.HasValue)
                requestCountBetweenCleanups = (int)RequestCountBetweenCleanups;

            if (FileRetentionHours.HasValue
                && (FileRetentionHours.Value > 0.0))
            {
                var ticks = (long)((double)FileRetentionHours * TimeSpan.TicksPerHour);
                fileRetentionTimeSpan = new TimeSpan(ticks);
            }

            var requestConsoleOutputFormat = (RequestConsoleOutputFormat.IsNullOrEmptyTrimmed())
                ? _requestConsoleOutputFormatDefault
                : RequestConsoleOutputFormat;

            var responseConsoleOutputFormat = (ResponseConsoleOutputFormat.IsNullOrEmptyTrimmed())
                ? _responseConsoleOutputFormatDefault
                : ResponseConsoleOutputFormat;

            return new MessageInspectorEndpointBehavior(
                enabled, requestCountBetweenCleanups, fileRetentionTimeSpan, RequestMessageFile, ResponseMessageFile, requestConsoleOutputFormat, responseConsoleOutputFormat);
        }

    }



    /// <summary>
    ///     Endpoint behavior.
    /// </summary>
    public class MessageInspectorEndpointBehavior : IEndpointBehavior
    {

        private readonly bool _enabled = false;
        private readonly int _requestCountBetweenCleanups = 0;
        private readonly TimeSpan? _fileRetentionTimeSpan;
        private readonly string _requestMessageFile;
        private readonly string _responseMessageFile;
        private readonly string _requestConsoleOutputFormat;
        private readonly string _responseConsoleOutputFormat;


        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInspectorEndpointBehavior" /> class.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="requestCountBetweenCleanups">The request count between cleanups.</param>
        /// <param name="fileRetentionTimeSpan">The file retention time span.</param>
        /// <param name="requestMessageFile">The request message file.</param>
        /// <param name="responseMessageFile">The response message file.</param>
        /// <param name="requestConsoleOutputFormat">The request console output format.</param>
        /// <param name="responseConsoleOutputFormat">The response console output format.</param>
        public MessageInspectorEndpointBehavior(
            bool enabled,
            int requestCountBetweenCleanups,
            TimeSpan? fileRetentionTimeSpan,
            string requestMessageFile,
            string responseMessageFile,
            string requestConsoleOutputFormat,
            string responseConsoleOutputFormat)
        {
            _enabled = enabled;
            _requestCountBetweenCleanups = requestCountBetweenCleanups;
            _fileRetentionTimeSpan = fileRetentionTimeSpan;
            _requestMessageFile = requestMessageFile;
            _responseMessageFile = responseMessageFile;
            _requestConsoleOutputFormat = requestConsoleOutputFormat;
            _responseConsoleOutputFormat = responseConsoleOutputFormat;
        } // Constructor


        /// <summary>
        ///     Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }


        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        /// <exception cref="ArgumentNullException">endpoint
        /// or
        /// clientRuntime</exception>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (endpoint == null) throw new ArgumentNullException(nameof(endpoint));
            if (clientRuntime == null) throw new ArgumentNullException(nameof(clientRuntime));

            // Do not create the message inspectors if any of these criteria is fulfilled
            if (!_fileRetentionTimeSpan.HasValue
                || (_fileRetentionTimeSpan.Value.TotalSeconds < 1)
                || _requestMessageFile.IsNullOrEmptyTrimmed()
                || _responseMessageFile.IsNullOrEmptyTrimmed())
                return;

            clientRuntime.MessageInspectors.Add(new MessageInspector(_enabled, _requestCountBetweenCleanups, _fileRetentionTimeSpan, 
                _requestMessageFile, _responseMessageFile, _requestConsoleOutputFormat, _responseConsoleOutputFormat));
        }


        /// <summary>
        ///     Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(
            ServiceEndpoint endpoint,
            EndpointDispatcher endpointDispatcher)
        {
            // No implementation necessary
        }


        /// <summary>
        ///     Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(
            ServiceEndpoint endpoint)
        {
            // No implementation necessary
        }

    }



    /// <summary>
    ///     Client message inspector.
    /// </summary>
    public class MessageInspector : IClientMessageInspector
    {
        // Shared between all instances of the message inspector, i.e. all requests/responses during the task uptime
        private static int _lastLogHour = 0;
        private static int _lastMessageId = 0;

        private readonly bool _enabled = false;
        private readonly int _requestCountBetweenCleanups = 0;
        private readonly TimeSpan? _fileRetentionTimeSpan;
        private readonly string _requestMessageFile;
        private readonly string _responseMessageFile;
        private readonly string _requestConsoleOutputFormat;
        private readonly string _responseConsoleOutputFormat;


        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInspector" /> class.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="requestCountBetweenCleanups">The request count between cleanups.</param>
        /// <param name="fileRetentionTimeSpan">The file retention time span.</param>
        /// <param name="requestMessageFile">The request message file.</param>
        /// <param name="responseMessageFile">The response message file.</param>
        /// <param name="requestConsoleOutputFormat">The request console output format.</param>
        /// <param name="responseConsoleOutputFormat">The response console output format.</param>
        public MessageInspector(
            bool enabled,
            int requestCountBetweenCleanups,
            TimeSpan? fileRetentionTimeSpan,
            string requestMessageFile,
            string responseMessageFile,
            string requestConsoleOutputFormat,
            string responseConsoleOutputFormat)
        {
            _enabled = enabled;
            _requestCountBetweenCleanups = requestCountBetweenCleanups;
            _fileRetentionTimeSpan = fileRetentionTimeSpan;
            _requestMessageFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(requestMessageFile));
            _responseMessageFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(responseMessageFile));
            _requestConsoleOutputFormat = requestConsoleOutputFormat;
            _responseConsoleOutputFormat = responseConsoleOutputFormat;
        } // Constructor


        /// <summary>
        /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to
        /// the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        /// <exception cref="ArgumentNullException">reply</exception>
        public void AfterReceiveReply(
            ref Message reply,
            object correlationState)
        {
            if (reply == null) throw new ArgumentNullException(nameof(reply));

            // Implement this method to inspect/modify messages after a message
            // is received but prior to passing it back to the client

            if (!_enabled) return;

            var filePath = _responseMessageFile;

            // Patch the file path AFTER the call to cleanup
            PatchFilePath(ref filePath, _lastMessageId);

            // Write the response to file
            Console.WriteLine(_responseConsoleOutputFormat, Path.GetFileName(filePath));
            File.WriteAllText(filePath, reply.ToString());
        }


#pragma warning disable CS1734 // XML comment has a paramref tag, but there is no parameter by that name
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>
        /// The object that is returned as the <paramref name="correlationState" />argument of the
        /// <see cref="IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message, object)" />
        /// method. This is null if no correlation state is used.The best practice is to make this a
        /// <see cref="System.Guid" /> to ensure that no two <paramref name="correlationState" /> objects are the same.
        /// </returns>
        /// <exception cref="ArgumentNullException">request
        /// or
        /// channel</exception>
        /// <remarks>
        /// The method writes the SOAP request to a file prior to the send.
        /// </remarks>
#pragma warning restore CS1734 // XML comment has a paramref tag, but there is no parameter by that name
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public object BeforeSendRequest(
            ref Message request,
            IClientChannel channel)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (channel == null) throw new ArgumentNullException(nameof(channel));

            // Implement this method to inspect/modify messages before they 
            // are sent to the service

            if (!_enabled) return null;

            SetXmlFileVariables(_requestMessageFile);

            // Cleanup old files in the background for each nnn requests
            if ((_requestCountBetweenCleanups > 0)
                && (_fileRetentionTimeSpan != null)
                && (_fileRetentionTimeSpan > TimeSpan.Zero)
                && ((_lastMessageId == 0) || (_lastMessageId % _requestCountBetweenCleanups == 0)))
            {
                ThreadPool.QueueUserWorkItem(CleanupThreadWorker.Process, new ProcessArg()
                {
                    FileRetentionTimeSpan = _fileRetentionTimeSpan,
                    RequestFilePath = _requestMessageFile,
                    ResponseFilePath = _responseMessageFile,
                });
            }

            var filePath = _requestMessageFile;

            // Patch the file path
            PatchFilePath(ref filePath, ++_lastMessageId);

            // Write the request to file
            Console.WriteLine(_requestConsoleOutputFormat, Path.GetFileName(filePath));
            File.WriteAllText(filePath, request.ToString());

            return null;
        }


        /// <summary>
        /// Patches the file path middle part with current time and message ID.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="messageId">The message identifier.</param>
        private static void PatchFilePath(
            ref string filePath,
            int messageId)
        {
            if (filePath == null) return;

            filePath = Path.GetFullPath(filePath);

#pragma warning disable IDE0059 // Value assigned to symbol is never used
            var left = string.Empty;
            var middle = string.Empty;
            var right = string.Empty;
#pragma warning restore IDE0059 // Value assigned to symbol is never used

            SplitFilePath(filePath, out left, out middle, out right);

            // Set the new file path
            if (!(left.IsNullOrEmptyTrimmed() || middle.IsNullOrEmptyTrimmed() || right.IsNullOrEmptyTrimmed()))
            {
                var rgx = new Regex("#+");
                var match = rgx.Match(middle);
                var now = DateTime.Now;

                // Patch the 1st hour component, if more than one exists
                var idx1 = middle.IndexOf(".HH", StringComparison.InvariantCultureIgnoreCase);
                var idx2 = middle.LastIndexOf(".HH", StringComparison.InvariantCultureIgnoreCase);
                if ((now.Hour > _lastLogHour)
                    && (idx1 > 0)
                    && (idx2 > idx1))
                    middle = middle.Remove(idx1, 3).Insert(idx1, $".{_lastLogHour}");

                // Patch the time
                middle = now.ToString(middle, CultureInfo.InvariantCulture);
                if (match.Success)
                    middle = middle.Replace(match.Value, messageId.ToString(match.Value.Replace('#', '0'), CultureInfo.InvariantCulture));

                filePath = left + middle + right;
            }
        }


        /// <summary>
        /// Sets the XML file variables _lastLogHour and _lastMessageId.
        /// </summary>
        /// <param name="requestFilePath">The request file path.</param>
        /// <remarks>
        /// <para>If the task instance is just started and not handled any requests yet, the variables are reset.</para>
        /// <para>If the task instance has handled requests before, the variables are checked against the last request XML file and set.</para>
        /// <para>The _messageId is reset on the first request every hour in this version.</para>
        /// </remarks>
        private static void SetXmlFileVariables(
            string requestFilePath)
        {
            if (requestFilePath.IsNullOrEmptyTrimmed())
                return;

            var currentHour = DateTime.Now.Hour;

            // If lastLogHour is not 0, i.e. the instance has handled requests before, check against current hour
            if (_lastLogHour != 0)
            {
                if (_lastLogHour != currentHour)
                {
                    _lastLogHour = currentHour;
                    _lastMessageId = 0;
                }

                return;
            }

            // The rest of the routine is used at initial instance start

            var left = string.Empty;
            var middle = string.Empty;
            var right = string.Empty;

            SplitFilePath(requestFilePath, out left, out middle, out right);

            if (left.IsNullOrEmptyTrimmed()
                || middle.IsNullOrEmptyTrimmed()
                || right.IsNullOrEmptyTrimmed())
                return;

            var dir = Path.GetDirectoryName(requestFilePath);
            var leftFn = left.Replace(dir, string.Empty);

            if (!leftFn.IsNullOrEmptyTrimmed()
                && (leftFn.StartsWith("\\", StringComparison.InvariantCultureIgnoreCase)))
                leftFn = leftFn.Substring(1);

            var requestFileSearch = leftFn + "*" + right;
            var di = new DirectoryInfo(Path.GetDirectoryName(requestFilePath));
            var fis = di.GetFiles(requestFileSearch, SearchOption.TopDirectoryOnly);
            var lastFi = fis.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (lastFi == null)
            {
                _lastMessageId = 0;
                _lastLogHour = currentHour;
                return;
            }

            // Set the log variable lastLogHour
            _lastLogHour = lastFi.LastWriteTime.Hour;

            // Set the log variable lastMessageId
            if (_lastLogHour != currentHour)
            {
                _lastLogHour = currentHour;
                _lastMessageId = 0;
            }
            else
            {
                var realRequestTemplateFilePath = requestFilePath.Replace("{", string.Empty).Replace("}", string.Empty);
                var idx1 = realRequestTemplateFilePath.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
                var idx2 = realRequestTemplateFilePath.LastIndexOf("#", StringComparison.InvariantCultureIgnoreCase);

                if ((idx1 > 0)
                    && (idx1 < idx2))
                {
                    var numberPart = lastFi.FullName.Substring(idx1, 1 + idx2 - idx1);
                    _ = int.TryParse(numberPart, out _lastMessageId);
                }
            }
        }


        /// <summary>
        /// Splits the file path into left, middle and right part.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="left">The left.</param>
        /// <param name="middle">The middle.</param>
        /// <param name="right">The right.</param>
        private static void SplitFilePath(
            string filePath,
            out string left,
            out string middle,
            out string right)
        {
            left = string.Empty;
            middle = string.Empty;
            right = string.Empty;

            var idx1 = filePath.IndexOf('{');
            var idx2 = filePath.IndexOf('}');

            if ((idx1 > 0)
                && (idx1 < idx2))
            {
                left = filePath.Substring(0, idx1);
                middle = filePath.Substring(idx1 + 1, idx2 - idx1 - 1);
                right = filePath.Substring(idx2 + 1);
            }
        }



        /// <summary>
        /// Process argument type.
        /// </summary>
        private struct ProcessArg
        {

            /// <summary>
            /// Gets or sets the file retention time span.
            /// </summary>
            /// <value>
            /// The file retention time span.
            /// </value>
            public TimeSpan? FileRetentionTimeSpan { get; set; }

            /// <summary>
            /// Gets or sets the request file path.
            /// </summary>
            /// <value>
            /// The request file path.
            /// </value>
            public string RequestFilePath { get; set; }

            /// <summary>
            /// Gets or sets the response file path.
            /// </summary>
            /// <value>
            /// The response file path.
            /// </value>
            public string ResponseFilePath { get; set; }

        }



        /// <summary>
        /// Clean up thread worker, removing old files.
        /// </summary>
        private class CleanupThreadWorker
        {

            /// <summary>
            /// Processes the specified thread context.
            /// </summary>
            /// <param name="threadContext">The thread context.</param>
            internal static void Process(
                object threadContext)
            {
                try
                {
                    var infoState = (ProcessArg)threadContext;

                    CleanupFiles(infoState.FileRetentionTimeSpan, infoState.RequestFilePath);
                    CleanupFiles(infoState.FileRetentionTimeSpan, infoState.ResponseFilePath);
                }
                catch (Exception)
                {
                    // This routine is not essential and is not allowed to fail.
                }
            }


            /// <summary>
            /// Cleans up the files.
            /// </summary>
            /// <param name="fileRetentionTimeSpan">The file retention time span.</param>
            /// <param name="filePath">The file path.</param>
            private static void CleanupFiles(
                TimeSpan? fileRetentionTimeSpan,
                string filePath)
            {
                if (string.IsNullOrEmpty(filePath)
                    || (fileRetentionTimeSpan == null))
                    return;

                try
                {
                    var len = filePath.Length;
                    var idx1 = filePath.IndexOf('{');
                    var idx2 = filePath.IndexOf('}');

                    // Set the search file path while removing the 2 curly braces and the text between them
                    if ((idx1 > 0)
                        && (idx1 < idx2))
                    {
                        var left = filePath.Substring(0, idx1);
                        var middle = filePath.Substring(idx1 + 1, idx2 - idx1 - 1);
                        var right = filePath.Substring(idx2 + 1);

                        // middle = string.Empty.PadRight(middle.Length, '?'); // Why does this not work?
                        middle = "*";

                        var fileSearchPath = left + middle + right;

                        var dir = Path.GetDirectoryName(fileSearchPath);
                        var name = Path.GetFileName(fileSearchPath);

                        if (string.IsNullOrEmpty(dir)
                            || string.IsNullOrEmpty(name)) return;

                        len -= 2; // Compensate for the 2 curly braces

                        var files = Directory.GetFiles(dir, name, SearchOption.TopDirectoryOnly);

                        foreach (var file in files)
                        {
                            if ((DateTime.Now - File.GetLastWriteTime(file) > fileRetentionTimeSpan.Value)
                                && (file.Length == len))
                                File.Delete(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fejl i CleanupFiles: {Constants.NewLine}{ex}");

                    if (EventLog.SourceExists("Application"))
                        EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Warning);
                }
            }

        }

    }

}
