using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Remus.Exceptions;

namespace Remus
{
    /// <summary>
    ///     Represents a command.
    /// </summary>
    [PublicAPI]
    public sealed class Command : IEquatable<Command>
    {
        internal readonly CommandManager CommandManager;
        internal readonly ISet<CommandHandlerSchema> HandlerSchemas = new HashSet<CommandHandlerSchema>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Command" /> class with the specified command manager, name,
        ///     description and handler.
        /// </summary>
        /// <param name="commandManager">The <see cref="Remus.CommandManager" /> associated with this command.</param>
        /// <param name="name">The name.</param>
        internal Command(CommandManager commandManager, string name)
        {
            CommandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Registers a new command handler for this command.
        /// </summary>
        /// <param name="commandHandlerSchema">The command handler schema.</param>
        internal void RegisterHandler(CommandHandlerSchema commandHandlerSchema)
        {
            HandlerSchemas.Add(commandHandlerSchema);
        }

        /// <summary>
        ///     Runs the specified command using the most appropriate handler.
        /// </summary>
        /// <param name="sender">The command sender, which must not be <see langword="null" />.</param>
        /// <param name="inputData">The input metadata, which must not be <see langword="null" />.</param>
        internal void Run(ICommandSender sender, InputMetadata inputData)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (inputData is null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }

            var handlerSchema = Binder.ResolveMethodCall(this, sender, inputData, out var args);
            if (handlerSchema is null)
            {
                return;
            }

            try
            {
                handlerSchema.Callback.Invoke(handlerSchema.Callback.IsStatic ? null : handlerSchema.HandlerObject,
                    args);
            }
            catch (TargetInvocationException ex)
            {
                throw new CommandException("Command failed.", ex);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Command? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CommandManager.Equals(other.CommandManager) && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is Command other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CommandManager, Name);
        }
    }
}