using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class InternalFileUploadServiceTests
    {
        private IFileTransferService _fileUpload;

        public InternalFileUploadServiceTests()
        {
            _fileUpload = TestServiceProvider.Current.GetRequiredService<IFileTransferService>();
        }


        [Fact]
        public void UploadFileAsync()
        {
            var uploadApi = "https://domain.com/api/upload";
            var uploadFile = @"c:\temp.txt";

            var result = _fileUpload.UploadFileAsync(uploadApi, uploadFile).Result;
            Assert.NotEmpty(result);
        }

    }
}
