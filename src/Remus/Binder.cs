using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Remus.Attributes;
using Remus.Exceptions;
using Remus.Extensions;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;

namespace Remus
{
    /// <summary>
    ///     Represents the binder. This class maps parsed arguments to the parameters of a command handler.
    /// </summary>
    internal static class Binder
    {
        [CanBeNull]
        public static CommandHandlerSchema? ResolveMethodCall(
            [NotNull] Command command,
            [NotNull] ICommandSender commandSender,
            [NotNull] IArgumentParser inputMetadata,
            out object?[] arguments)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (commandSender is null)
            {
                throw new ArgumentNullException(nameof(commandSender));
            }

            if (inputMetadata is null)
            {
                throw new ArgumentNullException(nameof(inputMetadata));
            }

            arguments = Array.Empty<object>();
            var result = default(CommandHandlerSchema);
            var bestScore = -1D;

            var commandHandlerSchemas = command.HandlerSchemas.ToArray();
            for (var i = 0; i < commandHandlerSchemas.Length; ++i)
            {
                var schema = commandHandlerSchemas[i];
                var handler = schema.Callback;
                var parameters = handler.GetParameters();
                if (parameters.Length == 0 && inputMetadata.Arguments.Count == 0 &&
                    inputMetadata.Options.Count == 0)
                {
                    return schema;
                }

                try
                {
                    var score = EvaluateMethodScore(parameters, commandSender, command.CommandService.TypeParsers,
                        inputMetadata, out var args);
                    if (score > bestScore)
                    {
                        result = schema;
                        arguments = args;
                        bestScore = score;
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        private static double EvaluateMethodScore(
            [NotNull] ParameterInfo[] parameters,
            [NotNull] ICommandSender commandSender,
            [NotNull] ITypeParserCollection parsers,
            [NotNull] IArgumentParser inputMetadata,
            out object?[] arguments)
        {
            arguments = new object?[parameters.Length];
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parsers is null)
            {
                throw new ArgumentNullException(nameof(parsers));
            }

            if (inputMetadata is null)
            {
                throw new ArgumentNullException(nameof(inputMetadata));
            }

            if (parameters.Length < inputMetadata.Arguments.Count)
            {
                return -1;
            }

            const double stringParameterCost = 0.01;

            var explicitParameterCount = 0;
            var implicitParameterCount = 0;
            var stringParameterCount = 0;
            var argumentIndex = 0;

            for (var i = 0; i < parameters.Length; ++i)
            {
                var parameter = parameters[i];
                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                {
                    ++implicitParameterCount;
                    continue;
                }

                if (parameter.ParameterType == typeof(ICommandSender))
                {
                    arguments[i] = commandSender;
                    ++implicitParameterCount;
                    continue;
                }

                var parserType = parameter.GetCustomAttribute<CommandArgParserAttribute>()?.ParserType ??
                                 parameter.ParameterType;
                var parser = parsers.GetParser(parserType);
                if (parser is null)
                {
                    throw new TypeParserException($"Missing type parser for type '{parameter.ParameterType}'");
                }

                var optionAttribute = parameter.GetCustomAttribute<OptionalArgumentAttribute>();
                if (optionAttribute is not null)
                {
                    var optionValue = inputMetadata.Options.GetValueOrDefault(optionAttribute.Name,
                        inputMetadata.Options.GetValueOrDefault(optionAttribute.ShortName ?? ""));
                    arguments[i] = optionValue == default
                        ? parameter.ParameterType.GetDefaultValue()
                        : parser.Parse(optionValue);
                    ++implicitParameterCount;
                }
                else
                {
                    // TODO: write an analyzer to enforce [OptionalArgumentAttribute] on optional parameters
                    if (argumentIndex >= inputMetadata.Arguments.Count)
                    {
                        return -1;
                    }

                    arguments[i] = parser.Parse(inputMetadata.Arguments[argumentIndex++]);
                    ++explicitParameterCount;
                }
            }

            return explicitParameterCount == parameters.Length - implicitParameterCount
                ? (double) explicitParameterCount / parameters.Length - stringParameterCount * stringParameterCost
                : -1;
        }
    }
}