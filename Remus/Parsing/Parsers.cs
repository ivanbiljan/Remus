using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Remus.Extensions;

namespace Remus.Parsing {
    public static class Parsers {
        private static readonly IDictionary<Type, Func<string, object>> ParsingRules =
            new Dictionary<Type, Func<string, object>> {
                [typeof(byte)] = input => byte.Parse(input),
                [typeof(sbyte)] = input => sbyte.Parse(input),
                [typeof(short)] = input => short.Parse(input),
                [typeof(ushort)] = input => ushort.Parse(input),
                [typeof(int)] = input => int.Parse(input),
                [typeof(uint)] = input => uint.Parse(input),
                [typeof(long)] = input => long.Parse(input),
                [typeof(ulong)] = input => ulong.Parse(input),
                [typeof(float)] = input => float.Parse(input),
                [typeof(double)] = input => double.Parse(input),
                [typeof(decimal)] = input => decimal.Parse(input),
                [typeof(bool)] = input => bool.Parse(input),
                [typeof(char)] = input => input[0],
                [typeof(string)] = input => input
            };

        public static void AddRule(Type type, Func<string, object> rule, bool overrideExisting = false) {
            if (ParsingRules.ContainsKey(type) && !overrideExisting) {
                return;
            }

            ParsingRules[type] = rule;
        }

        public static void RemoveRule(Type type) => ParsingRules.Remove(type);

        public static Func<string, object>? GetRule(Type type) => ParsingRules.GetValueOrDefault(type);
    }
}