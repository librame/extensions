using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class AuditResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<AuditResource>>();

            // en-US
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            var delete = localizer[r => r.State];
            Assert.Equal("State", delete);

            // zh-CN
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");

            delete = localizer[r => r.State];
            Assert.Equal("状态", delete);

            // zh-TW
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-TW");

            delete = localizer[r => r.State];
            Assert.Equal("狀態", delete);
        }
    }
}
