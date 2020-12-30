namespace Remus.TypeParsing
{
    internal sealed class Int64Parser : ITypeParser<long>
    {
        public long Parse(string input)
        {
            return long.Parse(input);
        }

        public bool TryParse(string input, out long obj)
        {
            return long.TryParse(input, out obj);
        }
    }
}