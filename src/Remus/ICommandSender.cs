namespace Remus
{
    /// <summary>
    ///     Describes a command sender.
    /// </summary>
    public interface ICommandSender
    {
        void SendMessage(string message);
    }
}