using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class InternalPhysicalFileServiceTests
    {
        private IFileService _file;

        public InternalPhysicalFileServiceTests()
        {
            _file = TestServiceProvider.Current.GetRequiredService<IFileService>();
        }


        [Fact]
        public void GetProviderAsyncTest()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..");
            var provider = _file.GetProviderAsync(root).Result;
            var files = provider.GetDirectoryContents(@"bin\Debug\netcoreapp2.2");
            Assert.NotEmpty(files);
        }

    }
}
