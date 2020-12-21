using System;
using System.Reflection;

namespace Remus
{
    /// <summary>
    ///     Represents the binder. This class maps parsed arguments to the parameters of a command handler.
    /// </summary>
    internal static class Binder
    {
        public static CommandHandlerSchema? ResolveMethodCall(
            CommandHandlerSchema[] commandHandlersSchemas,
            InputMetadata inputMetadata,
            out object[] arguments)
        {
            arguments = Array.Empty<object>();
            var result = default(CommandHandlerSchema);
            var bestScore = -1D;
            for (var i = 0; i < commandHandlersSchemas.Length; ++i)
            {
                var schema = commandHandlersSchemas[i];
                var handler = schema.Callback;
                var parameters = handler.GetParameters();
                if (parameters.Length == 0 && inputMetadata.RequiredArguments.Count == 0 &&
                    inputMetadata.Flags.Count == 0 && inputMetadata.Options.Count == 0)
                {
                    return schema;
                }

                var score = EvaluateMethodScore(parameters, inputMetadata);
                if (score > bestScore)
                {
                    result = schema;
                    bestScore = score;
                }
            }

            return result;
        }

        private static double EvaluateMethodScore(ParameterInfo[] parameters, InputMetadata inputMetadata)
        {
            if (parameters.Length == 0 && inputMetadata.RequiredArguments.Count > 0)
            {
                return -1;
            }

            return -1;
        }
    }
}