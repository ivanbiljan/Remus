using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class Int32Parser : ITypeParser<int> {
        public int Parse(string input) => int.Parse(input);
    }
}
