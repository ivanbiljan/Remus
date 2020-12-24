using Remus.Attributes;

namespace Remus.Tests
{
    public sealed class TestCommandRegistry
    {
        public int Number { get; private set; }
        public bool Boolean { get; private set; }
        public string String { get; private set; }

        [CommandHandler("test", "")]
        public void TestOverload1()
        {
            Number = 1024;
        }

        [CommandHandler("test", "")]
        public void TestOverload2(int x)
        {
            Number = x;
        }

        [CommandHandler("test", "")]
        public void TestOverload3([OptionalArgument("x", "")] int x)
        {
            Number = 0;
        }
    }
}