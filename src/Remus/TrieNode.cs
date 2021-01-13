using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remus {
    /// <summary>
    /// Represents a node in a trie.
    /// </summary>
    internal sealed class TrieNode
    {
        public TrieNode(char letter, bool isFullWord)
        {
            Letter = letter;
            IsFullWord = isFullWord;
        }
        
        public char Letter { get; }
        
        public bool IsFullWord { get; }
        
        public IDictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    }

    internal sealed class Trie
    {
        private readonly TrieNode _root = new TrieNode('\0', false);

        public void AddWord(string word)
        {
            var currentNode = _root;
            for (var i = 0; i < word.Length; ++i)
            {
                var currentLetter = word[i];
                if (!currentNode.Children.TryGetValue(currentLetter, out var nextNode))
                {
                    nextNode = new TrieNode(currentLetter, i == word.Length - 1);
                    currentNode.Children.Add(currentLetter, nextNode);
                }

                currentNode = nextNode;
            }
        }

        public void AddWords(string[] words)
        {
            foreach (var word in words)
            {
                AddWord(word);
            }
        }

        public IList<Command> GetCommandSuggestions(string searchQuery)
        {
            var commands = new List<Command>();
            if (searchQuery is null)
            {
                throw new ArgumentNullException(nameof(searchQuery));
            }
            
            return commands;
        }
    }
}