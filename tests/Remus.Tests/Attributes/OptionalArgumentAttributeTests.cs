using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remus.Attributes;
using Xunit;

namespace Remus.Tests.Attributes 
{
    public sealed class OptionalArgumentAttributeTests 
    {
        [Fact]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new OptionalArgumentAttribute(null!, "Description"));
        }

        [Fact]
        public void Ctor_NullDescription_ThrowsArgumentNullException() 
        {
            Assert.Throws<ArgumentNullException>(() => new OptionalArgumentAttribute("name", null!));
        }

        [Fact]
        public void Ctor_IsCorrect()
        {
            var optionalArgumentAttribute = new OptionalArgumentAttribute("name", "Description");

            Assert.Equal("name", optionalArgumentAttribute.Name);
            Assert.Equal("Description", optionalArgumentAttribute.Description);
        }

        [Fact]
        public void ShortName_SetGet_IsCorrect()
        {
            var optionalArgumentAttribute = new OptionalArgumentAttribute("name", "Description");

            Assert.Null(optionalArgumentAttribute.ShortName);

            optionalArgumentAttribute.ShortName = "short";

            Assert.Equal("short", optionalArgumentAttribute.ShortName);
        }
    }
}