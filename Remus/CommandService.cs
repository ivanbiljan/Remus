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
    public sealed class CommandService {
        private readonly IDictionary<string, Command> _commands = new Dictionary<string, Command>();

        public ImmutableArray<Command> Commands => _commands.Values.ToImmutableArray();

        /// <summary>
        /// Registers commands described by the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Register(object obj) {
            var methods = obj.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var method in methods) {
                var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is null) {
                    continue;
                }

                var parameters = method.GetParameters();
                _commands.Add(commandAttribute.Name,
                    new Command(commandAttribute.Name, commandAttribute.Description, method,
                        method.IsStatic ? null : obj));
            }
        }
        
        public void Evaluate(ICommandSender sender, string input) {
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