using System;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Remus.Tests
{
    public sealed class CommandTests
    {
        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Command(null!, new Mock<ICommandService>().Object, "test"));
        }

        [Fact]
        public void Ctor_NullCommandService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Command(new Mock<ILogger>().Object, null!, "test"));
        }

        [Fact]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Command(new Mock<ILogger>().Object, new Mock<ICommandService>().Object, null!));
        }

        [Fact]
        public void Ctor_IsCorrect()
        {
            var logger = new Mock<ILogger>();
            var commandService = new Mock<ICommandService>();

            var command = new Command(logger.Object, commandService.Object, "test");

            Assert.Equal("test", command.Name);
        }
    }
}