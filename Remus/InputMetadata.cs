using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Remus {
    /// <summary>
    /// Represents a command input analyzer. This class lexes an input string to extract meaningful information.
    /// TODO: Figure out a better name for this
    /// </summary>
    internal sealed class InputMetadata {
        // Generic command syntax: commandname [options/flags] requiredArg1 requiredArg2 "required arg 3"

        private InputMetadata() {
            // Don't expose the constructor
            // Callers should rely on the Parse() method
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string? CommandName { get; private set; }

        /// <summary>
        /// Gets a read-only collection of required arguments.
        /// </summary>
        public IReadOnlyList<string> RequiredArguments { get; private set; } = null!;

        /// <summary>
        /// Gets a read-only collection of option-value pairs.
        /// </summary>
        public IReadOnlyDictionary<string, string> Options { get; private set; } = null!;

        /// <summary>
        /// Gets a read-only collection of flags.
        /// </summary>
        public IReadOnlyList<char> Flags { get; private set; } = null!;

        /// <summary>
        /// Parses a given input string by trying to match it against a list of available command names.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="availableCommandNames">A readonly collection of available command names.</param>
        public static InputMetadata Parse(string input, IReadOnlyCollection<string> availableCommandNames) {
            if (string.IsNullOrWhiteSpace(input)) {
                throw new ArgumentException(nameof(input));
            }

            Debug.Assert(input.Length <= 1024);

            var index = 0;
            var tokens = TokenizeInput(input);
            var commandName = ParseCommandName(availableCommandNames, tokens, ref index);
            var (options, flags) = ParseOptionals(tokens, ref index);
            var arguments = ParseArguments(tokens, ref index);
            return new InputMetadata {
                CommandName = commandName,
                Options = options,
                Flags = flags,
                RequiredArguments = arguments
            };
        }

        private static string? ParseCommandName(IReadOnlyCollection<string> availableCommandNames, IReadOnlyList<string> tokens, ref int index) {
            Debug.Assert(tokens.Count > 0);

            var commandName = default(string?);
            var builder = new StringBuilder(tokens[0]);
            for (var i = 1; i < tokens.Count; ++i) {
                var tempCommand = builder.ToString();
                if (!availableCommandNames.Contains(tempCommand)) {
                    break;
                }

                commandName = tempCommand;
                index = i;
                builder.Append(" " + tokens[i]);
            }

            return commandName;
        }

        private static (Dictionary<string, string>, List<char>) ParseOptionals(IReadOnlyList<string> tokens, ref int index) {
            var options = new Dictionary<string, string>();
            var flags = new List<char>();
            for (var i = index; i < tokens.Count; ++i) {
                var token = tokens[index];
                if (!token.StartsWith("-")) { // No options left to consume
                    continue;
                }

                if (!token.StartsWith("--")) {
                    flags.Add(token[1]);
                } else {
                    var option = token[2..];
                    var indexOfEquals = option.IndexOf('=');
                    if (indexOfEquals > 0) {
                        options[option[..indexOfEquals]] = option[(indexOfEquals + 1)..];
                    } else {
                        // Missing options will be handled by the command service or whatever responsible for binding and invocation
                        options[option] = tokens.ElementAtOrDefault(i + 1);
                        ++index;
                    }
                }

                ++index;
            }

            return (options, flags);
        }

        private static List<string> ParseArguments(IReadOnlyList<string> tokens, ref int index) {
            var arguments = new List<string>();
            for (; index < tokens.Count; ++index) {
                arguments.Add(tokens[index]);
            }

            return arguments;
        }

        /// <summary>
        ///     Parses arguments from the specified input string. Supports quotation marks and escape characters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A read-only list of tokens (arguments) extracted from the input string.</returns>
        private static IReadOnlyList<string> TokenizeInput(string input) {
            var parameters = new List<string>();
            var stringBuilder = new StringBuilder(input.Length);
            var inQuotes = false;
            var isEscaped = false;
            foreach (var currentCharacter in input) {
                switch (currentCharacter) {
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
                    case '\n': {
                            if (inQuotes || isEscaped) {
                                stringBuilder.Append(currentCharacter);
                                isEscaped = false;
                            }
                            else {
                                if (stringBuilder.Length == 0) {
                                    continue;
                                }

                                CommitPendingArgument();
                            }

                            break;
                        }

                    case '"': {
                            if (isEscaped) {
                                stringBuilder.Append(currentCharacter);
                                isEscaped = false;
                                continue;
                            }

                            inQuotes = !inQuotes;
                            if (inQuotes) {
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