using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.TypeParsing {
    internal sealed class SByteParser : ITypeParser<sbyte> {
        public sbyte Parse(string input) => sbyte.Parse(input);
    }
}
