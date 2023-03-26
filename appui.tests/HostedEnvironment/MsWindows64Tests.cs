using appui.shared.HostedEnvironment;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace appui.tests.HostedEnvironment
{
    public class MsWindows64Tests
    {
        [Fact]
        public void When_CreateInstance_NewInstance()
        {
            var instance = new MsWindows64(null);
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(null, true)]
        public async Task When_ExecuteArgIsNull_NoException(string fileName, bool expected)
        {
            var mock = new MsWindows64(null);
            await mock.Execute(fileName);
            Assert.True(expected);
        }

        [Fact]
        public async Task When_ExecuteFileNameIsToooLong_NoException()
        {
            var logger = new Mock<ILogger<MsWindows64>>();
            var mock = new MsWindows64(logger.Object);

            var file = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            await mock.Execute(file);
            Assert.True(true);
        }
    }
}