using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Remus.Parsing.TypeParsers {
    /// <summary>
    /// Defines a contract for a parser manager.
    /// </summary>
    public interface IParserCollection
    {
        /// <summary>
        /// Adds a type parser.
        /// </summary>
        /// <param name="parser">The parser, which must not be <see langword="null"/>.</param>
        void AddParser([NotNull] ITypeParser parser);

        /// <summary>
        /// Adds a parser that operators on instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type this parser parses.</typeparam>
        /// <param name="parser">The parser, which must not be <see langword="null"/>.</param>
        void AddParser<T>([NotNull] ITypeParser<T> parser);

        /// <summary>
        /// Removes a type parser.
        /// </summary>
        /// <param name="parser">The parser, which must not be <see langword="null"/>.</param>
        void RemoveParser([NotNull] ITypeParser parser);

        /// <summary>
        /// Removes a parser that operators on instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type this parser parses.</typeparam>
        /// <param name="parser">The parser, which must not be <see langword="null"/>.</param>
        void RemoveParser<T>([NotNull] ITypeParser<T> parser);

        /// <summary>
        /// Gets a parser for the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        void GetParser<T>();

        /// <summary>
        /// Gets a parser for the specified type.
        /// </summary>
        /// <param name="type">The type, which must not be <see langword="null"/>.</param>
        void GetParser([NotNull] Type type);
    }
}