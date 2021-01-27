using System;
using Remus.Attributes;
using Xunit;

namespace Remus.Tests.Attributes
{
    public sealed class CommandAttributeTests
    {
        [Fact]
        public void Ctor_EmptyName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CommandHandlerAttribute("", ""));
        }

        [Fact]
        public void Ctor_NullDescription_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandHandlerAttribute("name", null!));
        }

        [Fact]
        public void Ctor_IsCorrect()
        {
            var command = new CommandHandlerAttribute("name", "Description");

            Assert.Equal("name", command.Name);
            Assert.Equal("Description", command.Description);
        }

        [Fact]
        public void HelpText_SetGet_IsCorrect()
        {
            var command = new CommandHandlerAttribute("name", "Description");

            Assert.Null(command.HelpText);

            command.HelpText = "Help text";

            Assert.Equal("Help text", command.HelpText);
        }

        [Fact]
        public void Syntax_SetGet_IsCorrect()
        {
            var command = new CommandHandlerAttribute("name", "Description");

            Assert.Null(command.Syntax);

            command.HelpText = "Syntax";

            Assert.Equal("Syntax", command.Syntax);
        }
    }
}