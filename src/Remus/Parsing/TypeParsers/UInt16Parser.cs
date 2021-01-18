namespace Remus.Parsing.TypeParsers
{
    internal sealed class UInt16Parser : ITypeParser<ushort>
    {
        public bool TryParse(string input, out ushort obj)
        {
            return ushort.TryParse(input, out obj);
        }

        public ushort Parse(string input)
        {
            return ushort.Parse(input);
        }
    }
}