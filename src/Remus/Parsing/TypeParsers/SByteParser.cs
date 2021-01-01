namespace Remus.Parsing.TypeParsers
{
    internal sealed class SByteParser : ITypeParser<sbyte>
    {
        public sbyte Parse(string input)
        {
            return sbyte.Parse(input);
        }

        public bool TryParse(string input, out sbyte obj)
        {
            return sbyte.TryParse(input, out obj);
        }
    }
}