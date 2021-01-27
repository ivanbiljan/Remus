namespace Remus.Parsing.TypeParsers
{
    internal sealed class UInt32Parser : ITypeParser<uint>
    {
        public bool TryParse(string input, out uint obj)
        {
            return uint.TryParse(input, out obj);
        }

        public uint Parse(string input)
        {
            return uint.Parse(input);
        }
    }
}