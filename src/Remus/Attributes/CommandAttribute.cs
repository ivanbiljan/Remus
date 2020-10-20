﻿using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Remus.Attributes {
    /// <summary>
    /// Describes a command handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [PublicAPI]
    public sealed class CommandAttribute : Attribute {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        public string? HelpText { get; set; }

        /// <summary>
        /// Gets or sets the syntax.
        /// </summary>
        public string? Syntax { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class with the specified <paramref name="name"/> and <paramref name="description"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public CommandAttribute(string name, string description) {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(nameof(name));
            }

            if (description is null) {
                throw new ArgumentNullException(nameof(description));
            }

            Name = name;
            Description = description;
        }
    }
}