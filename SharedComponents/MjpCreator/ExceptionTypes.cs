using System;
using System.Runtime.Serialization;


namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// Syntax exception.
    /// </summary>
    [Serializable]
    public class PathNotFoundException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PathNotFoundException"/> class. Default constructor.
        /// </summary>
        public PathNotFoundException()
        {
        } // Default constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="PathNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PathNotFoundException(string message)
            : base(message)
        {
        } // Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="PathNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PathNotFoundException(string message,
            Exception innerException)
            : base(message, innerException)
        {
        } // Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="PathNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or <see cref="System.Exception.HResult"/> is zero (0).</exception>
        protected PathNotFoundException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        } // Constructor


        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    } // class PathNotFoundException



    /// <summary>
    /// Syntax exception.
    /// </summary>
    [Serializable]
    public class SyntaxException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxException"/> class. Default constructor.
        /// </summary>
        public SyntaxException()
        {
        } // Default constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SyntaxException(string message)
            : base(message)
        {
        } // Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public SyntaxException(string message,
            Exception innerException)
            : base(message, innerException)
        {
        } // Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or <see cref="System.Exception.HResult"/> is zero (0).</exception>
        protected SyntaxException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        } // Constructor


        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    } // class SyntaxException

}
