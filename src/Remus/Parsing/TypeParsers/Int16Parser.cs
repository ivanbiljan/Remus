namespace Remus.Parsing.TypeParsers
{
    internal sealed class Int16Parser : ITypeParser<short>
    {
        public bool TryParse(string input, out short obj)
        {
            return short.TryParse(input, out obj);
        }

        public short Parse(string input)
        {
            return short.Parse(input);
        }
    }
}