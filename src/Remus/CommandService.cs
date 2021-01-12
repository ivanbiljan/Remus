﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
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
    public sealed class CommandService : ICommandService
    {
        private const BindingFlags HandlerBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        private readonly ILogger _logger;
        private readonly IDictionary<object, List<Command>>
            _objectsToCommands = new Dictionary<object, List<Command>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService"/> class with the specified <see cref="ILogger"/>, <see cref="IArgumentParser"/> and <see cref="ITypeParserCollection"/>.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance, which must not be <see langword="null"/>.</param>
        /// <param name="argumentParser">The <see cref="IArgumentParser"/> instance, which must not be <see langword="null"/>.</param>
        /// <param name="parsers">The <see cref="ITypeParserCollection"/> instance, which must not be <see langword="null"/>.</param>
        public CommandService([NotNull] ILogger logger, [NotNull] IArgumentParser argumentParser, [NotNull] ITypeParserCollection parsers)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ArgumentParser = argumentParser ?? throw new ArgumentNullException(nameof(argumentParser));
            TypeParsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        /// <inheritdoc />
        public IArgumentParser ArgumentParser { get; }

        /// <inheritdoc />
        public ITypeParserCollection TypeParsers { get; }

        /// <inheritdoc />
        public void Register(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var commandsForObject = _objectsToCommands.GetValueOrDefault(obj, new List<Command>())!;
            var methods = obj.GetType().GetMethods(HandlerBindingFlags);
            foreach (var method in methods)
            {
                var commandHandlerAttribute = method.GetCustomAttribute<CommandHandlerAttribute>();
                if (commandHandlerAttribute is null)
                {
                    continue;
                }

                if (_objectsToCommands.Any(kvp =>
                    kvp.Value.Contains(new Command(_logger, this, commandHandlerAttribute.Name)) && kvp.Key != obj))
                {
                    _logger.LogWarning(
                        $"Command {commandHandlerAttribute.Name} is already defined by a different object and was skipped.");
                    continue;
                }

                var commandHandlerSchema = new CommandHandlerSchema(commandHandlerAttribute, method, obj);
                var command = commandsForObject.FirstOrDefault(c => c.Name == commandHandlerAttribute.Name);
                if (command is null) {
                    command = new Command(_logger, this, commandHandlerAttribute.Name);
                    commandsForObject.Add(command);
                }

                command.HandlerSchemas.Add(commandHandlerSchema);
            }

            _objectsToCommands[obj] = commandsForObject;
        }

        /// <inheritdoc />
        public void Deregister(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _objectsToCommands.Remove(obj);
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

            ArgumentParser.Parse(input, _objectsToCommands.Values.SelectMany(c => c.Select(c => c.Name)).ToList());
            if (string.IsNullOrWhiteSpace(ArgumentParser.CommandName))
            {
            }
        }
    }
}