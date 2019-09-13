#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
            var compress = buffer.Compress();
            Assert.NotEqual(buffer.Length, compress.Length);

            var decompress = compress.Decompress();
            Assert.Equal(buffer.Length, decompress.Length);

            var raw = decompress.AsEncodingString();
            Assert.Equal(str, raw);
        }

    }
}
