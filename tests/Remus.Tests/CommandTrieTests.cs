using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Remus.Attributes;
using Xunit;

namespace Remus.Tests {
    public sealed class CommandTrieTests {
        [Fact]
        public void AddCommand_NullCommand_ThrowsArgumentNullException()
        {
            var trie = new CommandTrie();

            Assert.Throws<ArgumentNullException>(() => trie.AddCommand(null!));
        }
        
        [Fact]
        public void AddCommands_NullCommands_ThrowsArgumentNullException()
        {
            var trie = new CommandTrie();

            Assert.Throws<ArgumentNullException>(() => trie.AddCommands(null!));
        }
        
        [Fact]
        public void GetCommandSuggestions_NullSearchQuery_ThrowsArgumentException()
        {
            var trie = new CommandTrie();

            Assert.Throws<ArgumentException>(() => trie.GetCommandSuggestions(null!));
        }

        [Fact]
        public void GetCommandSuggestions_NoCommands_ReturnsEmptyList()
        {
            var trie = new CommandTrie();

            var suggestions = trie.GetCommandSuggestions("test");

            Assert.Empty(suggestions);
        }

        [Fact]
        public void GetCommandSuggestions_BadPrefix_ReturnsEmptyList()
        {
            var trie = new CommandTrie();
            var mockLogger = new Mock<ILogger>();
            var mockCommandService = new Mock<ICommandService>();
            trie.AddCommand(new Command(mockLogger.Object, mockCommandService.Object, "test"));

            var suggestions = trie.GetCommandSuggestions("tst");

            Assert.Empty(suggestions);
        }

        [Fact]
        public void GetCommandSuggestions_IsCorrect() {
            var trie = new CommandTrie();
            var mockLogger = new Mock<ILogger>();
            var mockCommandService = new Mock<ICommandService>();
            var command = new Command(mockLogger.Object, mockCommandService.Object, "test");
            var command2 = new Command(mockLogger.Object, mockCommandService.Object, "test subcommand");
            trie.AddCommands(command, command2);

            var suggestions = trie.GetCommandSuggestions("test");

            Assert.Equal(new List<Command> {command, command2}, suggestions);
        }
    }
}