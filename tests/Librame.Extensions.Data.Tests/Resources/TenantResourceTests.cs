using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Localizations;

    public class TenantResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IEnhancedStringLocalizer<TenantResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IEnhancedStringLocalizer<TenantResource> localizer, string cultureName)
        {
            LocalizationRegistration.Register(cultureName);

            var name = localizer[r => r.Name];
            Assert.False(name.ResourceNotFound);

            var host = localizer[r => r.Host];
            Assert.False(host.ResourceNotFound);

            var defaultConnection = localizer[r => r.DefaultConnectionString];
            Assert.False(defaultConnection.ResourceNotFound);

            var writeConnection = localizer[r => r.WriteConnectionString];
            Assert.False(writeConnection.ResourceNotFound);

            var separation = localizer[r => r.WriteConnectionSeparation];
            Assert.False(separation.ResourceNotFound);
        }

    }
}
