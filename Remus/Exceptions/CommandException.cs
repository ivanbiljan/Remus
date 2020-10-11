using System;
using JetBrains.Annotations;

namespace Remus.Exceptions {
    /// <summary>
    /// Represents a command exception.
    /// </summary>
    [PublicAPI]
    public class CommandException : Exception {
        protected CommandException() {
            
        }
        
        protected CommandException(string message, Exception innerException) : base(message, innerException) {
            
        }
        
        protected CommandException(string message) : base(message) {
            
        }
    }
}