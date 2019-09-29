using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class HumanizationResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionLocalizer<HumanizationResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IExpressionLocalizer<HumanizationResource> localizer, string cultureName)
        {
            CultureInfo.CurrentCulture
                = CultureInfo.CurrentUICulture
                = new CultureInfo(cultureName);

            var minutesAgo = localizer[r => r.HumanizedMinutesAgo];
            Assert.False(minutesAgo.ResourceNotFound);

            var hoursAgo = localizer[r => r.HumanizedHoursAgo];
            Assert.False(hoursAgo.ResourceNotFound);

            var daysAgo = localizer[r => r.HumanizedDaysAgo];
            Assert.False(daysAgo.ResourceNotFound);

            var monthsAgo = localizer[r => r.HumanizedMonthsAgo];
            Assert.False(monthsAgo.ResourceNotFound);

            var yearsAgo = localizer[r => r.HumanizedYearsAgo];
            Assert.False(yearsAgo.ResourceNotFound);
        }

    }
}
