using Remus.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Remus.Tests.Extensions {
    public sealed class TypeExtensionsTests {
        [Fact]
        public void ValueType_ReturnsDefault() {
            var value = TypeExtensions.GetDefaultValue(typeof(int));

            Assert.Equal(0, value);
        }

        [Fact]
        public void ReferenceType_ReturnsNull() {
            var value = TypeExtensions.GetDefaultValue(typeof(object));

            Assert.Null(value);
        }
    }
}