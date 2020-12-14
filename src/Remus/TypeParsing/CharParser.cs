namespace Remus.TypeParsing
{
    internal sealed class CharParser : ITypeParser<char>
    {
        public char Parse(string input)
        {
            return char.Parse(input);
        }
    }
}