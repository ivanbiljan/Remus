namespace Remus.Parsing {
    /// <summary>
    /// Describes a generic type parser.
    /// </summary>
    /// <typeparam name="T">The type this parser encapsulates.</typeparam>
    public interface ITypeParser<out T> : ITypeParser {
        /// <summary>
        /// Parses an object of type <typeparamref name="T"/> from the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed object.</returns>
        new T Parse(string input);

        object? ITypeParser.Parse(string input) => Parse(input);
    }
}