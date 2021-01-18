using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Remus.Parsing.Arguments;
using Remus.Parsing.TypeParsers;
using Xunit;

namespace Remus.Tests
{
    public sealed class CommandServiceTests
    {
        [Fact]
        public void Ctor_NullArgumentParser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandService(new Mock<ILogger>().Object, null!, new Mock<ITypeParserCollection>().Object));
        }

        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandService(null!, new Mock<IArgumentParser>().Object,
                    new Mock<ITypeParserCollection>().Object));
        }

        [Fact]
        public void Ctor_NullTypeParserCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object, null!));
        }

        [Fact]
        public void Deregister_IsCorrect()
        {
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandRegistry = new TestCommandRegistry();
            var commandService = new CommandService(logger.Object, argumentParser.Object, typeParsers.Object);

            commandService.Register(commandRegistry);
            Assert.NotEmpty(commandService.GetCommands());

            commandService.Deregister(commandRegistry);
            Assert.Empty(commandService.GetCommands());
        }

        [Fact]
        public void Deregister_NullObject_ThrowsArgumentNullException()
        {
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandService = new CommandService(logger.Object, argumentParser.Object, typeParsers.Object);

            Assert.Throws<ArgumentNullException>(() => commandService.Deregister(null!));
        }

        [Fact]
        public void Evaluate_InvalidCommand_IsLogged()
        {
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandService = new CommandService(logger.Object, argumentParser.Object, typeParsers.Object);

            commandService.Register(new TestCommandRegistry());
            commandService.Evaluate("command", new Mock<ICommandSender>().Object);

            logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), () => Times.Exactly(1));
        }

        [Fact]
        public void Evaluate_IsCorrect()
        {
            var sender = new Mock<ICommandSender>().Object;
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandRegistry = new TestCommandRegistry();
            var commandService = new CommandService(logger.Object, new DefaultArgumentParser(), new Parsers());

            commandService.Register(commandRegistry);

            commandService.Evaluate("test", sender);
            Assert.Equal(1024, commandRegistry.Number);

            commandService.Evaluate("test 2048", sender);
            Assert.Equal(2048, commandRegistry.Number);

            commandService.Evaluate("test -x 200", sender);
            Assert.Equal(0, commandRegistry.Number);

            commandService.Evaluate("test2 123 true 123", sender);
            Assert.Equal(123, commandRegistry.Number);
            Assert.True(commandRegistry.Boolean);
            Assert.Equal("123", commandRegistry.String);

            commandService.Evaluate("test2 str true 123", sender);
            Assert.Equal(-1, commandRegistry.Number);
            Assert.True(commandRegistry.Boolean);
            Assert.Equal("123", commandRegistry.String);
        }

        [Fact]
        public void Evaluate_NullInput_ThrowsArgumentNullException()
        {
            var commandService = new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object,
                new Mock<ITypeParserCollection>().Object);

            Assert.Throws<ArgumentNullException>(
                () => commandService.Evaluate(null!, new Mock<ICommandSender>().Object));
        }

        [Fact]
        public void Evaluate_NullSender_ThrowsArgumentNullException()
        {
            var commandService = new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object,
                new Mock<ITypeParserCollection>().Object);

            Assert.Throws<ArgumentNullException>(() => commandService.Evaluate("test", null!));
        }

        [Fact]
        public void GetCommands_CustomPredicate_IsCorrect()
        {
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandRegistry = new TestCommandRegistry();
            var commandService = new CommandService(logger.Object, argumentParser.Object, typeParsers.Object);

            commandService.Register(commandRegistry);

            var expected = new List<Command>
            {
                new Command(logger.Object, commandService, "test2", commandRegistry)
            };

            Assert.Equal(expected, commandService.GetCommands(c => c.Name == "test2"));
        }

        [Fact]
        public void GetCommands_NoCommands_ReturnsEmptyCollection()
        {
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandService = new CommandService(logger.Object, argumentParser.Object, typeParsers.Object);

            Assert.Empty(commandService.GetCommands());
        }

        [Fact]
        public void GetCommands_NullPredicate_ReturnsAllCommands()
        {
            var logger = new Mock<ILogger>();
            var argumentParser = new Mock<IArgumentParser>();
            var typeParsers = new Mock<ITypeParserCollection>();
            var commandRegistry = new TestCommandRegistry();
            var commandService = new CommandService(logger.Object, argumentParser.Object, typeParsers.Object);

            commandService.Register(commandRegistry);

            var expected = new List<Command>
            {
                new Command(logger.Object, commandService, "test", commandRegistry),
                new Command(logger.Object, commandService, "test2", commandRegistry)
            };

            Assert.Equal(expected, commandService.GetCommands());
        }

        [Fact]
        public void Register_IsCorrect()
        {
            var commandService = new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object,
                new Mock<ITypeParserCollection>().Object);

            commandService.Register(new TestCommandRegistry());

            Assert.Equal(new[] {"test", "test2"}, commandService.GetCommands().Select(c => c.Name));
        }

        [Fact]
        public void Register_NullObject_ThrowsArgumentNullException()
        {
            var commandService = new CommandService(new Mock<ILogger>().Object, new Mock<IArgumentParser>().Object,
                new Mock<ITypeParserCollection>().Object);

            Assert.Throws<ArgumentNullException>(() => commandService.Register(null!));
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