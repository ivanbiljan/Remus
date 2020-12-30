namespace Remus.TypeParsing
{
    internal sealed class UInt32Parser : ITypeParser<uint>
    {
        public uint Parse(string input)
        {
            return uint.Parse(input);
        }

        public bool TryParse(string input, out uint obj)
        {
            return uint.TryParse(input, out obj);
        }
    }
}