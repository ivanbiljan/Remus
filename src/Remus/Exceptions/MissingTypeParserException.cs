using System;
using JetBrains.Annotations;

namespace Remus.Exceptions
{
    [PublicAPI]
    public sealed class MissingTypeParserException : CommandException
    {
        public MissingTypeParserException(Type type) : base($"Missing type parser for type '{type}'")
        {
        }
    }
}