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
        ///     Parses a given input string against a given list of commands.
        /// </summary>
        /// <param name="input">The string, which must not be <see langword="null" />.</param>
        /// <param name="availableCommands">The list of available commands.</param>
        ArgumentParserResult Parse([NotNull] string input, [NotNull] [ItemNotNull] IReadOnlyCollection<string> availableCommands);
    }
}