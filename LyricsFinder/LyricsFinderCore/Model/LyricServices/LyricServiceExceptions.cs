using MediaCenter.McWs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Exception is thrown when the lyrics quota is exceeded.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricsQuotaExceededException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        public LyricsQuotaExceededException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricsQuotaExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LyricsQuotaExceededException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricsQuotaExceededException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }



    /// <summary>
    /// General lyric service exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class GeneralLyricServiceException : Exception
    {

        /// <summary>
        /// Gets the credit object.
        /// </summary>
        /// <value>
        /// The credit object.
        /// </value>
        public CreditType Credit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the lyric service was in a "get all" mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the lyric service was in a "get all" mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsGetAll { get; private set; }

        /// <summary>
        /// Gets the Media Center MPL item.
        /// </summary>
        /// <value>
        /// The Media Center MPL item.
        /// </value>
        public McMplItem McItem { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLyricServiceException" /> class.
        /// </summary>
        public GeneralLyricServiceException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLyricServiceException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected GeneralLyricServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLyricServiceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public GeneralLyricServiceException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLyricServiceException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="isGetAll">if set to <c>true</c> the lyric service was in a "get all" mode; else <c>false</c>.</param>
        /// <param name="credit">The credit object.</param>
        /// <param name="mcItem">The Media Center MPL item.</param>
        public GeneralLyricServiceException(string message, bool isGetAll, CreditType credit, McMplItem mcItem)
            : base(message)
        {
            Credit = credit;
            IsGetAll = isGetAll;
            McItem = mcItem;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLyricServiceException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public GeneralLyricServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralLyricServiceException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="isGetAll">if set to <c>true</c> the lyric service was in a "get all" mode; else <c>false</c>.</param>
        /// <param name="credit">The credit object.</param>
        /// <param name="mcItem">The Media Center MPL item.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public GeneralLyricServiceException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Exception innerException)
            : base(message, innerException)
        {
            Credit = credit;
            IsGetAll = isGetAll;
            McItem = mcItem;
        }

    }



    /// <summary>
    /// Lyric service communication exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricServiceCommunicationException : GeneralLyricServiceException
    {

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <value>
        /// The request URI.
        /// </value>
        public Uri RequestUri { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException" /> class.
        /// </summary>
        public LyricServiceCommunicationException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricServiceCommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LyricServiceCommunicationException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="isGetAll">if set to <c>true</c> the lyric service was in a "get all" mode; else <c>false</c>.</param>
        /// <param name="credit">The credit object.</param>
        /// <param name="mcItem">The Media Center MPL item.</param>
        /// <param name="requestUri">The request URI.</param>
        public LyricServiceCommunicationException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Uri requestUri)
            : base(message, isGetAll, credit, mcItem)
        {
            RequestUri = requestUri;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceCommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="isGetAll">if set to <c>true</c> the lyric service was in a "get all" mode; else <c>false</c>.</param>
        /// <param name="credit">The credit object.</param>
        /// <param name="mcItem">The Media Center MPL item.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceCommunicationException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Uri requestUri, Exception innerException)
            : base(message, isGetAll, credit, mcItem, innerException)
        {
            RequestUri = requestUri;
        }

    }

}
