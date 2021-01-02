using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remus.Parsing.Arguments;
using Xunit;

namespace Remus.Tests.Parsing.Arguments {
    public sealed class DefaultArgumentParserTests
    {
        [Fact]
        public void Parse_NullOrEmptyInputString_ThrowsArgumentException()
        {
            var argumentParser = new DefaultArgumentParser();
            
            Assert.Throws<ArgumentException>(() => argumentParser.Parse(string.Empty, Array.Empty<string>()));
            Assert.Throws<ArgumentException>(() => argumentParser.Parse(null!, Array.Empty<string>()));
        }

        [Fact]
        public void Parse_NullAvailableCommandNames_ThrowsArgumentNullException()
        {
            var argumentParser = new DefaultArgumentParser();

            Assert.Throws<ArgumentException>(() => argumentParser.Parse("test", null!));
        }

        [Fact]
        public void Parse_IsCorrect()
        {
            var argumentParser = new DefaultArgumentParser();
            var inputString = "tar  Required\\ argument  -x -v --file=\"File 1.txt\" --arg2 \"Hello, \\\"World\\\"\"";

            argumentParser.Parse(inputString, new[] {"tar"});

            var expectedOptions = new Dictionary<string, string>
            {
                ["x"] = null,
                ["v"] = null,
                ["file"] = "File 1.txt",
                ["arg2"] = "Hello, \"World\""
            };

            Assert.Equal("tar", argumentParser.CommandName);
            Assert.Equal(expectedOptions, argumentParser.Options);
            Assert.Equal(new[] {"Required argument"}, argumentParser.Arguments);
        }
    }
}