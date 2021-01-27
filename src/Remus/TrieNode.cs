using System.Collections.Generic;

namespace Remus
{
    /// <summary>
    ///     Represents a node in a trie.
    /// </summary>
    internal sealed class TrieNode
    {
        public TrieNode(char letter)
        {
            Letter = letter;
        }

        public IDictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();

        public bool IsFullWord => Word != null;

        public char Letter { get; }

        public string? Word { get; set; }
    }
}