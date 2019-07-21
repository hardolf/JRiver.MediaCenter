using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using MediaCenter.SharedComponents;


namespace MediaCenter.McWs
{

    /// <summary>
    /// JRiver MediaCenter REST Web service response type for the Alive command.
    /// </summary>
    /// <seealso cref="MediaCenter.McWs.McResponse" />
    [Serializable]
    public class McAliveResponse : McResponse
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement]
        public string AccessKey { get; set; }

        [XmlElement]
        public string FriendlyName { get; set; }

        [XmlElement]
        public string LibraryVersion { get; set; }

        [XmlElement]
        public string Platform { get; set; }

        [XmlElement]
        public string ProductVersion { get; set; }

        [XmlElement]
        public string ProgramName { get; set; }

        [XmlElement]
        public string ProgramVersion { get; set; }

        [XmlElement]
        public string RuntimeGUID { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        /// <summary>
        /// Initializes a new instance of the <see cref="McAliveResponse"/> class.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public McAliveResponse(string xml)
            : base(xml)
        {
            this.FillPropertiesFromItems();
        }

    }



    /// <summary>
    /// JRiver MediaCenter REST Web service response type for the Authentication command.
    /// </summary>
    [Serializable]
    public class McAuthenticationResponse : McResponse
    {

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [XmlElement]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the read only.
        /// </summary>
        /// <value>
        /// The read only.
        /// </value>
        [XmlElement]
        public string ReadOnly
        {
            get
            {
                return (IsReadOnly) ? "1" : "0";
            }
            set
            {
                IsReadOnly = (value.IsNullOrEmptyTrimmed() || value.Equals("0", StringComparison.InvariantCultureIgnoreCase)) ? false : true;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McAuthenticationResponse" /> class.
        /// </summary>
        protected McAuthenticationResponse()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McAuthenticationResponse"/> class.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public McAuthenticationResponse(string xml)
            : base(xml)
        {
            this.FillPropertiesFromItems();
        }

    }


    /// <summary>
    /// JRiver MediaCenter REST Web service response type for the Info command.
    /// </summary>
    /// <seealso cref="MediaCenter.McWs.McResponse" />
    [Serializable]
    public class McInfoResponse : McResponse
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement]
        public string Album { get; set; }

        [XmlElement]
        public string Artist { get; set; }

        [XmlElement]
        public string Bitdepth { get; set; }

        [XmlElement]
        public string Bitrate { get; set; }

        [XmlElement]
        public string DurationMS { get; set; }

        [XmlElement]
        public string Channels { get; set; }

        [XmlElement]
        public string Chapter { get; set; }

        [XmlElement]
        public string ElapsedTimeDisplay { get; set; }

        [XmlElement]
        public string FileKey { get; set; }

        [XmlElement]
        public string ImageURL { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string NextFileKey { get; set; }

        [XmlElement]
        public string PlayingNowChangeCounter { get; set; }

        [XmlElement]
        public string PlayingNowPosition { get; set; }

        [XmlElement]
        public string PlayingNowPositionDisplay { get; set; }

        [XmlElement]
        public string PlayingNowTracks { get; set; }

        [XmlElement]
        public string PositionDisplay { get; set; }

        [XmlElement]
        public string PositionMS { get; set; }

        [XmlElement]
        public string RemainingTimeDisplay { get; set; }

        [XmlElement]
        public string SampleRate { get; set; }

        [XmlElement]
        public string State { get; set; }

        [XmlElement]
        public string Status { get; set; }

        [XmlElement]
        public string TotalTimeDisplay { get; set; }

        [XmlElement]
        public string Volume { get; set; }

        [XmlElement]
        public string VolumeDisplay { get; set; }

        [XmlElement]
        public string ZoneID { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        /// <summary>
        /// Initializes a new instance of the <see cref="McAliveResponse"/> class.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public McInfoResponse(string xml)
            : base(xml)
        {
            this.FillPropertiesFromItems();
        }

    }



    /// <summary>
    /// JRiver MediaCenter REST Web service response type for the SetInfo command.
    /// </summary>
    [Serializable]
    public class McSetInfoResponse : McResponse
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="McSetInfoResponse" /> class.
        /// </summary>
        protected McSetInfoResponse()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McSetInfoResponse"/> class.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public McSetInfoResponse(string xml)
            : base(xml)
        {
            this.FillPropertiesFromItems();
        }

    }

}
