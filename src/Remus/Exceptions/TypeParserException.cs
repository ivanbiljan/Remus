using System;
using JetBrains.Annotations;

namespace Remus.Exceptions
{
    /// <summary>
    /// Represents a type parser exception.
    /// </summary>
    [PublicAPI]
    public sealed class TypeParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParserException"/> class.
        /// </summary>
        public TypeParserException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParserException"/> class with the specified message.
        /// </summary>
        /// <param name="message">The message, which must not be <see langword="null"/>.</param>
        public TypeParserException(string message) : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParserException"/> class with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The message, which must not be <see langword="null"/>.</param>
        /// <param name="innerException">The inner exception, which must not be <see langword="null"/>.</param>
        public TypeParserException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}