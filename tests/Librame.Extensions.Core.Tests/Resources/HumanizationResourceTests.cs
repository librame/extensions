using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Core;

    public class HumanizationResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionStringLocalizer<HumanizationResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IExpressionStringLocalizer<HumanizationResource> localizer, string cultureName)
        {
            AssemblyHelper.RegisterCultureInfos(cultureName);

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
