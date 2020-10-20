using System;
using JetBrains.Annotations;

namespace Remus.Exceptions {
    /// <summary>
    /// Represents a command exception.
    /// </summary>
    [PublicAPI]
    public class CommandException : Exception {
        public CommandException() {
            
        }
        
        public CommandException(string message, Exception innerException) : base(message, innerException) {
            
        }
        
        public CommandException(string message) : base(message) {
            
        }
    }
}