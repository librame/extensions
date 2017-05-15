using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class GuidUtilityTests
    {
        [TestMethod()]
        public void ToHexTest()
        {
            //var g1 = Guid.NewGuid().AsHex();
            //var g2 = Guid.NewGuid().AsHex();
            //var g3 = Guid.NewGuid().AsHex();
            //var g4 = Guid.NewGuid().AsHex();
            //var g5 = Guid.NewGuid().AsHex();

            var g = Guid.Parse("c4a54925-06bf-4ef3-9e53-847979253e98");
            var hex = GuidUtility.AsHex(g);
            Assert.IsTrue(hex.Length > 0);
        }

        [TestMethod()]
        public void FromHexTest()
        {
            // 25-49-A5-C4-BF-06-F3-4E-9E-53-84-79-79-25-3E-98
            var hex = "2549A5C4BF06F34E9E53847979253E98";
            var g = GuidUtility.FromHex(hex);
            Assert.IsNotNull(g);
        }
    }

}
