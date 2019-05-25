using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.McRestService
{

    /// <summary>
    /// JRiver MediaCenter REST Web service response type for the Alive command.
    /// </summary>
    /// <seealso cref="MediaCenter.LyricsFinder.Model.McRestService.McResponse" />
    [Serializable]
    internal class McAliveResponse : McResponse
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
    internal class McAuthenticationResponse : McResponse
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
    /// JRiver MediaCenter REST Web service response type for the SetInfo command.
    /// </summary>
    [Serializable]
    internal class McSetInfoResponse : McResponse
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
