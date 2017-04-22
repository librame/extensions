using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.UnitTests.Utility
{
    [TestClass()]
    public class UriUtilityTests
    {
        private readonly string _url = null;

        public UriUtilityTests()
        {
            _url = "http://comment.war.163.com/photoview_bbs/PHOT23T4U00014T8.html";
        }


        [TestMethod()]
        public void IsUrlTest()
        {
            Assert.IsTrue(UriUtility.IsUrl(_url));
        }

        [TestMethod()]
        public void IsHttpOrHttpsUrlTest()
        {
            Assert.IsTrue(UriUtility.IsHttpOrHttpsUrl(_url));
        }

        [TestMethod()]
        public void ReadContentTest()
        {
            string content = _url.ReadContent();
            Assert.IsFalse(string.IsNullOrEmpty(content));
        }

        [TestMethod()]
        public void ReadContentLengthTest()
        {
            long length = _url.ReadContentLength();
            Assert.IsTrue(length > 0);
        }
    }


    [TestClass()]
    public class UriUtilityExtensionsTests
    {

    }
}
