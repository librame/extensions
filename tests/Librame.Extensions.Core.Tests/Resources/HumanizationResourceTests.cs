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
            CultureUtility.Register(new CultureInfo(cultureName));

            var minutesAgo = localizer.GetString(r => r.HumanizedMinutesAgo);
            Assert.False(minutesAgo.ResourceNotFound);

            var hoursAgo = localizer.GetString(r => r.HumanizedHoursAgo);
            Assert.False(hoursAgo.ResourceNotFound);

            var daysAgo = localizer.GetString(r => r.HumanizedDaysAgo);
            Assert.False(daysAgo.ResourceNotFound);

            var monthsAgo = localizer.GetString(r => r.HumanizedMonthsAgo);
            Assert.False(monthsAgo.ResourceNotFound);

            var yearsAgo = localizer.GetString(r => r.HumanizedYearsAgo);
            Assert.False(yearsAgo.ResourceNotFound);
        }

    }
}
