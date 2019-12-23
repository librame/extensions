using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Localizers;
    using Resources;

    public class DictionaryStringLocalizerTests
    {
        public static readonly string[] DefaultNames
            = new string[] { "TestName", "测试名称", "測試名稱" };


        public class TestResource : IResource
        {
            public string Name { get; set; }
        }

        public class TestResource_en_US : ResourceDictionary
        {
            public TestResource_en_US()
                : base()
            {
                AddOrUpdate("Name", DefaultNames[0], (key, value) => DefaultNames[0]);
            }
        }

        public class TestResource_zh_CN : ResourceDictionary
        {
            public TestResource_zh_CN()
                : base()
            {
                AddOrUpdate("Name", DefaultNames[1], (key, value) => DefaultNames[1]);
            }
        }

        public class TestResource_zh_TW : ResourceDictionary
        {
            public TestResource_zh_TW()
                : base()
            {
                AddOrUpdate("Name", DefaultNames[2], (key, value) => DefaultNames[2]);
            }
        }


        [Fact]
        public void AllTest()
        {
            var cultures = new string[] { "en-US", "zh-CN", "zh-TW" };
            for (var i = 0; i < cultures.Length; i++)
            {
                CultureInfo.CurrentUICulture = new CultureInfo(cultures[i]);

                var localizer = TestServiceProvider.Current.GetRequiredService<IDictionaryStringLocalizer<TestResource>>();
                var name = localizer.GetString(r => r.Name);
                Assert.False(name.ResourceNotFound);
                Assert.Equal(DefaultNames[i], name.Value);
            }
        }
    }
}
