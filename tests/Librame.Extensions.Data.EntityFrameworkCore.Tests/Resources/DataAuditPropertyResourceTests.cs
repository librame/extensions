using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core;

    public class DataAuditPropertyResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataAuditPropertyResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<DataAuditPropertyResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var propertyName = localizer.GetString(r => r.PropertyName);
            Assert.False(propertyName.ResourceNotFound);

            var propertyTypeName = localizer.GetString(r => r.PropertyTypeName);
            Assert.False(propertyTypeName.ResourceNotFound);

            var oldValue = localizer.GetString(r => r.OldValue);
            Assert.False(oldValue.ResourceNotFound);

            var newValue = localizer.GetString(r => r.NewValue);
            Assert.False(newValue.ResourceNotFound);

            var auditId = localizer.GetString(r => r.AuditId);
            Assert.False(auditId.ResourceNotFound);

            var audit = localizer.GetString(r => r.Audit);
            Assert.False(audit.ResourceNotFound);
        }

    }
}
