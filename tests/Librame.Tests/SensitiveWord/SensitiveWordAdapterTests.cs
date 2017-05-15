using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.SensitiveWord;

namespace Librame.Tests.SensitiveWord
{
    [TestClass()]
    public class SensitiveWordAdapterTests
    {
        private readonly ISensitiveWordsFilter _filter;

        public SensitiveWordAdapterTests()
        {
            _filter = LibrameArchitecture.AdapterManager.SensitiveWord.Filter;
        }


        [TestMethod()]
        public void FiltingTest()
        {
            var content = "经常有吧友在发帖和回复的时候，发现自己的帖子里面出现了枪支中间加字和";
            content += "空格也许能避免这种情况，但有时候弹药并不知道哪些是敏感词，怎么办呢？";
            content += "于是经过我多方调查总结，终于编成了A片这部百度敏感词指北，有了它，再也不用怕敏感词了！";

            var filtingContent = _filter.Filting(content);

            Assert.IsTrue(_filter.Exists);
            Assert.AreNotEqual(content, filtingContent);
        }

    }
}
