using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class FileServiceTests
    {
        private IFileService _service;

        public FileServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<IFileService>();
        }


        [Fact]
        public async void AllTest()
        {
            var provider = (_service as FileService).Options.FileProviders.First() as PhysicalStorageFileProvider;

            var now = DateTime.Now;
            var subdir = Directory.CreateDirectory(Path.Combine(provider.Root, now.Ticks.ToString()));

            var file = Path.Combine(subdir.FullName, "test.txt");
            var text = $"Now: {now.ToString()}";
            File.WriteAllText(file, text);

            var contents = await _service.GetDirectoryContentsAsync(subdir.Name).ConfigureAndResultAsync();
            Assert.NotEmpty(contents);

            var fileInfo = contents.First();
            Assert.Equal(file, fileInfo.PhysicalPath);

            var copyFile = Path.Combine(subdir.FullName, "copy_test.txt");

            // file to copyFile
            using (var writeStream = new FileStream(copyFile, FileMode.Create))
            {
                await _service.ReadAsync(fileInfo, writeStream).ConfigureAndWaitAsync();
            }
            Assert.Equal(text, File.ReadAllText(copyFile));

            File.Delete(file);

            // copyFile to file
            using (var readStream = new FileStream(copyFile, FileMode.Open))
            {
                await _service.WriteAsync(fileInfo, readStream).ConfigureAndWaitAsync();
            }
            Assert.Equal(text, File.ReadAllText(fileInfo.PhysicalPath));

            File.Delete(copyFile);
            File.Delete(file);
            Directory.Delete(subdir.FullName);
        }

    }
}
