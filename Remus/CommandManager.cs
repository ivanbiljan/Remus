using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Remus.Attributes;
using Remus.Exceptions;
using Remus.Extensions;

namespace Remus {
    /// <summary>
    /// Represents a command service.
    /// </summary>
    public sealed class CommandManager {
        private const BindingFlags HandlerBindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        private readonly IDictionary<string, Command?> _commands = new Dictionary<string, Command?>();

        /// <summary>
        /// Gets an immutable array of all commands.
        /// </summary>
        public ImmutableArray<Command?> Commands => _commands.Values.ToImmutableArray();

        /// <summary>
        /// Registers commands described by the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Register(object obj) {
            if (obj is null) {
                throw new ArgumentNullException(nameof(obj));
            }

            var methods = obj.GetType().GetMethods(HandlerBindingFlags);
            foreach (var method in methods) {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is null) {
                    continue;
                }

                _commands.Add(commandAttribute.Name,
                    new Command(commandAttribute.Name, commandAttribute.Description, method,
                        method.IsStatic ? null : obj));
            }
        }

        /// <summary>
        /// Deregisters commands described by the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Deregister(object obj) {
            if (obj is null) {
                throw new ArgumentNullException(nameof(obj));
            }

            var methods = obj.GetType().GetMethods(HandlerBindingFlags);
            for (var i = 0; i < methods.Length; ++i) {
                var method = methods[i];
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is null) {
                    continue;
                }

                _commands.Remove(commandAttribute.Name);
            }
        }
        
        /// <summary>
        /// Evaluates a specified input string using the given command sender.
        /// </summary>
        /// <param name="sender">The sender, which must not be <see langword="null"/>.</param>
        /// <param name="input">The input string, which must not be <see langword="null"/>.</param>
        public void Evaluate(ICommandSender sender, string input) {
            if (sender is null) {
                throw new ArgumentNullException(nameof(sender));
            }

            if (input is null) {
                throw new ArgumentNullException(nameof(input));
            }

            var inputData = LexicalAnalyzer.Parse(input, _commands.Keys.ToList());
            if (string.IsNullOrWhiteSpace(inputData.CommandName)) {
                throw new InvalidCommandException(input);
            }

            var command = _commands.GetValueOrDefault(inputData.CommandName);
            if (command is null) {
                throw new InvalidCommandException(inputData.CommandName);
            }

            command.Run(sender, inputData);
        }
    }
}