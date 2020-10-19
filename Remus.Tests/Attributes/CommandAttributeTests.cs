using System;
using System.Collections.Generic;
using System.Text;
using Remus.Attributes;
using Xunit;

namespace Remus.Tests.Attributes {
    public sealed class CommandAttributeTests {
        [Fact]
        public void Ctor_EmptyName_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new CommandAttribute("", ""));
        }

        [Fact]
        public void Ctor_IsCorrect() {
            var command = new CommandAttribute("name", "");

            Assert.Equal("name", command.Name);
            Assert.Equal(string.Empty, command.Description);
        }
    }
}
