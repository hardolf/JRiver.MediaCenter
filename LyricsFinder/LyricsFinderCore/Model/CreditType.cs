using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Lyrics credit type.
    /// </summary>
    [Serializable]
    public class CreditType
    {

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        [XmlElement]
        public virtual string Company { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        [XmlElement]
        public virtual string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the credit date.
        /// </summary>
        /// <value>
        /// The credit date.
        /// </value>
        [XmlElement]
        public virtual DateTime CreditDate { get; set; }

        /// <summary>
        /// Gets or sets the credit text format.
        /// </summary>
        /// <value>
        /// The credit text format.
        /// </value>
        [XmlElement]
        public virtual string CreditTextFormat { get; set; }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        [XmlElement]
        public virtual string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        [XmlElement]
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the credit URL.
        /// </summary>
        /// <value>
        /// The credit URL.
        /// </value>
        [XmlElement]
        public virtual SerializableUri CreditUrl { get; set; }

        /// <summary>
        /// Gets or sets the service URL.
        /// </summary>
        /// <value>
        /// The service URL.
        /// </value>
        [XmlElement]
        public virtual SerializableUri ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [XmlElement]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [XmlElement]
        public string Token { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="CreditType" /> class.
        /// </summary>
        public CreditType()
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("Lyrics found in {ServiceName} on {Date}");
            sb.AppendLine("{Company} - {CreditUrl}");
            sb.AppendLine("{ServiceName} - {ServiceUrl}");
            sb.AppendLine("{Copyright}");
            sb.AppendLine("--------------------------------------------------");

            CreditTextFormat = sb.ToString();
            DateFormat = "yyyy.MM.dd";
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var ret = new StringBuilder(CreditTextFormat);

            ret.Replace("{Company}", Company);
            ret.Replace("{Date}", CreditDate.ToString(DateFormat, CultureInfo.InvariantCulture));
            ret.Replace("{ServiceName}", ServiceName);
            ret.Replace("{CreditUrl}", CreditUrl.ToString());
            ret.Replace("{ServiceUrl}", ServiceUrl.ToString());
            ret.Replace("{Copyright}", Copyright);

            return ret.ToString().LfToCrLf();
        }

    }

}
