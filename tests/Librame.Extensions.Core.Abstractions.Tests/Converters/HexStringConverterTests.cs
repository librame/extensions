using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Converters;

    public class HexStringConverterTests
    {
        [Fact]
        public void AllTest()
        {
            var buffer = RandomNumberGenerator.Create().GenerateByteArray(16);

            var test = new ReadOnlyMemory<byte>(buffer);
            var converter = HexStringConverter.Default;

            var value = converter.ConvertToString(test);
            Assert.NotEmpty(value);

            var source = converter.ConvertFromString<ReadOnlyMemory<byte>>(value);
            Assert.Equal(test.Length, source.Length);
            Assert.True(buffer.SequenceEqual(source.ToArray()));
        }
    }
}
