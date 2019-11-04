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

            // RTL
            var compress = buffer.RtlCompress(); // buffer.Length = 29
            Assert.NotEqual(buffer.Length, compress.Length);

            var decompress = compress.RtlDecompress();
            Assert.Equal(buffer.Length, decompress.Length);

            var raw = decompress.AsEncodingString();
            Assert.Equal(str, raw);

            // Deflate
            var compressedBuffer = buffer.DeflateCompress(); // buffer.Length = 25
            var decompressedBuffer = compressedBuffer.DeflateDecompress();
            Assert.True(buffer.SequenceEqual(decompressedBuffer));
            Assert.Equal(str, decompressedBuffer.AsEncodingString());

            var filePath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToFileTime() + ".txt");
            File.WriteAllText(filePath, str);

            var fileInfo = new FileInfo(filePath);
            var compressedFileInfo = fileInfo.DeflateCompress();
            Assert.NotEqual(fileInfo.FullName, compressedFileInfo.FullName);
            Assert.True(compressedFileInfo.Exists);

            var decompressedFileInfo = compressedFileInfo.DeflateDecompress();
            Assert.Equal(fileInfo.FullName, decompressedFileInfo.FullName);

            compressedFileInfo.Delete();
            Assert.False(File.Exists(compressedFileInfo.FullName)); // compressedFileInfo.Exists = true

            // GZip
            compressedBuffer = buffer.GZipCompress(); // buffer.Length = 43
            decompressedBuffer = compressedBuffer.GZipDecompress();
            Assert.True(buffer.SequenceEqual(decompressedBuffer));
            Assert.Equal(str, decompressedBuffer.AsEncodingString());

            filePath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToFileTime() + ".txt");
            File.WriteAllText(filePath, str);

            fileInfo = new FileInfo(filePath);
            compressedFileInfo = fileInfo.GZipCompress();
            Assert.NotEqual(fileInfo.FullName, compressedFileInfo.FullName);
            Assert.True(compressedFileInfo.Exists);

            decompressedFileInfo = compressedFileInfo.GZipDecompress();
            Assert.Equal(fileInfo.FullName, decompressedFileInfo.FullName);

            compressedFileInfo.Delete();
            Assert.False(File.Exists(compressedFileInfo.FullName)); // compressedFileInfo.Exists = true
        }

    }
}
