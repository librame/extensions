using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class StringLocalizerTests
    {
        [Fact]
        public void AllTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<TestResource>>();
            Assert.True(localizer.GetString(r => r.Name).ResourceNotFound);
        }

    }

    public class TestResource : IResource
    {
        public string Name { get; set; }
    }
}
