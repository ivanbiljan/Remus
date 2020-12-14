using System;
using JetBrains.Annotations;

namespace Remus.Extensions
{
    /// <summary>
    ///     Provides extension methods for types.
    /// </summary>
    public static class TypeExtensions
    {
        [CanBeNull]
        public static object? GetDefaultValue(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}