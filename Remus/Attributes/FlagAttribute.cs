using System;
using JetBrains.Annotations;

namespace Remus.Attributes {
    /// <summary>
    /// Describes a command flag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [PublicAPI]
    public sealed class FlagAttribute : Attribute {
        /// <summary>
        /// Gets the long identifier.
        /// </summary>
        public string? LongName { get; }
        
        /// <summary>
        /// Gets the short identifier.
        /// </summary>
        public string? ShortName { get; }

        public FlagAttribute(string? longName = null, string? shortName = null) {
            LongName = longName;
            ShortName = shortName;
        }
    }
}