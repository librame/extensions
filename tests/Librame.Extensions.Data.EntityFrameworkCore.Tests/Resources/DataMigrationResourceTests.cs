using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

    public class DataMigrationResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataMigrationResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IStringLocalizer<DataMigrationResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var localized = localizer.GetString(r => r.AccessorName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.ModelSnapshotName);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.ModelHash);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.ModelBody);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
