namespace Remus.Parsing.TypeParsers
{
    internal sealed class StringParser : ITypeParser<string>
    {
        public bool TryParse(string input, out string obj)
        {
            obj = input;
            return true;
        }

        public string Parse(string input)
        {
            return input;
        }
    }
}