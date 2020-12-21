using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Remus.Attributes;
using Remus.Exceptions;
using Remus.Extensions;
using Remus.TypeParsing;

namespace Remus
{
    /// <summary>
    ///     Represents a command service.
    /// </summary>
    public sealed class CommandManager
    {
        private const BindingFlags HandlerBindingFlags =
            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        private readonly IDictionary<object, List<Command>>
            _objectsToCommands = new Dictionary<object, List<Command>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandManager" /> class with the specified parsers.
        /// </summary>
        /// <param name="parsers">The parsers.</param>
        public CommandManager(Parsers parsers)
        {
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        /// <summary>
        ///     Gets an immutable array of all commands.
        /// </summary>
        public IEnumerable<Command> Commands => _objectsToCommands.Values.SelectMany(c => c).AsEnumerable();

        /// <summary>
        ///     Gets the parsers for this command manager.
        /// </summary>
        public Parsers Parsers { get; }

        /// <summary>
        ///     Registers commands described by the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Register(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var commandsForObject = _objectsToCommands.GetValueOrDefault(obj, new List<Command>());
            var methods = obj.GetType().GetMethods(HandlerBindingFlags);
            foreach (var method in methods)
            {
                var commandHandlerAttribute = method.GetCustomAttribute<CommandHandlerAttribute>();
                if (commandHandlerAttribute is null)
                {
                    continue;
                }
            }
        }

        /// <summary>
        ///     Deregisters commands described by the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Deregister(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var methods = obj.GetType().GetMethods(HandlerBindingFlags);
            for (var i = 0; i < methods.Length; ++i)
            {
                var method = methods[i];
                var commandAttribute = method.GetCustomAttribute<CommandHandlerAttribute>();
                if (commandAttribute is null)
                {
                    continue;
                }
            }
        }

        /// <summary>
        ///     Evaluates a specified input string using the given command sender.
        /// </summary>
        /// <param name="sender">The sender, which must not be <see langword="null" />.</param>
        /// <param name="input">The input string, which must not be <see langword="null" />.</param>
        public void Evaluate(ICommandSender sender, string input)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var inputData = InputMetadata.Parse(input, _commands.Keys.ToList());
            if (string.IsNullOrWhiteSpace(inputData.CommandName))
            {
                throw new InvalidCommandException(input);
            }

            var command = _objectsToCommands.Values.SelectMany(c => c)
                                            .FirstOrDefault(c => c.Name == inputData.CommandName);
            if (command is null)
            {
                throw new InvalidCommandException(inputData.CommandName);
            }

            command.Run(sender, inputData);
        }
    }
}