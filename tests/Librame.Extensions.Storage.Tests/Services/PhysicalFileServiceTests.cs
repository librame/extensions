using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class PhysicalFileServiceTests
    {
        private IFileService _file;

        public PhysicalFileServiceTests()
        {
            _file = TestServiceProvider.Current.GetRequiredService<IFileService>();
        }


        [Fact]
        public async void GetProviderAsyncTest()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..");
            var provider = await _file.GetProviderAsync(root);
            var files = provider.GetDirectoryContents(@"bin\Debug\netcoreapp2.2");
            Assert.NotEmpty(files);
        }

    }
}
