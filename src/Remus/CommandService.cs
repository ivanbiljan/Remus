﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Remus.Attributes;
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

        private readonly CommandTrie _commandTrie = new CommandTrie();
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandService" /> class with the specified <see cref="ILogger" />,
        ///     <see cref="IArgumentParser" /> and <see cref="ITypeParserCollection" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" /> instance, which must not be <see langword="null" />.</param>
        /// <param name="argumentParser">The <see cref="IArgumentParser" /> instance, which must not be <see langword="null" />.</param>
        /// <param name="parsers">The <see cref="ITypeParserCollection" /> instance, which must not be <see langword="null" />.</param>
        public CommandService(
            [NotNull] ILogger logger,
            [NotNull] IArgumentParser argumentParser,
            [NotNull] ITypeParserCollection parsers)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ArgumentParser = argumentParser ?? throw new ArgumentNullException(nameof(argumentParser));
            TypeParsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        /// <inheritdoc />
        public IArgumentParser ArgumentParser { get; }

        /// <inheritdoc />
        public IEnumerable<Command> GetCommands(Predicate<Command>? predicate = null)
        {
            return _commandTrie.Commands.Where(c => predicate?.Invoke(c) ?? true);
        }

        /// <inheritdoc />
        public IEnumerable<Command> GetCommands(string name)
        {
            return _commandTrie.GetCommandSuggestions(name);
        }

        /// <inheritdoc />
        public ITypeParserCollection TypeParsers { get; }

        /// <inheritdoc />
        public void Deregister(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var commands = _commandTrie.Commands.ToList();
            for (var i = 0; i < commands.Count; ++i)
            {
                if (commands[i].HandlerObject != obj)
                {
                    continue;
                }
                
                _commandTrie.RemoveCommand(commands[i].Name);
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

            var parserResult = ArgumentParser.Parse(input, _commandTrie.Commands.Select(c => c.Name).ToList());
            if (string.IsNullOrWhiteSpace(parserResult.CommandName))
            {
                _logger.LogInformation("Invalid command.");
                return;
            }

            var command = _commandTrie.GetCommandSuggestions(parserResult.CommandName)[0];
            command.Run(sender, parserResult);
        }

        /// <inheritdoc />
        public void Register(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var methods = obj.GetType().GetMethods(HandlerBindingFlags);
            foreach (var method in methods)
            {
                var commandHandlerAttribute = method.GetCustomAttribute<CommandHandlerAttribute>();
                if (commandHandlerAttribute is null)
                {
                    continue;
                }

                var command = _commandTrie.GetCommand(commandHandlerAttribute.Name);
                if (command != null && command.HandlerObject != obj)
                {
                    _logger.LogWarning(
                        $"Command '{commandHandlerAttribute.Name}' is already defined by a different object and was skipped.");
                    continue;
                }

                var commandHandlerSchema = new CommandHandlerSchema(commandHandlerAttribute, method, obj);
                if (command is null)
                {
                    command = new Command(_logger, this, commandHandlerAttribute.Name, obj);
                    _commandTrie.AddCommand(command);
                }

                command.RegisterHandler(commandHandlerSchema);
            }
        }
    }
}