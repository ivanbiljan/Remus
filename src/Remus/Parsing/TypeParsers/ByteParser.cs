namespace Remus.Parsing.TypeParsers
{
    internal sealed class ByteParser : ITypeParser<byte>
    {
        public byte Parse(string input)
        {
            return byte.Parse(input);
        }

        public bool TryParse(string input, out byte obj)
        {
            return byte.TryParse(input, out obj);
        }
    }
}