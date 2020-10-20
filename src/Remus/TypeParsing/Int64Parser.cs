using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class Int64Parser : ITypeParser<long> {
        public long Parse(string input) => long.Parse(input);
    }
}
