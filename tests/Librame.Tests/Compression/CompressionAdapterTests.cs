using Librame.Compression;
using Librame.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;

namespace Librame.Tests.Compression
{
    [TestClass()]
    public class CompressionAdapterTests
    {
        private readonly string _testDirectory = @"D:\Temp\";
        private readonly ICompressionAdapter _adapter = null;

        public CompressionAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.Compression;
        }
        

        [TestMethod()]
        public void CompressZipTest()
        {
            // 压缩文件
            string zipFileName = TestHelper.DefaultDirectory.AppendPath("test.zip");

            _adapter.Compress(zipFileName, (zip) => zip.AddDirectory(_testDirectory));
            Assert.IsTrue(File.Exists(zipFileName));

            // 解压目录
            string decompressDirectory = _testDirectory.Append("test");
            Directory.CreateDirectory(decompressDirectory);

            _adapter.Decompress(zipFileName, decompressDirectory);
            File.Delete(zipFileName);

            // 解压后的文件
            var files = Directory.EnumerateFiles(decompressDirectory).ToList();
            Assert.IsTrue(files.Count > 0);

            // 删除解压后的文件及文件夹
            foreach (var f in files)
                File.Delete(f);

            Directory.Delete(decompressDirectory);

            // 是否删除成功
            Assert.IsFalse(Directory.Exists(decompressDirectory));
        }

    }
}
