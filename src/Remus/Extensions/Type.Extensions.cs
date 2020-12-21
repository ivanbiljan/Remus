using System;
using JetBrains.Annotations;

namespace Remus.Extensions
{
    /// <summary>
    ///     Provides extension methods for types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a default value for the specified type.
        /// </summary>
        /// <param name="type">The type, which must not be <see langword="null"/>.</param>
        /// <returns>The default value for the type.</returns>
        [CanBeNull]
        public static object? GetDefaultValue(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Gets a type's "friendly" name.
        /// </summary>
        /// <param name="type">The type, which must not be <see langword="null"/>.</param>
        /// <returns>The type's friendly name.</returns>
        public static string GetFriendlyName(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Type.GetTypeCode(type) switch
            {
                TypeCode.Boolean => "bool",
                TypeCode.Byte => "byte",
                TypeCode.Char => "char",
                TypeCode.DateTime => "date",
                TypeCode.Decimal => "floating point number",
                TypeCode.Double => "floating point number",
                TypeCode.Int16 => "integer",
                TypeCode.Int32 => "integer",
                TypeCode.Int64 => "integer",
                TypeCode.Single => "floating point number",
                TypeCode.String => "string",
                _ => type.Name
            };
        }
    }
}