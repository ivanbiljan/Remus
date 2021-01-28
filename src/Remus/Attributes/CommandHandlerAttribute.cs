using System;
using System.Linq;
using System.Reflection.Metadata;
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
        public CommandHandlerAttribute(string name, string description) : this(new[] {name}, description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Command name must not be null or empty", nameof(name));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerAttribute"/> class with the specified command path and description.
        /// </summary>
        /// <param name="path">The path, which must not be <see langword="null"/> or empty.</param>
        /// <param name="description">The description, which must not be <see langword="null"/>.</param>
        public CommandHandlerAttribute(string[] path, string description)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException("Path must not be empty", nameof(path));
            }

            Name = string.Join(" ", path.Select(p => p.Trim()));
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