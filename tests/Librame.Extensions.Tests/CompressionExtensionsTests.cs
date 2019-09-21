#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class CompressionExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var str = nameof(CompressionExtensionsTests);
            
            var buffer = str.FromEncodingString();
            var compress = buffer.Compress(); // buffer.Length = 29
            Assert.NotEqual(buffer.Length, compress.Length);

            var decompress = compress.Decompress();
            Assert.Equal(buffer.Length, decompress.Length);

            var raw = decompress.AsEncodingString();
            Assert.Equal(str, raw);

            var zipCompress = buffer.GZipCompress(); // buffer.Length = 43
            var zipDecompress = zipCompress.GZipDecompress();
            Assert.True(buffer.SequenceEqual(zipDecompress));
            Assert.Equal(str, zipDecompress.AsEncodingString());

            var filePath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToFileTime() + ".txt");
            File.WriteAllText(filePath, str);

            var fileInfo = new FileInfo(filePath);
            var zipFileInfo = fileInfo.GZipCompress();
            Assert.NotEqual(fileInfo.FullName, zipFileInfo.FullName);
            Assert.True(zipFileInfo.Exists);

            var unzipFileInfo = zipFileInfo.GZipDecompress();
            Assert.Equal(fileInfo.FullName, unzipFileInfo.FullName);

            zipFileInfo.Delete();
            Assert.False(File.Exists(zipFileInfo.FullName)); // zipFileInfo.Exists = true
        }

    }
}
