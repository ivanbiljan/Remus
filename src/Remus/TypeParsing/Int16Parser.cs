namespace Remus.TypeParsing
{
    internal sealed class Int16Parser : ITypeParser<short>
    {
        public short Parse(string input)
        {
            return short.Parse(input);
        }
    }
}