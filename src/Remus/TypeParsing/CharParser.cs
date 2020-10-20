using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class CharParser : ITypeParser<char> {
        public char Parse(string input) => char.Parse(input);
    }
}
