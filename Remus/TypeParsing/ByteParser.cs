using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class ByteParser : ITypeParser<byte> {
        public byte Parse(string input) => byte.Parse(input);
    }
}
