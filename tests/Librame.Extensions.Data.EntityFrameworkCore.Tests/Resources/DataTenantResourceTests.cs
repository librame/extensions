using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using System.Globalization;

namespace Librame.Extensions.Data.Tests
{
    using Core.Utilities;
    using Resources;

    public class DataTenantResourceTests
    {
        [Fact]
        public void ResourceTest()
        {
            var cultureNames = new string[] { "en-US", "zh-CN", "zh-TW" };
            var localizer = TestServiceProvider.Current.GetRequiredService<IStringLocalizer<DataTenantResource>>();

            foreach (var name in cultureNames)
                RunTest(localizer, name);
        }
        
        private void RunTest(IStringLocalizer<DataTenantResource> localizer, string cultureName)
        {
            CultureUtility.Register(new CultureInfo(cultureName));

            var name = localizer.GetString(r => r.Name);
            Assert.False(name.ResourceNotFound);

            var host = localizer.GetString(r => r.Host);
            Assert.False(host.ResourceNotFound);

            var defaultConnection = localizer.GetString(r => r.DefaultConnectionString);
            Assert.False(defaultConnection.ResourceNotFound);

            var writeConnection = localizer.GetString(r => r.WriteConnectionString);
            Assert.False(writeConnection.ResourceNotFound);

            var separation = localizer.GetString(r => r.WriteConnectionSeparation);
            Assert.False(separation.ResourceNotFound);
        }

    }
}
