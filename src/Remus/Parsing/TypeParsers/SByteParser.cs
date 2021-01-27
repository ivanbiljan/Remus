namespace Remus.Parsing.TypeParsers
{
    internal sealed class SByteParser : ITypeParser<sbyte>
    {
        public bool TryParse(string input, out sbyte obj)
        {
            return sbyte.TryParse(input, out obj);
        }

        public sbyte Parse(string input)
        {
            return sbyte.Parse(input);
        }
    }
}