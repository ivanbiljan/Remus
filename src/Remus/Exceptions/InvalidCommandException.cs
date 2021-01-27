using System;
using JetBrains.Annotations;

namespace Remus.Exceptions
{
    /// <summary>
    ///     Represents an invalid command exception.
    /// </summary>
    [PublicAPI]
    public sealed class InvalidCommandException : Exception
    {
        public InvalidCommandException()
        {
        }

        public InvalidCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidCommandException(string message) : base(message)
        {
        }
    }
}