using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Remus.Attributes;
using Remus.Extensions;
using Remus.Parsing;

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
            for (var i = 0; i < parameters.Length; i++) {
                var parameter = parameters[i];
                var flagAttribute = parameter.GetCustomAttribute<FlagAttribute>();
                var optionAttribute = parameter.GetCustomAttribute<OptionalArgumentAttribute>();
                if (!(flagAttribute is null) && parameter.ParameterType == typeof(bool)) {
                    if (!string.IsNullOrWhiteSpace(flagAttribute.LongName)) {
                        _flags[flagAttribute.LongName] = i;
                    }
                    if (!string.IsNullOrWhiteSpace(flagAttribute.ShortName)) {
                        _flags[flagAttribute.ShortName] = i;
                    }
                } else if (!(optionAttribute is null)) {
                    _options[optionAttribute.Name] = i;
                }
                else {
                    if (_flags.Count > 0 || _options.Count > 0) {
                        throw new Exception("Optional parameters must not precede required parameters");
                    }
                }
            }

            _handlerObject = obj;
        }

        internal void Run(ICommandSender sender, string input) {
            // Generic command syntax: commandname [options/flags] requiredArg1 requiredArg2 "required arg 3"
            var handlerParameters = _handler.GetParameters();
            var invocationArgs = new object?[handlerParameters.Length];
            for (int i = 0; i < invocationArgs.Length; ++i) {
                var parameter = handlerParameters[i];
                if (parameter.GetCustomAttribute<OptionalArgumentAttribute>() == null &&
                    parameter.GetCustomAttribute<FlagAttribute>() == null) {
                    continue;
                }

                invocationArgs[i] = parameter.ParameterType.GetDefaultValue();
            }
            
            var parsedArgs = ParseArguments(input);
            var index = 0;
            
            HandleOptionals();
            HandleArguments();

            try {
                _handler.Invoke(_handlerObject, invocationArgs);
            }
            catch (TargetInvocationException e) {
                sender.SendMessage(e.Message);
            }

            void HandleOptionals() {
                for (; index < parsedArgs.Count; ++index) {
                    var arg = parsedArgs[index];
                    if (!arg.StartsWith("-")) { // No options left to consume
                        break;
                    }

                    if (arg.StartsWith("--")) {
                        var split = arg.TrimStart('-').Split('=');
                        var option = split[0];
                        var optionIndex = _options.GetValueOrDefault(option, () => -1);
                        if (optionIndex == -1) {
                            continue;
                        }

                        var parameter = handlerParameters[optionIndex];
                        var parser = Parsers.GetRule(parameter.ParameterType);
                        if (parser == null) {
                            throw new Exception($"Missing parser for type '{parameter.ParameterType}'");
                        }
                        
                        if (split.Length > 1) {
                            invocationArgs[optionIndex] = parser(split[1]);
                        }
                        else {
                            if (index + 1 >= parsedArgs.Count) {
                                throw new Exception("Missing value for option");
                            }

                            invocationArgs[optionIndex] = parser(parsedArgs[++index]);
                        }
                    }
                    else {
                        var flag = arg.Substring(1);
                        var flagIndex = _flags.GetValueOrDefault(flag, () => -1);
                        if (flagIndex == -1) {
                            continue;
                        }

                        invocationArgs[flagIndex] = true;
                    }
                }
            }

            void HandleArguments() {
                var parameterIndex = 0;
                invocationArgs[parameterIndex++] = sender;
                for (; index < parsedArgs.Count; ++index) {
                    if (parameterIndex > handlerParameters.Length - _flags.Count - _options.Count) {
                        throw new Exception("Invalid syntax");
                    }
                    
                    var parameter = handlerParameters[parameterIndex];
                    var parsingRule = Parsers.GetRule(parameter.ParameterType);
                    if (parsingRule is null) {
                        throw new Exception($"Missing parser for type '{parameter.ParameterType.Name}'");
                    }

                    invocationArgs[parameterIndex++] = parsingRule(parsedArgs[index]);
                }
            }
        }
        
        /// <inheritdoc />
        public override string ToString() {
            return Name;
        }
        
        /// <summary>
        ///     Parses arguments from the specified input string. Supports quotation marks and escape characters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parameters.</returns>
        private static List<string> ParseArguments(string input)
        {
            var parameters = new List<string>();
            var stringBuilder = new StringBuilder(input.Length);
            var inQuotes = false;
            var isEscaped = false;
            foreach (var currentCharacter in input)
            {
                switch (currentCharacter)
                {
                    case '\\':
                        if (isEscaped) {
                            stringBuilder.Append(currentCharacter);
                            isEscaped = false;
                            continue;
                        }

                        isEscaped = true;
                        break;
                    case ' ':
                    case '\t':
                    case '\n':
                    {
                        if (inQuotes || isEscaped)
                        {
                            stringBuilder.Append(currentCharacter);
                            isEscaped = false;
                        }
                        else
                        {
                            if (stringBuilder.Length == 0)
                            {
                                continue;
                            }
                            
                            CommitPendingArgument();
                        }

                        break;
                    }

                    case '"':
                    {
                        if (isEscaped) {
                            stringBuilder.Append(currentCharacter);
                            isEscaped = true;
                            continue;
                        }
                        
                        inQuotes = !inQuotes;
                        if (inQuotes)
                        {
                            continue;
                        }
                        
                        CommitPendingArgument();
                        break;
                    }

                    default:
                        stringBuilder.Append(currentCharacter);
                        break;
                }
            }
            
            CommitPendingArgument();
            return parameters;

            void CommitPendingArgument() {
                if (stringBuilder.Length == 0) {
                    return;
                }
                
                parameters.Add(stringBuilder.ToString());
                stringBuilder.Clear();
            }
        }
    }
}