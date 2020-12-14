namespace Remus.TypeParsing
{
    internal sealed class UInt32Parser : ITypeParser<uint>
    {
        public uint Parse(string input)
        {
            return uint.Parse(input);
        }
    }
}