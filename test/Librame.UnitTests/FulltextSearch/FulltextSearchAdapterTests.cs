using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.FulltextSearch;

namespace Librame.UnitTests.FulltextSearch
{
    [TestClass()]
    public class FulltextSearchAdapterTests
    {
        private readonly IFulltextSearchAdapter _adapter;

        public FulltextSearchAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.FulltextSearchAdapter;
        }


        [TestMethod()]
        public void FulltextSearchTokenTest()
        {
            var words = _adapter.Token("这是一个悲伤的、可歌可泣的爱情故事。");
            Assert.IsNotNull(words);
            Assert.IsTrue(words.Length > 0);
        }

    }
}