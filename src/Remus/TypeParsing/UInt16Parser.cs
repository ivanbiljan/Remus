﻿namespace Remus.TypeParsing
{
    internal sealed class UInt16Parser : ITypeParser<ushort>
    {
        public ushort Parse(string input)
        {
            return ushort.Parse(input);
        }

        public bool TryParse(string input, out ushort obj)
        {
            return ushort.TryParse(input, out obj);
        }
    }
}