using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

    public class DataAuditResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataAuditResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<DataAuditResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var localized = localizer.GetString(r => r.EntityId);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.EntityTypeName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.TableName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.StateName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Properties);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
