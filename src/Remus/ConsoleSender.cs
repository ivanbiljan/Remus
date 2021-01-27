using System;
using JetBrains.Annotations;

namespace Remus
{
    /// <summary>
    ///     Represents a CLI sender.
    /// </summary>
    [PublicAPI]
    public sealed class ConsoleSender : ICommandSender
    {
        /// <inheritdoc />
        public void SendMessage([NotNull] string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Console.WriteLine(message);
        }
    }
}