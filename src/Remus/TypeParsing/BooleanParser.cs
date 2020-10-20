using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class BooleanParser : ITypeParser<bool> {
        public bool Parse(string input) => bool.Parse(input);
    }
}
