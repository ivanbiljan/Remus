namespace Remus.TypeParsing
{
    internal sealed class ByteParser : ITypeParser<byte>
    {
        public byte Parse(string input)
        {
            return byte.Parse(input);
        }
    }
}