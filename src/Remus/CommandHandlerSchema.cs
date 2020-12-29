using System;
using System.Collections.Generic;
using System.Reflection;
using Remus.Attributes;

namespace Remus
{
    /// <summary>
    ///     Represents a command handler.
    /// </summary>
    internal sealed class CommandHandlerSchema
    {
        private readonly CommandHandlerAttribute _commandHandlerAttribute;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandHandlerSchema" /> class.
        /// </summary>
        /// <param name="commandHandlerAttribute">The command handler attribute, which must not be <see langword="null" />.</param>
        /// <param name="callback">The callback, which must not be <see langword="null" />.</param>
        /// <param name="handlerObject">The handler object, i.e the instance this handler belongs to.</param>
        public CommandHandlerSchema(
            CommandHandlerAttribute commandHandlerAttribute,
            MethodInfo callback,
            object? handlerObject = null)
        {
            _commandHandlerAttribute = commandHandlerAttribute ??
                                       throw new ArgumentNullException(nameof(commandHandlerAttribute));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            HandlerObject = handlerObject;
        }

        /// <summary>
        ///     Gets the handler object.
        /// </summary>
        internal object? HandlerObject { get; init; }

        /// <summary>
        ///     Gets the callback.
        /// </summary>
        internal MethodInfo Callback { get; init; }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        public string Description => _commandHandlerAttribute.Description;

        /// <summary>
        ///     Gets the syntax.
        /// </summary>
        public string? Syntax => _commandHandlerAttribute.Syntax;

        /// <summary>
        ///     Gets the help text.
        /// </summary>
        public string? HelpText => _commandHandlerAttribute.HelpText;
    }
}