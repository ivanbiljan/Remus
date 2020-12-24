using System;
using Remus.Attributes;
using Xunit;

namespace Remus.Tests.Attributes
{
    public sealed class FlagAttributeTests
    {
        [Fact]
        public void Ctor_NonLetterIdentifier_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new FlagAttribute('0', string.Empty));
        }

        [Fact]
        public void Ctor_IsCorrect()
        {
            var flag = new FlagAttribute('x', "N/A");

            Assert.Equal('x', flag.Identifier);
            Assert.Equal("N/A", flag.Description);
        }
    }
}