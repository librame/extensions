using System.Globalization;
using Microsoft.Extensions.Localization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractResourceDictionaryStringLocalizerTests
    {
        public static readonly string[] DefaultNames
            = new string[] { "TestName", "测试名称", "測試名稱" };

        public class TestResource : IResource
        {
            public string Name { get; set; }
        }

        public class TestResource_en_US : AbstractResourceDictionary
        {
            public TestResource_en_US()
                : base()
            {
                AddOrUpdate("Name", DefaultNames[0], (key, value) => DefaultNames[0]);
            }
        }

        public class TestResource_zh_CN : AbstractResourceDictionary
        {
            public TestResource_zh_CN()
                : base()
            {
                AddOrUpdate("Name", DefaultNames[1], (key, value) => DefaultNames[1]);
            }
        }

        public class TestResource_zh_TW : AbstractResourceDictionary
        {
            public TestResource_zh_TW()
                : base()
            {
                AddOrUpdate("Name", DefaultNames[2], (key, value) => DefaultNames[2]);
            }
        }


        public class TestResourceDictionaryStringLocalizer : AbstractResourceDictionaryStringLocalizer<TestResource>
        {
            public TestResourceDictionaryStringLocalizer(CultureInfo cultureInfo)
                : base(AbstractResourceDictionary.GetResourceDictionary<TestResource>(cultureInfo))
            {
            }

            public TestResourceDictionaryStringLocalizer(IResourceDictionary resourceDictionary)
                : base(resourceDictionary)
            {
            }


            public override IStringLocalizer WithCulture(CultureInfo cultureInfo)
            {
                var resourceDictionary = AbstractResourceDictionary.GetResourceDictionary<TestResource>(cultureInfo);
                return new TestResourceDictionaryStringLocalizer(resourceDictionary);
            }
        }


        [Fact]
        public void AllTest()
        {
            var cultures = new string[] { "en-US", "zh-CN", "zh-TW" };
            for (var i = 0; i < cultures.Length; i++)
            {
                var localizer = new TestResourceDictionaryStringLocalizer(new CultureInfo(cultures[i]));
                var name = localizer[r => r.Name];
                Assert.False(name.ResourceNotFound);
                Assert.Equal(DefaultNames[i], name.Value);
            }
        }
    }
}
