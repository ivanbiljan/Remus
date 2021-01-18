using System;
using System.Collections.Generic;
using Remus.Extensions;

namespace Remus.Parsing.TypeParsers
{
    /// <summary>
    ///     Represents a collection of parsers.
    /// </summary>
    public sealed class Parsers : ITypeParserCollection
    {
        private static readonly IDictionary<Type, ITypeParser> PrimitiveParsers = new Dictionary<Type, ITypeParser>
        {
            [typeof(bool)] = new BooleanParser(),
            [typeof(byte)] = new ByteParser(),
            [typeof(sbyte)] = new SByteParser(),
            [typeof(short)] = new Int16Parser(),
            [typeof(ushort)] = new UInt16Parser(),
            [typeof(int)] = new Int32Parser(),
            [typeof(uint)] = new UInt32Parser(),
            [typeof(long)] = new Int64Parser(),
            [typeof(ulong)] = new UInt64Parser(),
            [typeof(char)] = new CharParser(),
            [typeof(string)] = new StringParser()
        };

        private readonly IDictionary<Type, ITypeParser?> _parsers = new Dictionary<Type, ITypeParser?>();

        /// <inheritdoc />
        public ITypeParser? GetParser(Type type)
        {
            if (type.IsPrimitive || type == typeof(string))
            {
                return PrimitiveParsers[type];
            }

            return _parsers.GetValueOrDefault(type);
        }

        /// <inheritdoc />
        public ITypeParser<T>? GetParser<T>()
        {
            return GetParser(typeof(T)) as ITypeParser<T>;
        }

        /// <inheritdoc />
        public void AddParser(Type type, ITypeParser parser)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            _parsers[type] = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        /// <inheritdoc />
        public void AddParser<T>(ITypeParser<T> parser)
        {
            AddParser(typeof(T), parser);
        }

        /// <inheritdoc />
        public void RemoveParser(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            _parsers.Remove(type);
        }

        /// <inheritdoc />
        public void RemoveParser<T>()
        {
            RemoveParser(typeof(T));
        }
    }
}