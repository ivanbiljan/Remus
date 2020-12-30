﻿namespace Remus.TypeParsing
{
    internal sealed class Int32Parser : ITypeParser<int>
    {
        public int Parse(string input)
        {
            return int.Parse(input);
        }

        public bool TryParse(string input, out int obj)
        {
            return int.TryParse(input, out obj);
        }
    }
}