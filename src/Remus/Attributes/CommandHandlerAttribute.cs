using System;
using JetBrains.Annotations;

namespace Remus.Attributes
{
    /// <summary>
    ///     Describes a command handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [PublicAPI]
    public sealed class CommandHandlerAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandHandlerAttribute" /> class with the specified
        ///     <paramref name="name" />
        ///     and <paramref name="description" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name" /> is <see langword="null" /> or empty.</exception>
        public CommandHandlerAttribute(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Command name must not be null or empty", nameof(name));
            }

            Name = name;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Gets or sets the help text.
        /// </summary>
        public string? HelpText { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets or sets the syntax.
        /// </summary>
        public string? Syntax { get; set; }
    }
}