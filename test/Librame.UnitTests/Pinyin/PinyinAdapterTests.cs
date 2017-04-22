using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Pinyin;

namespace Librame.UnitTests.Pinyin
{
    [TestClass()]
    public class PinyinPluginTests
    {
        private readonly IPinyinAdapter _adapter;

        public PinyinPluginTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.PinyinAdapter;
        }
        
        [TestMethod()]
        public void PinyinParseTest()
        {
            var info = _adapter.Parse("测试中文");
            Assert.IsNotNull(info);
            Assert.IsNotNull(info.AllWords);
            Assert.IsNotNull(info.Name);
        }

    }
}