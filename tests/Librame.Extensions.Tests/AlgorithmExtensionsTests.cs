using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class AlgorithmExtensionsTests
    {
        private readonly string _rawString
            = nameof(AlgorithmExtensionsTests);

        private readonly RandomNumberGenerator _generator
            = RandomNumberGenerator.Create();


        #region Base and Hex

        [Fact]
        public void Base32StringTest()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            var base32 = bytes.AsBase32String();
            Assert.True(bytes.SequenceEqual(base32.FromBase32String()));
        }

        [Fact]
        public void Base64StringTest()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            var base64 = bytes.AsBase64String();
            Assert.True(bytes.SequenceEqual(base64.FromBase64String()));
        }

        [Fact]
        public void HexStringTest()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            var hex = bytes.AsHexString();
            Assert.True(bytes.SequenceEqual(hex.FromHexString()));
        }

        #endregion


        #region Hash Algorithm

        [Fact]
        public void Md5Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().Md5Base64String());
        }

        [Fact]
        public void Sha1Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().Sha1Base64String());
        }

        [Fact]
        public void Sha256Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().Sha256Base64String());
        }

        [Fact]
        public void Sha384Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().Sha384Base64String());
        }

        [Fact]
        public void Sha512Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().Sha512Base64String());
        }

        #endregion


        #region HMAC Algorithm

        [Fact]
        public void HmacMd5Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().HmacMd5().AsBase64String());
        }

        [Fact]
        public void HmacSha1Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().HmacSha1().AsBase64String());
        }

        [Fact]
        public void HmacSha256Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().HmacSha256().AsBase64String());
        }

        [Fact]
        public void HmacSha384Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().HmacSha384().AsBase64String());
        }

        [Fact]
        public void HmacSha512Base64StringTest()
        {
            Assert.NotEmpty(_rawString.FromEncodingString().HmacSha512().AsBase64String());
        }

        #endregion


        #region Symmetric Algorithm

        [Fact]
        public void AesBase64StringTest()
        {
            var base64 = _rawString.FromEncodingString().AsAes().AsBase64String();
            Assert.Equal(_rawString, base64.FromBase64String().FromAes().AsEncodingString());
        }

        [Fact]
        public void DesBase64StringTest()
        {
            var base64 = _rawString.FromEncodingString().AsDes().AsBase64String();
            Assert.Equal(_rawString, base64.FromBase64String().FromDes().AsEncodingString());
        }

        [Fact]
        public void TripleDesBase64StringTest()
        {
            var base64 = _rawString.FromEncodingString().AsTripleDes().AsBase64String();
            Assert.Equal(_rawString, base64.FromBase64String().FromTripleDes().AsEncodingString());
        }

        #endregion


        #region Asymmetric Algorithm

        [Fact]
        public void RsaEndecryptTest()
        {
            var parameters = RSA.Create().ExportParameters(true);

            var base64 = _rawString.FromEncodingString().AsRsa(parameters).AsBase64String();
            Assert.Equal(_rawString, base64.FromBase64String().FromRsa(parameters).AsEncodingString());
        }

        #endregion

    }
}