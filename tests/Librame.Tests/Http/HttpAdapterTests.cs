using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Http;

namespace Librame.Tests.Http
{
    [TestClass()]
    public class HttpAdapterTests
    {
        private readonly IHttpAdapter _adapter;

        public HttpAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.Http;
        }


        [TestMethod()]
        public void GetWebTest()
        {
            var url = "http://m.mydrivers.com";

            _adapter.GetWeb(url, (s) =>
            {
                Assert.IsNotNull(s);
                Assert.IsTrue(s.Length > 0);
            });
        }

    }
}