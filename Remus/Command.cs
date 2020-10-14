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
        private readonly IDictionary<string, int> _flags = new Dictionary<string, int>();
        private readonly IDictionary<string, int> _options = new Dictionary<string, int>();
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

        [NotNull] [ItemNotNull] public IEnumerable<string> Options => _options.Keys;

        [NotNull] [ItemNotNull] public IEnumerable<string> Flags => _flags.Keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the specified <paramref name="name"/>, <paramref name="description"/> and handler method.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="handler">The handler method.</param>
        /// <param name="obj">The handler object.</param>
        internal Command(string name, string description, MethodInfo handler, object? obj = null) {
            Name = name;
            Description = description;
            _handler = handler;

            var parameters = handler.GetParameters();
            for (var i = 0; i < parameters.Length; i++) { // for loops are faster than foreach, especially on arrays
                var parameter = parameters[i];
                var flagAttribute = parameter.GetCustomAttribute<FlagAttribute>();
                if (!(flagAttribute is null) && parameter.ParameterType == typeof(bool)) {
                    if (!string.IsNullOrWhiteSpace(flagAttribute.LongName)) {
                        _flags[flagAttribute.LongName] = i;
                    }
                    if (!string.IsNullOrWhiteSpace(flagAttribute.ShortName)) {
                        _flags[flagAttribute.ShortName] = i;
                    }
                } else if (parameter.IsOptional) {
                    var optionName = parameter.GetCustomAttribute<OptionalArgumentAttribute>()?.Name ?? parameter.Name!;
                    _options[optionName] = i;
                }
                else {
                    if (_flags.Count > 0 || _options.Count > 0) {
                        throw new Exception("Optional parameters must not precede required parameters");
                    }
                }
            }

            _handlerObject = obj;
        }

        internal void Run(ICommandSender sender, LexicalAnalyzer inputData) {
            //var parameters = _handler.GetParameters();
            //var arguments = new object?[parameters.Length];
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
                }

                var parser = Parsers.GetTypeParser(parameter.ParameterType);
                if (parser is null) {
                    throw new MissingTypeParserException(parameter.ParameterType);
                }

                if (parameter.IsOptional) {
                    var optionAttribute = parameter.GetCustomAttribute<OptionalArgumentAttribute>();
                    if (optionAttribute == null) {
                        arguments[i] = parameter.ParameterType.GetDefaultValue();
                        continue;
                    }

                    var optionValue = inputData.Options.GetValueOrDefault(optionAttribute.Name);
                    if (string.IsNullOrWhiteSpace(optionValue)) {
                        arguments[i] = parameter.ParameterType.GetDefaultValue();
                        continue;
                    }

                    arguments[i] = parser.Parse(optionValue);
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