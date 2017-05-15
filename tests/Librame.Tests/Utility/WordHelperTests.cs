using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class WordHelperTests
    {
        [TestMethod()]
        public void WordAsPluralizeTest()
        {
            string str = WordHelper.AsPluralize("Aphorism");

            Assert.IsTrue(str == "Aphorisms");
        }

    }

}
