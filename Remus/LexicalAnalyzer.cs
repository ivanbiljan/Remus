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
    /// </summary>
    internal sealed class LexicalAnalyzer {
        private LexicalAnalyzer() {
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
        public IReadOnlyCollection<string> RequiredArguments { get; private set; } = null!;

        /// <summary>
        /// Gets a read-only collection of option-value pairs.
        /// </summary>
        public IReadOnlyCollection<(string, string)> Options { get; private set; } = null!;

        /// <summary>
        /// Gets a read-only collection of flags.
        /// </summary>
        public IReadOnlyCollection<string> Flags { get; private set; } = null!;

        /// <summary>
        /// Parses a given input string by trying to match it against a list of available command names.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="availableCommandNames">A readonly collection of available command names.</param>
        public static LexicalAnalyzer Parse(string input, IReadOnlyList<string> availableCommandNames) {
            if (string.IsNullOrWhiteSpace(input)) {
                throw new ArgumentException(nameof(input));
            }

            Debug.Assert(input.Length <= 1024);

            var index = 0;
            var tokens = TokenizeInput(input);
            var commandName = ParseCommandName(availableCommandNames, tokens, ref index);
            if (commandName == null) {
                return new LexicalAnalyzer {
                    CommandName = null,
                    RequiredArguments = Array.Empty<string>(),
                    Options = Array.Empty<(string, string)>(),
                    Flags = Array.Empty<string>()
                };
            }

            var (options, flags) = ParseOptionals(tokens, ref index);
            var arguments = ParseArguments(tokens, ref index);
            return new LexicalAnalyzer {
                CommandName = commandName,
                Options = options,
                Flags = flags,
                RequiredArguments = arguments
            };
        }

        private static string? ParseCommandName(IReadOnlyList<string> availableCommandNames, IReadOnlyList<string> tokens, ref int index) {
            Debug.Assert(tokens.Count > 0);

            var commandName = default(string?);
            var builder = new StringBuilder(tokens[0]);
            for (index = 0; index < tokens.Count; ++index) {
                var temp = builder.Append(" " + tokens[index]).ToString();
                if (!availableCommandNames.Contains(temp)) {
                    break;
                }

                commandName = temp;
            }

            return commandName;
        }

        private static (List<(string, string)>, List<string>) ParseOptionals(IReadOnlyList<string> tokens, ref int index) {
            var options = new List<(string, string)>();
            var flags = new List<string>();
            for (; index < tokens.Count; ++index) {
                var token = tokens[index];
                if (!token.StartsWith("-")) { // No options left to consume
                    continue;
                }

                if (!token.StartsWith("--")) {
                    flags.Add(token[1..]);
                } else {
                    var option = token[2..];
                    var indexOfEquals = option.IndexOf('=');
                    if (indexOfEquals > 0) {
                        options.Add((option[..indexOfEquals], option[(indexOfEquals + 1)..]));
                    } else {
                        // Missing options will be handled by the command service or whatever responsible for binding and invocation
                        options.Add((option, tokens.ElementAtOrDefault(index + 1)));
                    }
                }
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
                                isEscaped = true;
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
