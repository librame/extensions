using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Localizations;

    public class TestResourceTests
    {

        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionStringLocalizer<TestResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }

        private void RunTest(IExpressionStringLocalizer<TestResource> localizer, string cultureName)
        {
            LocalizationRegistration.Register(cultureName);

            var name = localizer[r => r.Name];
            Assert.False(name.ResourceNotFound);
        }

    }
}
