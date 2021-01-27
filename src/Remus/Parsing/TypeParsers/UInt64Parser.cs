namespace Remus.Parsing.TypeParsers
{
    internal sealed class UInt64Parser : ITypeParser<ulong>
    {
        public bool TryParse(string input, out ulong obj)
        {
            return ulong.TryParse(input, out obj);
        }

        public ulong Parse(string input)
        {
            return ulong.Parse(input);
        }
    }
}