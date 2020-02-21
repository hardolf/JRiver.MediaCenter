using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Contains the LyricsFinder data to be saved and loaded.
    /// </summary>
    [ComVisible(false)]
    [Serializable]
    [XmlRoot(ElementName = "LyricsFinderData")] // Namespace = "https://github.com/hardolf/JRiver.MediaCenter"
    public class LyricsFinderDataType
    {

        private Version _dataVersion;

        // Instantiate a Singleton of the Semaphore with a value of 1. 
        // This means that only 1 thread can be granted access at a time. 
        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);


        /// <summary>
        /// Gets the data file path.
        /// </summary>
        /// <value>
        /// The data file path.
        /// </value>
        [XmlIgnore]
        public string DataFilePath { get; set; }

        /// <summary>
        /// Gets or sets the data version.
        /// </summary>
        /// <value>
        /// The data version.
        /// </value>
        [XmlIgnore]
        public Version DataVersion { get => _dataVersion; set => _dataVersion = value; }

        /// <summary>
        /// Gets or sets the data version text.
        /// </summary>
        /// <value>
        /// The data version text.
        /// </value>
        [XmlElement(ElementName = "DataVersion")]
        public string DataVersionText
        {
            get { return _dataVersion?.ToString() ?? new Version().ToString(); }
            set { _dataVersion = (value.IsNullOrEmptyTrimmed()) ? new Version() : Version.Parse(value); }
        }

        /// <summary>
        /// Gets or sets the main data.
        /// </summary>
        /// <value>
        /// The main data.
        /// </value>
        [XmlElement]
        public MainDataType MainData { get; set; }

        /// <summary>
        /// Gets the active services.
        /// </summary>
        /// <value>
        /// The active services.
        /// </value>
        [XmlIgnore]
        public virtual List<AbstractLyricService> ActiveLyricServices
        {
            get
            {
                return LyricServices.Where(s => s.IsImplemented && s.IsActive).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the initial XML of the serialized <see cref="LyricsFinderDataType"/> object.
        /// </summary>
        /// <value>
        /// The initial XML.
        /// </value>
        [XmlIgnore]
        protected string InitialXml { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance data is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance data is changed; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsChanged
        {
            get
            {
                var currentXml = Serialize.XmlSerializeToString(this, XmlKnownTypes.ToArray());
                var ret = !currentXml.Equals(InitialXml, StringComparison.InvariantCulture);

                return ret;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is save ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it is OK to save; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsSaveOk { get; set; }

        /// <summary>
        /// Gets or sets the service list.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        [XmlArray("LyricServices"), XmlArrayItem("LyricService")]
        public List<AbstractLyricService> LyricServices { get; private set; }

        /// <summary>
        /// Gets or sets the XML known types.
        /// </summary>
        /// <value>
        /// The XML known types.
        /// </value>
        [XmlIgnore]
        public static List<Type> XmlKnownTypes { get; } = new List<Type>();


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsFinderDataType"/> class.
        /// </summary>
        public LyricsFinderDataType()
        {
            IsSaveOk = true;
            LyricServices = new List<AbstractLyricService>();
            MainData = new MainDataType();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsFinderDataType" /> class.
        /// </summary>
        /// <param name="dataFilePath">The saved data file path.</param>
        public LyricsFinderDataType(string dataFilePath)
            : this()
        {
            DataFilePath = Environment.ExpandEnvironmentVariables(dataFilePath);
        }


        /// <summary>
        /// Gets the lyric service by name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns><see cref="AbstractLyricService"/> descendant object.</returns>
        public AbstractLyricService GetLyricService(string serviceName)
        {
            AbstractLyricService ret = null;

            foreach (var service in LyricServices)
            {
                if (service.Credit.ServiceName.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret = service;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// Gets the lyric service T from the XML data file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object.
        /// </returns>
        public static AbstractLyricService GetLyricService<T>() where T : AbstractLyricService, ILyricService
        {
            LyricsFinderCoreConfigurationSectionHandler.Init(Assembly.GetExecutingAssembly());

            var xDataFile = Environment.ExpandEnvironmentVariables(LyricsFinderCoreConfigurationSectionHandler.LocalAppDataFile);
            var xDoc = new XmlDocument() { XmlResolver = null };
            var nodeXml = string.Empty;

            StreamReader sr = null;
            try
            {
                sr = new StreamReader(xDataFile);
                using (var xr = XmlReader.Create(sr, new XmlReaderSettings() { XmlResolver = null }))
                    xDoc.Load(xr);
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }

            var atts = xDoc.DocumentElement.Attributes;
            var serviceNodes = xDoc.GetElementsByTagName("LyricService");
            var knownTypes = new Type[] { typeof(AbstractLyricService), typeof(CreditType) };

            foreach (XmlElement serviceElement in serviceNodes)
            {
                var typeAtt = serviceElement.GetAttributeNode("xsi:type");

                if (typeAtt.Value == typeof(T).Name)
                {
                    // Cheat the serializer by faking/changing the root element name
                    var newServiceElement = xDoc.CreateElement(typeof(T).Name);

                    newServiceElement.InnerXml = serviceElement.InnerXml;
                    newServiceElement.Attributes.Remove(typeAtt);
                    nodeXml += "<?xml version=\"1.0\" encoding=\"utf - 8\"?>";
                    nodeXml += newServiceElement.OuterXml;
                    break;
                }
            }

            var ret = nodeXml.XmlDeserializeFromString<T>(knownTypes: knownTypes);

            ret.LyricsFinderData = new LyricsFinderDataType()
            {
                DataFilePath = xDataFile
            };

            return ret;
        }


        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <returns></returns>
        public static LyricsFinderDataType Load(string xmlFilePath)
        {
            xmlFilePath = Environment.ExpandEnvironmentVariables(xmlFilePath);

            // Conversion from v1.1 to v1.2
            var dict = new Dictionary<string, string>
            {
                { "<Service ", "<LyricService " },
                { "</Service>", "</LyricService>" }
            };

            var ret = Serialize.XmlDeserializeFromFile<LyricsFinderDataType>(xmlFilePath, OnUnknownElement, dict, XmlKnownTypes.ToArray());

            ret.DataFilePath = xmlFilePath;
            ret.InitialXml = Serialize.XmlSerializeToString(ret, XmlKnownTypes.ToArray());

            return ret;
        }


        /// <summary>
        /// Handles the UnknownElement event of the LyricsFinderDataType.Load method.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="XmlElementEventArgs"/> instance containing the event data.</param>
        public static void OnUnknownElement(object sender, XmlElementEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            // var elName = e.Element.Name;

            // Not yet needed:
            // if (elName.Equals("MainData", StringComparison.InvariantCultureIgnoreCase))
            // {
            //     var target = (MainDataType)e.ObjectBeingDeserialized;

            //     target.MainData = MainDataType.CreateFromConfiguration();
            // }
        }


        /// <summary>
        /// Saves this instance.
        /// </summary>
        public virtual async void SaveAsync()
        {
            // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
            await _semaphoreSlim.WaitAsync();

            try
            {
                if (IsChanged && IsSaveOk)
                {
                    InitialXml = Serialize.XmlSerializeToString(this, XmlKnownTypes.ToArray());
                    Serialize.XmlSerializeToFile(this, DataFilePath, XmlKnownTypes.ToArray());
                }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

    }

}
