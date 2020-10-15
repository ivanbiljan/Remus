using System;
using JetBrains.Annotations;

namespace Remus.Attributes {
    /// <summary>
    /// Describes an optional argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [PublicAPI]
    public sealed class OptionalArgumentAttribute : Attribute {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalArgumentAttribute"/> with the specified <paramref name="name"/> and <paramref name="description"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        public OptionalArgumentAttribute(string name, string description) {
            Name = name;
            Description = description;
        }
    }
}