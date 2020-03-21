using System;
using System.IO;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class PathExtensionsTests
    {
        [Fact]
        public void WithoutDevelopmentRelativePathTest()
        {
            var basePath = Directory.GetCurrentDirectory().WithoutDevelopmentRelativePath();
            Assert.EndsWith(@"\tests\Librame.Extensions.Tests", basePath);

            basePath = basePath.CombinePath(@"bin\x64\Release").WithoutDevelopmentRelativePath();
            Assert.EndsWith(@"\tests\Librame.Extensions.Tests", basePath);
        }


        [Fact]
        public void SubdirectoryTest()
        {
            var directoryName = @"C:\Users";
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

            Assert.Equal(result, basePath.CombinePath(@"\filename.ext"));
            Assert.Equal(result, basePath.CombinePath(@".\filename.ext"));
            Assert.Equal(result, @$"{basePath}\1\2".CombinePath(@"..\..\filename.ext"));

            Assert.Equal(result, basePath.CombinePath("/filename.ext"));
            Assert.Equal(result, basePath.CombinePath("./filename.ext"));
            Assert.Equal(result, @$"{basePath}\1\2".CombinePath("../../filename.ext"));

            Assert.Equal(result, basePath.CombinePath("./filename.ext"));
            Assert.Equal(result, basePath.CombinePath("/filename.ext"));

            // Test Linux Environment
            Assert.Equal("/app/filename.ext", "/app".CombinePath("filename.ext"));
            Assert.Equal("/app/123/filename.ext", @"/app\123".CombinePath("filename.ext"));
            Assert.Equal("/app/123/filename.ext", "/app".CombinePath(@"\123\filename.ext"));
        }


        [Fact]
        public void ExtractHasExtensionTest()
        {
            var paths = new string[] { @"c:\temp\filename.jpg", @"c:\temp\filename.png", @"c:\temp\filename.gif", @"c:\temp\filename.mp3" };

            var results = paths.ExtractHasExtension(new string[] { ".jpg", ".mp3" });
            Assert.False(results.IsEmpty());
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
