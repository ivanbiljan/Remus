using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class UInt32Parser : ITypeParser<uint> {
        public uint Parse(string input) => uint.Parse(input);
    }
}
