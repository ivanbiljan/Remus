using Remus.Extensions;
using Xunit;

namespace Remus.Tests.Extensions
{
    public sealed class TypeExtensionsTests
    {
        [Fact]
        public void ReferenceType_ReturnsNull()
        {
            var value = typeof(object).GetDefaultValue();

            Assert.Null(value);
        }

        [Fact]
        public void ValueType_ReturnsDefault()
        {
            var value = typeof(int).GetDefaultValue();

            Assert.Equal(0, value);
        }
    }
}