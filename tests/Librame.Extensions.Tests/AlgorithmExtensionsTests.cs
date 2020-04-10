using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class AlgorithmExtensionsTests
    {
        private readonly string _rawString
            = nameof(AlgorithmExtensionsTests);

        private readonly Encoding _encoding
            = Encoding.UTF8;

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
            Assert.NotEmpty(_rawString.Md5Base64String(_encoding));
        }


        [Fact]
        public void Sha1Base64StringTest()
        {
            Assert.NotEmpty(_rawString.Sha1Base64String(_encoding));
        }


        [Fact]
        public void Sha256Base64StringTest()
        {
            Assert.NotEmpty(_rawString.Sha256Base64String(_encoding));
        }


        [Fact]
        public void Sha384Base64StringTest()
        {
            Assert.NotEmpty(_rawString.Sha384Base64String(_encoding));
        }


        [Fact]
        public void Sha512Base64StringTest()
        {
            Assert.NotEmpty(_rawString.Sha512Base64String(_encoding));
        }

        #endregion


        #region HMAC Algorithm

        [Fact]
        public void HmacMd5Base64StringTest()
        {
            var key = "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Q==".FromBase64String();
            Assert.NotEmpty(_rawString.HmacMd5Base64String(key, _encoding));
        }


        [Fact]
        public void HmacSha1Base64StringTest()
        {
            var key = "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Q==".FromBase64String();
            Assert.NotEmpty(_rawString.HmacSha1Base64String(key, _encoding));
        }


        [Fact]
        public void HmacSha256Base64StringTest()
        {
            var key = "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Q==".FromBase64String();
            Assert.NotEmpty(_rawString.HmacSha256Base64String(key, _encoding));
        }


        [Fact]
        public void HmacSha384Base64StringTest()
        {
            var key = "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Rka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Rka278mPkmw45a3gtsNWe0=".FromBase64String();
            Assert.NotEmpty(_rawString.HmacSha384Base64String(key, _encoding));
        }


        [Fact]
        public void HmacSha512Base64StringTest()
        {
            var key = "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Rka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Rka278mPkmw45a3gtsNWe0=".FromBase64String();
            Assert.NotEmpty(_rawString.HmacSha512Base64String(key, _encoding));
        }

        #endregion


        #region Symmetric Algorithm

        [Fact]
        public void AesBase64StringTest()
        {
            var key = "JUmlxL8G806eU4R5eSU+mEmlxL8G806eU4R5eSU+mCU=".FromBase64String();
            var base64 = _rawString.AsAesBase64String(key, _encoding);
            Assert.Equal(_rawString, base64.FromAesBase64String(key, _encoding));
        }


        [Fact]
        public void DesBase64StringTest()
        {
            var key = "JUmlxL8G804=".FromBase64String();
            var base64 = _rawString.AsDesBase64String(key, _encoding);
            Assert.Equal(_rawString, base64.FromDesBase64String(key, _encoding));
        }


        [Fact]
        public void TripleDesBase64StringTest()
        {
            var key = "JUmlxL8G806eU4R5eSU+mEmlxL8G806e".FromBase64String();
            var base64 = _rawString.AsTripleDesBase64String(key, _encoding);
            Assert.Equal(_rawString, base64.FromTripleDesBase64String(key, _encoding));
        }

        #endregion


        //#region Asymmetric Algorithm : RSA

        //[Fact]
        //public void RsaBase64StringTest()
        //{
        //    var parameters = RSA.Create().ExportParameters(true);
        //    var base64 = _rawString.AsRsaBase64String(_encoding, parameters);
        //    Assert.Equal(_rawString, base64.FromRsaBase64String(_encoding, parameters));
        //}

        //#endregion

    }
}
