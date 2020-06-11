using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Localizers;

    public class DictionaryStringLocalizerTests
    {
        public static readonly string[] DefaultNames
            = new string[] { "TestName", "测试名称", "測試名稱" };


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
