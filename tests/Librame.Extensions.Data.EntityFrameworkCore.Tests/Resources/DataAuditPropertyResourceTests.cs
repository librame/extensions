using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

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
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var localized = localizer.GetString(r => r.Audit);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.AuditId);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.PropertyName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.PropertyTypeName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.OldValue);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.NewValue);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
