using System;
using System.Linq;
using Remus.Exceptions;
using Remus.TypeParsing;
using Xunit;

namespace Remus.Tests
{
    public sealed class CommandManagerTests
    {
        [Fact]
        public void Ctor_NullParsers_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandManager(null!));
        }

        [Fact]
        public void Register_NullObj_ThrowsArgumentNullException()
        {
            var commandManager = new CommandManager(new Parsers());

            Assert.Throws<ArgumentNullException>(() => commandManager.Register(null!));
        }

        [Fact]
        public void Register_IsCorrect()
        {
            var commandManager = new CommandManager(new Parsers());
            var commandRegistry = new TestCommandRegistry();

            commandManager.Register(commandRegistry);

            Assert.Equal(new[] {"test"}, commandManager.Commands.Select(c => c.Name));
        }

        [Fact]
        public void Evaluate_NullSender_ThrowsArgumentNullException()
        {
            var commandManager = new CommandManager(new Parsers());

            Assert.Throws<ArgumentNullException>(() => commandManager.Evaluate(null!, ""));
        }

        [Fact]
        public void Evaluate_NullInput_ThrowsArgumentNullException()
        {
            var commandManager = new CommandManager(new Parsers());

            Assert.Throws<ArgumentNullException>(() => commandManager.Evaluate(new ConsoleSender(), null!));
        }

        [Fact]
        public void Evaluate_InvalidCommand_ThrowsInvalidCommandException()
        {
            var commandManager = new CommandManager(new Parsers());

            Assert.Throws<InvalidCommandException>(() =>
                commandManager.Evaluate(new ConsoleSender(), "invalidcommand"));
        }

        [Fact]
        public void Evaluate_IsCorrect()
        {
            var sender = new ConsoleSender();
            var commandManager = new CommandManager(new Parsers());
            var commandRegistry = new TestCommandRegistry();
            commandManager.Register(commandRegistry);

            commandManager.Evaluate(sender, "test");
            Assert.Equal(1024, commandRegistry.Number);

            commandManager.Evaluate(sender, "test 2048");
            Assert.Equal(2048, commandRegistry.Number);

            commandManager.Evaluate(sender, "test -x 200");
            Assert.Equal(0, commandRegistry.Number);
            
            commandManager.Evaluate(sender, "test2 123 true 123");
            Assert.Equal(123, commandRegistry.Number);
            Assert.True(commandRegistry.Boolean);
            Assert.Equal("123", commandRegistry.String);

            commandManager.Evaluate(sender, "test2 str true 123");
            Assert.Equal(-1, commandRegistry.Number);
            Assert.True(commandRegistry.Boolean);
            Assert.Equal("123", commandRegistry.String);
        }
    }
}