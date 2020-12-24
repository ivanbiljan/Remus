using System;
using System.Collections.Generic;
using Xunit;

namespace Remus.Tests
{
    public sealed class LexicalAnalyzerTests
    {
        [Fact]
        public void Parse_EmptyInputString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => InputMetadata.Parse(string.Empty, Array.Empty<string>()));
        }

        [Fact]
        public void Parse_IsCorrect()
        {
            var inputString = "tar  Required\\ argument  -x -v --file=\"File 1.txt\" --arg2 \"Hello, \\\"World\\\"\"";
            var optionals = new Dictionary<string, string>
            {
                ["x"] = null,
                ["v"] = null,
                ["file"] = "File 1.txt",
                ["arg2"] = "Hello, \"World\""
            };

            var parsedData = InputMetadata.Parse(inputString, new[] {"tar"});

            Assert.Equal("tar", parsedData.CommandName);
            Assert.Equal(optionals, parsedData.Options);
            Assert.Equal(new[] {"Required argument"}, parsedData.RequiredArguments);
        }
    }
}