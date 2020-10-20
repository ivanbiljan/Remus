using JetBrains.Annotations;

namespace Remus.Exceptions {
    [PublicAPI]
    public sealed class InvalidCommandException : CommandException {
        public InvalidCommandException(string commandLine) : base($"Invalid command: '{commandLine}'") {
        }
    }
}