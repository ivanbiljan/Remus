using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class Int16Parser : ITypeParser<short> {
        public short Parse(string input) => short.Parse(input);
    }
}
