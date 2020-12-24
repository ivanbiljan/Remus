namespace Remus
{
    /// <summary>
    ///     Describes a command sender.
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        ///     Sends a specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SendMessage(string message);
    }
}