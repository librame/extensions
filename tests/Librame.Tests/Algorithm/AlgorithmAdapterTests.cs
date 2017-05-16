using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Algorithm;
using Librame.Algorithm.Asymmetries;

namespace Librame.Tests.Algorithm
{
    [TestClass()]
    public class AlgorithmAdapterTests
    {
        private readonly string _test = "Algorithm Test";
        private readonly IAlgorithmAdapter _adapter = null;

        public AlgorithmAdapterTests()
        {
            _adapter = LibrameArchitecture.Adapters.Algorithm;
        }
        

        [TestMethod()]
        public void AesTest()
        {
            var str = _adapter.BouncyCastleAes.Encrypt(_test);
            Assert.IsFalse(string.IsNullOrEmpty(str));

            str = _adapter.BouncyCastleAes.Decrypt(str);
            Assert.IsFalse(string.IsNullOrEmpty(str));

            Assert.AreEqual(str, _test);
        }
        
        [TestMethod()]
        public void DesTest()
        {
            var str = _adapter.BouncyCastleDes.Encrypt(_test);
            Assert.IsFalse(string.IsNullOrEmpty(str));

            str = _adapter.BouncyCastleDes.Decrypt(str);
            Assert.IsFalse(string.IsNullOrEmpty(str));

            Assert.AreEqual(str, _test);
        }

        [TestMethod()]
        public void RsaTest()
        {
            string _password = "123456";

            AsymmetryKeyPair keyPair;

            var str = _adapter.BouncyCastleRsa.Encrypt(_test, _password, out keyPair);
            Assert.IsFalse(string.IsNullOrEmpty(str));

            str = _adapter.BouncyCastleRsa.Decrypt(str, keyPair.Private, _password);
            Assert.IsFalse(string.IsNullOrEmpty(str));

            Assert.AreEqual(str, _test);
        }

    }
}