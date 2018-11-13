using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class DataStatusResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<DataStatusResource>>();

            // en-US
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            var delete = localizer[r => r.Delete];
            Assert.Equal("Delete", delete);

            // zh-CN
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");

            delete = localizer[r => r.Delete];
            Assert.Equal("删除", delete);

            // zh-TW
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-TW");

            delete = localizer[r => r.Delete];
            Assert.Equal("刪除", delete);
        }
    }
}
