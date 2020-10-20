using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Remus.Tests {
    public sealed class LexicalAnalyzerTests {
        [Fact]
        public void Parse_EmptyInputString_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => LexicalAnalyzer.Parse(string.Empty, Array.Empty<string>()));
        }

        [Fact]
        public void Parse_IsCorrect() {
            var inputString = "tar    -x -v --file=\"File 1.txt\" --arg2 \"Hello, \\\"World\\\"\" Required\\ argument";
            var optionals = new Dictionary<string, string>() {
                ["file"] = "File 1.txt",
                ["arg2"] = "Hello, \"World\""
            };

            var parsedData = LexicalAnalyzer.Parse(inputString, new string[] { "tar" });

            Assert.Equal("tar", parsedData.CommandName);
            Assert.Equal(new char[] { 'x', 'v' }, parsedData.Flags);
            Assert.Equal(optionals, parsedData.Options);
            Assert.Equal(new string[] { "Required argument" }, parsedData.RequiredArguments);
        }
    }
}
