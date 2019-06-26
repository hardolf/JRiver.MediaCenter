using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Gets the active services.
        /// </summary>
        /// <value>
        /// The active services.
        /// </value>
        [XmlIgnore]
        public virtual List<AbstractLyricService> ActiveServices {
            get
            {
                return Services.Where(s => s.IsImplemented && s.IsActive).ToList();
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
        [XmlArray("Services"), XmlArrayItem("Service")]
        public List<AbstractLyricService> Services { get; }

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
            Services = new List<AbstractLyricService>();
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
        /// Loads this instance.
        /// </summary>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <returns></returns>
        public static LyricsFinderDataType Load(string xmlFilePath)
        {
            xmlFilePath = Environment.ExpandEnvironmentVariables(xmlFilePath);

            var ret = Serialize.XmlDeserializeFromFile<LyricsFinderDataType>(xmlFilePath, XmlKnownTypes.ToArray());

            ret.SavedDataFilePath = xmlFilePath;
            ret.InitialXml = Serialize.XmlSerializeToString(ret, XmlKnownTypes.ToArray());

            return ret;
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

    }

}
