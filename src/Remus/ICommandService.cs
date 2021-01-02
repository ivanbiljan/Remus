using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;

namespace Remus {
    /// <summary>
    /// Defines a contract for a command service.
    /// </summary>
    [PublicAPI]
    public interface ICommandService
    {
        /// <summary>
        /// Gets the argument parser for this command service.
        /// </summary>
        IArgumentParser ArgumentParser { get; }
        
        /// <summary>
        /// Gets the parser collection for this command service.
        /// </summary>
        IParserCollection Parsers { get; }
        
        /// <summary>
        /// Registers commands defined by the specified object.
        /// </summary>
        /// <param name="obj">The object, which must not be <see langword="null"/>.</param>
        void Register([NotNull] object obj);

        /// <summary>
        /// Deregisters commands defined by the specified object.
        /// </summary>
        /// <param name="obj">The object, which must not be <see langword="null"/>.</param>
        void Deregister([NotNull] object obj);

        /// <summary>
        /// Evaluates the given input string using the specified command sender.
        /// </summary>
        /// <param name="input">The input string, which must not be <see langword="null"/>.</param>
        /// <param name="sender">The sender, which must not be <see langword="null"/>.</param>
        void Evaluate([NotNull] string input, [NotNull] ICommandSender sender);
    }
}