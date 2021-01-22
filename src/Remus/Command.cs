using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Remus.Parsing.Arguments;

namespace Remus
{
    /// <summary>
    ///     Represents a command.
    /// </summary>
    [PublicAPI]
    public sealed class Command : IEquatable<Command>
    {
        private readonly ILogger _logger;

        internal readonly ICommandService CommandService;
        internal readonly object? HandlerObject;
        internal readonly ISet<CommandHandlerSchema> HandlerSchemas = new HashSet<CommandHandlerSchema>();

        internal Command(
            [NotNull] ILogger logger,
            [NotNull] ICommandService commandService,
            [NotNull] string name,
            object? handlerObject = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CommandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            HandlerObject = handlerObject;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
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

            return CommandService.Equals(other.CommandService) && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is Command other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(CommandService, Name);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        ///     Registers a new command handler for this command.
        /// </summary>
        /// <param name="commandHandlerSchema">The command handler schema, which must not be <see langword="null" />.</param>
        internal void RegisterHandler([NotNull] CommandHandlerSchema commandHandlerSchema)
        {
            if (commandHandlerSchema is null)
            {
                throw new ArgumentNullException(nameof(commandHandlerSchema));
            }

            HandlerSchemas.Add(commandHandlerSchema);
        }

        /// <summary>
        ///     Runs the specified command using the most appropriate handler.
        /// </summary>
        /// <param name="sender">The command sender, which must not be <see langword="null" />.</param>
        /// <param name="inputData">The input metadata, which must not be <see langword="null" />.</param>
        internal void Run([NotNull] ICommandSender sender, [NotNull] ArgumentParserResult inputData)
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
                _logger.LogError($"Something went wrong while executing command '{Name}':\n{ex}");
            }
        }
    }
}