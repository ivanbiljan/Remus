using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Remus.Extensions;

namespace Remus.TypeParsing {
    public static class Parsers {
        private static readonly IDictionary<Type, ITypeParser?> TypeParsers = new Dictionary<Type, ITypeParser?> {
            [typeof(string)] = new StringParser()
        };

        public static void AddParser<T>(ITypeParser<T> parser, bool overrideExisting = false) {
            if (TypeParsers.ContainsKey(typeof(T)) && !overrideExisting) {
                return;
            }

            TypeParsers[typeof(T)] = parser;
        }

        public static ITypeParser? GetTypeParser(Type type) => TypeParsers.GetValueOrDefault(type);

        public static ITypeParser<T>? GetTypeParser<T>() => GetTypeParser(typeof(T)) as ITypeParser<T>;

        public static void RemoveParser<T>() => TypeParsers.Remove(typeof(T));
    }
}