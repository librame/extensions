using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Core.Identifiers;
    using Encryption.Services;

    public class SymmetricServiceTests
    {
        private readonly string _source;
        private readonly Encoding _encoding;
        private readonly SecurityIdentifier _identifier;
        private readonly ISymmetricService _symmetric;


        public SymmetricServiceTests()
        {
            _source = nameof(SymmetricServiceTests);
            _identifier = SecurityIdentifier.New();
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

            var encrypt = _symmetric.EncryptAes(buffer, _identifier).AsBase64String();
            Assert.NotEmpty(encrypt);

            var decrypt = _symmetric.DecryptAes(encrypt.FromBase64String(), _identifier)
                .AsEncodingString(_encoding);
            Assert.Equal(_source, decrypt);
        }


        [Fact]
        public void DesTest()
        {
            var buffer = _source.FromEncodingString(_encoding);

            buffer = _symmetric.EncryptDes(buffer);
            buffer = _symmetric.DecryptDes(buffer);
            Assert.Equal(_source, buffer.AsEncodingString(_encoding));

            var encrypt = _symmetric.EncryptDes(buffer, _identifier).AsBase64String();
            Assert.NotEmpty(encrypt);

            var decrypt = _symmetric.DecryptDes(encrypt.FromBase64String(), _identifier)
                .AsEncodingString(_encoding);
            Assert.Equal(_source, decrypt);
        }


        [Fact]
        public void TripleDesTest()
        {
            var buffer = _source.FromEncodingString(_encoding);

            buffer = _symmetric.EncryptTripleDes(buffer);
            buffer = _symmetric.DecryptTripleDes(buffer);
            Assert.Equal(_source, buffer.AsEncodingString(_encoding));

            var encrypt = _symmetric.EncryptTripleDes(buffer, _identifier).AsBase64String();
            Assert.NotEmpty(encrypt);

            var decrypt = _symmetric.DecryptTripleDes(encrypt.FromBase64String(), _identifier)
                .AsEncodingString(_encoding);
            Assert.Equal(_source, decrypt);
        }

    }
}
