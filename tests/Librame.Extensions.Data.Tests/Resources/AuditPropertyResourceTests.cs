using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Resources;

    public class AuditPropertyResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<AuditPropertyResource>>();

            // en-US
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            var delete = localizer[r => r.AuditId];
            Assert.Equal("Audit id", delete);

            // zh-CN
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");

            delete = localizer[r => r.AuditId];
            Assert.Equal("审计标识", delete);

            // zh-TW
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-TW");

            delete = localizer[r => r.AuditId];
            Assert.Equal("審計標識", delete);
        }
    }
}
