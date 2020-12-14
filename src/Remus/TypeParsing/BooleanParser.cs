namespace Remus.TypeParsing
{
    internal sealed class BooleanParser : ITypeParser<bool>
    {
        public bool Parse(string input)
        {
            return bool.Parse(input);
        }
    }
}