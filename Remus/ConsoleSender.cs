using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remus {
    /// <summary>
    /// Represents a CLI sender.
    /// </summary>
    [PublicAPI]
    public sealed class ConsoleSender : ICommandSender {
        public void SendMessage(string message) => Console.WriteLine(message);
    }
}