namespace Remus.Parsing.TypeParsers
{
    internal sealed class BooleanParser : ITypeParser<bool>
    {
        /// <inheritdoc />
        public bool Parse(string input)
        {
            return bool.Parse(input);
        }

        /// <inheritdoc />
        public bool TryParse(string input, out bool obj)
        {
            return bool.TryParse(input, out obj);
        }
    }
}