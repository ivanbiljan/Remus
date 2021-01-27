using System;
using Remus.Extensions;
using Xunit;

namespace Remus.Tests.Extensions
{
    public sealed class TypeExtensionsTests
    {
        [Fact]
        public void GetDefaultValue_NullType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.GetDefaultValue(null!));
        }
        
        [Fact]
        public void GetDefaultValue_ReferenceType_ReturnsNull()
        {
            var value = typeof(object).GetDefaultValue();

            Assert.Null(value);
        }

        [Fact]
        public void GetDefaultValue_ValueType_ReturnsDefault()
        {
            var value = typeof(int).GetDefaultValue();

            Assert.Equal(0, value);
        }

        [Fact]
        public void GetFriendlyName_NullType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.GetFriendlyName(null!));
        }
    }
}