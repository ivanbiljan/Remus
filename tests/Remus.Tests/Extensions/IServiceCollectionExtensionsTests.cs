using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Remus.Extensions;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;
using Xunit;

namespace Remus.Tests.Extensions {
    public sealed class IServiceCollectionExtensionsTests 
    {
        [Fact]
        public void AddRemus_NullServiceCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IServiceCollectionExtensions.AddRemus(null!));
        }

        [Fact]
        public void AddRemus_ServicesAreCorrectlyRegistered()
        {
            var logger = new Mock<ILogger>().Object;
            var provider = new ServiceCollection()
                           .AddSingleton(logger)
                           .AddRemus()
                           .BuildServiceProvider();

            Assert.IsType<DefaultArgumentParser>(provider.GetService(typeof(IArgumentParser)));
            Assert.IsType<Parsers>(provider.GetService(typeof(ITypeParserCollection)));
            Assert.IsType<CommandService>(provider.GetService(typeof(ICommandService)));
        }
    }
}
