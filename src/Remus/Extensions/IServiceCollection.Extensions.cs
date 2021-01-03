using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;

namespace Remus.Extensions {
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceProvider"/> class.
    /// </summary>
    [PublicAPI]
    public static class IServiceCollectionExtensions {
        /// <summary>
        /// Configures Remus' service bindings.
        /// </summary>
        /// <param name="serviceCollection">The service provider, which must not be <see langword="null"/>.</param>
        public static void AddCommandServices([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton<IArgumentParser, DefaultArgumentParser>();
            serviceCollection.AddSingleton<ITypeParserCollection, Parsers>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();
        }
    }
}
