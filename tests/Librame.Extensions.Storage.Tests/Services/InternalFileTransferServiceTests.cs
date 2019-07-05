using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    using Core;

    public class InternalFileTransferServiceTests
    {
        private IFileTransferService _fileTransfer;

        public InternalFileTransferServiceTests()
        {
            _fileTransfer = TestServiceProvider.Current.GetRequiredService<IFileTransferService>();
        }


        [Fact]
        public async void DownloadFileAsync()
        {
            var url = "https://github.com/librame/Librame/raw/dev/SPECIFICATION.md";
            var filePath = @"c:\Librame_SPECIFICATION.md";

            var locator = await _fileTransfer.DownloadFileAsync(url, filePath);
            Assert.NotNull(locator);
            Assert.True(locator.Exists());

            locator.Delete();
            Assert.False(locator.Exists());
        }

        [Fact]
        public async void UploadFileAsync()
        {
            var url = "https://domain.com/api/upload";
            var filePath = @"c:\temp.txt";

            var response = await _fileTransfer.UploadFileAsync(url, filePath);
            Assert.NotEmpty(response);
        }

    }
}
