using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
    [Serializable]
    [XmlRoot(ElementName = "LyricsFinderData")]
    public class LyricsFinderDataType
    {

        private Version _dataVersion;


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
        [XmlElement(IsNullable = true)]
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
        /// Gets the saved data file path.
        /// </summary>
        /// <value>
        /// The saved data file path.
        /// </value>
        [XmlIgnore]
        private string SavedDataFilePath { get; set; }

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
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsFinderDataType" /> class.
        /// </summary>
        /// <param name="savedDataFilePath">The saved data file path.</param>
        public LyricsFinderDataType(string savedDataFilePath)
            : this()
        {
            SavedDataFilePath = Environment.ExpandEnvironmentVariables(savedDataFilePath);
        }


        /// <summary>
        /// Creates from configuration.
        /// </summary>
        /// <returns></returns>
        public static LyricsFinderDataType CreateFromConfiguration()
        {
            var ret = new LyricsFinderDataType();
            var assy = Assembly.GetExecutingAssembly();
            var appVersion = assy.GetName().Version;

            ret.MainData = MainDataType.CreateFromConfiguration();
            ret.DataVersion = appVersion;

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

            ret.SavedDataFilePath = xmlFilePath;
            ret.Update();
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
        public virtual void Save()
        {
            if (IsChanged && IsSaveOk)
            {
                InitialXml = Serialize.XmlSerializeToString(this, XmlKnownTypes.ToArray());
                Serialize.XmlSerializeToFile(this, SavedDataFilePath, XmlKnownTypes.ToArray());
            }
        }


        /// <summary>
        /// Checks the loaded lyrics finder XML data and updates, if necessary.
        /// </summary>
        private void Update()
        {
            var assy = Assembly.GetExecutingAssembly();
            var appVersion = assy.GetName().Version;

            // Is update necessary?
            if ((DataVersion == null) || (DataVersion < new Version(1, 2)))
            {
                DataVersion = appVersion;

                if (MainData == null)
                    MainData = MainDataType.CreateFromConfiguration();
            }
        }

    }

}
