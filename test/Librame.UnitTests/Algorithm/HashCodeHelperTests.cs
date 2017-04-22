using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Algorithm;

namespace Librame.UnitTests.Algorithm
{
    [TestClass()]
    public class HashCodeHelperTests
    {
        private readonly string _test = "Test HashCode";
        
        [TestMethod()]
        public void BkdrTest()
        {
            int bkdr = HashCodeHelper.ToBkdr(_test);
            Assert.IsNotNull(bkdr);
        }

        [TestMethod()]
        public void ApTest()
        {
            int ap = HashCodeHelper.ToAp(_test);
            Assert.IsNotNull(ap);
        }

        [TestMethod()]
        public void SdbmTest()
        {
            int sdbm = HashCodeHelper.ToSdbm(_test);
            Assert.IsNotNull(sdbm);
        }

        [TestMethod()]
        public void RsTest()
        {
            int rs = HashCodeHelper.ToRs(_test);
            Assert.IsNotNull(rs);
        }

        [TestMethod()]
        public void JsTest()
        {
            int js = HashCodeHelper.ToJs(_test);
            Assert.IsNotNull(js);
        }

        [TestMethod()]
        public void WeinbergerTest()
        {
            int weinberger = HashCodeHelper.ToWeinberger(_test);
            Assert.IsNotNull(weinberger);
        }

        [TestMethod()]
        public void ElfTest()
        {
            int elf = HashCodeHelper.ToElf(_test);
            Assert.IsNotNull(elf);
        }

        [TestMethod()]
        public void DjbTest()
        {
            int djb = HashCodeHelper.ToDjb(_test);
            Assert.IsNotNull(djb);
        }

    }
}