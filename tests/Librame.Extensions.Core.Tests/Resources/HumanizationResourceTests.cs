using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Resources;
    using Utilities;

    public class HumanizationResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<HumanizationResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IStringLocalizer<HumanizationResource> localizer, string cultureName)
        {
            CultureInfoUtility.Register(new CultureInfo(cultureName));

            var localized = localizer.GetString(r => r.HumanizedMinutesAgo);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.HumanizedHoursAgo);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.HumanizedDaysAgo);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.HumanizedMonthsAgo);
            Assert.False(localized.ResourceNotFound);

            localized = localizer.GetString(r => r.HumanizedYearsAgo);
            Assert.False(localized.ResourceNotFound);
        }

    }
}
