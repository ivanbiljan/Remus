namespace Remus.Parsing.TypeParsers
{
    /// <summary>
    ///     Describes a generic type parser.
    /// </summary>
    /// <typeparam name="T">The type this parser encapsulates.</typeparam>
    public interface ITypeParser<T> : ITypeParser
    {
        /// <inheritdoc />
        object? ITypeParser.Parse(string input)
        {
            return Parse(input);
        }

        /// <summary>
        ///     Parses an object of type <typeparamref name="T" /> from the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed object.</returns>
        new T Parse(string input);

        /// <summary>
        /// Attempts to parse a given string into the specified type. Returns a boolean value indicating the success.
        /// </summary>
        /// <param name="input">The input string, which must not be <see langword="null"/>.</param>
        /// <param name="obj">The parsed object.</param>
        /// <returns>A boolean value indicating the success.</returns>
        bool TryParse(string input, out T obj);
    }
}