using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Data.Resources;

    public class DataStatusResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataStatusResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<DataStatusResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            // Groups
            var localized = localizer.GetString(r => r.GlobalGroup);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.ScopeGroup);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.StateGroup);
            Assert.False(localized.ResourceNotFound);

            // Global Group
            localized = localizer.GetString(r => r.Default);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Delete);
            Assert.False(localized.ResourceNotFound);

            // Scope Group
            localized = localizer.GetString(r => r.Public);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Protect);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Internal);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Private);
            Assert.False(localized.ResourceNotFound);

            // State Group
            localized = localizer.GetString(r => r.Active);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Pending);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Inactive);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Locking);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.Ban);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
