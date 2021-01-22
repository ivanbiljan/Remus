using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Remus.Parsing.Arguments
{
    /// <summary>
    /// Encapsulates information obtained by parsing an input string via <see cref="IArgumentParser.Parse(string, IReadOnlyCollection{string})"/>.
    /// </summary>
    [PublicAPI]
    public sealed class ArgumentParserResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParserResult"/> class with the specified command name.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        public ArgumentParserResult(string? commandName)
        {
            CommandName = commandName;
        }

        /// <summary>
        ///     Gets a read-only list of arguments.
        /// </summary>
        public IReadOnlyList<string> Arguments { get; init; } = new List<string>();

        /// <summary>
        ///     Gets the command name.
        /// </summary>
        public string? CommandName { get; init; }

        /// <summary>
        ///     Gets a read-only list of flags.
        /// </summary>
        public IReadOnlyList<char> Flags { get; init; } = new List<char>();

        /// <summary>
        ///     Gets a read-only dictionary of options.
        /// </summary>
        public IReadOnlyDictionary<string, string?> Options { get; init; } = new Dictionary<string, string?>();
    }
}