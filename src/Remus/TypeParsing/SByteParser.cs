namespace Remus.TypeParsing
{
    internal sealed class SByteParser : ITypeParser<sbyte>
    {
        public sbyte Parse(string input)
        {
            return sbyte.Parse(input);
        }
    }
}