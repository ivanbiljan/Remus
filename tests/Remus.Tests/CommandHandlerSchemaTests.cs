using System;
using Remus.Attributes;
using Xunit;

namespace Remus.Tests {
    public sealed class CommandHandlerSchemaTests {
        [Fact]
        public void Ctor_NullCommandHandlerAttribute_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandHandlerSchema(null!,
                typeof(CommandHandlerSchemaTests).GetMethod(nameof(Ctor_NullCallback_ThrowsArgumentNullException))!,
                null));
        }

        [Fact]
        public void Ctor_NullCallback_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CommandHandlerSchema(new CommandHandlerAttribute("name", "description"), null!, null));
        }

        [Fact]
        public void Description_GetSet_IsCorrect()
        {
            var schema = new CommandHandlerSchema(new CommandHandlerAttribute("name", "description"),
                typeof(CommandHandlerSchemaTests).GetMethod(nameof(Ctor_NullCallback_ThrowsArgumentNullException))!,
                null);

            Assert.Equal("description", schema.Description);
        }

        [Fact]
        public void HelpText_GetSet_IsCorrect()
        {
            var schema = new CommandHandlerSchema(new CommandHandlerAttribute("name", "description") {HelpText = "help"},
                typeof(CommandHandlerSchemaTests).GetMethod(nameof(Ctor_NullCallback_ThrowsArgumentNullException))!,
                null);

            Assert.Equal("help", schema.HelpText);
        }

        [Fact]
        public void Syntax_GetSet_IsCorrect() {
            var schema = new CommandHandlerSchema(new CommandHandlerAttribute("name", "description") { Syntax = "syntax" },
                typeof(CommandHandlerSchemaTests).GetMethod(nameof(Ctor_NullCallback_ThrowsArgumentNullException))!,
                null);

            Assert.Equal("syntax", schema.Syntax);
        }
    }
}
