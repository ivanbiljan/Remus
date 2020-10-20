using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class UInt64Parser : ITypeParser<ulong> {
        public ulong Parse(string input) => ulong.Parse(input);
    }
}
