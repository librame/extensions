using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ApplicationEnvironmentInfoTests
    {
        [Fact]
        public void AllTest()
        {
            var info = new ApplicationEnvironmentInfo();

            Assert.NotEmpty(info.ApplicationBasePath);
            Assert.NotEmpty(info.ApplicationName);
            Assert.NotEmpty(info.ApplicationVersion);

            Assert.NotEmpty(info.MachineName);
        }

    }
}
