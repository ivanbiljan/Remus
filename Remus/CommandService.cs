using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Remus.Attributes;
using Remus.Exceptions;

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
                if (parameters.Length < 1 || parameters[0].ParameterType != typeof(ICommandSender)) {
                    // Log
                    throw new Exception("Command handler is missing a sender");
                }

                _commands.Add(commandAttribute.Name,
                    new Command(commandAttribute.Name, commandAttribute.Description, method,
                        method.IsStatic ? null : obj));
            }
        }
        
        public void Evaluate(ICommandSender sender, string input) {
            var command = default(Command);
            var builder = new StringBuilder(input.Length);
            var span = input.AsSpan();
            while (true) {
                var offset = span.IndexOf(' ');
                builder.Append(offset > 0 ? span[..(offset + 1)] : span);
                if (!_commands.TryGetValue(builder.ToString().Trim(), out var temp)) {
                    break;
                }
                
                command = temp;
                span = span[(offset + 1)..];
            }

            if (command is null) {
                // Throw an exception
                throw new Exception();
            }

            command.Run(sender, span.ToString());
        }
    }
}