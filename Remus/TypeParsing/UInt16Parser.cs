using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class UInt16Parser : ITypeParser<ushort> {
        public ushort Parse(string input) => ushort.Parse(input);
    }
}
