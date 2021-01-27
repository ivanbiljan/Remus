namespace Remus.Parsing.TypeParsers
{
    internal sealed class Int32Parser : ITypeParser<int>
    {
        public bool TryParse(string input, out int obj)
        {
            return int.TryParse(input, out obj);
        }

        public int Parse(string input)
        {
            return int.Parse(input);
        }
    }
}