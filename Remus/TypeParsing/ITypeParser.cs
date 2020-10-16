namespace Remus.TypeParsing {
    /// <summary>
    /// Describes a type parser.
    /// </summary>
    public interface ITypeParser {
        /// <summary>
        /// Parses an object from the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed object.</returns>
        object? Parse(string input);
    }
}