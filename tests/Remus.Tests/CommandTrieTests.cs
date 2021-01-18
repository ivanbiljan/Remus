﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Remus.Tests
{
    public sealed class CommandTrieTests
    {
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
            var command = new Command(mockLogger.Object, mockCommandService.Object, "test");
            trie.AddCommand(command);

            var suggestions = trie.GetCommandSuggestions("tst");

            Assert.Empty(suggestions);
        }

        [Fact]
        public void GetCommandSuggestions_IsCorrect()
        {
            var trie = new CommandTrie();
            var mockLogger = new Mock<ILogger>();
            var mockCommandService = new Mock<ICommandService>();
            var command = new Command(mockLogger.Object, mockCommandService.Object, "test");
            var command2 = new Command(mockLogger.Object, mockCommandService.Object, "test subcommand");
            trie.AddCommands(command, command2);

            var suggestions = trie.GetCommandSuggestions("test");

            Assert.Equal(new List<Command> {command, command2}, suggestions);
        }

        [Fact]
        public void RemoveCommand_NullCommand_ThrowsArgumentNullException()
        {
            var trie = new CommandTrie();

            Assert.Throws<ArgumentNullException>(() => trie.RemoveCommand(null!));
        }

        [Fact]
        public void RemoveCommand_IsCorrect()
        {
            var trie = new CommandTrie();
            var mockLogger = new Mock<ILogger>();
            var mockCommandService = new Mock<ICommandService>();
            var commandNames = new[] {"cmd", "cmdsub", "cmd sub", "cmd sub c", "cmd set", "sub"};
            foreach (var commandName in commandNames)
            {
                trie.AddCommand(new Command(mockLogger.Object, mockCommandService.Object, commandName));
            }

            trie.RemoveCommand("cmdsub");
            trie.RemoveCommand("cmd sub");
            trie.RemoveCommand("sub");

            var expected = new List<Command>
            {
                new Command(mockLogger.Object, mockCommandService.Object, "cmd"),
                new Command(mockLogger.Object, mockCommandService.Object, "cmd sub c"),
                new Command(mockLogger.Object, mockCommandService.Object, "cmd set")
            };

            Assert.Equal(expected, trie.GetCommandSuggestions("cmd"));
            Assert.Empty(trie.GetCommandSuggestions("sub"));
        }
    }
}