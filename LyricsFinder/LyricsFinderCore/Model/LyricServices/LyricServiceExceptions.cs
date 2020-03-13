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
    /// Lyric service exception base class, generally non-fatal.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricServiceBaseException : Exception
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
        /// Initializes a new instance of the <see cref="LyricServiceBaseException" /> class.
        /// </summary>
        public LyricServiceBaseException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceBaseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LyricServiceBaseException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceBaseException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceBaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceBaseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricServiceBaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info is null) throw new System.ArgumentNullException(nameof(info));

            Credit = (CreditType)info.GetValue(nameof(Credit), typeof(CreditType));
            IsGetAll = (bool)info.GetValue(nameof(IsGetAll), typeof(bool));
            McItem = (McMplItem)info.GetValue(nameof(McItem), typeof(McMplItem));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceBaseException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="isGetAll">if set to <c>true</c> the lyric service was in a "get all" mode; else <c>false</c>.</param>
        /// <param name="credit">The credit object.</param>
        /// <param name="mcItem">The Media Center MPL item.</param>
        public LyricServiceBaseException(string message, bool isGetAll, CreditType credit, McMplItem mcItem)
            : base(message)
        {
            Credit = credit;
            IsGetAll = isGetAll;
            McItem = mcItem;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceBaseException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="isGetAll">if set to <c>true</c> the lyric service was in a "get all" mode; else <c>false</c>.</param>
        /// <param name="credit">The credit object.</param>
        /// <param name="mcItem">The Media Center MPL item.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceBaseException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Exception innerException)
            : base(message, innerException)
        {
            Credit = credit;
            IsGetAll = isGetAll;
            McItem = mcItem;
        }


        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info is null) throw new System.ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);

            info.AddValue(nameof(Credit), Credit);
            info.AddValue(nameof(IsGetAll), IsGetAll);
            info.AddValue(nameof(McItem), McItem);
        }

    }



    /// <summary>
    /// Lyric service communication exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricServiceCommunicationException : LyricServiceBaseException
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
        /// <param name="message">The message that describes the error.</param>
        public LyricServiceCommunicationException(string message)
            : base(message)
        {
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
        /// Initializes a new instance of the <see cref="LyricServiceCommunicationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricServiceCommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RequestUri = (Uri)info.GetValue(nameof(RequestUri), typeof(Uri));
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


        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(RequestUri), RequestUri);
        }

    }



    /// <summary>
    /// Exception is thrown when the IP is banned.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricServiceIpBannedException : LyricServiceBaseException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBannedException" /> class.
        /// </summary>
        public LyricServiceIpBannedException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBannedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricServiceIpBannedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBannedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LyricServiceIpBannedException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBannedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceIpBannedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBannedException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isGetAll">if set to <c>true</c> [is get all].</param>
        /// <param name="credit">The credit.</param>
        /// <param name="mcItem">The mc item.</param>
        public LyricServiceIpBannedException(string message, bool isGetAll, CreditType credit, McMplItem mcItem)
                    : base(message, isGetAll, credit, mcItem)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBannedException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isGetAll">if set to <c>true</c> [is get all].</param>
        /// <param name="credit">The credit.</param>
        /// <param name="mcItem">The mc item.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceIpBannedException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Exception innerException)
                    : base(message, isGetAll, credit, mcItem, innerException)
        {
        }

    }



    /// <summary>
    /// Exception is thrown when the IP is in danger of being banned.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricServiceIpBanWarningException : LyricServiceBaseException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBanWarningException" /> class.
        /// </summary>
        public LyricServiceIpBanWarningException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBanWarningException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected LyricServiceIpBanWarningException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBanWarningException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LyricServiceIpBanWarningException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBanWarningException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceIpBanWarningException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBanWarningException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isGetAll">if set to <c>true</c> [is get all].</param>
        /// <param name="credit">The credit.</param>
        /// <param name="mcItem">The mc item.</param>
        public LyricServiceIpBanWarningException(string message, bool isGetAll, CreditType credit, McMplItem mcItem)
                    : base(message, isGetAll, credit, mcItem)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricServiceIpBanWarningException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isGetAll">if set to <c>true</c> [is get all].</param>
        /// <param name="credit">The credit.</param>
        /// <param name="mcItem">The mc item.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public LyricServiceIpBanWarningException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Exception innerException)
                    : base(message, isGetAll, credit, mcItem, innerException)
        {
        }

    }



    /// <summary>
    /// Exception is thrown when the lyrics quota is exceeded.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ComVisible(false)]
    [Serializable]
    public class LyricsQuotaExceededException : LyricServiceBaseException
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


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isGetAll">if set to <c>true</c> [is get all].</param>
        /// <param name="credit">The credit.</param>
        /// <param name="mcItem">The mc item.</param>
        public LyricsQuotaExceededException(string message, bool isGetAll, CreditType credit, McMplItem mcItem)
                    : base(message, isGetAll, credit, mcItem)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsQuotaExceededException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isGetAll">if set to <c>true</c> [is get all].</param>
        /// <param name="credit">The credit.</param>
        /// <param name="mcItem">The mc item.</param>
        /// <param name="innerException">The inner exception.</param>
        public LyricsQuotaExceededException(string message, bool isGetAll, CreditType credit, McMplItem mcItem, Exception innerException)
                    : base(message, isGetAll, credit, mcItem, innerException)
        {
        }

    }

}
