using System;
using JetBrains.Annotations;

namespace Remus.Attributes
{
    /// <summary>
    ///     Describes a command flag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [PublicAPI]
    public sealed class FlagAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FlagAttribute" /> class with the specified identifier and description.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="description">The description.</param>
        public FlagAttribute(char identifier, string description)
        {
            Identifier = identifier;
            Description = description;
        }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Gets letter that identifies the flag.
        /// </summary>
        public char Identifier { get; }
    }
}