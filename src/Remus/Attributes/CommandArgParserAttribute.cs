using System;
using JetBrains.Annotations;
using Remus.Parsing.TypeParsers;

namespace Remus.Attributes
{
    /// <summary>
    ///     This attribute is used to mark parameters that wish to rely on specialized type parsers when parsing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [PublicAPI]
    public sealed class CommandArgParserAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandArgParserAttribute" /> with the specified type of parser.
        /// </summary>
        /// <param name="parserType">The type of parser.</param>
        public CommandArgParserAttribute([NotNull] Type parserType)
        {
            if (parserType is null)
            {
                throw new ArgumentNullException(nameof(parserType));
            }

            if (!typeof(ITypeParser).IsAssignableFrom(parserType))
            {
                throw new ArgumentException($"Type must implement {nameof(ITypeParser)}", nameof(parserType));
            }

            ParserType = parserType;
        }

        /// <summary>
        ///     Gets the type of parser.
        /// </summary>
        public Type ParserType { get; }
    }
}