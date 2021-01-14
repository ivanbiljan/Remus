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
        public TrieNode(char letter)
        {
            Letter = letter;
        }
        
        public char Letter { get; }
        
        public string? Word { get; set; }

        public bool IsFullWord => Word != null;
        
        public IDictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    }
}