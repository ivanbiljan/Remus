namespace Remus.TypeParsing
{
    internal sealed class UInt64Parser : ITypeParser<ulong>
    {
        public ulong Parse(string input)
        {
            return ulong.Parse(input);
        }

        public bool TryParse(string input, out ulong obj)
        {
            return ulong.TryParse(input, out obj);
        }
    }
}