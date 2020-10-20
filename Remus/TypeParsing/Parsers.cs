using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Remus.Extensions;

namespace Remus.TypeParsing {
    public sealed class Parsers {
        //private static readonly IDictionary<Type, ITypeParser?> TypeParsers = new Dictionary<Type, ITypeParser?> {
        //    [typeof(string)] = new StringParser()
        //};

        //public static void AddParser<T>(ITypeParser<T> parser, bool overrideExisting = false) {
        //    if (TypeParsers.ContainsKey(typeof(T)) && !overrideExisting) {
        //        return;
        //    }

        //    TypeParsers[typeof(T)] = parser;
        //}

        //public static ITypeParser? GetTypeParser(Type type) => TypeParsers.GetValueOrDefault(type);

        //public static ITypeParser<T>? GetTypeParser<T>() => GetTypeParser(typeof(T)) as ITypeParser<T>;

        //public static void RemoveParser<T>() => TypeParsers.Remove(typeof(T));

        private static readonly IDictionary<Type, ITypeParser> PrimitiveParsers = new Dictionary<Type, ITypeParser> {
            [typeof(bool)] = new BooleanParser(),
            [typeof(byte)] = new ByteParser(),
            [typeof(sbyte)] = new SByteParser(),
            [typeof(short)] = new Int16Parser(),
            [typeof(ushort)] = new UInt16Parser(),
            [typeof(int)] = new Int32Parser(),
            [typeof(uint)] = new UInt32Parser(),
            [typeof(long)] = new Int64Parser(),
            [typeof(ulong)] = new UInt64Parser(),
            [typeof(char)] = new CharParser()
        };

        private readonly IDictionary<Type, ITypeParser?> _parsers = new Dictionary<Type, ITypeParser?>();

        /// <summary>
        /// Adds a parser for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type the parser encapsulates.</typeparam>
        /// <param name="parser">The parser.</param>
        public void AddParser<T>(ITypeParser<T> parser) => _parsers[typeof(T)] = parser ?? throw new ArgumentNullException(nameof(parser));

        /// <summary>
        /// Removes a parser of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public void RemoveParser<T>() => _parsers.Remove(typeof(T));

        /// <summary>
        /// Gets a parser for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The parser, or <see langword="null"/> if no parser is defined for the given type.</returns>
        public ITypeParser<T>? GetParser<T>() {
            if (typeof(T).IsPrimitive) {
                return PrimitiveParsers[typeof(T)] as ITypeParser<T>;
            }

            return _parsers.GetValueOrDefault(typeof(T)) as ITypeParser<T>;
        }
    }
}