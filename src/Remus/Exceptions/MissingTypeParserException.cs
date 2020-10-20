using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remus.Exceptions {
    [PublicAPI]
    public sealed class MissingTypeParserException : CommandException {
        public MissingTypeParserException(Type type) : base($"Missing type parser for type '{type}'") {

        }
    }
}