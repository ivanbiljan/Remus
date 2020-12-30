namespace Remus.TypeParsing
{
    internal sealed class BooleanParser : ITypeParser<bool>
    {
        /// <inheritdoc />
        public bool Parse(string input) => bool.Parse(input);

        /// <inheritdoc />
        public bool TryParse(string input, out bool obj) => bool.TryParse(input, out obj);
    }
}