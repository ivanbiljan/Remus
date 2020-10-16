using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly MethodInfo _handler;
        private readonly object? _handlerObject;
        
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
        public string? Syntax { get; }

        // TODO: Reconsider this
        /// <summary>
        /// Gets the help text (à la a man page).
        /// </summary>
        public string HelpText {
            get {
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the specified <paramref name="name"/>, <paramref name="description"/> and handler method.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="handler">The handler method.</param>
        /// <param name="obj">The handler object.</param>
        internal Command(string name, string description, MethodInfo handler, object? obj = null) {
            var requiredParameters = new List<string>();
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

                requiredParameters.Add(parameter.Name!);
            }

            Name = name;
            Description = description;
            Syntax = $"{Name} [options/flags] {string.Join(" ", requiredParameters.Select(p => $"<{p}>"))}";
            _handler = handler;
            _handlerObject = obj;
        }

        internal void Run(ICommandSender sender, LexicalAnalyzer inputData) {
            var arguments = BindParameters(sender, inputData);
            try {
                _handler.Invoke(_handlerObject, arguments);
            } catch (TargetInvocationException ex) {
                sender.SendMessage($"An unknown error has occured while executing the command: '{ex.Message}'");
            }
        }

        private object?[] BindParameters(ICommandSender sender, LexicalAnalyzer inputData) {
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

                var parser = Parsers.GetTypeParser(parameter.ParameterType);
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
                    arguments[i] = flagAttribute != null ? true : parser.Parse(inputData.RequiredArguments[argumentIndex++]);
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