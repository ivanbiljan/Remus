using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remus.Parsing.Arguments
{
    /// <summary>
    /// Represents the default argument parser. Aims to replicate nix style option parsing and uses whitespace as argument separators.
    /// </summary>
    internal sealed class DefaultArgumentParser : IArgumentParser
    {
        /// <inheritdoc />
        public char[] Separators { get; } = {' ', '\t', '\n'};

        /// <inheritdoc />
        public string? CommandName { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<string> Arguments { get; private set; } = new List<string>();

        /// <inheritdoc />
        public IReadOnlyList<char> Flags { get; private set; } = new List<char>();

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string?> Options { get; private set; } = new Dictionary<string, string?>();

        /// <inheritdoc />
        public void Parse(string input, IReadOnlyCollection<string> availableCommandNames)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input must not be null or empty.", nameof(input));
            }

            if (availableCommandNames is null)
            {
                throw new ArgumentNullException(nameof(availableCommandNames));
            }

            if (availableCommandNames.Count == 0)
            {
                return;
            }

            var index = 0;
            var tokens = TokenizeInput(input);
            CommandName = ParseCommandName(availableCommandNames, tokens, ref index);
            Arguments = ParseArguments(tokens, ref index);
            Options = ParseOptionals(tokens, ref index);
        }

        private static string? ParseCommandName(
            IReadOnlyCollection<string> availableCommandNames,
            IReadOnlyList<string> tokens,
            ref int index)
        {
            // This performs a greedy match
            Debug.Assert(tokens.Count > 0);

            var commandName = default(string);
            var i = 1;
            var builder = new StringBuilder(tokens[0]);
            do
            {
                var tempCommand = builder.ToString();
                if (!availableCommandNames.Contains(tempCommand))
                {
                    break;
                }

                commandName = tempCommand;
                index = i;
                builder.Append(" " + tokens.ElementAtOrDefault(i));
            } while (i < tokens.Count);

            return commandName;
        }

        private static Dictionary<string, string?> ParseOptionals(
            IReadOnlyList<string> tokens,
            ref int index)
        {
            var currentOption = default(string);
            var options = new Dictionary<string, string?>();
            for (var i = index; i < tokens.Count; ++i)
            {
                var token = tokens[index];
                if (!token.StartsWith("-"))
                {
                    // No options left to consume
                    if (currentOption == default)
                    {
                        break;
                    }

                    options[currentOption] = token;
                    currentOption = default;
                    ++index;
                    continue;
                }

                if (currentOption != default)
                {
                    options[currentOption] = default;
                }

                if (!token.StartsWith("--"))
                {
                    currentOption = token[1].ToString();
                }
                else
                {
                    currentOption = token[2..];
                    var indexOfEquals = currentOption.IndexOf('=');
                    if (indexOfEquals > 0)
                    {
                        options[currentOption[..indexOfEquals]] = currentOption[(indexOfEquals + 1)..];
                        currentOption = default;
                    }
                }

                ++index;
            }

            return options;
        }

        private static List<string> ParseArguments(IReadOnlyList<string> tokens, ref int index)
        {
            var arguments = new List<string>();
            for (; index < tokens.Count; ++index)
            {
                if (tokens[index][0] == '-')
                {
                    break;
                }

                arguments.Add(tokens[index]);
            }

            return arguments;
        }

        /// <summary>
        ///     Parses arguments from the specified input string. Supports quotation marks and escape characters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A read-only list of tokens (arguments) extracted from the input string.</returns>
        private IReadOnlyList<string> TokenizeInput(string input)
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
                    {
                        if (isEscaped)
                        {
                            stringBuilder.Append(currentCharacter);
                            isEscaped = false;
                            continue;
                        }

                        isEscaped = true;
                        break;
                    }

                    case '"':
                    {
                        if (isEscaped)
                        {
                            stringBuilder.Append(currentCharacter);
                            isEscaped = false;
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
                    {
                        if (!Separators.Contains(currentCharacter))
                        {
                            stringBuilder.Append(currentCharacter);
                        }
                        else
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
                        }

                        break;
                    }
                }
            }

            CommitPendingArgument();
            return parameters;

            void CommitPendingArgument()
            {
                if (stringBuilder.Length == 0)
                {
                    return;
                }

                parameters.Add(stringBuilder.ToString());
                stringBuilder.Clear();
            }
        }
    }
}