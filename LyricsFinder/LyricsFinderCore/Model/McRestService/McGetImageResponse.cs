using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace MediaCenter.LyricsFinder.Model.McRestService
{

    /// <summary>
    /// JRiver MediaCenter REST Web service response type for the GetImage command.
    /// </summary>
    [Serializable]
    internal class McGetImageResponse
    {

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [XmlElement]
        public Bitmap Image { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="McGetImageResponse"/> class.
        /// </summary>
        protected McGetImageResponse()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McGetImageResponse" /> class.
        /// </summary>
        /// <param name="rsp">The response.</param>
        public McGetImageResponse(Bitmap rsp)
        {
            Image = rsp;
        }

    }

}
