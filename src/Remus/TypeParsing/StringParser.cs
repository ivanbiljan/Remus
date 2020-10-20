namespace Remus.TypeParsing {
    internal sealed class StringParser : ITypeParser<string> {
        public string Parse(string input) => input;
    }
}