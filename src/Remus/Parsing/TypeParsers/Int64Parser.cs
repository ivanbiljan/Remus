﻿namespace Remus.Parsing.TypeParsers
{
    internal sealed class Int64Parser : ITypeParser<long>
    {
        public bool TryParse(string input, out long obj)
        {
            return long.TryParse(input, out obj);
        }

        public long Parse(string input)
        {
            return long.Parse(input);
        }
    }
}