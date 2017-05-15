using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class StringUtilityTests
    {
        [TestMethod()]
        public void TrimCommaTest()
        {
            string str = "test trim comma";
            string newstr = StringUtility.TrimComma(",,,,,,,,," + str + ",,,");

            Assert.AreEqual(str, newstr, true);
        }

        [TestMethod()]
        public void TrimPeriodTest()
        {
            string str = "test trim period";
            string newstr = StringUtility.TrimPeriod("......" + str + ".....");

            Assert.AreEqual(str, newstr, true);
        }

        [TestMethod()]
        public void TrimSemicolonTest()
        {
            string str = "test trim semicolon";
            string newstr = StringUtility.TrimSemicolon(";;;;;" + str + ";;;;;");

            Assert.AreEqual(str, newstr, true);
        }
    }


    [TestClass()]
    public class StringUtilityExtensionsTests
    {

    }
}
