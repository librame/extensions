using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    using Core.Combiners;
    using Services;

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
            var url = "https://mat1.gtimg.com/pingjs/ext2020/qqindex2018/dist/img/qq_logo_2x.png";
            var filePath = @"d:\qq_logo.png";

            var combiner = await _fileTransfer.DownloadFileAsync(url, filePath).ConfigureAndResultAsync();
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
