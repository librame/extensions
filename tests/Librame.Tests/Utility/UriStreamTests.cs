using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class UriStreamTests
    {
        private readonly UriStream _stream = null;

        public UriStreamTests()
        {
            _stream = new UriStream("http://comment.war.163.com/photoview_bbs/PHOT23T4U00014T8.html");
        }


        [TestMethod()]
        public void GetContentLengthTest()
        {
            long length = _stream.GetContentLength();
            Assert.IsTrue(length > 0);
        }

        [TestMethod()]
        public void GetContentTest()
        {
            string content = _stream.GetContent();
            Assert.IsFalse(string.IsNullOrEmpty(content));
        }
    }


    [TestClass()]
    public class UriStreamExtensionsTests
    {

    }
}
