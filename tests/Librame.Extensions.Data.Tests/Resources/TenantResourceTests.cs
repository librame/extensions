using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class TenantResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<TenantResource>>();

            // en-US
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            var name = localizer[r => r.Name];
            Assert.Equal("Name", name);

            // zh-CN
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");

            name = localizer[r => r.Name];
            Assert.Equal("名称", name);

            // zh-TW
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-TW");

            name = localizer[r => r.Name];
            Assert.Equal("名稱", name);
        }
    }
}
