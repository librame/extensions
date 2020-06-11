using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Localizers;

    public class EnhancedStringLocalizerTests
    {
        [Fact]
        public void AllTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<TestResource>>();
            Assert.True(localizer[r => r.Name].ResourceNotFound);
            Assert.True(localizer.GetString(r => r.Name).ResourceNotFound);
        }

    }
}
