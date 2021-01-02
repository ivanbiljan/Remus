using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Remus.Attributes;
using Remus.Exceptions;
using Remus.Extensions;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;

namespace Remus
{
    /// <summary>
    ///     Represents a command service.
    /// </summary>
    public sealed class CommandManager : ICommandService
    {
        private const BindingFlags HandlerBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        private readonly IDictionary<object, List<Command>>
            _objectsToCommands = new Dictionary<object, List<Command>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager"/> class with the specified <see cref="IArgumentParser"/> and <see cref="IParserCollection"/>.
        /// </summary>
        /// <param name="argumentParser">The <see cref="IArgumentParser"/> instance, which must not be <see langword="null"/>.</param>
        /// <param name="parsers">The <see cref="IParserCollection"/> instance, which must not be <see langword="null"/>.</param>
        public CommandManager([NotNull] IArgumentParser argumentParser, [NotNull] IParserCollection parsers)
        {
            ArgumentParser = argumentParser ?? throw new ArgumentNullException(nameof(argumentParser));
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        /// <inheritdoc />
        public IArgumentParser ArgumentParser { get; }

        /// <inheritdoc />
        public IParserCollection Parsers { get; }

        /// <inheritdoc />
        public void Register(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        /// <inheritdoc />
        public void Deregister(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        /// <inheritdoc />
        public void Evaluate(string input, ICommandSender sender)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            
            throw new NotImplementedException();
        }
    }
}