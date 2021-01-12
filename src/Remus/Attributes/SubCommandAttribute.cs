using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Remus.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    [PublicAPI]
    public sealed class SubCommandAttribute : Attribute {
        /// <summary>
        /// Gets the sub-command name.
        /// </summary>
        public string SubCommandName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubCommandAttribute"/> class with the specified sub-command name.
        /// </summary>
        /// <param name="subCommandName">The sub-command name, which must not be <see langword="null"/>.</param>
        public SubCommandAttribute(string subCommandName)
        {
            SubCommandName = subCommandName ?? throw new ArgumentNullException(nameof(subCommandName));
        }
    }
}
