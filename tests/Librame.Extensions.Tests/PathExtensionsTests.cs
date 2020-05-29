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
            var dir = AppContext.BaseDirectory;

            var path = dir.CombinePath("test.txt");
            var changePath = path.ChangeFileName((baseName, ext) => $"change{ext}");

            Assert.Equal(dir.CombinePath("change.txt"), changePath);
        }


        [Fact]
        public void GetFileBaseNameAndExtensionTest()
        {
            var filePath = @"c:\temp\filename.ext";
            (string baseName, string extension) = filePath.GetFileBaseNameAndExtension(out var basePath);
            Assert.Equal("filename", baseName);
            Assert.Equal(".ext", extension);
            Assert.NotEmpty(basePath);

            var virtualPath = "/test/path";
            (baseName, extension) = virtualPath.GetFileBaseNameAndExtension(out basePath);
            Assert.Equal("path", baseName);
            Assert.Equal(string.Empty, extension);
            Assert.NotEmpty(basePath);

            virtualPath = "/test/.path";
            (baseName, extension) = virtualPath.GetFileBaseNameAndExtension(out basePath);
            Assert.Equal(string.Empty, baseName);
            Assert.Equal(".path", extension);
            Assert.NotEmpty(basePath);
        }


        [Fact]
        public void GetFileNameWithoutPathTest()
        {
            var filePath = @"c:\temp\filename.ext";
            Assert.Equal("filename.ext", filePath.GetFileNameWithoutPath(out var basePath));
            Assert.NotEmpty(basePath);

            filePath = "/app/filename.ext";
            Assert.Equal("filename.ext", filePath.GetFileNameWithoutPath(out basePath));
            Assert.NotEmpty(basePath);
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

            Assert.True(path.TryGetExtension(new string[] { ".jpg", ".png", "gif" }, out string extension));
            Assert.Equal(".jpg", extension);
        }

    }
}
