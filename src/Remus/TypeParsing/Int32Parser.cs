namespace Remus.TypeParsing
{
    internal sealed class Int32Parser : ITypeParser<int>
    {
        public int Parse(string input)
        {
            return int.Parse(input);
        }
    }
}