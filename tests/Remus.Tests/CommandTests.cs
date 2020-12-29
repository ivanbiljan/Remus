using System;
using Remus.TypeParsing;
using Xunit;

namespace Remus.Tests
{
    public sealed class CommandTests
    {
        [Fact]
        public void Ctor_NullCommandManager_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Command(null!, ""));
        }

        [Fact]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Command(new CommandManager(new Parsers()), null!));
        }

        [Fact]
        public void Name_IsCorrect()
        {
            var command = new Command(new CommandManager(new Parsers()), "test");

            Assert.Equal("test", command.Name);
        }

        [Fact]
        public void Equals_CommandsNotEqual_ReturnsFalse()
        {
            var commandManager = new CommandManager(new Parsers());
            var command1 = new Command(commandManager, "test");
            var command2 = new Command(commandManager, "test2");

            Assert.False(command1.Equals(command2));
        }

        [Fact]
        public void Equals_CommandsEqual_ReturnsTrue()
        {
            var commandManager = new CommandManager(new Parsers());
            var command1 = new Command(commandManager, "test");
            var command2 = new Command(commandManager, "test");

            Assert.True(command1.Equals(command2));
        }
    }
}