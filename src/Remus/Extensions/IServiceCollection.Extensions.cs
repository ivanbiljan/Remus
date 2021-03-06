﻿using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;

namespace Remus.Extensions
{
    /// <summary>
    ///     Provides extension methods for the <see cref="IServiceProvider" /> class.
    /// </summary>
    [PublicAPI]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds Remus' services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="serviceCollection">The service collection, which must not be <see langword="null" />.</param>
        /// <returns>The modified <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddRemus([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton<IArgumentParser, DefaultArgumentParser>();
            serviceCollection.AddSingleton<ITypeParserCollection, Parsers>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();
            return serviceCollection;
        }
    }
}