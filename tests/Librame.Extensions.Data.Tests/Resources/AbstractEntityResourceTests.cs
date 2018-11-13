using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class AbstractEntityResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<AbstractEntityResource>>();

            // en-US
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            var delete = localizer[r => r.DataStatus];
            Assert.Equal("Data status", delete);

            // zh-CN
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");

            delete = localizer[r => r.DataStatus];
            Assert.Equal("数据状态", delete);

            // zh-TW
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-TW");

            delete = localizer[r => r.DataStatus];
            Assert.Equal("數據狀態", delete);
        }
    }
}
