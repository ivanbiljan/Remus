using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Remus.Tests 
{
    public sealed class ConsoleSenderTests
    {
        [Fact]
        public void SendMessage_NullMessage_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsoleSender().SendMessage(null!));
        }
        
        [Fact]
        public void SendMessage_IsCorrect()
        {
            var consoleSender = new ConsoleSender();
            var output = new StringWriter();
            Console.SetOut(output);

            consoleSender.SendMessage("Hello, World");

            Assert.Equal("Hello, World\r\n", output.ToString());
        }
    }
}