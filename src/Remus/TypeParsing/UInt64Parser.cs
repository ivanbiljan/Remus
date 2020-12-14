namespace Remus.TypeParsing
{
    internal sealed class UInt64Parser : ITypeParser<ulong>
    {
        public ulong Parse(string input)
        {
            return ulong.Parse(input);
        }
    }
}