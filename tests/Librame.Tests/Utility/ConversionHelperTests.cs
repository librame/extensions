using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class ConversionHelperTests
    {
        [TestMethod()]
        public void ConversionAsOrDefaultTest()
        {
            bool isNull;
            string str = null;
            str = str.AsOrDefault("test", out isNull);

            Assert.IsTrue(isNull && !string.IsNullOrEmpty(str));
        }
        
    }
}
