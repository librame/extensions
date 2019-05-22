using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class PathExtensionsTests
    {
        [Fact]
        public void AsFileInfoTest()
        {
            var file = "C:\\Windows\\notepad.exe".AsFileInfo();
            Assert.NotNull(file);
        }

        [Fact]
        public void AsDirectoryInfoTest()
        {
            var directory = "C:\\Users".AsDirectoryInfo();
            Assert.NotNull(directory);
        }

        [Fact]
        public void SubdirectoryTest()
        {
            var directory = "C:\\Users".AsDirectoryInfo();
            directory = directory.Subdirectory("Administrator");
            Assert.NotNull(directory);
        }


        [Fact]
        public void ChangeFileNameTest()
        {
            var path = AppContext.BaseDirectory.AppendPathOrUri("test.txt");
            var changePath = path.ChangeFileName((baseName, ext) => $"change{ext}");

            Assert.Equal(AppContext.BaseDirectory.AppendPathOrUri("change.txt"), changePath);
        }


        [Fact]
        public void AppendPathOrUriTest()
        {
            var result = @"c:\temp\filename.ext";
            Assert.Equal(result, @"c:\temp\1\2".AppendPathOrUri("..\\..\\filename.ext"));

            result = @"http://www.domain.name/controller/action";
            Assert.Equal(result, $"http://www.domain.name/webapi/entities".AppendPathOrUri("/controller/action"));
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
        public void CombineUriTest()
        {
            var result = @"http://www.domain.name/controller/action";
            var baseUri = "http://www.domain.name/";

            Assert.Equal(result, baseUri.CombineUriToString("controller/action"));
            Assert.Equal(result, $"{baseUri}webapi/entities".CombineUriToString("/controller/action"));
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
