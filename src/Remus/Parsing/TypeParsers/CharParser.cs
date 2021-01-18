namespace Remus.Parsing.TypeParsers
{
    internal sealed class CharParser : ITypeParser<char>
    {
        public bool TryParse(string input, out char obj)
        {
            return char.TryParse(input, out obj);
        }

        public char Parse(string input)
        {
            return char.Parse(input);
        }
    }
}