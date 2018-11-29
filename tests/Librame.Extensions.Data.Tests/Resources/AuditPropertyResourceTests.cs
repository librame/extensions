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
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<AuditPropertyResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IEnhancedStringLocalizer<AuditPropertyResource> localizer, string cultureName)
        {
            CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);

            var propertyName = localizer[r => r.PropertyName];
            Assert.False(propertyName.ResourceNotFound);

            var propertyTypeName = localizer[r => r.PropertyTypeName];
            Assert.False(propertyTypeName.ResourceNotFound);

            var oldValue = localizer[r => r.OldValue];
            Assert.False(oldValue.ResourceNotFound);

            var newValue = localizer[r => r.NewValue];
            Assert.False(newValue.ResourceNotFound);

            var auditId = localizer[r => r.AuditId];
            Assert.False(auditId.ResourceNotFound);

            var audit = localizer[r => r.Audit];
            Assert.False(audit.ResourceNotFound);
        }

    }
}
