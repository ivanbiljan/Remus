namespace Remus.TypeParsing
{
    internal sealed class Int64Parser : ITypeParser<long>
    {
        public long Parse(string input)
        {
            return long.Parse(input);
        }
    }
}