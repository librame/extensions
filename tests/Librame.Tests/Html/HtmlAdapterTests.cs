using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Html;
using Librame.Utility;
using System.IO;

namespace Librame.Tests.Html
{
    [TestClass()]
    public class HtmlAdapterTests
    {
        private readonly IHtmlAdapter _adapter;

        public HtmlAdapterTests()
        {
            _adapter = LibrameArchitecture.Adapters.Html;
        }


        [TestMethod()]
        public void TokenTest()
        {
            var file = TestHelper.DefaultDirectory.AppendPath("test.html");
            var url = "http://m.mydrivers.com";
            _adapter.ParseWeb(url, (doc) =>
            {
                doc.Save(file);
            });
            
            Assert.IsTrue(File.Exists(file));
        }

    }
}