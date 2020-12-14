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
        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}