using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Remus.Attributes;
using Remus.Exceptions;
using Remus.Extensions;
using Remus.TypeParsing;

namespace Remus {
    /// <summary>
    /// Represents a command.
    /// </summary>
    [PublicAPI]
    public sealed class Command {
        // This is only used so we don't have to loop through the handler's parameters each time HelpText is accessed
        private readonly ISet<(string, string)> _options = new HashSet<(string, string)>();
        private readonly ISet<(char, string)> _flags = new HashSet<(char, string)>();

        private readonly CommandManager _commandManager;
        private readonly MethodInfo _handler;
        private readonly object? _handlerObject;
        private string? _helpText;
        
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the syntax.
        /// </summary>
        public string? Syntax { get; set; }

        /// <summary>
        /// Gets an immutable array of flags this command defines.
        /// </summary>
        public ImmutableArray<char> Flags => _flags.Select(f => f.Item1).ToImmutableArray();

        /// <summary>
        /// Gets an immutable array of options this command defines.
        /// </summary>
        public ImmutableArray<string> Options => _options.Select(o => o.Item1).ToImmutableArray();

        // TODO: Reconsider this
        /// <summary>
        /// Gets the help text (à la a man page).
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string HelpText {
            get {
                if (_helpText != null) {
                    return _helpText;
                }

                var helpTextBuilder = new StringBuilder("NAME\n")
                    .AppendLine($"\t{Name}")
                    .AppendLine("DESCRIPTION")
                    .AppendLine($"\t{Description}")
                    .AppendLine("OPTIONS\n");
                foreach (var (option, description) in _options) {
                    helpTextBuilder.AppendLine($"\t{option} - {description}");
                }

                helpTextBuilder.AppendLine("FLAGS\n");
                foreach (var (flag, description) in _flags) {
                    helpTextBuilder.AppendLine($"\t{flag} - {description}");
                }

                return helpTextBuilder.ToString();
            }
            set => _helpText = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the specified command manager, name, description and handler.
        /// </summary>
        /// <param name="commandManager">The <see cref="CommandManager"/> associated with this command.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="handler">The handler method.</param>
        /// <param name="obj">The handler object.</param>
        internal Command(CommandManager commandManager, string name, string description, MethodInfo handler, object? obj = null) {
            if (commandManager is null) {
                throw new ArgumentNullException(nameof(commandManager));
            }

            if (name is null) {
                throw new ArgumentNullException(nameof(name));
            }

            if (handler is null) {
                throw new ArgumentNullException(nameof(handler));
            }
            
            var parameters = handler.GetParameters();
            for (var i = 0; i < parameters.Length; ++i) { // for loops are faster than foreach, especially on arrays
                var parameter = parameters[i];
                if (parameter.ParameterType == typeof(ICommandSender)) {
                    continue;
                }

                if (parameter.IsOptional) {
                    var optionalArgumentAttribute = parameter.GetCustomAttribute<OptionalArgumentAttribute>();
                    _options.Add((optionalArgumentAttribute?.Name ?? parameter.Name!, optionalArgumentAttribute?.Description ?? "N/A"));
                    continue;
                }

                if (parameter.ParameterType == typeof(bool)) {
                    var flagAttribute = parameter.GetCustomAttribute<FlagAttribute>();
                    if (flagAttribute != null) {
                        _flags.Add((flagAttribute.Identifier, flagAttribute.Description));
                        continue;
                    }
                }
            }

            _commandManager = commandManager;
            _handler = handler;
            _handlerObject = obj;

            Name = name;
            Description = description;
        }

        internal void Run(ICommandSender sender, InputMetadata inputData) {
            var arguments = BindParameters(sender, inputData);
            try {
                _handler.Invoke(_handlerObject, arguments);
            } catch (TargetInvocationException ex) {
                sender.SendMessage($"An unknown error has occured while executing the command: '{ex.Message}'");
            }
        }

        private object?[] BindParameters(ICommandSender sender, InputMetadata inputData) {
            var parameters = _handler.GetParameters();
            if (parameters.Length == 0) {
                return Array.Empty<object>();
            }

            var argumentIndex = 0;
            var arguments = new object?[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i) {
                var parameter = parameters[i];
                if (parameter.ParameterType == typeof(ICommandSender)) {
                    arguments[i] = sender;
                    continue;
                }

                var parser = _commandManager.Parsers.GetParser(parameter.ParameterType);
                if (parser is null) {
                    throw new MissingTypeParserException(parameter.ParameterType);
                }

                if (parameter.IsOptional) {
                    var optionName = parameter.GetCustomAttribute<OptionalArgumentAttribute>()?.Name ?? parameter.ParameterType.Name;
                    var result = parameter.ParameterType.GetDefaultValue();
                    var inputOptionValue = inputData.Options.GetValueOrDefault(optionName);
                    if (!string.IsNullOrWhiteSpace(inputOptionValue)) {
                        result = parser.Parse(inputOptionValue);
                    }

                    arguments[i] = result;

                } else {
                    var flagAttribute = parameter.GetCustomAttribute<FlagAttribute>();
                    if (flagAttribute != null) {
                        if (parameter.ParameterType == typeof(bool) && inputData.Flags.Contains(flagAttribute.Identifier)) {
                            arguments[i] = true;
                        }
                    } else {
                        arguments[i] = parser.Parse(inputData.RequiredArguments[argumentIndex++]);
                    }
                }
            }

            return arguments;
        }

        /// <inheritdoc />
        public override string ToString() {
            return Name;
        }
    }
}