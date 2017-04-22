using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Librame.Utility;

namespace Librame.UnitTests.Utility
{
    [TestClass()]
    public class GuidUtilityTests
    {
        [TestMethod()]
        public void ToBitTest()
        {
            var g = Guid.Parse("c4a54925-06bf-4ef3-9e53-847979253e98");
            var bit = g.AsBit();
            Assert.IsTrue(bit.Length > 0);
        }

        [TestMethod()]
        public void FromBitTest()
        {
            // 25-49-A5-C4-BF-06-F3-4E-9E-53-84-79-79-25-3E-98
            var bit = "2549A5C4BF06F34E9E53847979253E98";
            var g = bit.FromBitAsGuid();
            Assert.IsNotNull(g);
        }
    }

}
