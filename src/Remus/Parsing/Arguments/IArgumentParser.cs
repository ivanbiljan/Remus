using System.Collections.Generic;
using JetBrains.Annotations;

namespace Remus.Parsing.Arguments
{
    /// <summary>
    ///     Defines a contract for an argument parser.
    /// </summary>
    [PublicAPI]
    public interface IArgumentParser
    {
        /// <summary>
        ///     Gets an array of separators.
        /// </summary>
        char[] Separators { get; }

        /// <summary>
        ///     Gets the command name.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        ///     Gets a read-only list of arguments.
        /// </summary>
        IReadOnlyList<string> Arguments { get; }

        /// <summary>
        ///     Gets a read-only list of flags.
        /// </summary>
        IReadOnlyList<char> Flags { get; }

        /// <summary>
        ///     Gets a read-only dictionary of options.
        /// </summary>
        IReadOnlyDictionary<string, string?> Options { get; }

        /// <summary>
        ///     Parses a given input string against a given list of commands.
        /// </summary>
        /// <param name="input">The string, which must not be <see langword="null" />.</param>
        /// <param name="availableCommands">The list of available commands.</param>
        void Parse([NotNull] string input, [NotNull] [ItemNotNull] IReadOnlyCollection<string> availableCommands);
    }
}