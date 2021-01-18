using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;

namespace Remus
{
    /// <summary>
    ///     Defines a contract for a command service.
    /// </summary>
    [PublicAPI]
    public interface ICommandService
    {
        /// <summary>
        ///     Gets the argument parser for this command service.
        /// </summary>
        IArgumentParser ArgumentParser { get; }

        /// <summary>
        ///     Gets the parser collection for this command service.
        /// </summary>
        ITypeParserCollection TypeParsers { get; }

        /// <summary>
        ///     Deregisters commands defined by the specified object.
        /// </summary>
        /// <param name="obj">The object, which must not be <see langword="null" />.</param>
        void Deregister([NotNull] object obj);

        /// <summary>
        ///     Evaluates the given input string using the specified command sender.
        /// </summary>
        /// <param name="input">The input string, which must not be <see langword="null" />.</param>
        /// <param name="sender">The sender, which must not be <see langword="null" />.</param>
        void Evaluate([NotNull] string input, [NotNull] ICommandSender sender);

        /// <summary>
        ///     Returns an enumerable collection of commands that match the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate, which must not be <see langword="null" />.</param>
        /// <returns>An enumerable collection of commands that match the given predicate.</returns>
        IEnumerable<Command> GetCommands(Predicate<Command>? predicate = null);

        /// <summary>
        ///     Registers commands defined by the specified object.
        /// </summary>
        /// <param name="obj">The object, which must not be <see langword="null" />.</param>
        void Register([NotNull] object obj);
    }
}