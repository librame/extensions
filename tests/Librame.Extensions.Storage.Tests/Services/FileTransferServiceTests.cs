using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    using Core.Combiners;
    using Storage.Services;

    public class FileTransferServiceTests
    {
        private IFileTransferService _fileTransfer;

        public FileTransferServiceTests()
        {
            _fileTransfer = TestServiceProvider.Current.GetRequiredService<IFileTransferService>();
        }


        [Fact]
        public async void DownloadFileAsync()
        {
            var url = "https://www.baidu.com/img/baidu_jgylogo3.gif";
            var filePath = @"d:\baidu_jgylogo3.gif";
            File.Delete(filePath);

            var combiner = await _fileTransfer.DownloadFileAsync(url, filePath).ConfigureAwait();
            Assert.NotNull(combiner);
            Assert.True(combiner.Exists());

            combiner.Delete();
            Assert.False(combiner.Exists());
        }

        [Fact]
        public void UploadFileAsync()
        {
            var url = "https://domain.com/api/upload";
            var filePath = @"d:\_never.txt";

            Assert.ThrowsAsync<FileNotFoundException>(() =>
            {
                return _fileTransfer.UploadFileAsync(url, filePath);
            });
        }

    }
}
