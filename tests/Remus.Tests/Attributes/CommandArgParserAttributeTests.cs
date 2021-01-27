using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Remus.Attributes;
using Remus.Parsing.TypeParsers;
using Xunit;

namespace Remus.Tests.Attributes {
    public sealed class CommandArgParserAttributeTests 
    {
        [Fact]
        public void Ctor_NullParserType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandArgParserAttribute(null!));
        }

        [Fact]
        public void Ctor_ParserIsNotOfTypeITypeParser_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CommandArgParserAttribute(typeof(object)));
        }

        [Fact]
        public void Ctor_IsCorrect()
        {
            var parserType = new Mock<ITypeParser>().Object.GetType();

            var commandArgParserAttribute = new CommandArgParserAttribute(parserType);

            Assert.Equal(parserType, commandArgParserAttribute.ParserType);
        }
    }
}
