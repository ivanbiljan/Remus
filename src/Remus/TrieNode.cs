using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Remus {
    /// <summary>
    /// Represents a node in a trie.
    /// </summary>
    internal sealed class TrieNode
    {
        public TrieNode(char letter)
        {
            Letter = letter;
        }
        
        public char Letter { get; }
        
        public string? Word { get; set; }

        public bool IsFullWord => Word != null;
        
        public IDictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    }

    /// <summary>
    /// Represents a command trie.
    /// </summary>
    internal sealed class CommandTrie
    {
        private readonly IDictionary<string, Command> _commandMap = new Dictionary<string, Command>();
        private readonly TrieNode _root = new TrieNode('\0');

        /// <summary>
        /// Adds a command to the trie.
        /// </summary>
        /// <param name="command">The command, which must not be <see langword="null"/>.</param>
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
        /// Adds a list of commands to the trie.
        /// </summary>
        /// <param name="commands">The command list, which must not be <see langword="null"/>.</param>
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
        /// Gets a list of commands
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
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
                    Debug.Assert(_commandMap.ContainsKey(node.Word!));
                    commands.Add(_commandMap[node.Word!]);
                }

                foreach (var (_, childNode) in node.Children)
                {
                    RecurseSubtree(childNode);
                }
            }
        }
    }
}