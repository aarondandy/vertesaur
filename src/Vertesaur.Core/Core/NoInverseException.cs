using System;

namespace Vertesaur
{

    /// <summary>
    /// An exception that is thrown when a request for an inverse transformation is not available.
    /// </summary>
    /// <remarks>
    /// Often a property such as <c>HasInverse</c> should be checked before attempting to get an inverse of a transformation.
    /// </remarks>
    public class NoInverseException : Exception
    {

        private const string DefaultMessage = "There is no valid inverse.";

        /// <summary>
        /// A default no inverse exception.
        /// </summary>
        public NoInverseException() : base(null) { }

        /// <summary>
        /// A no inverse exception with a custom message.
        /// </summary>
        /// <param name="message">The optional exception message.</param>
        public NoInverseException(string message) : base(message ?? DefaultMessage) { }

        /// <summary>
        /// A no inverse exception with a custom message an nested exception. 
        /// </summary>
        /// <param name="message">The optional exception message.</param>
        /// <param name="innerException">The exception which caused this exception.</param>
        public NoInverseException(string message, Exception innerException) : base(message ?? DefaultMessage, innerException) { }

    }
}
