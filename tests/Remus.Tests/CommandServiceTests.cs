using System;
using System.Linq;
using Moq;
using Remus.Exceptions;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Remus.Tests
{
    public sealed class CommandServiceTests
    {
        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandService(null!, new Mock<IArgumentParser>().Object,
                    new Mock<ITypeParserCollection>().Object));
        }

        [Fact]
        public void Ctor_NullArgumentParser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandService(new Mock<ILogger>().Object, null!, new Mock<ITypeParserCollection>().Object));
        }

        [Fact]
        public void Ctor_NullTypeParserCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object, null!));
        }

        [Fact]
        public void Register_NullObject_ThrowsArgumentNullException()
        {
            var commandService = new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object,
                new Mock<ITypeParserCollection>().Object);

            Assert.Throws<ArgumentNullException>(() => commandService.Register(null!));
        }

        [Fact]
        public void Register_IsCorrect()
        {
            var commandService = new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object,
                new Mock<ITypeParserCollection>().Object);

            commandService.Register(new TestCommandRegistry());

            Assert.Equal(new[] {"test", "test2"}, commandService.GetCommands().Select(c => c.Name));
        }

        //[Fact]
        //public void Evaluate_IsCorrect()
        //{
        //    var sender = new ConsoleSender();
        //    var commandManager = new CommandManager(new Parsers());
        //    var commandRegistry = new TestCommandRegistry();
        //    commandManager.Register(commandRegistry);

        //    commandManager.Evaluate(sender, "test");
        //    Assert.Equal(1024, commandRegistry.Number);

        //    commandManager.Evaluate(sender, "test 2048");
        //    Assert.Equal(2048, commandRegistry.Number);

        //    commandManager.Evaluate(sender, "test -x 200");
        //    Assert.Equal(0, commandRegistry.Number);
            
        //    commandManager.Evaluate(sender, "test2 123 true 123");
        //    Assert.Equal(123, commandRegistry.Number);
        //    Assert.True(commandRegistry.Boolean);
        //    Assert.Equal("123", commandRegistry.String);

        //    commandManager.Evaluate(sender, "test2 str true 123");
        //    Assert.Equal(-1, commandRegistry.Number);
        //    Assert.True(commandRegistry.Boolean);
        //    Assert.Equal("123", commandRegistry.String);
        //}
    }
}