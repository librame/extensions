using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class SymmetricServiceTests
    {
        private readonly string _source;
        private readonly Encoding _encoding;
        private readonly ISymmetricService _symmetric;


        public SymmetricServiceTests()
        {
            _source = nameof(SymmetricServiceTests);
            _encoding = Encoding.UTF8;
            _symmetric = TestServiceProvider.Current.GetRequiredService<ISymmetricService>();
        }


        [Fact]
        public void AesTest()
        {
            var buffer = _source.FromEncodingString(_encoding);

            buffer = _symmetric.EncryptAes(buffer);
            buffer = _symmetric.DecryptAes(buffer);

            Assert.Equal(_source, buffer.AsEncodingString(_encoding));
        }


        [Fact]
        public void DesTest()
        {
            var buffer = _source.FromEncodingString(_encoding);

            buffer = _symmetric.EncryptDes(buffer);
            buffer = _symmetric.DecryptDes(buffer);

            Assert.Equal(_source, buffer.AsEncodingString(_encoding));
        }


        [Fact]
        public void TripleDesTest()
        {
            var buffer = _source.FromEncodingString(_encoding);

            buffer = _symmetric.EncryptTripleDes(buffer);
            buffer = _symmetric.DecryptTripleDes(buffer);

            Assert.Equal(_source, buffer.AsEncodingString(_encoding));
        }

    }
}
