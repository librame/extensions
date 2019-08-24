using System;
using System.IO;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class PathExtensionsTests
    {
        [Fact]
        public void SubdirectoryTest()
        {
            var directoryName = "C:\\Users";
            var directory = new DirectoryInfo(directoryName);

            Assert.Equal(directoryName.Subdirectory("Administrator").FullName,
                directory.Subdirectory("Administrator").FullName);
        }


        [Fact]
        public void ChangeFileNameTest()
        {
            var path = AppContext.BaseDirectory.CombinePath("test.txt");
            var changePath = path.ChangeFileName((baseName, ext) => $"change{ext}");

            Assert.Equal(AppContext.BaseDirectory.CombinePath("change.txt"), changePath);
        }


        [Fact]
        public void CombinePathTest()
        {
            var result = @"c:\temp\filename.ext";
            var basePath = @"c:\temp";

            Assert.Equal(result, basePath.CombinePath("filename.ext"));
            Assert.Equal(result, basePath.CombinePath("\\filename.ext"));
            Assert.Equal(result, $"{basePath}\\1\\2".CombinePath("..\\..\\filename.ext"));
        }


        [Fact]
        public void ExtractHasExtensionTest()
        {
            var paths = new string[] { @"c:\temp\filename.jpg", @"c:\temp\filename.png", @"c:\temp\filename.gif", @"c:\temp\filename.mp3" };

            var results = paths.ExtractHasExtension(new string[] { ".jpg", ".mp3" });
            Assert.False(results.IsNullOrEmpty());
        }

        [Fact]
        public void TryHasExtensionTest()
        {
            var path = @"c:\temp\filename.jpg";

            Assert.True(path.TryHasExtension(new string[] { ".jpg", ".png", "gif" }, out string extension));
            Assert.Equal(".jpg", extension);
        }

    }
}
