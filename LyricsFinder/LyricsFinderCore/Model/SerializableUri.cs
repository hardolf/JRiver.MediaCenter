using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// Serializable Uri type.
    /// </summary>
    /// <seealso cref="System.Uri" />
    [Serializable]
    public class SerializableUri : Uri
    {

        const string _dummyUrl = "http://localhost";

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        public SerializableUri() : base(_dummyUrl)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="uriString">A URI.</param>
        public SerializableUri(string uriString) : base(uriString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="uriString">The URI.</param>
        /// <param name="dontEscape"><see langword="true" /> if <paramref name="uriString" /> is completely escaped; otherwise, <see langword="false" />. See Remarks.</param>
        [Obsolete("Obsolete constructor.")]
        public SerializableUri(string uriString, bool dontEscape) : base(uriString, dontEscape)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="System.Uri" /> instance.</param>
        /// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
        public SerializableUri(string uriString, UriKind uriKind) : base(uriString, uriKind)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI to add to the base URI.</param>
        public SerializableUri(Uri baseUri, string relativeUri) : base(baseUri, relativeUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="baseUri">An absolute <see cref="System.Uri" /> that is the base for the new <see cref="System.Uri" /> instance.</param>
        /// <param name="relativeUri">A relative <see cref="System.Uri" /> instance that is combined with <paramref name="baseUri" />.</param>
        public SerializableUri(Uri baseUri, Uri relativeUri) : base(baseUri, relativeUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI to add to the base URI.</param>
        /// <param name="dontEscape"><see langword="true" /> if <paramref name="relativeUri" /> is completely escaped; otherwise, <see langword="false" />. See Remarks.</param>
        [Obsolete("Obsolete constructor.")]
        public SerializableUri(Uri baseUri, string relativeUri, bool dontEscape) : base(baseUri, relativeUri, dontEscape)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableUri"/> class.
        /// </summary>
        /// <param name="serializationInfo">An instance of the <see cref="System.Runtime.Serialization.SerializationInfo" /> class containing the information required to serialize the new <see cref="System.Uri" /> instance.</param>
        /// <param name="streamingContext">An instance of the <see cref="System.Runtime.Serialization.StreamingContext" /> class containing the source of the serialized stream associated with the new <see cref="System.Uri" /> instance.</param>
        protected SerializableUri(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

    }

}
