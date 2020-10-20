using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Remus.Extensions {
    /// <summary>
    /// Provides extension methods for the <see cref="IDictionary{TKey,TValue}"/> type.
    /// </summary>
    public static class IDictionaryExtensions {
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default) where TKey : notnull {
            if (dictionary is null) {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (key is null) {
                throw new ArgumentNullException(nameof(key));
            }

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}