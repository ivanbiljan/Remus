using Xunit;

namespace Remus.Tests
{
    public sealed class TrieNodeTests
    {
        [Theory]
        [InlineData('a')]
        [InlineData('\0')]
        [InlineData(' ')]
        public void GetLetter_IsCorrect(char letter)
        {
            var node = new TrieNode(letter);

            Assert.Equal(letter, node.Letter);
        }
    }
}