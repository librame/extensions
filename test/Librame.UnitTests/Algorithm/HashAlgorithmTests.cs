using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Algorithm;

namespace Librame.UnitTests.Algorithm
{
    [TestClass()]
    public class HashAlgorithmTests
    {
        private readonly string _test = "Hash Test";
        private readonly IHashAlgorithm _hash = null;

        public HashAlgorithmTests()
        {
            _hash = LibrameArchitecture.AdapterManager.AlgorithmAdapter.Hash;
        }

        [TestMethod()]
        public void Md5Test()
        {
            string md5 = _hash.ToMd5(_test);
            Assert.IsNotNull(md5);
        }

        [TestMethod()]
        public void Sha1Test()
        {
            string sha1 = _hash.ToSha1(_test);
            Assert.IsNotNull(sha1);
        }

        [TestMethod()]
        public void Sha256Test()
        {
            string sha256 = _hash.ToSha256(_test);
            Assert.IsNotNull(sha256);
        }

        [TestMethod()]
        public void Sha384Test()
        {
            string sha384 = _hash.ToSha384(_test);
            Assert.IsNotNull(sha384);
        }

        [TestMethod()]
        public void Sha512Test()
        {
            string sha512 = _hash.ToSha512(_test);
            Assert.IsNotNull(sha512);
        }

    }
}