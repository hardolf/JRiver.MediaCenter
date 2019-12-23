using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Lyrics credit type.
    /// </summary>
    [ComVisible(false)]
    [Serializable]
    public class CreditType
    {
        private Uri _creditUrl;
        private Uri _serviceUrl;


        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        [XmlElement]
        public virtual string Company { get; set; }

        /// <summary>
        /// Gets or sets the copyright of the individual song item.
        /// </summary>
        /// <value>
        /// The copyright text.
        /// </value>
        [XmlIgnore]
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
        /// Gets or sets the display properties.
        /// </summary>
        /// <value>
        /// The display properties.
        /// </value>
        [XmlIgnore]
        public virtual Dictionary<string, DisplayProperty> DisplayProperties { get; private set; }

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
        [XmlIgnore]
        public virtual Uri CreditUrl { get => _creditUrl; set => _creditUrl = value; }

        /// <summary>
        /// Gets or sets the credit URL text.
        /// </summary>
        /// <value>
        /// The credit URL text.
        /// </value>
        [XmlElement]
        public virtual string CreditUrlText
        {
            get { return _creditUrl?.AbsoluteUri; }
            set { _creditUrl = (value.IsNullOrEmptyTrimmed()) ? null : new UriBuilder(value).Uri; }
        }

        /// <summary>
        /// Gets or sets the service URL.
        /// </summary>
        /// <value>
        /// The service URL.
        /// </value>
        [XmlIgnore]
        public virtual Uri ServiceUrl { get => _serviceUrl; set => _serviceUrl = value; }

        /// <summary>
        /// Gets or sets the service URL text.
        /// </summary>
        /// <value>
        /// The service URL text.
        /// </value>
        [XmlElement]
        public virtual string ServiceUrlText
        {
            get { return _serviceUrl?.AbsoluteUri; }
            set { _serviceUrl = (value.IsNullOrEmptyTrimmed()) ? null : new UriBuilder(value).Uri; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CreditType" /> class.
        /// </summary>
        public CreditType()
        {
            DisplayProperties = new Dictionary<string, DisplayProperty>();

            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("Lyrics found in {ServiceName} on {Date}");
            sb.AppendLine("{Company} - {CreditUrl}");
            sb.AppendLine("{ServiceName} - {ServiceUrl}");
            sb.AppendLine("{Copyright}");
            sb.AppendLine("--------------------------------------------------");

            CreditDate = DateTime.Now;
            CreditTextFormat = sb.ToString();
            DateFormat = "yyyy.MM.dd";
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns><see cref="CreditType"/> object.</returns>
        public virtual CreditType Clone()
        {
            var ret = new CreditType
            {
                Company = Company,
                Copyright = Copyright,
                CreditDate = DateTime.Now,
                CreditTextFormat = CreditTextFormat,
                CreditUrl = CreditUrl,
                // CreditUrlText = CreditUrlText,
                DateFormat = DateFormat,
                DisplayProperties = new Dictionary<string, DisplayProperty>(),
                ServiceName = ServiceName,
                ServiceUrl = ServiceUrl,
                // ServiceUrlText = ServiceUrlText
            };

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Creates the display properties.
        /// </summary>
        public virtual void CreateDisplayProperties()
        {
            DisplayProperties.Clear();

            DisplayProperties.Add(nameof(ServiceName), ServiceName, "Name", isEditAllowed: true);
            DisplayProperties.Add(nameof(Company), Company, isEditAllowed: true);
            DisplayProperties.Add(nameof(CreditUrl), CreditUrl, "Company Website", isEditAllowed: true);
            DisplayProperties.Add(nameof(Copyright), Copyright, "Copyright text", isEditAllowed: true);
            DisplayProperties.Add(nameof(CreditTextFormat), CreditTextFormat, "Credit text format", isEditAllowed: true);
            DisplayProperties.Add(nameof(ServiceUrl), ServiceUrl, "Service URL", isEditAllowed: true);
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

            CreditDate = DateTime.Now;

            ret.Replace("{Company}", Company);
            ret.Replace("{Date}", CreditDate.ToString(DateFormat, CultureInfo.InvariantCulture));
            ret.Replace("{ServiceName}", ServiceName);
            ret.Replace("{CreditUrl}", CreditUrl?.ToString() ?? string.Empty);
            ret.Replace("{ServiceUrl}", ServiceUrl?.ToString() ?? string.Empty);
            ret.Replace("{Copyright}", Copyright);

            return ret.ToString().LfToCrLf();
        }


        /// <summary>
        /// Validates the display properties.
        /// </summary>
        public virtual void ValidateDisplayProperties()
        {
            var dps = new Dictionary<string, DisplayProperty>();

            dps.Add(nameof(ServiceName), ServiceName);
            dps.Add(nameof(Company), Company);
            dps.Add(nameof(CreditUrl), CreditUrl);
            dps.Add(nameof(Copyright), Copyright);
            dps.Add(nameof(CreditTextFormat), CreditTextFormat);
            dps.Add(nameof(ServiceUrl), ServiceUrl);
        }

    }

}
