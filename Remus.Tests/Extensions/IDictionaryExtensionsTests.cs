using Remus.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Remus.Tests.Extensions {
    public sealed class IDictionaryExtensionsTests {
        [Fact]
        public void GetValueOrDefault_NullDictionary_ThrowsArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => IDictionaryExtensions.GetValueOrDefault<object, object>(null, null));
        }

        [Fact]
        public void GetValueOrDefault_NullKey_ThrowsArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => IDictionaryExtensions.GetValueOrDefault(new Dictionary<object, object>(), null));
        }

        [Fact]
        public void GetValueOrDefault_InvalidKey_ReturnsDefaultValue() {
            var value = IDictionaryExtensions.GetValueOrDefault(new Dictionary<string, string>(), "invalid");

            Assert.Null(value);
        }
    }
}