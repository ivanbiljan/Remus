using System;
using System.Collections.Generic;
using Remus.Parsing.Arguments;
using Xunit;

namespace Remus.Tests.Parsing.Arguments
{
    public sealed class DefaultArgumentParserTests
    {
        [Fact]
        public void Parse_IsCorrect()
        {
            var argumentParser = new DefaultArgumentParser();
            var inputString = "tar  Required\\ argument  -x -v --file=\"File 1.txt\" --arg2 \"Hello, \\\"World\\\"\"";

            var parserResult = argumentParser.Parse(inputString, new[] {"tar"});

            var expectedOptions = new Dictionary<string, string>
            {
                ["x"] = null,
                ["v"] = null,
                ["file"] = "File 1.txt",
                ["arg2"] = "Hello, \"World\""
            };

            Assert.Equal("tar", parserResult.CommandName);
            Assert.Equal(expectedOptions, parserResult.Options);
            Assert.Equal(new[] {"Required argument"}, parserResult.Arguments);
        }

        [Fact]
        public void Parse_NullAvailableCommandNames_ThrowsArgumentNullException()
        {
            var argumentParser = new DefaultArgumentParser();

            Assert.Throws<ArgumentNullException>(() => argumentParser.Parse("test", null!));
        }

        [Fact]
        public void Parse_NullOrEmptyInputString_ThrowsArgumentException()
        {
            var argumentParser = new DefaultArgumentParser();

            Assert.Throws<ArgumentException>(() => argumentParser.Parse(string.Empty, Array.Empty<string>()));
            Assert.Throws<ArgumentException>(() => argumentParser.Parse(null!, Array.Empty<string>()));
        }
    }
}