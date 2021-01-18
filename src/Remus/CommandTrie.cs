using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Remus
{
    /// <summary>
    ///     Represents a command trie.
    /// </summary>
    internal sealed class CommandTrie
    {
        private readonly IDictionary<string, Command> _commandMap = new Dictionary<string, Command>();
        private readonly TrieNode _root = new TrieNode('\0');

        /// <summary>
        ///     Gets an enumerable collection of commands.
        /// </summary>
        [ItemNotNull]
        public IEnumerable<Command> Commands => _commandMap.Values;

        /// <summary>
        ///     Adds a command to the trie.
        /// </summary>
        /// <param name="command">The command, which must not be <see langword="null" />.</param>
        public void AddCommand([NotNull] Command command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var word = command.Name;
            var currentNode = _root;
            for (var i = 0; i < word.Length; ++i)
            {
                var currentLetter = word[i];
                if (!currentNode.Children.TryGetValue(currentLetter, out var nextNode))
                {
                    var isFullWord = i == word.Length - 1;
                    nextNode = new TrieNode(currentLetter);
                    if (isFullWord)
                    {
                        _commandMap.Add(word, command);
                        nextNode.Word = word;
                    }

                    currentNode.Children.Add(currentLetter, nextNode);
                }

                currentNode = nextNode;
            }
        }

        /// <summary>
        ///     Adds a list of commands to the trie.
        /// </summary>
        /// <param name="commands">The command list, which must not be <see langword="null" />.</param>
        public void AddCommands([NotNull] params Command[] commands)
        {
            if (commands is null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }

        /// <summary>
        ///     Gets a list of commands that match the given prefix.
        /// </summary>
        /// <param name="searchQuery">The prefix to search for.</param>
        /// <returns>A list of commands that match the given prefix.</returns>
        public IList<Command> GetCommandSuggestions([NotNull] string searchQuery)
        {
            var commands = new List<Command>();
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                throw new ArgumentException("Search query must not be null or whitespace", nameof(searchQuery));
            }

            if (!_root.Children.ContainsKey(searchQuery[0]))
            {
                return commands;
            }

            var currentNode = _root.Children[searchQuery[0]];
            for (var i = 1; i < searchQuery.Length; ++i)
            {
                if (currentNode.Children.TryGetValue(searchQuery[i], out var nextNode))
                {
                    currentNode = nextNode;
                }
                else
                {
                    return commands;
                }
            }

            RecurseSubtree(currentNode);
            return commands;

            void RecurseSubtree(TrieNode node)
            {
                if (node.IsFullWord)
                {
                    Debug.Assert(_commandMap.ContainsKey(node.Word!), "_commandMap.ContainsKey(node.Word!)");
                    commands.Add(_commandMap[node.Word!]);
                }

                foreach (var (_, childNode) in node.Children)
                {
                    RecurseSubtree(childNode);
                }
            }
        }

        /// <summary>
        ///     Removes a command with the specified name.
        /// </summary>
        /// <param name="name">The name, which must not be <see langword="null" />.</param>
        public void RemoveCommand([NotNull] string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var currentNode = _root;
            RemoveHelper(currentNode, 0);

            TrieNode? RemoveHelper(TrieNode? node, int currentCharacterIndex)
            {
                if (node is null)
                {
                    return null;
                }

                if (!node.Children.TryGetValue(name[currentCharacterIndex], out var childNode))
                {
                    return node;
                }

                if (currentCharacterIndex == name.Length - 1)
                {
                    if (childNode.IsFullWord)
                    {
                        _commandMap.Remove(childNode.Word!);
                        childNode.Word = null;
                    }

                    if (childNode.Children.Count == 0)
                    {
                        childNode = null;
                    }
                }
                else
                {
                    childNode = RemoveHelper(childNode, currentCharacterIndex + 1);
                    if (childNode is null)
                    {
                        node.Children.Remove(name[currentCharacterIndex]);
                    }
                }

                return node;
            }
        }
    }
}